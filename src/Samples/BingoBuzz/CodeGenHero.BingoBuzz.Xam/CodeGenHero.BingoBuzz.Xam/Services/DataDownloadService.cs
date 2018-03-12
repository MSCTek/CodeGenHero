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

        public async Task InsertAllDataCleanLocalDB()
        {
            try
            {
                var bingoContents = await _webAPIDataService.GetAllPagesBingoContentsAsync(null);
                int numBingoContentsInserted = await _asyncConnection.InsertAllAsync(bingoContents.Select(x => x.ToModelData()).ToList());
                _log.Debug($"Inserted {numBingoContentsInserted} bingo contents records", LogMessageType.Instance.Info_Synchronization);

                var bingoInstanceContents = await _webAPIDataService.GetAllPagesBingoInstanceContentsAsync(null);
                int numBingoInstanceContentsInserted = await _asyncConnection.InsertAllAsync(bingoInstanceContents.Select(x => x.ToModelData()).ToList());
                _log.Debug($"Inserted {numBingoInstanceContentsInserted} bingo instance contents records", LogMessageType.Instance.Info_Synchronization);

                var bingoInstanceEvents = await _webAPIDataService.GetAllPagesBingoInstanceEventsAsync(null);
                int numBingoInstanceEventsInserted = await _asyncConnection.InsertAllAsync(bingoInstanceEvents.Select(x => x.ToModelData()).ToList());
                _log.Debug($"Inserted {numBingoInstanceEventsInserted} bingo instance events records", LogMessageType.Instance.Info_Synchronization);

                var bingoInstanceEventTypes = await _webAPIDataService.GetAllPagesBingoInstanceEventTypesAsync();
                int numBingoInstanceEventTypesInserted = await _asyncConnection.InsertAllAsync(bingoInstanceEventTypes.Select(x => x.ToModelData()).ToList());
                _log.Debug($"Inserted {numBingoInstanceEventTypesInserted} bingo instance event type records", LogMessageType.Instance.Info_Synchronization);

                var bingoInstances = await _webAPIDataService.GetAllPagesBingoInstancesAsync(null);
                int numBingoInstancesInserted = await _asyncConnection.InsertAllAsync(bingoInstances.Select(x => x.ToModelData()).ToList());
                _log.Debug($"Inserted {numBingoInstancesInserted} bingo instance records", LogMessageType.Instance.Info_Synchronization);

                var bingoCompanies = await _webAPIDataService.GetAllPagesCompaniesAsync(null);
                int numBingoCompaniesInserted = await _asyncConnection.InsertAllAsync(bingoCompanies.Select(x => x.ToModelData()).ToList());
                _log.Debug($"Inserted {numBingoCompaniesInserted} bingo company records", LogMessageType.Instance.Info_Synchronization);

                var bingoFrequencyTypes = await _webAPIDataService.GetAllPagesFrequencyTypesAsync();
                int numBingoFrequencyTypesInserted = await _asyncConnection.InsertAllAsync(bingoFrequencyTypes.Select(x => x.ToModelData()).ToList());
                _log.Debug($"Inserted {numBingoFrequencyTypesInserted} bingo frequency type records", LogMessageType.Instance.Info_Synchronization);

                var meetingAttendees = await _webAPIDataService.GetAllPagesMeetingAttendeesAsync(null);
                int numMeetingAttendeesInserted = await _asyncConnection.InsertAllAsync(meetingAttendees.Select(x => x.ToModelData()).ToList());
                _log.Debug($"Inserted {numMeetingAttendeesInserted} meeting attendee records", LogMessageType.Instance.Info_Synchronization);

                var meetings = await _webAPIDataService.GetAllPagesMeetingsAsync(null);
                int numMeetingsInserted = await _asyncConnection.InsertAllAsync(meetings.Select(x => x.ToModelData()).ToList());
                _log.Debug($"Inserted {numMeetingsInserted} meeting records", LogMessageType.Instance.Info_Synchronization);

                var meetingSchedules = await _webAPIDataService.GetAllPagesMeetingSchedulesAsync(null);
                int numMeetingSchedulesInserted = await _asyncConnection.InsertAllAsync(meetingSchedules.Select(x => x.ToModelData()).ToList());
                _log.Debug($"Inserted {numMeetingSchedulesInserted} meeting schedule records", LogMessageType.Instance.Info_Synchronization);

                var notificationMethodTypes = await _webAPIDataService.GetAllPagesNotificationMethodTypesAsync();
                int numNotificationMethodTypesInserted = await _asyncConnection.InsertAllAsync(notificationMethodTypes.Select(x => x.ToModelData()).ToList());
                _log.Debug($"Inserted {numNotificationMethodTypesInserted} notification method type records", LogMessageType.Instance.Info_Synchronization);

                var notificationRules = await _webAPIDataService.GetAllPagesNotificationRulesAsync();
                int numNotificationRulesInserted = await _asyncConnection.InsertAllAsync(notificationRules.Select(x => x.ToModelData()).ToList());
                _log.Debug($"Inserted {numNotificationRulesInserted} notification rule records", LogMessageType.Instance.Info_Synchronization);

                var recurrenceRules = await _webAPIDataService.GetAllPagesRecurrenceRulesAsync();
                int numRecurrenceRulesInserted = await _asyncConnection.InsertAllAsync(recurrenceRules.Select(x => x.ToModelData()).ToList());
                _log.Debug($"Inserted {numRecurrenceRulesInserted} recurrence rule records", LogMessageType.Instance.Info_Synchronization);

                var users = await _webAPIDataService.GetAllPagesUsersAsync(null);
                int numUsersInserted = await _asyncConnection.InsertAllAsync(users.Select(x => x.ToModelData()).ToList());
                _log.Debug($"Inserted {numUsersInserted} user records", LogMessageType.Instance.Info_Synchronization);
            }
            catch (Exception ex)
            {
                _log.Error("DataLoadService: Loading Error", LogMessageType.Instance.Exception_Synchronization, ex: ex);
            }
        }
    }
}