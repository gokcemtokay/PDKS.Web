using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace PDKS.Data.Context
{
    /// <summary>
    /// Design-time DbContext factory for EF Core migrations with PostgreSQL
    /// Dosya konumu: PDKS.Data/Context/PDKSDbContextFactory.cs
    /// </summary>
    public class PDKSDbContextFactory : IDesignTimeDbContextFactory<PDKSDbContext>
    {
        public PDKSDbContext CreateDbContext(string[] args)
        {
            // appsettings.json'dan connection string oku
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../PDKS.WebUI"))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<PDKSDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Connection string yoksa default kullan (PostgreSQL)
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = "Host=185.149.103.221;Port=5432;Database=pdks_db;Username=postgres;Password=xxx";
            }

            // ✅ PostgreSQL kullanımı
            optionsBuilder.UseNpgsql(connectionString);

            return new PDKSDbContext(optionsBuilder.Options);
        }
    }
}