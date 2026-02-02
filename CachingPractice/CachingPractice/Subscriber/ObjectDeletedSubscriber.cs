using CachingPractice.Publisher;
using MassTransit;
using Message;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace CachingPractice.Subscriber
{
    public class ObjectDeletedSubscriber : IConsumer<ObjectDeleted>
    {
        private readonly IMemoryCache _cache;
        private readonly IDistributedCache _distributedCache;

        public ObjectDeletedSubscriber(IMemoryCache cache, IDistributedCache distributedCache)
        {
            _cache = cache;
            _distributedCache = distributedCache;
        }

        public async Task Consume(ConsumeContext<ObjectDeleted> context)
        {
            var message = context.Message;
            if (message.InstanceId == InstanceInfo.InstanceId)
            {
                return;
            }

            if (!string.IsNullOrEmpty(message.Key))
            {
                _cache.Remove($"{message.Key}");
                await _distributedCache.RemoveAsync($"{message.Key}");
            }
        }
    }
}
