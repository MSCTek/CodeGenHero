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
        }

        public async Task InsertAllDataCleanLocalDB()
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


                //we only want meetings for which our user is involved

                //var userEmail = ((App)Application.Current).CurrentUserEmail;
                //use the email to get the user 

                // TODO:  Pick it up here.
                Guid userId = new Guid("9F3441F1-625C-439E-96EB-19EC41076408");

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
                    var usersModeData = usersDTO.Select(x => x.ToModelData()).ToList();
                    int numUsersInserted = 0;
                    foreach (var u in usersModeData)
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