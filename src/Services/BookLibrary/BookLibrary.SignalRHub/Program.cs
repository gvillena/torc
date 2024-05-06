using Microsoft.AspNetCore.Mvc;
using Torc.BuildingBlocks.EventBus.Absractions;
using Torc.Services.BookLibrary.SignalRHub.IntegrationEvents.EventHandlers;
using Torc.Services.BookLibrary.SignalRHub.IntegrationEvents.Events;
using Torc.Services.BookLibrary.SignalRHub.IntegrationEvents.Extensions;

namespace Torc.Services.BookLibrary.SignalRHub
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAzureServiceBus(builder.Configuration);
            builder.Services.AddCors(o => o.AddPolicy("CorsPolicy", builder => {
                builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .WithOrigins("http://localhost:8080");
            }));
            builder.Services.AddSignalR();
            
            builder.Services.AddSingleton<BookAddedIntegrationEventHandler>();

            var app = builder.Build();

            app.UseCors("CorsPolicy");
            app.MapHub<NotificationHub>("/hub/notifications");
            
            var eventBus = app.Services.GetRequiredService<IEventBus>();

            eventBus.Subscribe<BookAddedIntegrationEvent, BookAddedIntegrationEventHandler>();

            app.Run();
        }
    }
}
