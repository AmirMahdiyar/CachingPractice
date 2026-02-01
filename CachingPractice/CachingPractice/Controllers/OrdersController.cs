using CachingPractice.Publisher;
using CachingPractice.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Hybrid;

namespace CachingPractice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly HybridCache _cache;
        private readonly IOrderRepository _db;
        private readonly ISendEvent _eventSender;
        public OrdersController(HybridCache cache, IOrderRepository db, ISendEvent eventSender)
        {
            _cache = cache;
            _db = db;
            _eventSender = eventSender;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(Order order, CancellationToken ct)
        {
            _db.AddOrder(order);
            await Task.CompletedTask;
            await _cache.SetAsync($"product:{order.Id}", "salam");
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> GetOrders(CancellationToken ct)
        {
            var response = await _cache.GetOrCreateAsync<IEnumerable<Order>>
                ("products"
                , async x =>
                {
                    return await _db.GetOrdersAsync(ct);
                });
            return Ok(response);
        }
        [HttpGet("Order/{id}")]
        public async Task<IActionResult> GetOrder([FromRoute] Guid id, CancellationToken ct)
        {
            var response = await _cache.GetOrCreateAsync
                ($"product:{id}",
                async x =>
                {
                    Console.WriteLine("We went to Db");
                    return await _db.GetOrderAsync(id, ct);
                }
                );
            return Ok(response);
        }
        [HttpDelete("Order/{id}")]
        public async Task<IActionResult> DeleteOrder([FromRoute] Guid id, CancellationToken ct)
        {
            var order = await _db.GetOrderAsync(id, ct);
            _db.RemoveOrder(order);
            await _eventSender.SendAsync($"testproduct:{id}", ct);
            return Ok();
        }
    }
}
