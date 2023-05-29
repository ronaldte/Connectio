using Microsoft.AspNetCore.SignalR;

namespace Connectio.Hubs
{
    public class MessageHub : Hub
    {
        public async Task SendMessage(string username, string text)
        {
            await Clients.User(username).SendAsync("ReceiveMessage", text);
        }
    }
}
