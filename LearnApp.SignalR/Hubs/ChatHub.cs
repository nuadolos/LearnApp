using Microsoft.AspNetCore.SignalR;

namespace LearnApp.SignalR.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string message) =>
            await Clients.All.SendAsync("MessageReceived", message);
    }
}
