using CodeGenHero.BingoBuzz.DTO.BB;
using CodeGenHero.BingoBuzz.Xam.Interfaces;
using CodeGenHero.BingoBuzz.Xam.ModelData.DemoBB;
using CodeGenHero.BingoBuzz.Xam.ModelObj.BB;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CodeGenHero.Logging;

namespace CodeGenHero.BingoBuzz.Xam.Services.Design
{
    public class DesignDataLoadService : IDataLoadService
    {
        private SQLiteAsyncConnection _asyncConnection;
        private SQLiteConnection _connection;
        private IDatabase _database;
        private ILoggingService _log;

        public DesignDataLoadService(ILoggingService log, IDatabase database)
        {
            _log = log;
            _database = database;
            _asyncConnection = _database.GetAsyncConnection();
            _connection = _database.GetConnection();
        }

        public async Task InsertAllDataCleanLocalDB()
        {
            var bingoContents = new List<ModelData.BB.BingoContent>() { DemoBingoContent.SampleBingoContent01, DemoBingoContent.SampleBingoContent02, DemoBingoContent.SampleBingoContent03, DemoBingoContent.SampleBingoContent04, DemoBingoContent.SampleBingoContent05, DemoBingoContent.SampleBingoContent06, DemoBingoContent.SampleBingoContent07, DemoBingoContent.SampleBingoContent08, DemoBingoContent.SampleBingoContent09, DemoBingoContent.SampleBingoContent10,
                                                                                                             DemoBingoContent.SampleBingoContent11, DemoBingoContent.SampleBingoContent12, DemoBingoContent.SampleBingoContent13, DemoBingoContent.SampleBingoContent14, DemoBingoContent.SampleBingoContent15, DemoBingoContent.SampleBingoContent16, DemoBingoContent.SampleBingoContent17, DemoBingoContent.SampleBingoContent18, DemoBingoContent.SampleBingoContent19, DemoBingoContent.SampleBingoContent20,
                                                                                                            DemoBingoContent.SampleBingoContent21, DemoBingoContent.SampleBingoContent22, DemoBingoContent.SampleBingoContent23, DemoBingoContent.SampleBingoContent24, DemoBingoContent.SampleBingoContent25 };
            int numBingoContentsInserted = await _asyncConnection.InsertAllAsync(bingoContents);
            _log.Debug($"Inserted {numBingoContentsInserted} bingo contents records", LogMessageType.Instance.Info_Synchronization);

            /*var bingoInstanceContents = new List<ModelData.BB.BingoInstanceContent>() { DemoBingoInstanceContents.SampleBingoInstanceContent01_01, DemoBingoInstanceContents.SampleBingoInstanceContent02_01, DemoBingoInstanceContents.SampleBingoInstanceContent03_01, DemoBingoInstanceContents.SampleBingoInstanceContent04_01, DemoBingoInstanceContents.SampleBingoInstanceContent05_01, DemoBingoInstanceContents.SampleBingoInstanceContent06_01, DemoBingoInstanceContents.SampleBingoInstanceContent07_01, DemoBingoInstanceContents.SampleBingoInstanceContent08_01, DemoBingoInstanceContents.SampleBingoInstanceContent09_01, DemoBingoInstanceContents.SampleBingoInstanceContent10_01,
                                                                                                                                      DemoBingoInstanceContents.SampleBingoInstanceContent11_01, DemoBingoInstanceContents.SampleBingoInstanceContent12_01, DemoBingoInstanceContents.SampleBingoInstanceContent13_01, DemoBingoInstanceContents.SampleBingoInstanceContent14_01, DemoBingoInstanceContents.SampleBingoInstanceContent15_01, DemoBingoInstanceContents.SampleBingoInstanceContent16_01, DemoBingoInstanceContents.SampleBingoInstanceContent17_01, DemoBingoInstanceContents.SampleBingoInstanceContent18_01, DemoBingoInstanceContents.SampleBingoInstanceContent19_01, DemoBingoInstanceContents.SampleBingoInstanceContent20_01,
                                                                                                                                      DemoBingoInstanceContents.SampleBingoInstanceContent21_01, DemoBingoInstanceContents.SampleBingoInstanceContent22_01, DemoBingoInstanceContents.SampleBingoInstanceContent23_01, DemoBingoInstanceContents.SampleBingoInstanceContent24_01, DemoBingoInstanceContents.SampleBingoInstanceContent25_01 };
            int numBingoInstanceContentsInserted = await _asyncConnection.InsertAllAsync(bingoInstanceContents);
            _log.Debug($"Inserted {numBingoInstanceContentsInserted} bingo instance contents records", LogMessageType.Instance.Info_Synchronization);

            var bingoInstanceContents2 = new List<ModelData.BB.BingoInstanceContent>() { DemoBingoInstanceContents.SampleBingoInstanceContent01_02, DemoBingoInstanceContents.SampleBingoInstanceContent02_02, DemoBingoInstanceContents.SampleBingoInstanceContent03_02, DemoBingoInstanceContents.SampleBingoInstanceContent04_02, DemoBingoInstanceContents.SampleBingoInstanceContent05_02, DemoBingoInstanceContents.SampleBingoInstanceContent06_02, DemoBingoInstanceContents.SampleBingoInstanceContent07_02, DemoBingoInstanceContents.SampleBingoInstanceContent08_02, DemoBingoInstanceContents.SampleBingoInstanceContent09_02, DemoBingoInstanceContents.SampleBingoInstanceContent10_02,
                                                                                                                                      DemoBingoInstanceContents.SampleBingoInstanceContent11_02, DemoBingoInstanceContents.SampleBingoInstanceContent12_02, DemoBingoInstanceContents.SampleBingoInstanceContent13_02, DemoBingoInstanceContents.SampleBingoInstanceContent14_02, DemoBingoInstanceContents.SampleBingoInstanceContent15_02, DemoBingoInstanceContents.SampleBingoInstanceContent16_02, DemoBingoInstanceContents.SampleBingoInstanceContent17_02, DemoBingoInstanceContents.SampleBingoInstanceContent18_02, DemoBingoInstanceContents.SampleBingoInstanceContent19_02, DemoBingoInstanceContents.SampleBingoInstanceContent20_02,
                                                                                                                                      DemoBingoInstanceContents.SampleBingoInstanceContent21_02, DemoBingoInstanceContents.SampleBingoInstanceContent22_02, DemoBingoInstanceContents.SampleBingoInstanceContent23_02, DemoBingoInstanceContents.SampleBingoInstanceContent24_02, DemoBingoInstanceContents.SampleBingoInstanceContent25_02 };
            int numBingoInstanceContentsInserted2 = await _asyncConnection.InsertAllAsync(bingoInstanceContents2);
            _log.Debug($"Inserted {numBingoInstanceContentsInserted2} bingo instance contents records", LogMessageType.Instance.Info_Synchronization);

            var bingoInstanceContents3 = new List<ModelData.BB.BingoInstanceContent>() { DemoBingoInstanceContents.SampleBingoInstanceContent01_03, DemoBingoInstanceContents.SampleBingoInstanceContent02_03, DemoBingoInstanceContents.SampleBingoInstanceContent03_03, DemoBingoInstanceContents.SampleBingoInstanceContent04_03, DemoBingoInstanceContents.SampleBingoInstanceContent05_03, DemoBingoInstanceContents.SampleBingoInstanceContent06_03, DemoBingoInstanceContents.SampleBingoInstanceContent07_03, DemoBingoInstanceContents.SampleBingoInstanceContent08_03, DemoBingoInstanceContents.SampleBingoInstanceContent09_03, DemoBingoInstanceContents.SampleBingoInstanceContent10_03,
                                                                                                                                      DemoBingoInstanceContents.SampleBingoInstanceContent11_03, DemoBingoInstanceContents.SampleBingoInstanceContent12_03, DemoBingoInstanceContents.SampleBingoInstanceContent13_03, DemoBingoInstanceContents.SampleBingoInstanceContent14_03, DemoBingoInstanceContents.SampleBingoInstanceContent15_03, DemoBingoInstanceContents.SampleBingoInstanceContent16_03, DemoBingoInstanceContents.SampleBingoInstanceContent17_03, DemoBingoInstanceContents.SampleBingoInstanceContent18_03, DemoBingoInstanceContents.SampleBingoInstanceContent19_03, DemoBingoInstanceContents.SampleBingoInstanceContent20_03,
                                                                                                                                      DemoBingoInstanceContents.SampleBingoInstanceContent21_03, DemoBingoInstanceContents.SampleBingoInstanceContent22_03, DemoBingoInstanceContents.SampleBingoInstanceContent23_03, DemoBingoInstanceContents.SampleBingoInstanceContent24_03, DemoBingoInstanceContents.SampleBingoInstanceContent25_03 };
            int numBingoInstanceContentsInserted3 = await _asyncConnection.InsertAllAsync(bingoInstanceContents3);
            _log.Debug($"Inserted {numBingoInstanceContentsInserted3} bingo instance contents records", LogMessageType.Instance.Info_Synchronization);
            */
            //var bingoInstanceEvents = _webAPIDataService.GetAllPagesBingoInstanceEventsAsync(null);
            //int numBingoInstanceEventsInserted = await _asyncConnection.InsertAllAsync(bingoContents.Select(x => x.ToModelData()).ToList());
            //_log.Debug($"Inserted {numBingoInstanceEventsInserted} bingo instance events records", LogMessageType.Instance.Info_Synchronization);

            var bingoInstanceEventTypes = new List<ModelData.BB.BingoInstanceEventType>() { DemoBingoInstanceEventType.SampleAttendeeJoinedEvent, DemoBingoInstanceEventType.SampleBingoEvent, DemoBingoInstanceEventType.SampleContentDisputedEvent,
                                                                                                                                             DemoBingoInstanceEventType.SampleContentTappedEvent, DemoBingoInstanceEventType.SampleGameEndedEvent, DemoBingoInstanceEventType.SampleGameStartedEvent };
            int numBingoInstanceEventTypesInserted = await _asyncConnection.InsertAllAsync(bingoInstanceEventTypes);
            _log.Debug($"Inserted {numBingoInstanceEventTypesInserted} bingo instance event type records", LogMessageType.Instance.Info_Synchronization);

            var bingoInstanceStatusTypes = new List<ModelData.BB.BingoInstanceStatusType>() { DemoBingoInstanceStatusType.SampleAbandonedStatus, DemoBingoInstanceStatusType.SampleActiveStatus, DemoBingoInstanceStatusType.SampleInactiveStatus };
            int numBingoInstanceStatusTypesInserted = await _asyncConnection.InsertAllAsync(bingoInstanceStatusTypes);
            _log.Debug($"Inserted {numBingoInstanceStatusTypesInserted} bingo instance status type records", LogMessageType.Instance.Info_Synchronization);

            /*var bingoInstances = new List<ModelData.BB.BingoInstance>() { DemoBingoInstance.SampleBingoInstanceMockupReview, DemoBingoInstance.SampleBingoInstanceSprintPlanning, DemoBingoInstance.SampleBingoInstanceSprintReview };
            int numBingoInstancesInserted = await _asyncConnection.InsertAllAsync(bingoInstances);
            _log.Debug($"Inserted {numBingoInstancesInserted} bingo instance records", LogMessageType.Instance.Info_Synchronization);
            */
            var companies = new List<ModelData.BB.Company>() { DemoCompany.SampleCompanyUSA };
            int numCompaniesInserted = await _asyncConnection.InsertAllAsync(companies);
            _log.Debug($"Inserted {numCompaniesInserted} company records", LogMessageType.Instance.Info_Synchronization);

            //var bingoFrequencyTypes = _webAPIDataService.GetAllPagesFrequencyTypesAsync();
            //int numBingoFrequencyTypesInserted = await _asyncConnection.InsertAllAsync(bingoContents.Select(x => x.ToModelData()).ToList());
            //_log.Debug($"Inserted {numBingoFrequencyTypesInserted} bingo frequency type records", LogMessageType.Instance.Info_Synchronization);

            var meetingAttendees = new List<ModelData.BB.MeetingAttendee>() { DemoMeetingAttendee.SampleMeetingAttendeeAlexanderMockupReview, DemoMeetingAttendee.SampleMeetingAttendeeAlexanderSprintPlanning, DemoMeetingAttendee.SampleMeetingAttendeeAlexanderSprintReview,
                DemoMeetingAttendee.SampleMeetingAttendeeGeorgeSprintPlanning, DemoMeetingAttendee.SampleMeetingAttendeeGeorgeSprintReview,
                DemoMeetingAttendee.SampleMeetingAttendeeThomasMockupReview, DemoMeetingAttendee.SampleMeetingAttendeeThomasSprintPlanning, DemoMeetingAttendee.SampleMeetingAttendeeThomasSprintReview };
            int numMeetingAttendeesInserted = await _asyncConnection.InsertAllAsync(meetingAttendees);
            _log.Debug($"Inserted {numMeetingAttendeesInserted} meeting attendee records", LogMessageType.Instance.Info_Synchronization);

            var meetings = new List<ModelData.BB.Meeting>() { DemoMeeting.SampleMeetingMockupReview, DemoMeeting.SampleMeetingSprintPlanning, DemoMeeting.SampleMeetingSprintReview };
            int numMeetingsInserted = await _asyncConnection.InsertAllAsync(meetings);
            _log.Debug($"Inserted {numMeetingsInserted} meeting records", LogMessageType.Instance.Info_Synchronization);

            //var meetingSchedules = new List<ModelData.BB.MeetingSchedule>() { DemoMeetingSchedule.SampleMeetingSchedule3DaysAway9am, DemoMeetingSchedule.SampleMeetingSchedule5DaysAway11am, DemoMeetingSchedule.SampleMeetingSchedule5DaysAway1pm };
            //int numMeetingSchedulesInserted = await _asyncConnection.InsertAllAsync(meetingSchedules);
            //_log.Debug($"Inserted {numMeetingSchedulesInserted} meeting schedule records", LogMessageType.Instance.Info_Synchronization);

            //var notificationMethodTypes = _webAPIDataService.GetAllPagesNotificationMethodTypesAsync();
            //int numNotificationMethodTypesInserted = await _asyncConnection.InsertAllAsync(bingoContents.Select(x => x.ToModelData()).ToList());
            //_log.Debug($"Inserted {numNotificationMethodTypesInserted} notification method type records", LogMessageType.Instance.Info_Synchronization);

            //var notificationRules = _webAPIDataService.GetAllPagesNotificationRulesAsync();
            //int numNotificationRulesInserted = await _asyncConnection.InsertAllAsync(bingoContents.Select(x => x.ToModelData()).ToList());
            //_log.Debug($"Inserted {numNotificationRulesInserted} notification rule records", LogMessageType.Instance.Info_Synchronization);

            //var recurrenceRules = _webAPIDataService.GetAllPagesRecurrenceRulesAsync();
            //int numRecurrenceRulesInserted = await _asyncConnection.InsertAllAsync(bingoContents.Select(x => x.ToModelData()).ToList());
            //_log.Debug($"Inserted {numRecurrenceRulesInserted} recurrence rule records", LogMessageType.Instance.Info_Synchronization);

            var users = new List<ModelData.BB.User>() { DemoUser.UserAlexander, DemoUser.UserGeorge, DemoUser.UserThomas };
            int numUsersInserted = await _asyncConnection.InsertAllAsync(users);
            _log.Debug($"Inserted {numUsersInserted} user records", LogMessageType.Instance.Info_Synchronization);
        }
    }
}