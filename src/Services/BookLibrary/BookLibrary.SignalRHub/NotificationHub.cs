using Microsoft.AspNetCore.SignalR;

namespace Torc.Services.BookLibrary.SignalRHub
{
    public class NotificationHub : Hub
    {
        public async Task SendNotification(string notificationName, object message)
            => await Clients.All.SendAsync($"{notificationName}Notification", message);
    }
}
