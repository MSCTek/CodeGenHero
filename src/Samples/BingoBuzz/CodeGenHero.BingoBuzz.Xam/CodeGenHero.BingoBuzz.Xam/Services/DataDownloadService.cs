using Microsoft.AppCenter.Analytics;
using CodeGenHero.BingoBuzz.API.Client;
using CodeGenHero.BingoBuzz.API.Client.Interface;
using CodeGenHero.BingoBuzz.Xam.Interfaces;
using CodeGenHero.BingoBuzz.Xam.ModelObj.BB;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeGenHero.Logging;
using CodeGenHero.DataService;
using CodeGenHero.BingoBuzz.Constants;
using cghConstants = CodeGenHero.DataService.Constants;
using Microsoft.AppCenter.Crashes;

namespace CodeGenHero.BingoBuzz.Xam.Services
{
	public class DataDownloadService : IDataDownloadService
	{
		private SQLiteAsyncConnection _asyncConnection;
		private SQLiteConnection _connection;
		private IDatabase _database;
		private ILoggingService _log;
		private IWebApiDataServiceBB _webAPIDataService;

		public DataDownloadService(ILoggingService log, IDatabase database)
		{
			_log = log;
			_database = database;
			IWebApiExecutionContext context = new WebApiExecutionContext(
				executionContextType: new WebApiExecutionContextType(),
				baseWebApiUrl: Consts.BaseWebApiUrl,
				baseFileUrl: Consts.BaseFileUrl,
				connectionIdentifier: Consts.ConnectionIdentifier
				);

			_webAPIDataService = new WebApiDataServiceBB(new LoggingService(), context);
			_asyncConnection = _database.GetAsyncConnection();
			_connection = _database.GetConnection();
		}

        public async Task InsertOrReplaceAuthenticatedUser(Guid userId)
        {
            var user = await _webAPIDataService.GetUserAsync(userId, 1);
            if (user.IsSuccessStatusCode)
            {
                int num = await _asyncConnection.InsertOrReplaceAsync(user.Data.ToModelData());
                _log.Debug($"Inserted authenticated user", LogMessageType.Instance.Info_Synchronization);
            }
            else
            {
                string whatHappened = "Can't find this BingoBuzz user!";
                _log.Error(whatHappened, LogMessageType.Instance.Info_Synchronization);
                throw new Exception(whatHappened);
            }
        }

        public async Task InsertOrReplaceAuthenticatedUser(string email, Guid userId, string givenName, string surName)
        {
            var user = await _webAPIDataService.GetUserAsync(userId, 1);
            if (user.IsSuccessStatusCode)
            {
                int num = await _asyncConnection.InsertOrReplaceAsync(user.Data.ToModelData());
                _log.Debug($"Inserted authenticated user", LogMessageType.Instance.Info_Synchronization);
            }
            else
            {
                //didn't find this user in our db yet, let's add them
                DTO.BB.User newUser = new DTO.BB.User()
                {
                    UserId = userId,
                    FirstName = givenName,
                    LastName = surName,
                    Email = email,
                    CreatedDate = DateTime.UtcNow,
                    CreatedUserId = userId,
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedUserId = userId,
                    IsDeleted = false
                };

                //send the new user to the Azure DB via the webAPI
                var result = await _webAPIDataService.CreateUserAsync(newUser);

                if (result.IsSuccessStatusCode)
                {
                    //insert the new user into the local SQLite db
                    int num = await _asyncConnection.InsertOrReplaceAsync(result.Data.ToModelData());
                    _log.Debug($"Inserted NEW authenticated user", LogMessageType.Instance.Info_Synchronization);
                }
                else
                {
                    string whatHappened = "Can't make a new BingoBuzz User!";
                    _log.Error(whatHappened, LogMessageType.Instance.Info_Synchronization);
                    throw new Exception(whatHappened);
                }
            }
        }


        public async Task InsertAllDataCleanLocalDB(Guid userId)
		{
			try
			{
				//we want all of the bingo contents records
				var bingoContents = await _webAPIDataService.GetAllPagesBingoContentsAsync(null);
				int numBingoContentsInserted = await _asyncConnection.InsertAllAsync(bingoContents.Select(x => x.ToModelData()).ToList());
				_log.Debug($"Inserted {numBingoContentsInserted} bingo contents records", LogMessageType.Instance.Info_Synchronization);

                //Load Types
                var notificationMethodTypes = await _webAPIDataService.GetAllPagesNotificationMethodTypesAsync();
                int numNotificationMethodTypesInserted = await _asyncConnection.InsertAllAsync(notificationMethodTypes.Select(x => x.ToModelData()).ToList());
                _log.Debug($"Inserted {numNotificationMethodTypesInserted} notification method type records", LogMessageType.Instance.Info_Synchronization);

                var bingoFrequencyTypes = await _webAPIDataService.GetAllPagesFrequencyTypesAsync();
                int numBingoFrequencyTypesInserted = await _asyncConnection.InsertAllAsync(bingoFrequencyTypes.Select(x => x.ToModelData()).ToList());
                _log.Debug($"Inserted {numBingoFrequencyTypesInserted} bingo frequency type records", LogMessageType.Instance.Info_Synchronization);

                var bingoInstanceEventTypes = await _webAPIDataService.GetAllPagesBingoInstanceEventTypesAsync();
                int numBingoInstanceEventTypesInserted = await _asyncConnection.InsertAllAsync(bingoInstanceEventTypes.Select(x => x.ToModelData()).ToList());
                _log.Debug($"Inserted {numBingoInstanceEventTypesInserted} bingo instance event type records", LogMessageType.Instance.Info_Synchronization);

                //load All Users - will get rid of this when there is better support for companies
                var bingoPlayers = await _webAPIDataService.GetAllPagesUsersAsync();
                int numBingoPlayersInserted = await _asyncConnection.InsertAllAsync(bingoPlayers.Select(x => x.ToModelData()).ToList());
                _log.Debug($"Inserted {numBingoPlayersInserted} bingo player records", LogMessageType.Instance.Info_Synchronization);

                //we only want meetings for which our user is involved
                var meetingAndAttendeesPageData = await _webAPIDataService.GetMeetingsAndAttendeesByUserId(userId, null, null);
                if (meetingAndAttendeesPageData.IsSuccessStatusCode)
				{
					List<DTO.BB.Meeting> meetingsAndAttendees = meetingAndAttendeesPageData.Data.Data;

                    //not inserting children here, only meetings
                    int numMeetingsInserted = await _asyncConnection.InsertAllAsync(meetingsAndAttendees.Select(x => x.ToModelData()).ToList());
                    _log.Debug($"Inserted {numMeetingsInserted} meeting records", LogMessageType.Instance.Info_Synchronization);

                    //inserting meeting attendees
                    List<DTO.BB.MeetingAttendee> attendeesDTO = meetingsAndAttendees.SelectMany(x => x.MeetingAttendees).Distinct().ToList();
                    int numMeetingAttendeesInserted = await _asyncConnection.InsertAllAsync(attendeesDTO.Select(v => v.ToModelData()).Distinct().ToList());
                    _log.Debug($"Inserted {numMeetingAttendeesInserted} meeting attendee records", LogMessageType.Instance.Info_Synchronization);

                    //insert or update the users
                    List<DTO.BB.User> usersDTO = attendeesDTO.Select(x => x.User_UserId).Distinct(new ModelData.Extensions.UserSameUser()).ToList();
                    var usersModelData = usersDTO.Select(x => x.ToModelData()).ToList();
                    int numUsersInserted = 0;
                    foreach (var u in usersModelData)
                    {
                        if (1 == await _asyncConnection.InsertOrReplaceAsync(u)) numUsersInserted++;
                    }
                    _log.Debug($"Inserted {numUsersInserted} user records", LogMessageType.Instance.Info_Synchronization);

                    //insert or update the instances
                    var instances = meetingsAndAttendees.SelectMany(x => x.BingoInstances).ToList();
                    int numInstancesInserted = await _asyncConnection.InsertAllAsync(instances.Select(x => x.ToModelData()).ToList());
                    _log.Debug($"Inserted {numInstancesInserted} instance records", LogMessageType.Instance.Info_Synchronization);

                    foreach (var instance in instances)
                    {
                        IList<IFilterCriterion> filterCriteria = new List<IFilterCriterion>() {
                            new FilterCriterion()
                            {
                                FieldName = nameof(DTO.BB.BingoInstanceContent.BingoInstanceId),
                                FieldType = "Guid",
                                FilterOperator = cghConstants.OPERATOR_ISEQUALTO,
                                Value = instance.BingoInstanceId
                            }
                        };

                        PageDataRequest pdr = new PageDataRequest(filterCriteria);

                        var instanceContentPageData = await _webAPIDataService.GetBingoInstanceContentsAsync(pdr);
                        if (instanceContentPageData.IsSuccessStatusCode)
                        {
                            //bingo instance contents
                            var bingoInstanceContents = instanceContentPageData.Data.Data;
                            int numBingoInstanceContentsInserted = await _asyncConnection.InsertAllAsync(bingoInstanceContents.Select(x => x.ToModelData()).ToList());
                            _log.Debug($"Inserted {numBingoInstanceContentsInserted} bingo instance contents records", LogMessageType.Instance.Info_Synchronization);

                            //bingo instance events
                            var bingoInstanceEvents = bingoInstanceContents.SelectMany(x => x.BingoInstanceEvents).Distinct().ToList();
                            int numBingoInstanceEventsInserted = await _asyncConnection.InsertAllAsync(bingoInstanceEvents.Select(x => x.ToModelData()).ToList());
                            _log.Debug($"Inserted {numBingoInstanceEventsInserted} bingo instance event records", LogMessageType.Instance.Info_Synchronization);
                        }
                    }
                }
                
                //just load all companies for right now... might not even need these at all
                var bingoCompanies = await _webAPIDataService.GetAllPagesCompaniesAsync(null);
                int numBingoCompaniesInserted = await _asyncConnection.InsertAllAsync(bingoCompanies.Select(x => x.ToModelData()).ToList());
                _log.Debug($"Inserted {numBingoCompaniesInserted} bingo company records", LogMessageType.Instance.Info_Synchronization);
                
				/*var meetingSchedules = await _webAPIDataService.GetAllPagesMeetingSchedulesAsync(null);
				int numMeetingSchedulesInserted = await _asyncConnection.InsertAllAsync(meetingSchedules.Select(x => x.ToModelData()).ToList());
				_log.Debug($"Inserted {numMeetingSchedulesInserted} meeting schedule records", LogMessageType.Instance.Info_Synchronization);
                
				var notificationRules = await _webAPIDataService.GetAllPagesNotificationRulesAsync();
				int numNotificationRulesInserted = await _asyncConnection.InsertAllAsync(notificationRules.Select(x => x.ToModelData()).ToList());
				_log.Debug($"Inserted {numNotificationRulesInserted} notification rule records", LogMessageType.Instance.Info_Synchronization);

				var recurrenceRules = await _webAPIDataService.GetAllPagesRecurrenceRulesAsync();
				int numRecurrenceRulesInserted = await _asyncConnection.InsertAllAsync(recurrenceRules.Select(x => x.ToModelData()).ToList());
				_log.Debug($"Inserted {numRecurrenceRulesInserted} recurrence rule records", LogMessageType.Instance.Info_Synchronization);
                */
				
			}
			catch (Exception ex)
			{
				_log.Error("DataLoadService: Loading Error", LogMessageType.Instance.Exception_Synchronization, ex: ex);
			}
		}
	}
}