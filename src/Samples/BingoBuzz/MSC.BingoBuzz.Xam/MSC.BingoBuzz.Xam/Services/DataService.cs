using CodeGenHero.EAMVCXamPOCO.DataService.Interface;
using CodeGenHero.EAMVCXamPOCO.Interface;
using Microsoft.AppCenter.Analytics;
using MSC.BingoBuzz.API.Client;
using MSC.BingoBuzz.API.Client.Interface;
using MSC.BingoBuzz.Constants;
using MSC.BingoBuzz.Xam.Interfaces;
using MSC.BingoBuzz.Xam.ModelObj.BB;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CodeGenHero.EAMVCXamPOCO.DataService.Constants.Enums;

namespace MSC.BingoBuzz.Xam.Services
{
    public class DataService : IDataService
    {
        private SQLiteAsyncConnection _asyncConnection;
        private SQLiteConnection _connection;
        private IDatabase _database;
        private ILoggingService _log;
        private IWebApiDataServiceBB _webAPIDataService;

        public DataService(ILoggingService log, IDatabase database)
        {
            _log = log;
            _database = database;
            IWebApiExecutionContext context = new WebApiExecutionContext()
            {
                BaseWebApiUrl = Consts.BaseWebApiUrl,
                ExecutionContextType = WebApiExecutionContextType.Base,
                BaseFileUrl = Consts.BaseFileUrl,
                ConnectionIdentifier = Consts.ConnectionIdentifier
            };
            _webAPIDataService = new WebApiDataServiceBB(new LoggingService(), context);
            _asyncConnection = _database.GetAsyncConnection();
            _connection = _database.GetConnection();
        }

        public async Task<List<ModelObj.BB.Meeting>> GetCurrentFutureMeetingsAsync()
        {
            //returns meetings with schedules, but no attendees
            List<ModelObj.BB.Meeting> returnMe = new List<ModelObj.BB.Meeting>();
            DateTime today = DateTime.Now.Date;
            var dataMeetingSchedules = (await _asyncConnection.Table<ModelData.BB.MeetingSchedule>().Where(x => x.StartDate > today && x.IsDeleted == false).OrderBy(x => x.StartDate).ToListAsync());
            foreach (var r in dataMeetingSchedules)
            {
                var dataMeeting = await _asyncConnection.Table<ModelData.BB.Meeting>().Where(x => x.MeetingId == r.MeetingId).FirstOrDefaultAsync();
                if (dataMeeting != null)
                {
                    var objMeeting = dataMeeting.ToModelObj();
                    objMeeting.MeetingSchedules.Add(r.ToModelObj());
                    returnMe.Add(objMeeting);
                }
            }
            return returnMe;
        }

        public async Task InsertAllDataCleanLocalDB()
        {
            var bingoContents = await _webAPIDataService.GetAllPagesBingoContentsAsync(null);
            int numBingoContentsInserted = await _asyncConnection.InsertAllAsync(bingoContents.Select(x => x.ToModelData()).ToList());
            _log.Debug($"Inserted {numBingoContentsInserted} bingo contents records", CodeGenHero.EAMVCXamPOCO.Enums.LogMessageType.Info_Synchronization);

            var bingoInstanceContents = _webAPIDataService.GetAllPagesBingoInstanceContentsAsync(null);
            int numBingoInstanceContentsInserted = await _asyncConnection.InsertAllAsync(bingoContents.Select(x => x.ToModelData()).ToList());
            _log.Debug($"Inserted {numBingoInstanceContentsInserted} bingo instance contents records", CodeGenHero.EAMVCXamPOCO.Enums.LogMessageType.Info_Synchronization);

            var bingoInstanceEvents = _webAPIDataService.GetAllPagesBingoInstanceEventsAsync(null);
            int numBingoInstanceEventsInserted = await _asyncConnection.InsertAllAsync(bingoContents.Select(x => x.ToModelData()).ToList());
            _log.Debug($"Inserted {numBingoInstanceEventsInserted} bingo instance events records", CodeGenHero.EAMVCXamPOCO.Enums.LogMessageType.Info_Synchronization);

            var bingoInstanceEventTypes = _webAPIDataService.GetAllPagesBingoInstanceEventTypesAsync();
            int numBingoInstanceEventTypesInserted = await _asyncConnection.InsertAllAsync(bingoContents.Select(x => x.ToModelData()).ToList());
            _log.Debug($"Inserted {numBingoInstanceEventTypesInserted} bingo instance event type records", CodeGenHero.EAMVCXamPOCO.Enums.LogMessageType.Info_Synchronization);

            var bingoInstances = _webAPIDataService.GetAllPagesBingoInstancesAsync(null);
            int numBingoInstancesInserted = await _asyncConnection.InsertAllAsync(bingoContents.Select(x => x.ToModelData()).ToList());
            _log.Debug($"Inserted {numBingoInstancesInserted} bingo instance records", CodeGenHero.EAMVCXamPOCO.Enums.LogMessageType.Info_Synchronization);

            var bingoCompanies = _webAPIDataService.GetAllPagesCompaniesAsync(null);
            int numBingoCompaniesInserted = await _asyncConnection.InsertAllAsync(bingoContents.Select(x => x.ToModelData()).ToList());
            _log.Debug($"Inserted {numBingoCompaniesInserted} bingo company records", CodeGenHero.EAMVCXamPOCO.Enums.LogMessageType.Info_Synchronization);

            var bingoFrequencyTypes = _webAPIDataService.GetAllPagesFrequencyTypesAsync();
            int numBingoFrequencyTypesInserted = await _asyncConnection.InsertAllAsync(bingoContents.Select(x => x.ToModelData()).ToList());
            _log.Debug($"Inserted {numBingoFrequencyTypesInserted} bingo frequency type records", CodeGenHero.EAMVCXamPOCO.Enums.LogMessageType.Info_Synchronization);

            var meetingAttendees = _webAPIDataService.GetAllPagesMeetingAttendeesAsync(null);
            int numMeetingAttendeesInserted = await _asyncConnection.InsertAllAsync(bingoContents.Select(x => x.ToModelData()).ToList());
            _log.Debug($"Inserted {numMeetingAttendeesInserted} meeting attendee records", CodeGenHero.EAMVCXamPOCO.Enums.LogMessageType.Info_Synchronization);

            var meetings = _webAPIDataService.GetAllPagesMeetingsAsync(null);
            int numMeetingsInserted = await _asyncConnection.InsertAllAsync(bingoContents.Select(x => x.ToModelData()).ToList());
            _log.Debug($"Inserted {numMeetingsInserted} meeting records", CodeGenHero.EAMVCXamPOCO.Enums.LogMessageType.Info_Synchronization);

            var meetingSchedules = _webAPIDataService.GetAllPagesMeetingSchedulesAsync(null);
            int numMeetingSchedulesInserted = await _asyncConnection.InsertAllAsync(bingoContents.Select(x => x.ToModelData()).ToList());
            _log.Debug($"Inserted {numMeetingSchedulesInserted} meeting schedule records", CodeGenHero.EAMVCXamPOCO.Enums.LogMessageType.Info_Synchronization);

            var notificationMethodTypes = _webAPIDataService.GetAllPagesNotificationMethodTypesAsync();
            int numNotificationMethodTypesInserted = await _asyncConnection.InsertAllAsync(bingoContents.Select(x => x.ToModelData()).ToList());
            _log.Debug($"Inserted {numNotificationMethodTypesInserted} notification method type records", CodeGenHero.EAMVCXamPOCO.Enums.LogMessageType.Info_Synchronization);

            var notificationRules = _webAPIDataService.GetAllPagesNotificationRulesAsync();
            int numNotificationRulesInserted = await _asyncConnection.InsertAllAsync(bingoContents.Select(x => x.ToModelData()).ToList());
            _log.Debug($"Inserted {numNotificationRulesInserted} notification rule records", CodeGenHero.EAMVCXamPOCO.Enums.LogMessageType.Info_Synchronization);

            var recurrenceRules = _webAPIDataService.GetAllPagesRecurrenceRulesAsync();
            int numRecurrenceRulesInserted = await _asyncConnection.InsertAllAsync(bingoContents.Select(x => x.ToModelData()).ToList());
            _log.Debug($"Inserted {numRecurrenceRulesInserted} recurrence rule records", CodeGenHero.EAMVCXamPOCO.Enums.LogMessageType.Info_Synchronization);

            var users = _webAPIDataService.GetAllPagesUsersAsync(null);
            int numUsersInserted = await _asyncConnection.InsertAllAsync(bingoContents.Select(x => x.ToModelData()).ToList());
            _log.Debug($"Inserted {numUsersInserted} user records", CodeGenHero.EAMVCXamPOCO.Enums.LogMessageType.Info_Synchronization);
        }
    }
}