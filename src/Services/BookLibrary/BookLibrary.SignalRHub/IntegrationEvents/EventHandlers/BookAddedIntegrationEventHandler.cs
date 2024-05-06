using Microsoft.AspNetCore.SignalR;
using Torc.BuildingBlocks.EventBus.Absractions;
using Torc.Services.BookLibrary.SignalRHub.IntegrationEvents.Events;

namespace Torc.Services.BookLibrary.SignalRHub.IntegrationEvents.EventHandlers
{
    public class BookAddedIntegrationEventHandler : IIntegrationEventHandler<BookAddedIntegrationEvent>
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public BookAddedIntegrationEventHandler(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task Handle(BookAddedIntegrationEvent @event)
        {
            await _hubContext.Clients.All.SendAsync("BookAddedNotification", new { bookTitle = @event.BookTitle, author = @event.Author });
        }
    }
}
