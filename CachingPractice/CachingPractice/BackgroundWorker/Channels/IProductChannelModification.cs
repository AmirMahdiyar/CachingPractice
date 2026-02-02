namespace CachingPractice.BackgroundWorker.Channels
{
    public interface IProductChannelModification
    {
        ValueTask AddToBackgroundWorkerQueue(Func<CancellationToken, ValueTask> task);
        ValueTask<Func<CancellationToken, ValueTask>> DequeueAsync(CancellationToken cancellationToken);
    }
}
