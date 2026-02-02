
using MassTransit;

namespace CachingPractice.Publisher
{
    public class SendEvent : ISendEvent
    {
        private readonly IPublishEndpoint _endPoint;
        public SendEvent(IPublishEndpoint endPoint) => _endPoint = endPoint;

        public async Task SendAsync<T>(T message, CancellationToken ct) where T : class
        {
            await _endPoint.Publish(message, ct);
        }
    }
    public static class InstanceInfo
    {
        public static readonly Guid InstanceId = Guid.NewGuid();
    }
}
