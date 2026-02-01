
using CachingPractice.Db;
using Microsoft.EntityFrameworkCore;

namespace CachingPractice.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext _orderRepository;

        public OrderRepository(OrderDbContext orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public void AddOrder(Order order)
        {
            _orderRepository
                .Order
                .Add(order);

            _orderRepository.SaveChanges();
        }

        public async Task<Order> GetOrderAsync(Guid id, CancellationToken ct)
        {
            return await _orderRepository
                .Order
                .SingleOrDefaultAsync(x => x.Id == id, ct);
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync(CancellationToken cancellationToken)
        {
            return await _orderRepository.Order.ToListAsync(cancellationToken);
        }

        public void RemoveOrder(Order order)
        {
            _orderRepository
                .Order
                .Remove(order);

            _orderRepository.SaveChanges();
        }
    }
}
