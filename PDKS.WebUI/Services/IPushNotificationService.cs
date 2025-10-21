using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Logging;
using PDKS.Data.Entities;
using PDKS.Data.Repositories;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PDKS.WebUI.Services
{
    public interface IPushNotificationService
    {
        Task<bool> SendNotificationAsync(int kullaniciId, string title, string body, Dictionary<string, string>? data = null);
        Task<bool> SendNotificationToMultipleAsync(List<int> kullaniciIds, string title, string body, Dictionary<string, string>? data = null);
        Task<bool> SendNotificationToTokenAsync(string fcmToken, string title, string body, Dictionary<string, string>? data = null);
    }


}