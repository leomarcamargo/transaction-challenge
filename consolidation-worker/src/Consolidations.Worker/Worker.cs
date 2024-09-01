using System.Text.Json;
using System.Text.Json.Serialization;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Consolidations.Domain.Entities;
using Consolidations.Domain.Repositories;
using Consolidations.Infrastructure.Core.Cache;
using Consolidations.Worker.Messages;
using Consolidations.Worker.Options;
using Microsoft.Extensions.Options;

namespace Consolidations.Worker;

public class Worker(IConsolidatedTransactionRepository consolidatedTransactionRepository,
    IConsumer<string, string> consumer,
    IOptions<KafkaOptions> kafkaOptions,
    ICacheProvider cacheProvider)
    : BackgroundService
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter() }
    };

    private const string TopicName = "app.transactions.created";

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await EnsureTopicExistsAsync();

        consumer.Subscribe(TopicName);

        while (!cancellationToken.IsCancellationRequested)
        {
            var consumeResult = consumer.Consume(cancellationToken);
            var message = JsonSerializer.Deserialize<TransactionCreatedMessage>(consumeResult.Message.Value, _jsonSerializerOptions);

            if (message != null)
                await ConsolidateTransactionAsync(message, cancellationToken);
        }
    }

    private async Task EnsureTopicExistsAsync()
    {
        var adminConfig = new AdminClientConfig
        {
            BootstrapServers = kafkaOptions.Value.BootstrapServers
        };

        using var adminClient = new AdminClientBuilder(adminConfig).Build();
        var metadata = adminClient.GetMetadata(TimeSpan.FromSeconds(10));
        if (metadata.Topics.All(t => t.Topic != TopicName))
        {
            await adminClient.CreateTopicsAsync([
                new TopicSpecification
                {
                    Name = TopicName,
                    NumPartitions = 1,
                    ReplicationFactor = 1
                }
            ]);
        }
    }

    private async Task ConsolidateTransactionAsync(TransactionCreatedMessage message, CancellationToken cancellationToken)
    {
        var transactionValue = message.Type == TransactionType.Credit
            ? message.Total
            : -message.Total;

        var cacheKey = CacheKeyName.GetDailyBalanceKey(message.Date);
        var cacheExpiration = message.Date.Date.AddDays(1).AddTicks(-1) - DateTimeOffset.Now;

        decimal existingDailyBalance;

        if (await cacheProvider.ExistsAsync(cacheKey, cancellationToken))
        {
            existingDailyBalance = await cacheProvider.GetAsync<decimal>(cacheKey, cancellationToken);
        }
        else
        {
            existingDailyBalance = await consolidatedTransactionRepository.GetSumOfTransactionsByDateAsync(message.Date.Date, cancellationToken);
            await cacheProvider.SetAsync(cacheKey, existingDailyBalance, cacheExpiration, cancellationToken);
        }

        var newDailyBalance = existingDailyBalance + transactionValue;

        var consolidatedTransaction = new ConsolidatedTransaction
        {
            TransactionId = message.Id,
            Type = message.Type,
            Total = message.Total,
            Date = message.Date.Date,
            DailyBalance = newDailyBalance,
            Description = message.Description
        };

        await consolidatedTransactionRepository.AddAsync(consolidatedTransaction, cancellationToken);

        await cacheProvider.SetAsync(cacheKey, newDailyBalance, cacheExpiration, cancellationToken);
    }
}