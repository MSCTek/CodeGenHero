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
            var newInstance = new ModelData.BB.BingoInstance()
            {
                BingoInstanceId = Guid.NewGuid(),
                BingoInstanceStatusTypeId = 2, //active
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

            await CreateNewBingoInstanceContentsNoRepeats(newInstance.BingoInstanceId);

            return newInstance.ToModelObj();
        }

        public async Task<bool> CreateNewMeeting(Meeting meeting, List<User> attendees)
        {
            meeting.CreatedDate = DateTime.UtcNow;
            meeting.UpdatedDate = DateTime.UtcNow;
            meeting.CreatedUserId = GetCurrentUserId();
            meeting.UpdatedUserId = GetCurrentUserId();
            meeting.IsDeleted = false;

            if (1 != await _asyncConnection.InsertAsync(meeting.ToModelData()))
            {
                var message = "Error Writing new meeting to SQLite";
                _log.Fatal(message, LogMessageType.Instance.Exception_Database);
                throw new Exception(message);
            }
            foreach (var a in attendees)
            {
                if (1 != await _asyncConnection.InsertAsync(new ModelData.BB.MeetingAttendee()
                {
                    MeetingId = meeting.MeetingId,
                    CreatedDate = DateTime.UtcNow,
                    CreatedUserId = GetCurrentUserId(),
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedUserId = GetCurrentUserId(),
                    UserId = a.UserId,
                    IsDeleted = false,
                    MeetingAttendeeId = Guid.NewGuid()
                }))
                {
                    var message = "Error Writing new meeting attendee to SQLite";
                    _log.Fatal(message, LogMessageType.Instance.Exception_Database);
                    throw new Exception(message);
                }
            }

            return true;
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

        //TODO: change this when authentication is wired up
        public Guid GetCurrentUserId()
        {
            return Guid.Parse("B79ED0E3-DDB9-4920-8900-FFC55A73B4B5");
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