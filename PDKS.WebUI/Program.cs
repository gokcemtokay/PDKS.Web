using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PDKS.Business.Services;
using PDKS.Data.Context;
using PDKS.Data.Repositories;
using System.Text;
using Microsoft.OpenApi.Models;
using AutoMapper;
using PDKS.WebUI.Hubs;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreRateLimit;


Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

var builder = WebApplication.CreateBuilder(args);

// --- 1. Servislerin Eklenmesi ---

builder.Services.AddAutoMapper(typeof(IVardiyaService).Assembly);
// Database Context - PostgreSQL
builder.Services.AddDbContext<PDKSDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true; 
});
// Add services to the container.
// MVC yerine API Controller'larını kullanacağımızı belirtiyoruz.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
    options.JsonSerializerOptions.WriteIndented = true;
});

builder.Services.AddSignalR();

// Swagger (OpenAPI) servisini ekleyerek API'yi test etmek için bir arayüz sağlıyoruz.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Swagger'a JWT Token ile yetkilendirme yapabilme özelliği ekliyoruz.
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PDKS API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. <br/> 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      <br/> Example: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});


// Register Repositories
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Register Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPersonelService, PersonelService>();
builder.Services.AddScoped<IGirisCikisService, GirisCikisService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<ITatilService, TatilService>();
builder.Services.AddScoped<IParametreService, ParametreService>();
builder.Services.AddScoped<IKullaniciService, KullaniciService>();
builder.Services.AddScoped<IDepartmanService, DepartmanService>();
builder.Services.AddScoped<IMesaiService, MesaiService>();
builder.Services.AddScoped<IVardiyaService, VardiyaService>();
builder.Services.AddScoped<IIzinService, IzinService>();
builder.Services.AddScoped<IExportAndEmailService, ExportAndEmailService>();
builder.Services.AddScoped<IBackupService, BackupService>();
builder.Services.AddScoped<ISirketService, SirketService>();
builder.Services.AddScoped<ICihazService, CihazService>();
//builder.Services.AddScoped<IAvansService, AvansService>();
//builder.Services.AddScoped<IMasrafService, MasrafService>();
//builder.Services.AddScoped<IAracService, AracService>();
//builder.Services.AddScoped<ISeyahatService, SeyahatService>();
builder.Services.AddScoped<IOnayAkisiService, OnayAkisiService>();
//builder.Services.AddScoped<IForumService, ForumService>();
//builder.Services.AddScoped<IToplantiService, ToplantiService>();
//builder.Services.AddScoped<IRaporService, RaporService>();
//builder.Services.AddScoped<IMobilPDKSService, MobilPDKSService>();
// Register Services kısmında namespace değişikliği:

builder.Services.AddScoped<IFileUploadService, FileUploadService>();
builder.Services.AddScoped<PDKS.Business.Services.IFileUploadService, PDKS.Business.Services.FileUploadService>();

// Push Notification Service
builder.Services.AddScoped<PDKS.Business.Services.IBildirimService, PDKS.Business.Services.BildirimService>();
builder.Services.AddScoped<PDKS.Business.Services.IPushNotificationService, PDKS.Business.Services.PushNotificationService>();
builder.Services.AddScoped<IOnayAkisiService, OnayAkisiService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IPersonelOzlukService, PersonelOzlukService>();
// API Versioning ekle (builder.Services kısmına)
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("X-Api-Version"),
        new QueryStringApiVersionReader("api-version")
    );
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// Response Compression (Mobil için önemli - veri tasarrufu)
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<Microsoft.AspNetCore.ResponseCompression.GzipCompressionProvider>();
    options.Providers.Add<Microsoft.AspNetCore.ResponseCompression.BrotliCompressionProvider>();
});

builder.Services.Configure<Microsoft.AspNetCore.ResponseCompression.GzipCompressionProviderOptions>(options =>
{
    options.Level = System.IO.Compression.CompressionLevel.Fastest;
});

// --- 2. Kimlik Doğrulama (Authentication) Yapılandırması (Cookie yerine JWT) ---
// Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };

        // ✅ SignalR için JWT Authentication
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;

                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/notificationHub"))
                {
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            }
        };
    });


// Yetkilendirme (Authorization) Politikaları aynı kalabilir.
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("AdminOrIK", policy => policy.RequireRole("Admin", "IK"));
    options.AddPolicy("ManagerAccess", policy => policy.RequireRole("Admin", "IK", "Yönetici"));
});

// Add HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// --- 3. CORS (Cross-Origin Resource Sharing) Yapılandırması ---
// React uygulamasından gelecek isteklere izin vermek için ekliyoruz.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder
            .WithOrigins(
                "http://localhost:56899",           // ⬅️ Development
                "http://localhost:5104",            // ⬅️ Development
                "http://pdks.ardenyazilim.com",     // ⬅️ Production
                "https://pdks.ardenyazilim.com"     // ⬅️ Production HTTPS
            )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// Rate Limiting (DDoS koruması)
builder.Services.AddMemoryCache();
builder.Services.Configure<AspNetCoreRateLimit.IpRateLimitOptions>(options =>
{
    options.EnableEndpointRateLimiting = true;
    options.StackBlockedRequests = false;
    options.HttpStatusCode = 429;
    options.RealIpHeader = "X-Real-IP";
    options.ClientIdHeader = "X-ClientId";
    options.GeneralRules = new List<AspNetCoreRateLimit.RateLimitRule>
    {
        new AspNetCoreRateLimit.RateLimitRule
        {
            Endpoint = "*",
            Period = "1m",
            Limit = 60
        }
    };
});

builder.Services.AddSingleton<AspNetCoreRateLimit.IIpPolicyStore, AspNetCoreRateLimit.MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<AspNetCoreRateLimit.IRateLimitCounterStore, AspNetCoreRateLimit.MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<AspNetCoreRateLimit.IRateLimitConfiguration, AspNetCoreRateLimit.RateLimitConfiguration>();
builder.Services.AddSingleton<AspNetCoreRateLimit.IProcessingStrategy, AspNetCoreRateLimit.AsyncKeyLockProcessingStrategy>();
builder.Services.AddInMemoryRateLimiting();

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

var app = builder.Build();

// --- 4. HTTP Request Pipeline (Middleware) Yapılandırması ---

// Development ortamında Swagger arayüzünü aktif hale getiriyoruz.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

// Development ortamında migration'ları otomatik uygula (opsiyonel)
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<PDKSDbContext>();
    try
    {
        dbContext.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Migration sırasında hata oluştu");
    }
}



app.UseHttpsRedirection();
app.UseResponseCompression();
app.UseIpRateLimiting();
// CORS politikasını aktif hale getiriyoruz. Bu satır UseRouting'den önce olmalı.
app.UseCors("AllowAll");

app.UseRouting();

// Authentication ve Authorization middleware'lerini ekliyoruz.
app.UseAuthentication();
app.UseAuthorization();

app.MapHub<NotificationHub>("/notificationHub");
// Controller'ları endpoint olarak map'liyoruz.
app.MapControllers();


// --- Seed Data (Başlangıç Verisi) - Bu kısım aynı kalabilir ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<PDKSDbContext>();
        var authService = services.GetRequiredService<IAuthService>();

        context.Database.EnsureCreated();

        var adminExists = context.Kullanicilar.Any(k => k.Email == "admin@pdks.com");
        if (!adminExists)
        {
            var adminPersonel = new PDKS.Data.Entities.Personel
            {
                AdSoyad = "Sistem Yöneticisi",
                SicilNo = "ADM001",
                DepartmanId = 1,
                Gorev = "Sistem Yöneticisi",
                Email = "admin@pdks.com",
                Telefon = "05001234567",
                Durum = true,
                GirisTarihi = DateTime.UtcNow,
                VardiyaId = 1,
                Maas = 15000,
                AvansLimiti = 5000,
                OlusturmaTarihi = DateTime.UtcNow
            };

            context.Personeller.Add(adminPersonel);
            context.SaveChanges();

            var adminKullanici = new PDKS.Data.Entities.Kullanici
            {
                PersonelId = adminPersonel.Id,
                Email = "admin@pdks.com",
                SifreHash = authService.HashPassword("admin123"),
                RolId = 1, // Admin role
                Aktif = true,
                OlusturmaTarihi = DateTime.UtcNow
            };

            context.Kullanicilar.Add(adminKullanici);
            context.SaveChanges();

            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Admin kullanıcı oluşturuldu: admin@pdks.com / admin123");
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Seed data oluşturulurken hata oluştu");
    }
}


app.Run();