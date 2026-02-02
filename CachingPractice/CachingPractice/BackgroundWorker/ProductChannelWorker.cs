
using CachingPractice.BackgroundWorker.Channels;

namespace CachingPractice.BackgroundWorker
{
    public class ProductChannelWorker : BackgroundService
    {
        private readonly IProductChannelModification _productChannel;

        public ProductChannelWorker(IProductChannelModification productChannel)
        {
            _productChannel = productChannel;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var taskItem = await _productChannel.DequeueAsync(stoppingToken);
                await taskItem(stoppingToken);
            }
        }
    }
}
