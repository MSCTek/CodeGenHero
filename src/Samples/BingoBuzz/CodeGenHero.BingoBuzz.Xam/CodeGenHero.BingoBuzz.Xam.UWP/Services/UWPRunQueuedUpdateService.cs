using CodeGenHero.BingoBuzz.Xam.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CodeGenHero.BingoBuzz.Xam.UWP.Services
{
    public class UWPRunQueuedUpdateService
    {
        private CancellationTokenSource _cts;

        public async Task StartAsync()
        {
            //TODO: fire this off with a defferral.
            //var deferral = e.SuspendingOperation.GetDeferral();

            _cts = new CancellationTokenSource();

            await DataUploadService.Instance.RunQueuedUpdatesAsync(_cts.Token);

            //deferral.Complete();
        }

        public void Stop()
        {
            _cts.Cancel();
        }

        //COULD ALSO DO IT SOMETHING LIKE THIS... but it would need to be registered.
        //https://docs.microsoft.com/en-us/windows/uwp/launch-resume/create-and-register-a-background-task
        /*public sealed class UWPUploadBackgroundTask : IBackgroundTask
        {
            private CancellationTokenSource _cts;
            private BackgroundTaskDeferral deferral;

            public void Run(IBackgroundTaskInstance taskInstance)
            {
                deferral = taskInstance.GetDeferral();
                _cts = new CancellationTokenSource();

                UploadDataService.Instance.RunQueuedUpdatesAsync(_cts.Token);

                deferral.Complete();
            }

            public void Stop()
            {
                deferral.Complete();
            }
        }*/

        //this is for the UWPUploadBackgroundTask, which is not currently implemented
        /*public bool CheckRegistration()
        {
            var isTaskRegistered = false;
            var exampleTaskName = "UWPUploadBackgroundTask";

            foreach (var task in BackgroundTaskRegistration.AllTasks)
            {
                if (task.Value.Name == exampleTaskName)
                {
                    isTaskRegistered = true;
                    break;
                 }
            }
            return isTaskRegistered;
        }*/
    }
}