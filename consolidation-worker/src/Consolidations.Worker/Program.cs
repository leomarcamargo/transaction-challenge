using Confluent.Kafka;
using Consolidations.Infrastructure.CrossCutting.Redis;
using Consolidations.Infrastructure.Data;
using Consolidations.Infrastructure.Data.Contexts;
using Consolidations.Worker;
using Consolidations.Worker.Options;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<ConsolidationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConsolidationConnection")), 
    ServiceLifetime.Singleton);

builder.Services.AddDataServices(builder.Configuration);
builder.Services.AddRedisServices(builder.Configuration);

var kafkaSettings = builder.Configuration.GetSection("KafkaSettings").Get<KafkaOptions>();
if (kafkaSettings == null || string.IsNullOrEmpty(kafkaSettings.BootstrapServers))
    throw new InvalidOperationException("Kafka settings are not configured");

builder.Services.Configure<KafkaOptions>(builder.Configuration.GetSection("KafkaSettings"));

var consumerConfig = new ConsumerConfig
{
    BootstrapServers = kafkaSettings.BootstrapServers,
    GroupId = kafkaSettings.GroupId,
    AutoOffsetReset = AutoOffsetReset.Earliest,
    AllowAutoCreateTopics = true
};

builder.Services.AddSingleton<IConsumer<string, string>>(sp =>
    new ConsumerBuilder<string, string>(consumerConfig).Build());

builder.Services.AddHostedService<Worker>();

var host = builder.Build();

var environment = host.Services.GetRequiredService<IHostEnvironment>();
if (environment.IsDevelopment())
{
    using var scope = host.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ConsolidationDbContext>();
    db.Database.Migrate();
}

host.Run();