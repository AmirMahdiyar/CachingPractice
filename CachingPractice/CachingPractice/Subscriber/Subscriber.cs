using CachingPractice.Publisher;
using MassTransit;
using Message;
using Microsoft.Extensions.Caching.Memory;

namespace CachingPractice.Subscriber
{
    public class Subscriber : IConsumer<ObjectKey>
    {
        private readonly IMemoryCache _cache;

        public Subscriber(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task Consume(ConsumeContext<ObjectKey> context)
        {
            var message = context.Message;
            if (message.InstanceId == InstanceInfo.InstanceId)
                await Task.CompletedTask;
            if (!_cache.TryGetValue<ObjectKey>(message.InstanceId, out var x))
                await Task.CompletedTask;
            _cache.Remove(x!.Key);
        }
    }
}
