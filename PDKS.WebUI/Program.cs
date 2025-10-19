using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PDKS.Business.Services;
using PDKS.Data.Context;
using PDKS.Data.Repositories;
using System.Text;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// --- 1. Servislerin Eklenmesi ---

// Database Context - PostgreSQL
builder.Services.AddDbContext<PDKSDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
// MVC yerine API Controller'lar�n� kullanaca��m�z� belirtiyoruz.
builder.Services.AddControllers();

// Swagger (OpenAPI) servisini ekleyerek API'yi test etmek i�in bir aray�z sa�l�yoruz.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Swagger'a JWT Token ile yetkilendirme yapabilme �zelli�i ekliyoruz.
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

// --- 2. Kimlik Do�rulama (Authentication) Yap�land�rmas� (Cookie yerine JWT) ---
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
    });


// Yetkilendirme (Authorization) Politikalar� ayn� kalabilir.
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("AdminOrIK", policy => policy.RequireRole("Admin", "IK"));
    options.AddPolicy("ManagerAccess", policy => policy.RequireRole("Admin", "IK", "Y�netici"));
});

// Add HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// --- 3. CORS (Cross-Origin Resource Sharing) Yap�land�rmas� ---
// React uygulamas�ndan gelecek isteklere izin vermek i�in ekliyoruz.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000") // React development sunucusunun adresi
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});


var app = builder.Build();

// --- 4. HTTP Request Pipeline (Middleware) Yap�land�rmas� ---

// Development ortam�nda Swagger aray�z�n� aktif hale getiriyoruz.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Development ortam�nda migration'lar� otomatik uygula (opsiyonel)
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
        logger.LogError(ex, "Migration s�ras�nda hata olu�tu");
    }
}

app.UseHttpsRedirection();

// CORS politikas�n� aktif hale getiriyoruz. Bu sat�r UseRouting'den �nce olmal�.
app.UseCors("AllowReactApp");

app.UseRouting();

// Authentication ve Authorization middleware'lerini ekliyoruz.
app.UseAuthentication();
app.UseAuthorization();

// Controller'lar� endpoint olarak map'liyoruz.
app.MapControllers();


// --- Seed Data (Ba�lang�� Verisi) - Bu k�s�m ayn� kalabilir ---
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
                AdSoyad = "Sistem Y�neticisi",
                SicilNo = "ADM001",
                DepartmanId = 1,
                Gorev = "Sistem Y�neticisi",
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
            logger.LogInformation("Admin kullan�c� olu�turuldu: admin@pdks.com / admin123");
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Seed data olu�turulurken hata olu�tu");
    }
}


app.Run();