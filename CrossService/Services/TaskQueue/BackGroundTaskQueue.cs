using System;
using System.Collections.Concurrent;

namespace CrossService
{
    internal class BackGroundTaskQueue : IBackGroundTaskQueue
    {
        private ConcurrentQueue<Func<CancellationToken, Task>> workItems = new ConcurrentQueue<Func<CancellationToken, Task>>();
        private SemaphoreSlim signal = new SemaphoreSlim(0);
        public int Size { get { return workItems.Count; } }

        public async Task<Func<CancellationToken, Task>> DequeueAsunc(CancellationToken cancellationToken)
        {
            await signal.WaitAsync(cancellationToken);
            workItems.TryDequeue(out var workitem);    
            return workitem;
        }

        public void QueueBackGroundWorkItem(Func<CancellationToken, Task> workItem)
        {
            if (workItem == null)
                throw new ArgumentNullException(nameof(workItem));
            workItems.Enqueue(workItem);
            signal.Release();
        }
    }
}
