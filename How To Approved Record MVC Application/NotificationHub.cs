using Microsoft.AspNetCore.SignalR;

namespace How_To_Approved_Record_MVC_Application
{
    public class NotificationHub : Hub
    {
        public async Task SendMessage(string message)
        {
            // Broadcast message to all connected clients
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
