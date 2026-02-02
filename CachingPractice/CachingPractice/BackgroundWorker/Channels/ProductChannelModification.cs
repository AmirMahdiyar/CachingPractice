using System.Threading.Channels;

namespace CachingPractice.BackgroundWorker.Channels
{
    public class ProductChannelModification : IProductChannelModification
    {
        private readonly Channel<Func<CancellationToken, ValueTask>> _channel;

        public ProductChannelModification(int capacity)
        {
            var options = new BoundedChannelOptions(capacity) { FullMode = BoundedChannelFullMode.Wait };
            _channel = Channel.CreateBounded<Func<CancellationToken, ValueTask>>(options);

        }

        public async ValueTask AddToBackgroundWorkerQueue(Func<CancellationToken, ValueTask> task)
        {
            if (task is null) throw new ArgumentNullException(nameof(task));
            await _channel.Writer.WriteAsync(task);
        }

        public async ValueTask<Func<CancellationToken, ValueTask>> DequeueAsync(CancellationToken cancellationToken)
        {
            return await _channel.Reader.ReadAsync(cancellationToken);
        }
    }
}
