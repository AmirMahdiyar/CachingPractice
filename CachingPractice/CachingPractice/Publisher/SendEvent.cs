
using MassTransit;
using Message;

namespace CachingPractice.Publisher
{
    public class SendEvent : ISendEvent
    {
        private readonly IPublishEndpoint _endPoint;

        public SendEvent(IPublishEndpoint endPoint)
        {
            _endPoint = endPoint;
        }

        public async Task SendAsync(string key, CancellationToken ct)
        {
            await _endPoint.Publish<ObjectKey>(new ObjectKey(key, Guid.NewGuid()), ct);
        }
    }
    public static class InstanceInfo
    {
        public static readonly Guid InstanceId = Guid.NewGuid();
    }
}
