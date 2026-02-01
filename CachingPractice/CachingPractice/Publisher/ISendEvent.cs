namespace CachingPractice.Publisher
{
    public interface ISendEvent
    {
        Task SendAsync(string key, CancellationToken ct);
    }
}
