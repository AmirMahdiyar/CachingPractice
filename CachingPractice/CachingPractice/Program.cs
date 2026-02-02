using CachingPractice;

var builder = WebApplication.CreateBuilder(args)
    .AddWebServices()
    .AddCaching()
    .AddDatabase()
    .AddHostedServices()
    .AddMessageBroker()
    .AddScopedDependencies()
    .AddSingletonDependencies()
    .AddSwagger();

// Add services to the container.

//builder.Services.AddControllers();
//builder.Services.AddHybridCache(opt =>
//{
//    opt.DefaultEntryOptions = new HybridCacheEntryOptions()
//    {
//        Expiration = TimeSpan.FromMinutes(2),
//        LocalCacheExpiration = TimeSpan.FromMinutes(2)
//    };

//});

//builder.Services.AddStackExchangeRedisCache(opt =>
//{
//    opt.InstanceName = "test";
//    opt.Configuration = "localhost:6379";
//});

////builder.AddRedisDistributedCache("Redis");
//builder.Services.AddDbContext<OrderDbContext>(opt =>
//{
//    opt.UseSqlServer("Server=.;Database=CachingPractice;Trusted_Connection=True;TrustServerCertificate=True");
//});

//builder.Services.AddMassTransit(opt =>
//{
//    opt.AddConsumer<ObjectDeletedSubscriber>();
//    opt.UsingRabbitMq((context, cfg) =>
//    {
//        cfg.Host("localhost", "/");
//        cfg.ConfigureEndpoints(context);
//    });

//});

//builder.Services.AddScoped<IOrderRepository, OrderRepository>();
//builder.Services.AddScoped<ISendEvent, SendEvent>();
//builder.Services.AddSingleton<StackExchange.Redis.IConnectionMultiplexer>(sp =>
//{
//    var redisConfiguration = "localhost:6379";
//    return StackExchange.Redis.ConnectionMultiplexer.Connect(redisConfiguration);
//});
//builder.Services.AddSingleton<IProductChannelModification>(new ProductChannelModification(100));
//builder.Services.AddHostedService<ProductChannelWorker>();

//builder.Services.AddSwaggerGen();

//builder.Services.AddMemoryCache();
var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
