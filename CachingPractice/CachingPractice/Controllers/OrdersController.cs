using CachingPractice.BackgroundWorker.Channels;
using CachingPractice.Publisher;
using CachingPractice.Repository;
using Message;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Caching.Memory;

namespace CachingPractice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly HybridCache _cache;
        private readonly IOrderRepository _db;
        private readonly ISendEvent _eventSender;
        private readonly IDistributedCache _distributedCache;
        private readonly IProductChannelModification _channelModification;
        private readonly IServiceScopeFactory _scopeFactory;
        public OrdersController(HybridCache cache, IOrderRepository db, ISendEvent eventSender, IDistributedCache distributedCache, IProductChannelModification channelModification, IServiceScopeFactory scopeFactory)
        {
            _cache = cache;
            _db = db;
            _eventSender = eventSender;
            _distributedCache = distributedCache;
            _channelModification = channelModification;
            _scopeFactory = scopeFactory;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(Order order, CancellationToken ct)
        {
            _db.AddOrder(order);
            await Task.CompletedTask;
            await _cache.SetAsync($"product:{order.Id}", order);
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> GetOrders(CancellationToken ct)
        {
            var result = await _cache.GetOrCreateAsync("product", async x =>
            {
                Console.WriteLine("raftam db ha !!!");
                return await _db.GetOrdersAsync(ct);
            });
            return Ok(result);
        }
        [HttpGet("Order/{id}")]
        public async Task<IActionResult> GetOrder([FromRoute] Guid id, CancellationToken ct)
        {
            var response = await _cache.GetOrCreateAsync<Order>
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
        public async Task<IActionResult> DeleteOrder([FromRoute] Guid id, CancellationToken ct2)
        {
            var order = await _db.GetOrderAsync(id, ct2);
            _db.RemoveOrder(order);
            await _eventSender.SendAsync<ObjectDeleted>(new ObjectDeleted($"product:{id}", InstanceInfo.InstanceId), ct2);
            var task = _channelModification.AddToBackgroundWorkerQueue(async (ct) =>
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var scopedDb = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                    var scopedRedis = scope.ServiceProvider.GetRequiredService<IDistributedCache>();
                    var scopedMemory = scope.ServiceProvider.GetRequiredService<IMemoryCache>();

                    scopedMemory.Remove("product");
                    await scopedRedis.RemoveAsync("product", ct);
                    var result = await scopedDb.GetOrdersAsync(ct);
                    await _cache.SetAsync("product", result);

                    Console.WriteLine("Task Executed safely with a new scope!");
                }
            });
            await task;
            return Ok();
        }
    }
}
