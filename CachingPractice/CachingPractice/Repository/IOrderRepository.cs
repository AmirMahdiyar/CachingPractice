namespace CachingPractice.Repository
{
    public interface IOrderRepository
    {
        void AddOrder(Order order);
        void RemoveOrder(Order order);
        Task<IEnumerable<Order>> GetOrdersAsync(CancellationToken cancellationToken);
        Task<Order> GetOrderAsync(Guid id, CancellationToken ct);
    }
}
