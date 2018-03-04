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

namespace CodeGenHero.BingoBuzz.Xam.Services
{
    public class DataLoadService : IDataLoadService
    {
        private SQLiteAsyncConnection _asyncConnection;
        private SQLiteConnection _connection;
        private IDatabase _database;
        private ILoggingService _log;
        private IWebApiDataServiceBB _webAPIDataService;

        public DataLoadService(ILoggingService log, IDatabase database)
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

        public async Task InsertAllDataCleanLocalDB()
        {
            var bingoContents = await _webAPIDataService.GetAllPagesBingoContentsAsync(null);
            int numBingoContentsInserted = await _asyncConnection.InsertAllAsync(bingoContents.Select(x => x.ToModelData()).ToList());
            _log.Debug($"Inserted {numBingoContentsInserted} bingo contents records", LogMessageType.Instance.Info_Synchronization);

            var bingoInstanceContents = _webAPIDataService.GetAllPagesBingoInstanceContentsAsync(null);
            int numBingoInstanceContentsInserted = await _asyncConnection.InsertAllAsync(bingoContents.Select(x => x.ToModelData()).ToList());
            _log.Debug($"Inserted {numBingoInstanceContentsInserted} bingo instance contents records", LogMessageType.Instance.Info_Synchronization);

            var bingoInstanceEvents = _webAPIDataService.GetAllPagesBingoInstanceEventsAsync(null);
            int numBingoInstanceEventsInserted = await _asyncConnection.InsertAllAsync(bingoContents.Select(x => x.ToModelData()).ToList());
            _log.Debug($"Inserted {numBingoInstanceEventsInserted} bingo instance events records", LogMessageType.Instance.Info_Synchronization);

            var bingoInstanceEventTypes = _webAPIDataService.GetAllPagesBingoInstanceEventTypesAsync();
            int numBingoInstanceEventTypesInserted = await _asyncConnection.InsertAllAsync(bingoContents.Select(x => x.ToModelData()).ToList());
            _log.Debug($"Inserted {numBingoInstanceEventTypesInserted} bingo instance event type records", LogMessageType.Instance.Info_Synchronization);

            var bingoInstances = _webAPIDataService.GetAllPagesBingoInstancesAsync(null);
            int numBingoInstancesInserted = await _asyncConnection.InsertAllAsync(bingoContents.Select(x => x.ToModelData()).ToList());
            _log.Debug($"Inserted {numBingoInstancesInserted} bingo instance records", LogMessageType.Instance.Info_Synchronization);

            var bingoCompanies = _webAPIDataService.GetAllPagesCompaniesAsync(null);
            int numBingoCompaniesInserted = await _asyncConnection.InsertAllAsync(bingoContents.Select(x => x.ToModelData()).ToList());
            _log.Debug($"Inserted {numBingoCompaniesInserted} bingo company records", LogMessageType.Instance.Info_Synchronization);

            var bingoFrequencyTypes = _webAPIDataService.GetAllPagesFrequencyTypesAsync();
            int numBingoFrequencyTypesInserted = await _asyncConnection.InsertAllAsync(bingoContents.Select(x => x.ToModelData()).ToList());
            _log.Debug($"Inserted {numBingoFrequencyTypesInserted} bingo frequency type records", LogMessageType.Instance.Info_Synchronization);

            var meetingAttendees = _webAPIDataService.GetAllPagesMeetingAttendeesAsync(null);
            int numMeetingAttendeesInserted = await _asyncConnection.InsertAllAsync(bingoContents.Select(x => x.ToModelData()).ToList());
            _log.Debug($"Inserted {numMeetingAttendeesInserted} meeting attendee records", LogMessageType.Instance.Info_Synchronization);

            var meetings = _webAPIDataService.GetAllPagesMeetingsAsync(null);
            int numMeetingsInserted = await _asyncConnection.InsertAllAsync(bingoContents.Select(x => x.ToModelData()).ToList());
            _log.Debug($"Inserted {numMeetingsInserted} meeting records", LogMessageType.Instance.Info_Synchronization);

            var meetingSchedules = _webAPIDataService.GetAllPagesMeetingSchedulesAsync(null);
            int numMeetingSchedulesInserted = await _asyncConnection.InsertAllAsync(bingoContents.Select(x => x.ToModelData()).ToList());
            _log.Debug($"Inserted {numMeetingSchedulesInserted} meeting schedule records", LogMessageType.Instance.Info_Synchronization);

            var notificationMethodTypes = _webAPIDataService.GetAllPagesNotificationMethodTypesAsync();
            int numNotificationMethodTypesInserted = await _asyncConnection.InsertAllAsync(bingoContents.Select(x => x.ToModelData()).ToList());
            _log.Debug($"Inserted {numNotificationMethodTypesInserted} notification method type records", LogMessageType.Instance.Info_Synchronization);

            var notificationRules = _webAPIDataService.GetAllPagesNotificationRulesAsync();
            int numNotificationRulesInserted = await _asyncConnection.InsertAllAsync(bingoContents.Select(x => x.ToModelData()).ToList());
            _log.Debug($"Inserted {numNotificationRulesInserted} notification rule records", LogMessageType.Instance.Info_Synchronization);

            var recurrenceRules = _webAPIDataService.GetAllPagesRecurrenceRulesAsync();
            int numRecurrenceRulesInserted = await _asyncConnection.InsertAllAsync(bingoContents.Select(x => x.ToModelData()).ToList());
            _log.Debug($"Inserted {numRecurrenceRulesInserted} recurrence rule records", LogMessageType.Instance.Info_Synchronization);

            var users = _webAPIDataService.GetAllPagesUsersAsync(null);
            int numUsersInserted = await _asyncConnection.InsertAllAsync(bingoContents.Select(x => x.ToModelData()).ToList());
            _log.Debug($"Inserted {numUsersInserted} user records", LogMessageType.Instance.Info_Synchronization);
        }
    }
}