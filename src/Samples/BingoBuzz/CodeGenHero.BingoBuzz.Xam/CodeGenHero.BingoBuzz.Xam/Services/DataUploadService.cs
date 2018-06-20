using CodeGenHero.BingoBuzz.API.Client;
using CodeGenHero.BingoBuzz.API.Client.Interface;
using CodeGenHero.BingoBuzz.Constants;
using CodeGenHero.BingoBuzz.Xam.Interfaces;
using CodeGenHero.BingoBuzz.Xam.Messages;
using CodeGenHero.BingoBuzz.Xam.ModelData.DataSync;
using CodeGenHero.DataService;
using CodeGenHero.Logging;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CodeGenHero.BingoBuzz.Xam.Services
{
    public class DataUploadService : IDataUploadService
    {
        private static DataUploadService _instance;
        private SQLiteAsyncConnection _asyncConnection;
        private SQLiteConnection _connection;
        private IDatabase _database;
        private ILoggingService _log;
        private IWebApiDataServiceBB _webAPIDataService;

        public DataUploadService(ILoggingService log, IDatabase database)
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

        public static DataUploadService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = ((App)Application.Current).Kernel.GetService(typeof(DataUploadService)) as DataUploadService;
                }
                return _instance;
            }
        }

        //queue a record in SQLite
        public async Task QueueAsync(Guid recordId, Constants.Enums.QueueableObjects objName)
        {
            try
            {
                Queue queue = new Queue()
                {
                    RecordId = recordId,
                    QueueableObject = objName.ToString(),
                    DateQueued = DateTime.UtcNow,
                    NumAttempts = 0,
                    Success = false
                };

                int count = await _asyncConnection.InsertOrReplaceAsync(queue);

                _log.Debug($"Queued {count} Queue records", LogMessageType.Instance.Info_Synchronization);
            }
            catch (Exception ex)
            {
                _log.Error($"Error in {nameof(QueueAsync)}", logMessageType: LogMessageType.Instance.Exception_Application, ex: ex);
            }
        }

        //run the oldest 10 updates in the SQLite database
        public async Task RunQueuedUpdatesAsync(CancellationToken cts)
        {
            try
            {
                //Take the oldest 10 records off the queue
                var queue = await _asyncConnection.Table<Queue>().Where(x => x.Success == false && x.NumAttempts < 5).OrderBy(s => s.DateQueued).Take(10).ToListAsync();

                _log.Debug($"Running {queue.Count} Queued Updates", LogMessageType.Instance.Info_Diagnostics);

                foreach (var q in queue)
                {
                    //if the system or the user has requested that the process is cancelled, then we need to stop and end gracefully.
                    if (cts.IsCancellationRequested)
                    {
                        break;
                    }

                    if (q.QueueableObject == Constants.Enums.QueueableObjects.BingoInstance.ToString())
                    {
                        if (await RunQueuedBingoInstanceInsert(q))
                        {
                            q.NumAttempts += 1;
                            q.Success = true;
                            await _asyncConnection.UpdateAsync(q);
                        }
                        else
                        {
                            q.NumAttempts += 1;
                            await _asyncConnection.UpdateAsync(q);
                        }
                    }
                    
                    if (q.QueueableObject == Constants.Enums.QueueableObjects.BingoInstanceEvent.ToString())
                    {
                        if (await RunQueuedBingoInstanceEventInsert(q))
                        {
                            q.NumAttempts += 1;
                            q.Success = true;
                            await _asyncConnection.UpdateAsync(q);
                        }
                        else
                        {
                            q.NumAttempts += 1;
                            await _asyncConnection.UpdateAsync(q);
                        }
                    }

                    if (q.QueueableObject == Constants.Enums.QueueableObjects.Meeting.ToString())
                    {
                        if (await RunQueuedMeetingInsert(q))
                        {
                            q.NumAttempts += 1;
                            q.Success = true;
                            await _asyncConnection.UpdateAsync(q);
                        }
                        else
                        {
                            q.NumAttempts += 1;
                            await _asyncConnection.UpdateAsync(q);
                        }
                    }

                    if (q.QueueableObject == Constants.Enums.QueueableObjects.MeetingAttendee.ToString())
                    {
                        if (await RunQueuedMeetingAttendeeInsert(q))
                        {
                            q.NumAttempts += 1;
                            q.Success = true;
                            await _asyncConnection.UpdateAsync(q);
                        }
                        else
                        {
                            q.NumAttempts += 1;
                            await _asyncConnection.UpdateAsync(q);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error($"Error in {nameof(RunQueuedUpdatesAsync)}", logMessageType: LogMessageType.Instance.Exception_Application, ex: ex);
            }
        }

        public void StartSafeQueuedUpdates()
        {
            if (Helpers.DoIHaveInternet) MessagingCenter.Send<StartUploadDataMessage>(new StartUploadDataMessage(), "StartUploadDataMessage");
        }

        private async Task<int> GetCountQueuedRecordsAsync()
        {
            return await _asyncConnection.Table<Queue>().Where(x => x.Success == false).CountAsync();
        }

        private async Task<bool> RunQueuedBingoInstanceInsert(Queue q)
        {
            if (_webAPIDataService == null) { return false; }

            var record = await _asyncConnection.Table<ModelData.BB.BingoInstance>().Where(x => x.BingoInstanceId == q.RecordId).FirstOrDefaultAsync();
            if (record != null)
            {
                //the bingo instance and the instance content need to go to the azure db together as an upload package
                var instanceWithContentDTO = record.ToDto();
                instanceWithContentDTO.BingoInstanceContents = new List<DTO.BB.BingoInstanceContent>(); 

                var contents = await _asyncConnection.Table<ModelData.BB.BingoInstanceContent>().Where(c => c.BingoInstanceId == record.BingoInstanceId).ToListAsync();
                foreach(var con in contents)
                {
                    instanceWithContentDTO.BingoInstanceContents.Add(con.ToDto());
                }
                
                var result = await _webAPIDataService.CreateBingoInstanceWithContentAsync(instanceWithContentDTO);
                if (result.IsSuccessStatusCode)
                {
                    _log.Debug($"Successfully Sent Queued BingoInstance Record with content", LogMessageType.Instance.Info_Synchronization);
                    return true;
                }
                _log.Error($"Error Sending Queued BingoInstance record {q.RecordId} with content", LogMessageType.Instance.Info_Synchronization, ex: result.Exception);
                return false;
            }
            return false;
        }

        private async Task<bool> RunQueuedBingoInstanceContentInsert(Queue q)
        {
            if (_webAPIDataService == null) { return false; }

            var record = await _asyncConnection.Table<ModelData.BB.BingoInstanceContent>().Where(x => x.BingoInstanceContentId == q.RecordId).FirstOrDefaultAsync();
            if (record != null)
            {
                var result = await _webAPIDataService.CreateBingoInstanceContentAsync(record.ToDto());
                if (result.IsSuccessStatusCode)
                {
                    _log.Debug($"Successfully Sent Queued BingoInstanceContent Record", LogMessageType.Instance.Info_Synchronization);
                    return true;
                }
                _log.Error($"Error Sending Queued BingoInstanceContent record {q.RecordId}", LogMessageType.Instance.Info_Synchronization, ex: result.Exception);
                return false;
            }
            return false;
        }

        private async Task<bool> RunQueuedBingoInstanceEventInsert(Queue q)
        {
            if (_webAPIDataService == null) { return false; }

            var record = await _asyncConnection.Table<ModelData.BB.BingoInstanceEvent>().Where(x => x.BingoInstanceEventId == q.RecordId).FirstOrDefaultAsync();
            if (record != null)
            {
                var result = await _webAPIDataService.CreateBingoInstanceEventAsync(record.ToDto());

                if (result.IsSuccessStatusCode)
                {
                    _log.Debug($"Successfully Sent Queued BingoInstanceEvent Record", LogMessageType.Instance.Info_Synchronization);
                    return true;
                }
                _log.Error($"Error Sending Queued BingoInstanceEvent record {q.RecordId}", LogMessageType.Instance.Info_Synchronization, ex: result.Exception);
                return false;
            }
            return false;
        }

        private async Task<bool> RunQueuedMeetingInsert(Queue q)
        {
            if (_webAPIDataService == null) { return false; }

            var record = await _asyncConnection.Table<ModelData.BB.Meeting>().Where(x => x.MeetingId == q.RecordId).FirstOrDefaultAsync();
            if (record != null)
            {
                var result = await _webAPIDataService.CreateMeetingAsync(record.ToDto());

                if (result.IsSuccessStatusCode)
                {
                    _log.Debug($"Successfully Sent Queued Meeting Record", LogMessageType.Instance.Info_Synchronization);
                    return true;
                }
                _log.Error($"Error Sending Queued Meeting record {q.RecordId}", LogMessageType.Instance.Info_Synchronization, ex: result.Exception);
                return false;
            }
            return false;
        }

        private async Task<bool> RunQueuedMeetingAttendeeInsert(Queue q)
        {
            if (_webAPIDataService == null) { return false; }

            var record = await _asyncConnection.Table<ModelData.BB.MeetingAttendee>().Where(x => x.MeetingAttendeeId == q.RecordId).FirstOrDefaultAsync();
            if (record != null)
            {
                var result = await _webAPIDataService.CreateMeetingAttendeeAsync(record.ToDto());

                if (result.IsSuccessStatusCode)
                {
                    _log.Debug($"Successfully Sent Queued Meeting Attendee Record", LogMessageType.Instance.Info_Synchronization);
                    return true;
                }
                _log.Error($"Error Sending Queued Meeting Attendee record {q.RecordId}", LogMessageType.Instance.Info_Synchronization, ex: result.Exception);
                return false;
            }
            return false;
        }
    }
}