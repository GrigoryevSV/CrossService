
namespace CrossService
{
    internal interface IBackGroundTaskQueue
    {
        int Size { get; }
        void QueueBackGroundWorkItem(Func<CancellationToken, Task> workItem);
        Task<Func<CancellationToken, Task>> DequeueAsunc(CancellationToken cancellationToken);
    }
}
