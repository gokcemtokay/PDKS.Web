using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebaseAdmin.Messaging;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Logging;
using PDKS.Data.Entities;
using PDKS.Data.Repositories;

namespace PDKS.WebUI.Services
{
    public class PushNotificationService : IPushNotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PushNotificationService> _logger;
        private static bool _firebaseInitialized = false;

        public PushNotificationService(IUnitOfWork unitOfWork, ILogger<PushNotificationService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            InitializeFirebase();
        }

        private void InitializeFirebase()
        {
            if (_firebaseInitialized) return;

            try
            {
                // Firebase credentials dosyasının yolu
                var credentialPath = Path.Combine(Directory.GetCurrentDirectory(), "firebase-credentials.json");

                if (File.Exists(credentialPath))
                {
                    FirebaseApp.Create(new AppOptions()
                    {
                        Credential = GoogleCredential.FromFile(credentialPath)
                    });

                    _firebaseInitialized = true;
                    _logger.LogInformation("Firebase initialized successfully");
                }
                else
                {
                    _logger.LogWarning("Firebase credentials file not found at: {Path}", credentialPath);
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error initializing Firebase");
            }
        }

        public async Task<bool> SendNotificationAsync(int kullaniciId, string title, string body, Dictionary<string, string>? data = null)
        {
            if (!_firebaseInitialized)
            {
                _logger.LogWarning("Firebase not initialized. Skipping push notification.");
                return false;
            }

            try
            {
                // Kullanıcının aktif cihaz token'larını al
                var deviceTokens = await _unitOfWork.GetRepository<DeviceToken>()
                    .FindAsync(dt => dt.KullaniciId == kullaniciId && dt.Aktif);

                if (!deviceTokens.Any())
                {
                    _logger.LogInformation("No active device tokens found for user {UserId}", kullaniciId);
                    return false;
                }

                var successCount = 0;

                foreach (var deviceToken in deviceTokens)
                {
                    var result = await SendNotificationToTokenAsync(deviceToken.Token, title, body, data);
                    if (result) successCount++;
                }

                return successCount > 0;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error sending push notification to user {UserId}", kullaniciId);
                return false;
            }
        }

        public async Task<bool> SendNotificationToMultipleAsync(List<int> kullaniciIds, string title, string body, Dictionary<string, string>? data = null)
        {
            if (!_firebaseInitialized)
            {
                _logger.LogWarning("Firebase not initialized. Skipping push notification.");
                return false;
            }

            try
            {
                var tokens = new List<string>();

                foreach (var kullaniciId in kullaniciIds)
                {
                    var deviceTokens = await _unitOfWork.GetRepository<DeviceToken>()
                        .FindAsync(dt => dt.KullaniciId == kullaniciId && dt.Aktif);

                    tokens.AddRange(deviceTokens.Select(dt => dt.Token));
                }

                if (!tokens.Any())
                {
                    _logger.LogInformation("No active device tokens found for users");
                    return false;
                }

                // MulticastMessage ile toplu gönderim
                var message = new MulticastMessage()
                {
                    Tokens = tokens,
                    Notification = new Notification()
                    {
                        Title = title,
                        Body = body
                    },
                    Data = data,
                    Android = new AndroidConfig()
                    {
                        Priority = Priority.High,
                        Notification = new AndroidNotification()
                        {
                            Sound = "default",
                            ClickAction = "FLUTTER_NOTIFICATION_CLICK"
                        }
                    },
                    Apns = new ApnsConfig()
                    {
                        Aps = new Aps()
                        {
                            Sound = "default",
                            Badge = 1
                        }
                    }
                };

                var response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);

                _logger.LogInformation("Successfully sent {SuccessCount} out of {TotalCount} notifications",
                    response.SuccessCount, tokens.Count);

                return response.SuccessCount > 0;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error sending push notifications to multiple users");
                return false;
            }
        }

        public async Task<bool> SendNotificationToTokenAsync(string fcmToken, string title, string body, Dictionary<string, string>? data = null)
        {
            if (!_firebaseInitialized)
            {
                _logger.LogWarning("Firebase not initialized. Skipping push notification.");
                return false;
            }

            try
            {
                var message = new Message()
                {
                    Token = fcmToken,
                    Notification = new Notification()
                    {
                        Title = title,
                        Body = body
                    },
                    Data = data,
                    Android = new AndroidConfig()
                    {
                        Priority = Priority.High,
                        Notification = new AndroidNotification()
                        {
                            Sound = "default",
                            ClickAction = "FLUTTER_NOTIFICATION_CLICK",
                            Color = "#2196F3"
                        }
                    },
                    Apns = new ApnsConfig()
                    {
                        Aps = new Aps()
                        {
                            Sound = "default",
                            Badge = 1,
                            ContentAvailable = true
                        }
                    }
                };

                var response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                _logger.LogInformation("Successfully sent notification. FCM Response: {Response}", response);

                return true;
            }
            catch (FirebaseMessagingException ex)
            {
                _logger.LogError(ex, "FCM Error sending notification to token {Token}. Error Code: {ErrorCode}",
                    fcmToken, ex.MessagingErrorCode);

                // Token geçersizse, deaktif et
                if (ex.MessagingErrorCode == MessagingErrorCode.Unregistered ||
                    ex.MessagingErrorCode == MessagingErrorCode.InvalidArgument)
                {
                    await DeactivateTokenAsync(fcmToken);
                }

                return false;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Unexpected error sending notification to token {Token}", fcmToken);
                return false;
            }
        }

        private async Task DeactivateTokenAsync(string token)
        {
            try
            {
                var deviceToken = await _unitOfWork.GetRepository<DeviceToken>()
                    .FirstOrDefaultAsync(dt => dt.Token == token);

                if (deviceToken != null)
                {
                    deviceToken.Aktif = false;
                    _unitOfWork.GetRepository<DeviceToken>().Update(deviceToken);
                    await _unitOfWork.SaveChangesAsync();

                    _logger.LogInformation("Deactivated invalid token: {Token}", token);
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error deactivating token {Token}", token);
            }
        }
    }
}
