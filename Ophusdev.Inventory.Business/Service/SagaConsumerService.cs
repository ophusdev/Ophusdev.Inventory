using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ophusdev.Inventory.Business.Abstraction;
using Ophusdev.Inventory.Shared;
using Ophusdev.Kafka.Abstraction;

namespace Ophusdev.Orchestrator.Business.Services
{
    public class SagaConsumerService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IKafkaConsumer _kafkaConsumer;

        public SagaConsumerService(IServiceProvider serviceProvider, IKafkaConsumer kafkaConsumer)
        {
            _serviceProvider = serviceProvider;
            _kafkaConsumer = kafkaConsumer;
        }

        private async Task HandleMessageAsync(string topic, string message)
        {
            using var scope = _serviceProvider.CreateScope();

            switch (topic)
            {
                case Topic.TOPIC_INVENTORY_REQUEST:
                    var inventoryRequest = System.Text.Json.JsonSerializer.Deserialize<InventoryRequest>(message);
                    
                    var inventoryService = scope.ServiceProvider.GetRequiredService<IInventoryRoomService>();
                    await inventoryService.ProcessInventoryRequestAsync(inventoryRequest);
                    
                    break;

                case Topic.TOPIC_COMPENSATION_REQUEST:
                    var compensationRequest = System.Text.Json.JsonSerializer.Deserialize<CompensationRequest>(message);

                    if (compensationRequest.Type != CompensationType.Inventory)
                    {
                        break;
                    }

                    inventoryService = scope.ServiceProvider.GetRequiredService<IInventoryRoomService>();
                    await inventoryService.CompensateInventoryAsync(compensationRequest.BookingId);

                    break;
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _ = Task.Run(async () =>
            {
                try
                {
                    await _kafkaConsumer.Subscribe(new[]
                    {
                        Topic.TOPIC_INVENTORY_REQUEST,
                        Topic.TOPIC_COMPENSATION_REQUEST
                    }, HandleMessageAsync, stoppingToken);

                }
                catch (ConsumeException ex)
                {
                    Console.WriteLine($"Kafka consume error: {ex.Message}");
                }
            }, stoppingToken);
        }
    }
}
