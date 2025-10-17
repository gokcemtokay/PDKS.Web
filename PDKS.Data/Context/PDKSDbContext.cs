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
        public DbSet<Departman> Departmanlar { get; set; }
        public DbSet<Mesai> Mesailer { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Bu ValueConverter hem DateTime hem de DateTime? tipleri için çalışacak.
            var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
                v => v.ToUniversalTime(), // Gelen değeri her zaman UTC'ye çevir
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc)); // Okunan değerin UTC olduğunu belirt

            var nullableDateTimeConverter = new ValueConverter<DateTime?, DateTime?>(
                v => v.HasValue ? v.Value.ToUniversalTime() : v, // Null değilse UTC'ye çevir
                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v); // Null değilse UTC olduğunu belirt

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    // Hem DateTime hem de DateTime? property'lerini yakala
                    if (property.ClrType == typeof(DateTime))
                    {
                        property.SetValueConverter(dateTimeConverter);
                    }
                    else if (property.ClrType == typeof(DateTime?))
                    {
                        property.SetValueConverter(nullableDateTimeConverter);
                    }
                }
            }

            base.OnModelCreating(modelBuilder);

            // ============== INDEXES ==============

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

            // Parametre için unique index
            modelBuilder.Entity<Parametre>()
                .HasIndex(p => p.Ad)
                .IsUnique();

            // Tatil için unique index
            modelBuilder.Entity<Tatil>()
                .HasIndex(t => t.Tarih)
                .IsUnique();

            // ============== RELATIONSHIPS ==============

            // Kullanici - Personel (One-to-One)
            modelBuilder.Entity<Kullanici>()
                .HasOne(k => k.Personel)
                .WithOne(p => p.Kullanici)
                .HasForeignKey<Kullanici>(k => k.PersonelId)
                .OnDelete(DeleteBehavior.Restrict);

            // Kullanici - Rol (Many-to-One)
            modelBuilder.Entity<Kullanici>()
                .HasOne(k => k.Rol)
                .WithMany(r => r.Kullanicilar)
                .HasForeignKey(k => k.RolId)
                .OnDelete(DeleteBehavior.Restrict);

            // GirisCikis - Personel (Many-to-One)
            modelBuilder.Entity<GirisCikis>()
                .HasOne(g => g.Personel)
                .WithMany(p => p.GirisCikislar)
                .HasForeignKey(g => g.PersonelId)
                .HasForeignKey(g => g.CihazId)
                .OnDelete(DeleteBehavior.Cascade);

            // GirisCikis - Cihaz (Many-to-One)
            modelBuilder.Entity<GirisCikis>()
                .HasOne(g => g.Cihaz)
                .WithMany()
                .HasForeignKey(g => g.CihazId)
                .OnDelete(DeleteBehavior.Restrict);

            // Izin - Personel (Many-to-One)
            modelBuilder.Entity<Izin>()
                .HasOne(i => i.Personel)
                .WithMany(p => p.Izinler)
                .HasForeignKey(i => i.PersonelId)
                .OnDelete(DeleteBehavior.Cascade);

            // Izin - OnaylayanKullanici (Many-to-One)
            modelBuilder.Entity<Izin>()
                .HasOne(i => i.OnaylayanKullanici)
                .WithMany()
                .HasForeignKey(i => i.OnaylayanKullaniciId)
                .OnDelete(DeleteBehavior.Restrict);

            // Avans - Personel (Many-to-One)
            modelBuilder.Entity<Avans>()
                .HasOne(a => a.Personel)
                .WithMany(p => p.Avanslar)
                .HasForeignKey(a => a.PersonelId)
                .OnDelete(DeleteBehavior.Cascade);

            // Avans - OnaylayanKullanici (Many-to-One)
            modelBuilder.Entity<Avans>()
                .HasOne(a => a.OnaylayanKullanici)
                .WithMany()
                .HasForeignKey(a => a.OnaylayanKullaniciId)
                .OnDelete(DeleteBehavior.Restrict);

            // Prim - Personel (Many-to-One)
            modelBuilder.Entity<Prim>()
                .HasOne(p => p.Personel)
                .WithMany(per => per.Primler)
                .HasForeignKey(p => p.PersonelId)
                .OnDelete(DeleteBehavior.Cascade);

            // Prim - OnaylayanKullanici (Many-to-One)
            modelBuilder.Entity<Prim>()
                .HasOne(p => p.OnaylayanKullanici)
                .WithMany()
                .HasForeignKey(p => p.OnaylayanKullaniciId)
                .OnDelete(DeleteBehavior.Restrict);

            // Mesai - Personel (Many-to-One)
            modelBuilder.Entity<Mesai>()
                .HasOne(m => m.Personel)
                .WithMany(p => p.Mesailer)
                .HasForeignKey(m => m.PersonelId)
                .OnDelete(DeleteBehavior.Cascade);

            // Mesai - OnaylayanKullanici (Many-to-One)
            modelBuilder.Entity<Mesai>()
                .HasOne(m => m.OnaylayanKullanici)
                .WithMany()
                .HasForeignKey(m => m.OnaylayanKullaniciId)
                .OnDelete(DeleteBehavior.Restrict);

            // Personel - Departman (Many-to-One)
            modelBuilder.Entity<Personel>()
                .HasOne(p => p.Departman)
                .WithMany(d => d.Personeller)
                .HasForeignKey(p => p.DepartmanId)
                .OnDelete(DeleteBehavior.Restrict);

            // Personel - Vardiya (Many-to-One)
            modelBuilder.Entity<Personel>()
                .HasOne(p => p.Vardiya)
                .WithMany(v => v.Personeller)
                .HasForeignKey(p => p.VardiyaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Departman - UstDepartman (Self-Referencing)
            modelBuilder.Entity<Departman>()
                .HasOne(d => d.UstDepartman)
                .WithMany(d => d.AltDepartmanlar)
                .HasForeignKey(d => d.UstDepartmanId)
                .OnDelete(DeleteBehavior.Restrict);

            // CihazLog - Cihaz (Many-to-One)
            modelBuilder.Entity<CihazLog>()
                .HasOne(cl => cl.Cihaz)
                .WithMany()
                .HasForeignKey(cl => cl.CihazId)
                .OnDelete(DeleteBehavior.Cascade);

            // Log - Kullanici (Many-to-One)
            modelBuilder.Entity<Log>()
                .HasOne(l => l.Kullanici)
                .WithMany(k => k.Loglar)
                .HasForeignKey(l => l.KullaniciId)
                .OnDelete(DeleteBehavior.Cascade);

            // Bildirim - Kullanici (Many-to-One)
            modelBuilder.Entity<Bildirim>()
                .HasOne(b => b.Kullanici)
                .WithMany(k => k.Bildirimler)
                .HasForeignKey(b => b.KullaniciId)
                .OnDelete(DeleteBehavior.Cascade);

            // ============== SEED DATA - ROLLER ==============

            modelBuilder.Entity<Rol>().HasData(
                new Rol { Id = 1, RolAdi = "Admin", Aciklama = "Sistem Yöneticisi" },
                new Rol { Id = 2, RolAdi = "IK", Aciklama = "İnsan Kaynakları" },
                new Rol { Id = 3, RolAdi = "Yönetici", Aciklama = "Departman Yöneticisi" },
                new Rol { Id = 4, RolAdi = "Personel", Aciklama = "Çalışan" }
            );

            // ============== SEED DATA - PARAMETRELER ==============

            modelBuilder.Entity<Parametre>().HasData(
                new Parametre
                {
                    Id = 1,
                    Ad = "Haftalık Çalışma Günü",
                    Deger = "5",
                    Birim = "gün",
                    Kategori = "Çalışma Saatleri",
                    Aciklama = "Haftalık standart çalışma günü sayısı",
                    KayitTarihi = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Parametre
                {
                    Id = 2,
                    Ad = "Günlük Çalışma Saati",
                    Deger = "8",
                    Birim = "saat",
                    Kategori = "Çalışma Saatleri",
                    Aciklama = "Günlük standart çalışma saati",
                    KayitTarihi = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Parametre
                {
                    Id = 3,
                    Ad = "Geç Kalma Toleransı",
                    Deger = "15",
                    Birim = "dakika",
                    Kategori = "Toleranslar",
                    Aciklama = "İşe geç kalma tolerans süresi",
                    KayitTarihi = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Parametre
                {
                    Id = 4,
                    Ad = "Erken Çıkış Toleransı",
                    Deger = "15",
                    Birim = "dakika",
                    Kategori = "Toleranslar",
                    Aciklama = "Erken çıkış tolerans süresi",
                    KayitTarihi = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Parametre
                {
                    Id = 5,
                    Ad = "Fazla Mesai Çarpanı",
                    Deger = "1.5",
                    Birim = "kat",
                    Kategori = "Mesai",
                    Aciklama = "Fazla mesai ücreti çarpanı",
                    KayitTarihi = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Parametre
                {
                    Id = 6,
                    Ad = "Hafta Sonu Mesai Çarpanı",
                    Deger = "2",
                    Birim = "kat",
                    Kategori = "Mesai",
                    Aciklama = "Hafta sonu mesai ücreti çarpanı",
                    KayitTarihi = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Parametre
                {
                    Id = 7,
                    Ad = "Yıllık İzin Günü",
                    Deger = "14",
                    Birim = "gün",
                    Kategori = "İzin",
                    Aciklama = "Yıllık ücretli izin günü sayısı",
                    KayitTarihi = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );

            // ============== SEED DATA - VARDIYA ==============

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