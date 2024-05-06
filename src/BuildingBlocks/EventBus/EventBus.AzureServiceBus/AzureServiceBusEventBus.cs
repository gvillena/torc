using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Azure.Identity;
using Torc.BuildingBlocks.EventBus.Absractions;
using Torc.BuildingBlocks.EventBus.Events;
using System.Text.Json;
using System.Diagnostics.Tracing;
using Microsoft.Extensions.DependencyInjection;

namespace Torc.BuildingBlocks.EventBus.AzureServiceBus
{
    public class AzureServiceBusEventBus : IEventBus
    {
        private const string INTEGRATION_EVENT_SUFFIX = "IntegrationEvent";

        private readonly Dictionary<string, AzureServiceBusEventBusInfo> _events;
        private readonly Dictionary<string, Type> _eventsType;
        private readonly Dictionary<string, Type> _handlersType;
        private readonly Dictionary<string, ServiceBusProcessor> _processors;
        private readonly ServiceBusClient _serviceBusClient;
        private readonly IServiceProvider _serviceProvider;

        public AzureServiceBusEventBus(string namespaceName, Dictionary<string, AzureServiceBusEventBusInfo> events, IServiceProvider serviceProvider)
        {
            _events = events;
            _eventsType = new Dictionary<string, Type>();
            _handlersType = new Dictionary<string, Type>();
            _processors = new Dictionary<string, ServiceBusProcessor>();
            _serviceBusClient = new ServiceBusClient($"{namespaceName}.servicebus.windows.net", new DefaultAzureCredential());
            _serviceProvider = serviceProvider;
        }

        public async Task Publish(IntegrationEvent @event)
        {
            var eventName = @event.GetType().Name.Replace(INTEGRATION_EVENT_SUFFIX, "");
            var jsonMessage = JsonSerializer.Serialize(@event, @event.GetType());
            var body = Encoding.UTF8.GetBytes(jsonMessage);

            var message = new ServiceBusMessage
            {
                MessageId = Guid.NewGuid().ToString(),
                Body = new BinaryData(body),
                Subject = eventName,
            };

            var topicName = _events[eventName].TopicName;
            var sender = _serviceBusClient.CreateSender(topicName);

            try
            {
                await sender.SendMessageAsync(message);
            }
            finally
            {
                await sender.DisposeAsync();
            }
        }

        public async Task Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventType = typeof(T);
            var eventName = eventType.Name.Replace(INTEGRATION_EVENT_SUFFIX, "");

            var topicName = _events[eventName].TopicName;
            var subscriptionName = _events[eventName].SubscriptionName;

            _eventsType[eventName] = eventType;
            _handlersType[eventName] = typeof(TH);

            var processor = _serviceBusClient.CreateProcessor(topicName, subscriptionName, new ServiceBusProcessorOptions());

            processor.ProcessMessageAsync += ProcessMessageHandler;
            processor.ProcessErrorAsync += ProcessErrorHandler;

            await processor.StartProcessingAsync();

            _processors.Add(eventName, processor);
        }

        public Task Unsubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            // TODO
            return Task.CompletedTask;
        }

        private async Task ProcessMessageHandler(ProcessMessageEventArgs args)
        {
            var eventName = args.Message.Subject;
            var eventType = _eventsType[eventName];
            var handlerType = _handlersType[eventName];
            var body = Encoding.UTF8.GetString(args.Message.Body);

            await using var scope = _serviceProvider.CreateAsyncScope();
            var handler = scope.ServiceProvider.GetRequiredService(handlerType);

            var integrationEvent = JsonSerializer.Deserialize(body, eventType, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
            await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });

            // complete the message. messages is deleted from the subscription. 
            await args.CompleteMessageAsync(args.Message);
        }

        private Task ProcessErrorHandler(ProcessErrorEventArgs args)
        {
            return Task.CompletedTask;
        }
    }
}
