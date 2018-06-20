using CodeGenHero.BingoBuzz.Xam.Interfaces;
using CodeGenHero.BingoBuzz.Xam.ModelObj.BB;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeGenHero.Logging;
using System.Threading;
using Microsoft.AppCenter.Crashes;

namespace CodeGenHero.BingoBuzz.Xam.Services
{
    public partial class DataRetrievalService : IDataRetrievalService
    {
        private SQLiteAsyncConnection _asyncConnection;

        private SQLiteConnection _connection;

        private IDatabase _database;

        private ILoggingService _log;

        private IStateService _stateService;

        public DataRetrievalService(ILoggingService log, IDatabase database, IStateService stateService)
        {
            _log = log;
            _database = database;
            _stateService = stateService;
            _asyncConnection = _database.GetAsyncConnection();
            _connection = _database.GetConnection();
        }

        public async Task<BingoInstance> CreateNewBingoInstance(Guid meetingId)
        {
            Guid instanceId = Guid.NewGuid();

            var newInstance = new ModelData.BB.BingoInstance()
            {
                BingoInstanceId = instanceId,
                BingoInstanceStatusTypeId = (int)Constants.Enums.BingoInstanceStatusType.Active,
                CreatedDate = DateTime.UtcNow,
                CreatedUserId = _stateService.GetCurrentUserId(),
                IncludeFreeSquareIndicator = true,
                IsDeleted = false,
                MeetingId = meetingId,
                NumberOfColumns = 5,
                NumberOfRows = 5,
                UpdatedDate = DateTime.UtcNow,
                UpdatedUserId = _stateService.GetCurrentUserId()
            };

            if (1 != await _asyncConnection.InsertAsync(newInstance))
            {
                var message = "Error Writing new meeting instance to SQLite";
                _log.Fatal(message, LogMessageType.Instance.Exception_Database);
                throw new Exception(message);
            }

            //queue the instance for upload
            await DataUploadService.Instance.QueueAsync(instanceId, Constants.Enums.QueueableObjects.BingoInstance);
            //create the new bingo instance content records, they will be queued for upload with the instance
            await CreateNewBingoInstanceContentsNoRepeats(newInstance.BingoInstanceId);
            
            //fire off safely backgrounded upload for the instance record and the content
            DataUploadService.Instance.StartSafeQueuedUpdates();
            
            return newInstance.ToModelObj();
        }

        public async Task<bool> CreateSendNewMeeting(Meeting meeting, List<User> attendees)
        {
            meeting.CreatedDate = DateTime.UtcNow;
            meeting.UpdatedDate = DateTime.UtcNow;
            meeting.CreatedUserId = _stateService.GetCurrentUserId();
            meeting.UpdatedUserId = _stateService.GetCurrentUserId();
            meeting.IsDeleted = false;

            if (1 != await _asyncConnection.InsertAsync(meeting.ToModelData()))
            {
                var message = "Error Writing new meeting to SQLite";
                _log.Fatal(message, LogMessageType.Instance.Exception_Database);
                throw new Exception(message);
            }

            //queue meeting for upload
            await DataUploadService.Instance.QueueAsync(meeting.MeetingId, Constants.Enums.QueueableObjects.Meeting);

            foreach (var a in attendees)
            {
                Guid id = Guid.NewGuid();
                if (1 != await _asyncConnection.InsertAsync(new ModelData.BB.MeetingAttendee()
                {
                    MeetingId = meeting.MeetingId,
                    CreatedDate = DateTime.UtcNow,
                    CreatedUserId = _stateService.GetCurrentUserId(),
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedUserId = _stateService.GetCurrentUserId(),
                    UserId = a.UserId,
                    IsDeleted = false,
                    MeetingAttendeeId = id
                }))
                {
                    var message = "Error Writing new meeting attendee to SQLite";
                    _log.Fatal(message, LogMessageType.Instance.Exception_Database);
                    throw new Exception(message);
                }
               
                await DataUploadService.Instance.QueueAsync(id, Constants.Enums.QueueableObjects.MeetingAttendee);                
            }
            //fire off safely backgrounded upload
            DataUploadService.Instance.StartSafeQueuedUpdates();

            return true;
        }

        public async Task<bool> CreateSendNewBingoInstanceEvent(Guid bingoInstanceContentId, Guid bingoInstanceId, BingoBuzz.Constants.Enums.BingoInstanceEventType eventType)
        {
            try
            {
                Guid eventId = Guid.NewGuid();

                //write it to SQLite
                if (1 != await _asyncConnection.InsertAsync(new ModelData.BB.BingoInstanceEvent()
                {
                    BingoInstanceEventId = eventId,
                    BingoInstanceContentId = bingoInstanceContentId,
                    BingoInstanceEventTypeId = (int)eventType,
                    BingoInstanceId = bingoInstanceId,
                    CreatedDate = DateTime.UtcNow,
                    CreatedUserId = _stateService.GetCurrentUserId(),
                    IsDeleted = false,
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedUserId = _stateService.GetCurrentUserId()
                }))
                {
                    var message = "Error Writing new bingo instance event to SQLite";
                    _log.Fatal(message, LogMessageType.Instance.Exception_Database);
                    throw new Exception(message);
                }

                //queue it for upload
                await DataUploadService.Instance.QueueAsync(eventId, Constants.Enums.QueueableObjects.BingoInstanceEvent);

                //fire off safely backgrounded upload
                DataUploadService.Instance.StartSafeQueuedUpdates();

                return true;
            }catch(Exception ex)
            {
                Crashes.TrackError(ex);
                return false;
            }
        }

        public async Task<List<ModelObj.BB.BingoInstanceContent>> GetBingoInstanceContentAsync(Guid bingoInstanceId)
        {
            //returns Bingo Instance Content
            List<ModelObj.BB.BingoInstanceContent> returnMe = new List<ModelObj.BB.BingoInstanceContent>();
            var dataInstanceContents = await _asyncConnection.Table<ModelData.BB.BingoInstanceContent>().Where(x => x.BingoInstanceId == bingoInstanceId && x.IsDeleted == false).ToListAsync();
            foreach (var d in dataInstanceContents)
            {
                var dataContent = await _asyncConnection.Table<ModelData.BB.BingoContent>().Where(x => x.BingoContentId == d.BingoContentId && x.IsDeleted == false).FirstOrDefaultAsync();
                if (dataContent != null)
                {
                    var objContent = d.ToModelObj();
                    objContent.BingoContent = dataContent.ToModelObj();
                    returnMe.Add(objContent);
                }
            }
            return returnMe;
        }

        public async Task<ModelObj.BB.BingoInstance> GetCurrentBingoInstanceOrNullAsync(Guid meetingId)
        {
            //get all instances for this meeting
            var dataInstance = (await _asyncConnection.Table<ModelData.BB.BingoInstance>()
                .Where(x => x.MeetingId == meetingId && x.BingoInstanceStatusTypeId != 4 && x.IsDeleted == false).FirstOrDefaultAsync());
            if (dataInstance != null)
            {
                return dataInstance.ToModelObj();
            }
            return null;
        }

        public async Task<int> GetTotalNumberOfBingos()
        {
            Guid currentUserId = _stateService.GetCurrentUserId();
            int bingoTypeId = (int)Constants.Enums.BingoInstanceEventType.Bingo;
            var count = (await _asyncConnection.Table<ModelData.BB.BingoInstanceEvent>()
                .Where(x => x.CreatedUserId == currentUserId && x.BingoInstanceEventTypeId == bingoTypeId && x.IsDeleted == false).CountAsync());
            return count;
        }

        public async Task<int> GetTotalNumberOfSquareClicks()
        {
            Guid currentUserId = _stateService.GetCurrentUserId();
            int squareClickTypeId = (int)Constants.Enums.BingoInstanceEventType.SquareClicked;
            var count = (await _asyncConnection.Table<ModelData.BB.BingoInstanceEvent>()
                .Where(x => x.CreatedUserId == currentUserId && x.BingoInstanceEventTypeId == squareClickTypeId && x.IsDeleted == false).CountAsync());
            return count;
        }

        public async Task<int> GetTotalNumberOfGames()
        {
            Guid currentUserId = _stateService.GetCurrentUserId();
            var count = (await _asyncConnection.Table<ModelData.BB.BingoInstance>()
                .Where(x => x.CreatedUserId == currentUserId && x.IsDeleted == false).CountAsync());
            return count;
        }

        public async Task<ModelObj.BB.Company> GetCompanyByIdOrNull(Guid companyId)
        {
            var company = (await _asyncConnection.Table<ModelData.BB.Company>()
                .Where(x => x.CompanyId == companyId && x.IsDeleted == false).FirstOrDefaultAsync());
            if (company != null)
            {
                return company.ToModelObj();
            }
            return null;
        }

        public async Task<ModelObj.BB.User> GetUserByEmailOrNullAsync(string email)
        {
            var emailLower = email.ToLower();
            var user = (await _asyncConnection.Table<ModelData.BB.User>()
                .Where(x => x.Email.ToLower() == emailLower  && x.IsDeleted == false).FirstOrDefaultAsync());
            if (user != null)
            {
                return user.ToModelObj();
            }
            return null;
        }

        public async Task<List<ModelObj.BB.MeetingAttendee>> GetMeetingAttendeesAsync(Guid meetingId)
        {
            //returns meeting attendees
            List<ModelObj.BB.MeetingAttendee> returnMe = new List<ModelObj.BB.MeetingAttendee>();
            var dataMeeting = await _asyncConnection.Table<ModelData.BB.Meeting>().Where(x => x.MeetingId == meetingId && x.IsDeleted == false).FirstOrDefaultAsync();
            if (dataMeeting != null)
            {
                var dataAttendees = await _asyncConnection.Table<ModelData.BB.MeetingAttendee>().Where(x => x.MeetingId == meetingId && x.IsDeleted == false).ToListAsync();
                foreach (var a in dataAttendees)
                {
                    var objAttend = a.ToModelObj();
                    objAttend.User_UserId = (await _asyncConnection.Table<ModelData.BB.User>().Where(x => x.UserId == objAttend.UserId && x.IsDeleted == false).FirstOrDefaultAsync()).ToModelObj();
                    returnMe.Add(objAttend);
                }
            }
            return returnMe;
        }

        public async Task<ModelObj.BB.Meeting> GetMeetingOrNullAsync(Guid meetingId)
        {
            var dataMeeting = await _asyncConnection.Table<ModelData.BB.Meeting>().Where(x => x.MeetingId == meetingId && x.IsDeleted == false).FirstOrDefaultAsync();
            if (dataMeeting != null)
            {
                return dataMeeting.ToModelObj();
            }

            return null;
        }

        public async Task<List<ModelObj.BB.Meeting>> GetMeetingsAsync()
        {
            List<ModelObj.BB.Meeting> returnMe = new List<ModelObj.BB.Meeting>();

            var dataMeetings = await _asyncConnection.Table<ModelData.BB.Meeting>().Where(x => x.IsDeleted == false).ToListAsync();
            if (dataMeetings.Any())
            {
                returnMe.AddRange(dataMeetings.Select(x => x.ToModelObj()).ToList());
            }

            return returnMe;
        }

        public async Task<List<ModelObj.BB.User>> GetUsersAsync()
        {
            List<ModelObj.BB.User> returnMe = new List<ModelObj.BB.User>();

            var dataUsers = await _asyncConnection.Table<ModelData.BB.User>().Where(x => x.IsDeleted == false).ToListAsync();
            if (dataUsers.Any())
            {
                returnMe.AddRange(dataUsers.Select(x => x.ToModelObj()).ToList());
            }

            return returnMe;
        }

        private async Task CreateNewBingoInstanceContentsNoRepeats(Guid bingoInstanceId)
        {
            var bingoInstance = await _asyncConnection.Table<ModelData.BB.BingoInstance>().Where(x => x.BingoInstanceId == bingoInstanceId).FirstOrDefaultAsync();

            if (bingoInstance != null)
            {
                int totalNumberOfSquaresNeeded = bingoInstance.NumberOfColumns * bingoInstance.NumberOfRows;

                int squareCount = totalNumberOfSquaresNeeded - 1;

                List<ModelData.BB.BingoContent> contentData = (await _asyncConnection.QueryAsync<ModelData.BB.BingoContent>($"SELECT * FROM BingoContent ORDER BY RANDOM() LIMIT {totalNumberOfSquaresNeeded};")).ToList();

                for (int c = 0; c < bingoInstance.NumberOfColumns; c++)
                {
                    for (int r = 0; r < bingoInstance.NumberOfRows; r++)
                    {
                        var bingoContent = contentData[squareCount];
                        var newInstanceContent = new ModelData.BB.BingoInstanceContent()
                        {
                            BingoInstanceContentId = Guid.NewGuid(),
                            BingoInstanceId = bingoInstanceId,
                            BingoInstanceContentStatusTypeId = (int)Constants.Enums.BingoInstanceContentStatusType.Untapped, 
                            UserId = _stateService.GetCurrentUserId(),
                            Row = r,
                            Col = c,
                            UpdatedDate = DateTime.UtcNow,
                            UpdatedUserId = _stateService.GetCurrentUserId(),
                            CreatedDate = DateTime.UtcNow,
                            CreatedUserId = _stateService.GetCurrentUserId(),
                            IsDeleted = false,
                            FreeSquareIndicator = bingoContent.FreeSquareIndicator,
                            BingoContentId = bingoContent.BingoContentId
                        };

                        if (1 != await _asyncConnection.InsertAsync(newInstanceContent))
                        {
                            _log.Error("Error writing new BingoInstanceContent records to SQLite", LogMessageType.Instance.Exception_Database);
                        }

                        squareCount--;
                    }
                }
            }
        }

        //Please retain this method in case it is more fun to have repeating squares
        private async Task CreateNewBingoInstanceContentsWithRepeats(Guid bingoInstanceId)
        {
            var bingoInstance = await _asyncConnection.Table<ModelData.BB.BingoInstance>().Where(x => x.BingoInstanceId == bingoInstanceId).FirstOrDefaultAsync();
            if (bingoInstance != null)
            {
                for (int c = 0; c < bingoInstance.NumberOfColumns; c++)
                {
                    for (int r = 0; r < bingoInstance.NumberOfRows; r++)
                    {
                        var bingoContent = await PickABingoContentRandomly();
                        var newInstanceContent = new ModelData.BB.BingoInstanceContent()
                        {
                            BingoInstanceContentId = Guid.NewGuid(),
                            BingoInstanceId = bingoInstanceId,
                            Row = r,
                            Col = c,
                            UpdatedDate = DateTime.UtcNow,
                            UpdatedUserId = _stateService.GetCurrentUserId(),
                            CreatedDate = DateTime.UtcNow,
                            CreatedUserId = _stateService.GetCurrentUserId(),
                            IsDeleted = false,
                            FreeSquareIndicator = bingoContent.FreeSquareIndicator,
                            BingoContentId = bingoContent.BingoContentId
                        };

                        if (1 != await _asyncConnection.InsertAsync(newInstanceContent))
                        {
                            _log.Error("Error writing new BingoInstanceContent records to SQLite", LogMessageType.Instance.Exception_Database);
                        }
                    }
                }
            }
        }

        private async Task<BingoContent> PickABingoContentRandomly()
        {
            var pick = (await _asyncConnection.QueryAsync<ModelData.BB.BingoContent>("SELECT * FROM BingoContent ORDER BY RANDOM() LIMIT 1;")).FirstOrDefault();
            return pick.ToModelObj();
        }
    }
}