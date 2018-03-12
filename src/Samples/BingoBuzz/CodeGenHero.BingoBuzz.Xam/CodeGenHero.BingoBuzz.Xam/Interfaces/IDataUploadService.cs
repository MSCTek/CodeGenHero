using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CodeGenHero.BingoBuzz.Xam.Interfaces
{
    public interface IDataUploadService
    {
        Task QueueAsync(Guid recordId, Constants.Enums.QueueableObjects objName);

        Task RunQueuedUpdatesAsync(CancellationToken cts);

        void StartSafeQueuedUpdates();
    }
}