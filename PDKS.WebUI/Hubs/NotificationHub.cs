using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace PDKS.WebUI.Hubs
{
    public class NotificationHub : Hub
    {
        // Kullanıcı bağlandığında
        public override async Task OnConnectedAsync()
        {
            var kullaniciId = Context.User?.FindFirst("sub")?.Value ??
                             Context.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrEmpty(kullaniciId))
            {
                // Kullanıcıyı kendi grubuna ekle (kullanıcı ID'sine göre)
                await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{kullaniciId}");
            }

            await base.OnConnectedAsync();
        }

        // Kullanıcı bağlantısı kesildiğinde
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var kullaniciId = Context.User?.FindFirst("sub")?.Value ??
                             Context.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrEmpty(kullaniciId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user_{kullaniciId}");
            }

            await base.OnDisconnectedAsync(exception);
        }

        // Client'tan mesaj gönderme (opsiyonel)
        public async Task SendMessageToUser(string userId, string message)
        {
            await Clients.Group($"user_{userId}").SendAsync("ReceiveMessage", message);
        }
    }
}