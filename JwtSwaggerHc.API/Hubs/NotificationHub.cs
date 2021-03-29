using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace JwtSwaggerHc.API.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendMessage(IHubContext<NotificationHub> hubContext, string clientMethod, string apiMethod, string message)
        {
            await hubContext.Clients.All.SendAsync(clientMethod, apiMethod, message);
        }
    }
}
