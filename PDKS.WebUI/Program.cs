using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PDKS.Business.Services;
using PDKS.Data.Context;
using PDKS.Data.Repositories;
using PDKS.WebUI.BackgroundServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Database Context - PostgreSQL
// Program.cs veya Startup.cs'de
builder.Services.AddDbContext<PDKSDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

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



// Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Auth/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    });

// Authorization Policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("AdminOrIK", policy => policy.RequireRole("Admin", "IK"));
    options.AddPolicy("ManagerAccess", policy => policy.RequireRole("Admin", "IK", "Yönetici"));
});

// Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddMemoryCache();

// Add HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Configure localization for Turkish
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { "tr-TR" };
    options.SetDefaultCulture("tr-TR");
    options.AddSupportedCultures(supportedCultures);
    options.AddSupportedUICultures(supportedCultures);
});

var app = builder.Build();

// Apply migrations automatically (optional - for development)
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
        logger.LogError(ex, "Migration sýrasýnda hata oluþtu");
    }
}

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Use Turkish culture
app.UseRequestLocalization();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Seed initial data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<PDKSDbContext>();
        var authService = services.GetRequiredService<IAuthService>();

        // Ensure database is created
        context.Database.EnsureCreated();

        // Seed default admin user if not exists
        var adminExists = context.Kullanicilar.Any(k => k.Email == "admin@pdks.com");
        if (!adminExists)
        {
            // Create admin personel
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

            // Create admin user
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
            logger.LogInformation("Admin kullanýcý oluþturuldu: admin@pdks.com / admin123");
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Seed data oluþturulurken hata oluþtu");
    }
}

app.Run();