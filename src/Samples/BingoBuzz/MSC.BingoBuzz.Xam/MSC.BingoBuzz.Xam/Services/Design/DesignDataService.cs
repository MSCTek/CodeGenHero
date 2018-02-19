using CodeGenHero.EAMVCXamPOCO.Interface;
using MSC.BingoBuzz.DTO.BB;
using MSC.BingoBuzz.Xam.Interfaces;
using MSC.BingoBuzz.Xam.ModelData.DemoBB;
using MSC.BingoBuzz.Xam.ModelObj.BB;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSC.BingoBuzz.Xam.Services.Design
{
    public class DesignDataService : IDataService
    {
        private SQLiteAsyncConnection _asyncConnection;
        private SQLiteConnection _connection;
        private IDatabase _database;
        private ILoggingService _log;

        public DesignDataService(ILoggingService log, IDatabase database)
        {
            _log = log;
            _database = database;
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
            //var bingoContents = ;
            //int numBingoContentsInserted = await _asyncConnection.InsertAllAsync(bingoContents.Select(x => x.ToModelData()).ToList());
            //_log.Debug($"Inserted {numBingoContentsInserted} bingo contents records", CodeGenHero.EAMVCXamPOCO.Enums.LogMessageType.Info_Synchronization);

            //var bingoInstanceContents = _webAPIDataService.GetAllPagesBingoInstanceContentsAsync(null);
            //int numBingoInstanceContentsInserted = await _asyncConnection.InsertAllAsync(bingoContents.Select(x => x.ToModelData()).ToList());
            //_log.Debug($"Inserted {numBingoInstanceContentsInserted} bingo instance contents records", CodeGenHero.EAMVCXamPOCO.Enums.LogMessageType.Info_Synchronization);

            //var bingoInstanceEvents = _webAPIDataService.GetAllPagesBingoInstanceEventsAsync(null);
            //int numBingoInstanceEventsInserted = await _asyncConnection.InsertAllAsync(bingoContents.Select(x => x.ToModelData()).ToList());
            //_log.Debug($"Inserted {numBingoInstanceEventsInserted} bingo instance events records", CodeGenHero.EAMVCXamPOCO.Enums.LogMessageType.Info_Synchronization);

            //var bingoInstanceEventTypes = _webAPIDataService.GetAllPagesBingoInstanceEventTypesAsync();
            //int numBingoInstanceEventTypesInserted = await _asyncConnection.InsertAllAsync(bingoContents.Select(x => x.ToModelData()).ToList());
            //_log.Debug($"Inserted {numBingoInstanceEventTypesInserted} bingo instance event type records", CodeGenHero.EAMVCXamPOCO.Enums.LogMessageType.Info_Synchronization);

            //var bingoInstances = _webAPIDataService.GetAllPagesBingoInstancesAsync(null);
            //int numBingoInstancesInserted = await _asyncConnection.InsertAllAsync(bingoContents.Select(x => x.ToModelData()).ToList());
            //_log.Debug($"Inserted {numBingoInstancesInserted} bingo instance records", CodeGenHero.EAMVCXamPOCO.Enums.LogMessageType.Info_Synchronization);

            var companies = new List<ModelData.BB.Company>() { DemoCompany.SampleCompanyUSA };
            int numCompaniesInserted = await _asyncConnection.InsertAllAsync(companies);
            _log.Debug($"Inserted {numCompaniesInserted}  company records", CodeGenHero.EAMVCXamPOCO.Enums.LogMessageType.Info_Synchronization);

            //var bingoFrequencyTypes = _webAPIDataService.GetAllPagesFrequencyTypesAsync();
            //int numBingoFrequencyTypesInserted = await _asyncConnection.InsertAllAsync(bingoContents.Select(x => x.ToModelData()).ToList());
            //_log.Debug($"Inserted {numBingoFrequencyTypesInserted} bingo frequency type records", CodeGenHero.EAMVCXamPOCO.Enums.LogMessageType.Info_Synchronization);

            var meetingAttendees = new List<ModelData.BB.MeetingAttendee>() { DemoMeetingAttendee.SampleMeetingAttendeeAlexanderMockupReview, DemoMeetingAttendee.SampleMeetingAttendeeAlexanderSprintPlanning, DemoMeetingAttendee.SampleMeetingAttendeeAlexanderSprintReview,
                DemoMeetingAttendee.SampleMeetingAttendeeGeorgeSprintPlanning, DemoMeetingAttendee.SampleMeetingAttendeeGeorgeSprintReview,
                DemoMeetingAttendee.SampleMeetingAttendeeThomasMockupReview, DemoMeetingAttendee.SampleMeetingAttendeeThomasSprintPlanning, DemoMeetingAttendee.SampleMeetingAttendeeThomasSprintReview };
            int numMeetingAttendeesInserted = await _asyncConnection.InsertAllAsync(meetingAttendees);
            _log.Debug($"Inserted {numMeetingAttendeesInserted} meeting attendee records", CodeGenHero.EAMVCXamPOCO.Enums.LogMessageType.Info_Synchronization);

            var meetings = new List<ModelData.BB.Meeting>() { DemoMeeting.SampleMeetingMockupReview, DemoMeeting.SampleMeetingSprintPlanning, DemoMeeting.SampleMeetingSprintReview };
            int numMeetingsInserted = await _asyncConnection.InsertAllAsync(meetings);
            _log.Debug($"Inserted {numMeetingsInserted} meeting records", CodeGenHero.EAMVCXamPOCO.Enums.LogMessageType.Info_Synchronization);

            var meetingSchedules = new List<ModelData.BB.MeetingSchedule>() { DemoMeetingSchedule.SampleMeetingSchedule3DaysAway9am, DemoMeetingSchedule.SampleMeetingSchedule5DaysAway11am, DemoMeetingSchedule.SampleMeetingSchedule5DaysAway1pm };
            int numMeetingSchedulesInserted = await _asyncConnection.InsertAllAsync(meetingSchedules);
            _log.Debug($"Inserted {numMeetingSchedulesInserted} meeting schedule records", CodeGenHero.EAMVCXamPOCO.Enums.LogMessageType.Info_Synchronization);

            //var notificationMethodTypes = _webAPIDataService.GetAllPagesNotificationMethodTypesAsync();
            //int numNotificationMethodTypesInserted = await _asyncConnection.InsertAllAsync(bingoContents.Select(x => x.ToModelData()).ToList());
            //_log.Debug($"Inserted {numNotificationMethodTypesInserted} notification method type records", CodeGenHero.EAMVCXamPOCO.Enums.LogMessageType.Info_Synchronization);

            //var notificationRules = _webAPIDataService.GetAllPagesNotificationRulesAsync();
            //int numNotificationRulesInserted = await _asyncConnection.InsertAllAsync(bingoContents.Select(x => x.ToModelData()).ToList());
            //_log.Debug($"Inserted {numNotificationRulesInserted} notification rule records", CodeGenHero.EAMVCXamPOCO.Enums.LogMessageType.Info_Synchronization);

            //var recurrenceRules = _webAPIDataService.GetAllPagesRecurrenceRulesAsync();
            //int numRecurrenceRulesInserted = await _asyncConnection.InsertAllAsync(bingoContents.Select(x => x.ToModelData()).ToList());
            //_log.Debug($"Inserted {numRecurrenceRulesInserted} recurrence rule records", CodeGenHero.EAMVCXamPOCO.Enums.LogMessageType.Info_Synchronization);

            var users = new List<ModelData.BB.User>() { DemoUser.UserAlexander, DemoUser.UserGeorge, DemoUser.UserThomas };
            int numUsersInserted = await _asyncConnection.InsertAllAsync(users);
            _log.Debug($"Inserted {numUsersInserted} user records", CodeGenHero.EAMVCXamPOCO.Enums.LogMessageType.Info_Synchronization);
        }
    }
}