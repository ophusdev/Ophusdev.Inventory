using Ophusdev.Kafka.Abstraction;
using Ophusdev.Inventory.Business.Abstraction;
using Ophusdev.Inventory.Shared;
using Ophusdev.Inventory.Business.Services;

namespace Ophusdev.Inventory.Api.Extensions
{
    public static class AddKafkaTopicsExtensions
    {
        public static IServiceCollection AddKafkaTopicHandlers(this IServiceCollection services)
        {
            services.AddSingleton<IKafkaHandlerRegistry>(sp =>
            {
                var topicTranslator = sp.GetRequiredService<ITopicTranslator>();
                var loggerRegistry = sp.GetRequiredService<ILogger<KafkaHandlerRegistry>>(); 
                var registry = new KafkaHandlerRegistry(topicTranslator, loggerRegistry); 

                var sagaConsumerLogger = sp.GetRequiredService<ILogger<SagaConsumerService>>();

                registry.RegisterTypedHandler<InventoryRequest>(
                    "TOPIC_INVENTORY_REQUEST",
                    async (inventoryService, response) =>
                    {
                        await inventoryService.ProcessInventoryRequestAsync(response);
                    }
                );

                registry.RegisterTypedHandler<CompensationRequest>(
                    "TOPIC_COMPENSATION_REQUEST",
                    async (inventoryService, response) =>
                    {
                        await inventoryService.CompensateInventoryAsync(response);
                    }
                );

                return registry;
            });

            return services;
        }
    }
}
