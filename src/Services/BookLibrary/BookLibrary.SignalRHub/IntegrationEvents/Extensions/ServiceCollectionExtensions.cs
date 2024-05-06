using Torc.BuildingBlocks.EventBus.Absractions;
using Torc.BuildingBlocks.EventBus.AzureServiceBus;

namespace Torc.Services.BookLibrary.SignalRHub.IntegrationEvents.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAzureServiceBus(this IServiceCollection services, IConfiguration configuration)
        {
            var azureServiceBus = configuration.GetSection("AzureServiceBus");

            if (!azureServiceBus.Exists())
            {
                return services;
            }

            var torcBookLibrary = azureServiceBus.GetRequiredSection("TorcBookLibrary");

            var namespaceName = torcBookLibrary.GetRequiredValue("NamespaceName");
            var events = torcBookLibrary.GetRequiredSection("Events");

            var bookAddedEvent = events.GetRequiredSection("BookAdded");
            var topicNAme = bookAddedEvent.GetRequiredValue("TopicName");
            var subscriptionName= bookAddedEvent.GetRequiredValue("SubscriptionName");

            var eventsInfo = new Dictionary<string, AzureServiceBusEventBusInfo>();
            eventsInfo["BookAdded"] = new AzureServiceBusEventBusInfo(topicNAme, subscriptionName);

            services.AddSingleton<IEventBus, AzureServiceBusEventBus>(serviceProvider => {
                return new AzureServiceBusEventBus(namespaceName, eventsInfo, serviceProvider);
            });

            return services;
        }

        public static IServiceCollection AddCors(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:8080", "http://localhost:5173")
                               .AllowCredentials()
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });

            return services;
        }
    }
}
