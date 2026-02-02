using CachingPractice.BackgroundWorker;
using CachingPractice.BackgroundWorker.Channels;
using CachingPractice.Db;
using CachingPractice.Publisher;
using CachingPractice.Repository;
using CachingPractice.Subscriber;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;

namespace CachingPractice
{
    public static class ProgramBuilder
    {
        public static WebApplicationBuilder AddCaching(this WebApplicationBuilder builder)
        {
            builder.Services.AddHybridCache(opt =>
            {
                opt.DefaultEntryOptions = new HybridCacheEntryOptions()
                {
                    Expiration = TimeSpan.FromMinutes(2),
                    LocalCacheExpiration = TimeSpan.FromMinutes(2)
                };

            });

            builder.Services.AddStackExchangeRedisCache(opt =>
            {
                opt.InstanceName = "test";
                opt.Configuration = "localhost:6379";
            });

            builder.Services.AddMemoryCache();

            return builder;
        }


        public static WebApplicationBuilder AddMessageBroker(this WebApplicationBuilder builder)
        {

            builder.Services.AddMassTransit(opt =>
            {
                opt.AddConsumer<ObjectDeletedSubscriber>();
                opt.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("localhost", "/");
                    cfg.ConfigureEndpoints(context);
                });

            });
            return builder;
        }

        public static WebApplicationBuilder AddDatabase(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<OrderDbContext>(opt =>
            {
                opt.UseSqlServer("Server=.;Database=CachingPractice;Trusted_Connection=True;TrustServerCertificate=True");
            });
            return builder;
        }


        public static WebApplicationBuilder AddScopedDependencies(this WebApplicationBuilder builder)
        {

            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<ISendEvent, SendEvent>();
            return builder;
        }


        public static WebApplicationBuilder AddSingletonDependencies(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<StackExchange.Redis.IConnectionMultiplexer>(sp =>
            {
                var redisConfiguration = "localhost:6379";
                return StackExchange.Redis.ConnectionMultiplexer.Connect(redisConfiguration);
            });
            builder.Services.AddSingleton<IProductChannelModification>(new ProductChannelModification(100));
            return builder;
        }


        public static WebApplicationBuilder AddHostedServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddHostedService<ProductChannelWorker>();
            return builder;
        }

        public static WebApplicationBuilder AddSwagger(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen();
            return builder;
        }

        public static WebApplicationBuilder AddWebServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
            builder.Services.AddAuthorization();
            return builder;
        }
    }
}
