namespace CachingPractice.Publisher
{
    public interface ISendEvent
    {
        Task SendAsync<T>(T message, CancellationToken ct) where T : class;
    }
}
