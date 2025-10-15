using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PDKS.Data.Entities;

namespace PDKS.Data.Context
{
    public class PDKSDbContext : DbContext
    {
        public PDKSDbContext(DbContextOptions<PDKSDbContext> options) : base(options)
        {
        }

        // DbSets
        public DbSet<Personel> Personeller { get; set; }
        public DbSet<Cihaz> Cihazlar { get; set; }
        public DbSet<Vardiya> Vardiyalar { get; set; }
        public DbSet<GirisCikis> GirisCikislar { get; set; }
        public DbSet<Izin> Izinler { get; set; }
        public DbSet<Rol> Roller { get; set; }
        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Log> Loglar { get; set; }
        public DbSet<Bildirim> Bildirimler { get; set; }
        public DbSet<Tatil> Tatiller { get; set; }
        public DbSet<Parametre> Parametreler { get; set; }
        public DbSet<CihazLog> CihazLoglari { get; set; }
        public DbSet<Avans> Avanslar { get; set; }
        public DbSet<Prim> Primler { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
                v => v.Kind == DateTimeKind.Utc ? v : v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
            );

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties()
                         .Where(p => p.ClrType == typeof(DateTime)))
                {
                    property.SetValueConverter(dateTimeConverter);
                }
            }


            base.OnModelCreating(modelBuilder);

            // Indexes
            modelBuilder.Entity<Personel>()
                .HasIndex(p => p.SicilNo)
                .IsUnique();

            modelBuilder.Entity<Personel>()
                .HasIndex(p => p.Email)
                .IsUnique();

            modelBuilder.Entity<Kullanici>()
                .HasIndex(k => k.Email)
                .IsUnique();

            modelBuilder.Entity<GirisCikis>()
                .HasIndex(g => g.GirisZamani);

            modelBuilder.Entity<GirisCikis>()
                .HasIndex(g => new { g.PersonelId, g.GirisZamani });

            // Relationships
            modelBuilder.Entity<Kullanici>()
                .HasOne(k => k.Personel)
                .WithOne(p => p.Kullanici)
                .HasForeignKey<Kullanici>(k => k.PersonelId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GirisCikis>()
                .HasOne(g => g.Personel)
                .WithMany(p => p.GirisCikislar)
                .HasForeignKey(g => g.PersonelId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Izin>()
                .HasOne(i => i.Personel)
                .WithMany(p => p.Izinler)
                .HasForeignKey(i => i.PersonelId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Avans>()
                .HasOne(a => a.Personel)
                .WithMany(p => p.Avanslar)
                .HasForeignKey(a => a.PersonelId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed Data - Roller
            modelBuilder.Entity<Rol>().HasData(
                new Rol { Id = 1, RolAdi = "Admin", Aciklama = "Sistem Yöneticisi" },
                new Rol { Id = 2, RolAdi = "IK", Aciklama = "İnsan Kaynakları" },
                new Rol { Id = 3, RolAdi = "Yönetici", Aciklama = "Departman Yöneticisi" },
                new Rol { Id = 4, RolAdi = "Personel", Aciklama = "Çalışan" }
            );

            // Seed Data - Parametreler
            modelBuilder.Entity<Parametre>().HasData(
                new Parametre { Id = 1, Anahtar = "FazlaMesaiOrani", Deger = "1.5", Aciklama = "Fazla mesai ücret oranı" },
                new Parametre { Id = 2, Anahtar = "GecKalmaToleransDakika", Deger = "15", Aciklama = "Geç kalma tolerans süresi (dakika)" },
                new Parametre { Id = 3, Anahtar = "ErkenCikisToLeransDakika", Deger = "15", Aciklama = "Erken çıkış tolerans süresi (dakika)" },
                new Parametre { Id = 4, Anahtar = "HaftalikcalismaGun", Deger = "5", Aciklama = "Haftalık çalışma günü sayısı" },
                new Parametre { Id = 5, Anahtar = "GunlukCalismaSaat", Deger = "8", Aciklama = "Günlük çalışma saati" }
            );

            // Seed Data - Vardiya
            modelBuilder.Entity<Vardiya>().HasData(
                new Vardiya
                {
                    Id = 1,
                    Ad = "Gündüz Vardiyası",
                    BaslangicSaati = new TimeSpan(8, 0, 0),
                    BitisSaati = new TimeSpan(17, 0, 0),
                    GeceVardiyasiMi = false,
                    EsnekVardiyaMi = false,
                    ToleransSuresiDakika = 15,
                    Aciklama = "Standart gündüz vardiyası 08:00-17:00",
                    Durum = true
                },
                new Vardiya
                {
                    Id = 2,
                    Ad = "Gece Vardiyası",
                    BaslangicSaati = new TimeSpan(20, 0, 0),
                    BitisSaati = new TimeSpan(5, 0, 0),
                    GeceVardiyasiMi = true,
                    EsnekVardiyaMi = false,
                    ToleransSuresiDakika = 15,
                    Aciklama = "Gece vardiyası 20:00-05:00",
                    Durum = true
                },
                new Vardiya
                {
                    Id = 3,
                    Ad = "Esnek Çalışma",
                    BaslangicSaati = new TimeSpan(9, 0, 0),
                    BitisSaati = new TimeSpan(18, 0, 0),
                    GeceVardiyasiMi = false,
                    EsnekVardiyaMi = true,
                    ToleransSuresiDakika = 30,
                    Aciklama = "Esnek çalışma saatleri",
                    Durum = true
                }
            );
        }
    }
}