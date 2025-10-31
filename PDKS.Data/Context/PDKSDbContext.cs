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
        public DbSet<KullaniciSirket> KullaniciSirketler { get; set; }
        public DbSet<Sirket> Sirketler { get; set; }

        public DbSet<GorevTanimi> GorevTanimlari { get; set; }
        public DbSet<IzinHakki> IzinHaklari { get; set; }
        public DbSet<OnayAkisi> OnayAkislari { get; set; }
        public DbSet<OnayAdimi> OnayAdimlari { get; set; }
        public DbSet<OnayKaydi> OnayKayitlari { get; set; }
        public DbSet<OnayDetay> OnayDetaylari { get; set; }
        public DbSet<AvansTalebi> AvansTalepleri { get; set; }
        public DbSet<Arac> Araclar { get; set; }
        public DbSet<AracTalebi> AracTalepleri { get; set; }
        public DbSet<AracKullanimRaporu> AracKullanimRaporlari { get; set; }
        public DbSet<SeyahatTalebi> SeyahatTalepleri { get; set; }
        public DbSet<SeyahatMasraf> SeyahatMasraflari { get; set; }
        public DbSet<MasrafTalebi> MasrafTalepleri { get; set; }
        public DbSet<Gorev> Gorevler { get; set; }
        public DbSet<GorevAtama> GorevAtamalari { get; set; }
        public DbSet<GorevYorum> GorevYorumlari { get; set; }
        public DbSet<MazeretBildirimi> MazeretBildirimleri { get; set; }
        public DbSet<DosyaTalebi> DosyaTalepleri { get; set; }
        public DbSet<ToplantiOdasi> ToplantiOdalari { get; set; }
        public DbSet<ToplantiOdasiRezervasyon> ToplantiOdasiRezervasyonlari { get; set; }
        public DbSet<Zimmet> Zimmetler { get; set; }
        public DbSet<MalzemeTalebi> MalzemeTalepleri { get; set; }
        public DbSet<Forum> Forumlar { get; set; }
        public DbSet<ForumKonu> ForumKonulari { get; set; }
        public DbSet<ForumMesaj> ForumMesajlari { get; set; }
        public DbSet<Duyuru> Duyurular { get; set; }
        public DbSet<Etkinlik> Etkinlikler { get; set; }
        public DbSet<EtkinlikKatilimci> EtkinlikKatilimcilari { get; set; }
        public DbSet<Anket> Anketler { get; set; }
        public DbSet<AnketSoru> AnketSorulari { get; set; }
        public DbSet<AnketCevap> AnketCevaplari { get; set; }
        public DbSet<Oneri> Oneriler { get; set; }
        public DbSet<Sikayet> Sikayetler { get; set; }
        public DbSet<DeviceToken> DeviceTokens { get; set; }
        public DbSet<PersonelAile> PersonelAileBilgileri { get; set; }
        public DbSet<PersonelAcilDurum> PersonelAcilDurumlar { get; set; }
        public DbSet<PersonelSaglik> PersonelSagliklar { get; set; }
        public DbSet<PersonelEgitim> PersonelEgitimler { get; set; }
        public DbSet<PersonelIsDeneyimi> PersonelIsDeneyimleri { get; set; }
        public DbSet<PersonelDil> PersonelDiller { get; set; }
        public DbSet<PersonelSertifika> PersonelSertifikalar { get; set; }
        public DbSet<PersonelPerformans> PersonelPerformanslar { get; set; }
        public DbSet<PersonelDisiplin> PersonelDisiplinler { get; set; }
        public DbSet<PersonelTerfi> PersonelTerfiler { get; set; }
        public DbSet<PersonelUcretDegisiklik> PersonelUcretDegisiklikler { get; set; }
        public DbSet<PersonelReferans> PersonelReferanslar { get; set; }
        public DbSet<PersonelZimmet> PersonelZimmetler { get; set; }
        public DbSet<PersonelYetkinlik> PersonelYetkinlikler { get; set; }
        public DbSet<PersonelEgitimKayit> PersonelEgitimKayitlari { get; set; }
        public DbSet<PersonelMaliBilgi> PersonelMaliBilgileri { get; set; }
        public DbSet<PersonelEkBilgi> PersonelEkBilgileri { get; set; }
        public DbSet<Menu> Menuler { get; set; }
        public DbSet<MenuRol> MenuRoller { get; set; }
        public DbSet<IslemYetki> IslemYetkiler { get; set; }
        public DbSet<RolIslemYetki> RolIslemYetkiler { get; set; }
        public DbSet<Puantaj> Puantajlar { get; set; }
        public DbSet<PuantajDetay> PuantajDetaylar { get; set; }
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

            modelBuilder.Entity<Arac>()
                .HasIndex(a => a.Plaka)
                .IsUnique();



            modelBuilder.Entity<Bildirim>()
                .HasIndex(b => new { b.KullaniciId, b.Okundu });

            modelBuilder.Entity<ToplantiOdasiRezervasyon>()
                .HasIndex(t => new { t.OdaId, t.BaslangicTarihi, t.BitisTarihi });

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
                .OnDelete(DeleteBehavior.Cascade);

            // GirisCikis - Cihaz (Many-to-One)
            modelBuilder.Entity<GirisCikis>()
                .HasOne(g => g.Cihaz)
                .WithMany(c => c.GirisCikislar) 
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
                .WithMany(c => c.CihazLoglari) 
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

            // ⭐ YENİ: Departman - Sirket (Many-to-One) 
            modelBuilder.Entity<Departman>()
                .HasOne(d => d.Sirket)
                .WithMany(s => s.Departmanlar)
                .HasForeignKey(d => d.SirketId)
                .OnDelete(DeleteBehavior.Restrict);

            // ⭐ YENİ: Sirket - AnaSirket (Self-Referencing) 
            modelBuilder.Entity<Sirket>()
                .HasOne(s => s.AnaSirketNavigation)
                .WithMany(s => s.BagliSirketler)
                .HasForeignKey(s => s.AnaSirketId)
                .OnDelete(DeleteBehavior.Restrict);

            // ⭐ YENİ: Personel - Sirket (Many-to-One) 
            modelBuilder.Entity<Personel>()
                .HasOne(p => p.Sirket)
                .WithMany(s => s.Personeller)
                .HasForeignKey(p => p.SirketId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<KullaniciSirket>()
                // Composite Primary Key (birden fazla atama için benzersizlik sağlar)
                .HasKey(ks => new { ks.KullaniciId, ks.SirketId });

            modelBuilder.Entity<KullaniciSirket>()
                .HasOne(ks => ks.Kullanici)
                .WithMany(k => k.KullaniciSirketler)
                .HasForeignKey(ks => ks.KullaniciId)
                .OnDelete(DeleteBehavior.Restrict); // Kullanıcı silinirse kayıtlar kalsın

            modelBuilder.Entity<KullaniciSirket>()
                .HasOne(ks => ks.Sirket)
                .WithMany(s => s.KullaniciSirketleri)
                .HasForeignKey(ks => ks.SirketId)
                .OnDelete(DeleteBehavior.Restrict); // Şirket silinirse kayıtlar kalsın (veya uygun aksiyon)

            modelBuilder.Entity<Vardiya>()
                .HasOne(v => v.Sirket)
                .WithMany(s => s.Vardiyalar) // Varsayım: Sirket entity'sinde Vardiyalar ICollection'ı var.
                .HasForeignKey(v => v.SirketId)
                .OnDelete(DeleteBehavior.Restrict);

            // GorevTanimi - Departman
            modelBuilder.Entity<GorevTanimi>()
                .HasOne(gt => gt.Departman)
                .WithMany()
                .HasForeignKey(gt => gt.DepartmanId)
                .OnDelete(DeleteBehavior.Restrict);

            // GorevTanimi - Sirket
            modelBuilder.Entity<GorevTanimi>()
                .HasOne(gt => gt.Sirket)
                .WithMany()
                .HasForeignKey(gt => gt.SirketId)
                .OnDelete(DeleteBehavior.Restrict);

            // IzinHakki - Personel
            modelBuilder.Entity<IzinHakki>()
                .HasOne(ih => ih.Personel)
                .WithMany()
                .HasForeignKey(ih => ih.PersonelId)
                .OnDelete(DeleteBehavior.Cascade);

            // IzinHakki - Sirket
            modelBuilder.Entity<IzinHakki>()
                .HasOne(ih => ih.Sirket)
                .WithMany()
                .HasForeignKey(ih => ih.SirketId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OnayAkisi>(entity =>
            {
                entity.ToTable("OnayAkislari");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.AkisAdi).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ModulTipi).IsRequired().HasMaxLength(50);

                entity.HasOne(e => e.Sirket)
                    .WithMany()
                    .HasForeignKey(e => e.SirketId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // 🆕 OnayAdimi konfigürasyonu
            modelBuilder.Entity<OnayAdimi>(entity =>
            {
                entity.ToTable("OnayAdimlari");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.AdimAdi).IsRequired().HasMaxLength(100);
                entity.Property(e => e.OnaylayanTipi).HasMaxLength(50);

                entity.HasOne(e => e.OnayAkisi)
                    .WithMany(a => a.OnayAdimlari)
                    .HasForeignKey(e => e.OnayAkisiId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // 🆕 OnayKaydi konfigürasyonu
            modelBuilder.Entity<OnayKaydi>(entity =>
            {
                entity.ToTable("OnayKayitlari");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ModulTipi).IsRequired().HasMaxLength(50);
                entity.Property(e => e.GenelDurum).IsRequired().HasMaxLength(50);

                entity.HasOne(e => e.OnayAkisi)
                    .WithMany()
                    .HasForeignKey(e => e.OnayAkisiId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.TalepEdenKullanici)
                    .WithMany()
                    .HasForeignKey(e => e.TalepEdenKullaniciId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // 🆕 OnayDetay konfigürasyonu
            modelBuilder.Entity<OnayDetay>(entity =>
            {
                entity.ToTable("OnayDetaylari");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Durum).IsRequired().HasMaxLength(50);

                entity.HasOne(e => e.OnayKaydi)
                    .WithMany(k => k.OnayDetaylari)
                    .HasForeignKey(e => e.OnayKaydiId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // AvansTalebi - Personel
            modelBuilder.Entity<AvansTalebi>()
                .HasOne(at => at.Personel)
                .WithMany()
                .HasForeignKey(at => at.PersonelId)
                .OnDelete(DeleteBehavior.Cascade);

            // AvansTalebi - Sirket
            modelBuilder.Entity<AvansTalebi>()
                .HasOne(at => at.Sirket)
                .WithMany()
                .HasForeignKey(at => at.SirketId)
                .OnDelete(DeleteBehavior.Restrict);

            // Arac - TahsisliPersonel
            modelBuilder.Entity<Arac>()
                .HasOne(a => a.TahsisliPersonel)
                .WithMany()
                .HasForeignKey(a => a.TahsisliPersonelId)
                .OnDelete(DeleteBehavior.Restrict);

            // Arac - Sirket
            modelBuilder.Entity<Arac>()
                .HasOne(a => a.Sirket)
                .WithMany()
                .HasForeignKey(a => a.SirketId)
                .OnDelete(DeleteBehavior.Restrict);

            // AracTalebi - Personel
            modelBuilder.Entity<AracTalebi>()
                .HasOne(at => at.Personel)
                .WithMany()
                .HasForeignKey(at => at.PersonelId)
                .OnDelete(DeleteBehavior.Cascade);

            // AracTalebi - Arac
            modelBuilder.Entity<AracTalebi>()
                .HasOne(at => at.Arac)
                .WithMany(a => a.AracTalepleri)
                .HasForeignKey(at => at.AracId)
                .OnDelete(DeleteBehavior.Restrict);

            // AracTalebi - Sirket
            modelBuilder.Entity<AracTalebi>()
                .HasOne(at => at.Sirket)
                .WithMany()
                .HasForeignKey(at => at.SirketId)
                .OnDelete(DeleteBehavior.Restrict);

            // Zimmet - Personel
            modelBuilder.Entity<Zimmet>()
                .HasOne(z => z.Personel)
                .WithMany()
                .HasForeignKey(z => z.PersonelId)
                .OnDelete(DeleteBehavior.Cascade);

            // Zimmet - Sirket
            modelBuilder.Entity<Zimmet>()
                .HasOne(z => z.Sirket)
                .WithMany()
                .HasForeignKey(z => z.SirketId)
                .OnDelete(DeleteBehavior.Restrict);

            // Gorev - Atayan
            modelBuilder.Entity<Gorev>()
                .HasOne(g => g.Atayan)
                .WithMany()
                .HasForeignKey(g => g.AtayanPersonelId)
                .OnDelete(DeleteBehavior.Restrict);

            // Gorev - UstGorev (Self-Referencing)
            modelBuilder.Entity<Gorev>()
                .HasOne(g => g.UstGorev)
                .WithMany(g => g.AltGorevler)
                .HasForeignKey(g => g.UstGorevId)
                .OnDelete(DeleteBehavior.Restrict);

            // Gorev - Sirket
            modelBuilder.Entity<Gorev>()
                .HasOne(g => g.Sirket)
                .WithMany()
                .HasForeignKey(g => g.SirketId)
                .OnDelete(DeleteBehavior.Restrict);

            // GorevAtama - Gorev
            modelBuilder.Entity<GorevAtama>()
                .HasOne(ga => ga.Gorev)
                .WithMany(g => g.GorevAtamalari)
                .HasForeignKey(ga => ga.GorevId)
                .OnDelete(DeleteBehavior.Cascade);

            // GorevAtama - Personel
            modelBuilder.Entity<GorevAtama>()
                .HasOne(ga => ga.Personel)
                .WithMany()
                .HasForeignKey(ga => ga.PersonelId)
                .OnDelete(DeleteBehavior.Restrict);

            // MazeretBildirimi - Personel
            modelBuilder.Entity<MazeretBildirimi>()
                .HasOne(mb => mb.Personel)
                .WithMany()
                .HasForeignKey(mb => mb.PersonelId)
                .OnDelete(DeleteBehavior.Cascade);

            // MazeretBildirimi - Onaylayici
            modelBuilder.Entity<MazeretBildirimi>()
                .HasOne(mb => mb.Onaylayici)
                .WithMany()
                .HasForeignKey(mb => mb.OnaylayiciPersonelId)
                .OnDelete(DeleteBehavior.Restrict);

            // MazeretBildirimi - Sirket
            modelBuilder.Entity<MazeretBildirimi>()
                .HasOne(mb => mb.Sirket)
                .WithMany()
                .HasForeignKey(mb => mb.SirketId)
                .OnDelete(DeleteBehavior.Restrict);

            // ToplantiOdasi - Sirket
            modelBuilder.Entity<ToplantiOdasi>()
                .HasOne(to => to.Sirket)
                .WithMany()
                .HasForeignKey(to => to.SirketId)
                .OnDelete(DeleteBehavior.Restrict);

            // ToplantiOdasiRezervasyon - Oda
            modelBuilder.Entity<ToplantiOdasiRezervasyon>()
                .HasOne(tor => tor.Oda)
                .WithMany(to => to.Rezervasyonlar)
                .HasForeignKey(tor => tor.OdaId)
                .OnDelete(DeleteBehavior.Cascade);

            // ToplantiOdasiRezervasyon - Personel
            modelBuilder.Entity<ToplantiOdasiRezervasyon>()
                .HasOne(tor => tor.Personel)
                .WithMany()
                .HasForeignKey(tor => tor.PersonelId)
                .OnDelete(DeleteBehavior.Restrict);

            // ToplantiOdasiRezervasyon - Sirket
            modelBuilder.Entity<ToplantiOdasiRezervasyon>()
                .HasOne(tor => tor.Sirket)
                .WithMany()
                .HasForeignKey(tor => tor.SirketId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DeviceToken>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Token).IsRequired().HasMaxLength(500);
                entity.Property(e => e.Platform).HasMaxLength(20);
                entity.Property(e => e.DeviceInfo).HasMaxLength(500);

                entity.HasIndex(e => new { e.KullaniciId, e.Token }).IsUnique();

                entity.HasOne(e => e.Kullanici)
                    .WithMany()
                    .HasForeignKey(e => e.KullaniciId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ============== YENİ PERSONEL ÖZLÜK İLİŞKİLERİ ==============

            // PersonelAile - Personel (Many-to-One)
            modelBuilder.Entity<PersonelAile>()
                .HasOne(pa => pa.Personel)
                .WithMany(p => p.AileBilgileri)
                .HasForeignKey(pa => pa.PersonelId)
                .OnDelete(DeleteBehavior.Cascade);

            // PersonelAcilDurum - Personel (Many-to-One)
            modelBuilder.Entity<PersonelAcilDurum>()
                .HasOne(pad => pad.Personel)
                .WithMany(p => p.AcilDurumBilgileri)
                .HasForeignKey(pad => pad.PersonelId)
                .OnDelete(DeleteBehavior.Cascade);

            // PersonelSaglik - Personel (One-to-One)
            modelBuilder.Entity<PersonelSaglik>()
                .HasOne(ps => ps.Personel)
                .WithOne(p => p.SaglikBilgisi)
                .HasForeignKey<PersonelSaglik>(ps => ps.PersonelId)
                .OnDelete(DeleteBehavior.Cascade);

            // PersonelEgitim - Personel (Many-to-One)
            modelBuilder.Entity<PersonelEgitim>()
                .HasOne(pe => pe.Personel)
                .WithMany(p => p.EgitimGecmisi)
                .HasForeignKey(pe => pe.PersonelId)
                .OnDelete(DeleteBehavior.Cascade);

            // PersonelIsDeneyimi - Personel (Many-to-One)
            modelBuilder.Entity<PersonelIsDeneyimi>()
                .HasOne(pid => pid.Personel)
                .WithMany(p => p.IsDeneyimleri)
                .HasForeignKey(pid => pid.PersonelId)
                .OnDelete(DeleteBehavior.Cascade);

            // PersonelDil - Personel (Many-to-One)
            modelBuilder.Entity<PersonelDil>()
                .HasOne(pd => pd.Personel)
                .WithMany(p => p.DilBecerileri)
                .HasForeignKey(pd => pd.PersonelId)
                .OnDelete(DeleteBehavior.Cascade);

            // PersonelSertifika - Personel (Many-to-One)
            modelBuilder.Entity<PersonelSertifika>()
                .HasOne(ps => ps.Personel)
                .WithMany(p => p.Sertifikalar)
                .HasForeignKey(ps => ps.PersonelId)
                .OnDelete(DeleteBehavior.Cascade);

            // PersonelPerformans - Personel (Many-to-One)
            modelBuilder.Entity<PersonelPerformans>()
                .HasOne(pp => pp.Personel)
                .WithMany(p => p.PerformansKayitlari)
                .HasForeignKey(pp => pp.PersonelId)
                .OnDelete(DeleteBehavior.Cascade);

            // PersonelPerformans - DegerlendiriciKullanici (Many-to-One)
            modelBuilder.Entity<PersonelPerformans>()
                .HasOne(pp => pp.DegerlendiriciKullanici)
                .WithMany()
                .HasForeignKey(pp => pp.DegerlendiriciKullaniciId)
                .OnDelete(DeleteBehavior.Restrict);

            // PersonelPerformans - OnaylayanKullanici (Many-to-One)
            modelBuilder.Entity<PersonelPerformans>()
                .HasOne(pp => pp.OnaylayanKullanici)
                .WithMany()
                .HasForeignKey(pp => pp.OnaylayanKullaniciId)
                .OnDelete(DeleteBehavior.Restrict);

            // PersonelDisiplin - Personel (Many-to-One)
            modelBuilder.Entity<PersonelDisiplin>()
                .HasOne(pd => pd.Personel)
                .WithMany(p => p.DisiplinKayitlari)
                .HasForeignKey(pd => pd.PersonelId)
                .OnDelete(DeleteBehavior.Cascade);

            // PersonelDisiplin - KararVerenYetkili (Many-to-One)
            modelBuilder.Entity<PersonelDisiplin>()
                .HasOne(pd => pd.KararVerenYetkili)
                .WithMany()
                .HasForeignKey(pd => pd.KararVerenYetkiliId)
                .OnDelete(DeleteBehavior.Restrict);

            // PersonelDisiplin - IptalEdenKullanici (Many-to-One)
            modelBuilder.Entity<PersonelDisiplin>()
                .HasOne(pd => pd.IptalEdenKullanici)
                .WithMany()
                .HasForeignKey(pd => pd.IptalEdenKullaniciId)
                .OnDelete(DeleteBehavior.Restrict);

            // PersonelTerfi - Personel (Many-to-One)
            modelBuilder.Entity<PersonelTerfi>()
                .HasOne(pt => pt.Personel)
                .WithMany(p => p.TerfiGecmisi)
                .HasForeignKey(pt => pt.PersonelId)
                .OnDelete(DeleteBehavior.Cascade);

            // PersonelTerfi - OnaylayanKullanici (Many-to-One)
            modelBuilder.Entity<PersonelTerfi>()
                .HasOne(pt => pt.OnaylayanKullanici)
                .WithMany()
                .HasForeignKey(pt => pt.OnaylayanKullaniciId)
                .OnDelete(DeleteBehavior.Restrict);

            // PersonelTerfi - EskiDepartman (Many-to-One)
            modelBuilder.Entity<PersonelTerfi>()
                .HasOne(pt => pt.EskiDepartman)
                .WithMany()
                .HasForeignKey(pt => pt.EskiDepartmanId)
                .OnDelete(DeleteBehavior.Restrict);

            // PersonelTerfi - YeniDepartman (Many-to-One)
            modelBuilder.Entity<PersonelTerfi>()
                .HasOne(pt => pt.YeniDepartman)
                .WithMany()
                .HasForeignKey(pt => pt.YeniDepartmanId)
                .OnDelete(DeleteBehavior.Restrict);

            // PersonelUcretDegisiklik - Personel (Many-to-One)
            modelBuilder.Entity<PersonelUcretDegisiklik>()
                .HasOne(pud => pud.Personel)
                .WithMany(p => p.UcretDegisiklikleri)
                .HasForeignKey(pud => pud.PersonelId)
                .OnDelete(DeleteBehavior.Cascade);

            // PersonelUcretDegisiklik - OnaylayanKullanici (Many-to-One)
            modelBuilder.Entity<PersonelUcretDegisiklik>()
                .HasOne(pud => pud.OnaylayanKullanici)
                .WithMany()
                .HasForeignKey(pud => pud.OnaylayanKullaniciId)
                .OnDelete(DeleteBehavior.Restrict);

            // PersonelReferans - Personel (Many-to-One)
            modelBuilder.Entity<PersonelReferans>()
                .HasOne(pr => pr.Personel)
                .WithMany(p => p.Referanslar)
                .HasForeignKey(pr => pr.PersonelId)
                .OnDelete(DeleteBehavior.Cascade);

            // PersonelZimmet - Personel (Many-to-One)
            modelBuilder.Entity<PersonelZimmet>()
                .HasOne(pz => pz.Personel)
                .WithMany(p => p.ZimmetliEsyalar)
                .HasForeignKey(pz => pz.PersonelId)
                .OnDelete(DeleteBehavior.Cascade);

            // PersonelZimmet - ZimmetVerenKullanici (Many-to-One)
            modelBuilder.Entity<PersonelZimmet>()
                .HasOne(pz => pz.ZimmetVerenKullanici)
                .WithMany()
                .HasForeignKey(pz => pz.ZimmetVerenKullaniciId)
                .OnDelete(DeleteBehavior.Restrict);

            // PersonelZimmet - IadeTeslimAlanKullanici (Many-to-One)
            modelBuilder.Entity<PersonelZimmet>()
                .HasOne(pz => pz.IadeTeslimAlanKullanici)
                .WithMany()
                .HasForeignKey(pz => pz.IadeTeslimAlanKullaniciId)
                .OnDelete(DeleteBehavior.Restrict);

            // PersonelYetkinlik - Personel (Many-to-One)
            modelBuilder.Entity<PersonelYetkinlik>()
                .HasOne(py => py.Personel)
                .WithMany(p => p.Yetkinlikler)
                .HasForeignKey(py => py.PersonelId)
                .OnDelete(DeleteBehavior.Cascade);

            // PersonelYetkinlik - DegerlendiriciKullanici (Many-to-One)
            modelBuilder.Entity<PersonelYetkinlik>()
                .HasOne(py => py.DegerlendiriciKullanici)
                .WithMany()
                .HasForeignKey(py => py.DegerlendiriciKullaniciId)
                .OnDelete(DeleteBehavior.Restrict);

            // PersonelEgitimKayit - Personel (Many-to-One)
            modelBuilder.Entity<PersonelEgitimKayit>()
                .HasOne(pek => pek.Personel)
                .WithMany(p => p.EgitimKayitlari)
                .HasForeignKey(pek => pek.PersonelId)
                .OnDelete(DeleteBehavior.Cascade);

            // PersonelMaliBilgi - Personel (One-to-One)
            modelBuilder.Entity<PersonelMaliBilgi>()
                .HasOne(pmb => pmb.Personel)
                .WithOne(p => p.MaliBilgi)
                .HasForeignKey<PersonelMaliBilgi>(pmb => pmb.PersonelId)
                .OnDelete(DeleteBehavior.Cascade);

            // PersonelEkBilgi - Personel (One-to-One)
            modelBuilder.Entity<PersonelEkBilgi>()
                .HasOne(peb => peb.Personel)
                .WithOne(p => p.EkBilgi)
                .HasForeignKey<PersonelEkBilgi>(peb => peb.PersonelId)
                .OnDelete(DeleteBehavior.Cascade);

            // ============== PERSONEL SELF-REFERENCING İLİŞKİLER ==============

            // Personel - YoneticiPersonel (Self-Referencing Many-to-One)
            modelBuilder.Entity<Personel>()
                .HasOne(p => p.YoneticiPersonel)
                .WithMany(p => p.AltCalisanlar)
                .HasForeignKey(p => p.YoneticiPersonelId)
                .OnDelete(DeleteBehavior.Restrict);

            // Personel - IkinciAmirPersonel (Self-Referencing Many-to-One)
            modelBuilder.Entity<Personel>()
                .HasOne(p => p.IkinciAmirPersonel)
                .WithMany(p => p.IkinciAmirOlduguCalisanlar)
                .HasForeignKey(p => p.IkinciAmirPersonelId)
                .OnDelete(DeleteBehavior.Restrict);

            // ============== INDEXES ==============

            // PersonelSertifika için sertifika numarası index
            modelBuilder.Entity<PersonelSertifika>()
                .HasIndex(ps => ps.SertifikaNumarasi);

            // PersonelSertifika için durum ve geçerlilik tarihi index (hatırlatma sistemi için)
            modelBuilder.Entity<PersonelSertifika>()
                .HasIndex(ps => new { ps.Durum, ps.GecerlilikTarihi });

            // PersonelPerformans için dönem ve tarih index
            modelBuilder.Entity<PersonelPerformans>()
                .HasIndex(pp => new { pp.PersonelId, pp.DegerlendirmeTarihi });

            // PersonelZimmet için zimmet durumu index
            modelBuilder.Entity<PersonelZimmet>()
                .HasIndex(pz => new { pz.PersonelId, pz.ZimmetDurumu });

            // PersonelTerfi için terfi tarihi index
            modelBuilder.Entity<PersonelTerfi>()
                .HasIndex(pt => new { pt.PersonelId, pt.TerfiTarihi });

            // PersonelUcretDegisiklik için değişiklik tarihi index
            modelBuilder.Entity<PersonelUcretDegisiklik>()
                .HasIndex(pud => new { pud.PersonelId, pud.DegisiklikTarihi });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.ToTable("Menuler");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.MenuAdi).IsRequired().HasMaxLength(100);
                entity.Property(e => e.MenuKodu).HasMaxLength(200);
                entity.Property(e => e.Url).HasMaxLength(200);
                entity.Property(e => e.Icon).HasMaxLength(100);

                entity.HasOne(e => e.UstMenu)
                    .WithMany(e => e.AltMenuler)
                    .HasForeignKey(e => e.UstMenuId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.MenuKodu).IsUnique();
            });

            // YENİ: MenuRol konfigürasyonu
            modelBuilder.Entity<MenuRol>(entity =>
            {
                entity.ToTable("MenuRoller");
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.Menu)
                    .WithMany(m => m.MenuRoller)
                    .HasForeignKey(e => e.MenuId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Rol)
                    .WithMany(r => r.MenuRoller)
                    .HasForeignKey(e => e.RolId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => new { e.MenuId, e.RolId }).IsUnique();
            });

            // YENİ: IslemYetki konfigürasyonu
            modelBuilder.Entity<IslemYetki>(entity =>
            {
                entity.ToTable("IslemYetkiler");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.IslemKodu).IsRequired().HasMaxLength(200);
                entity.Property(e => e.IslemAdi).IsRequired().HasMaxLength(200);
                entity.Property(e => e.ModulAdi).HasMaxLength(100);
                entity.Property(e => e.Aciklama).HasMaxLength(500);

                entity.HasIndex(e => e.IslemKodu).IsUnique();
            });

            // YENİ: RolIslemYetki konfigürasyonu
            modelBuilder.Entity<RolIslemYetki>(entity =>
            {
                entity.ToTable("RolIslemYetkiler");
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.Rol)
                    .WithMany(r => r.RolIslemYetkiler)
                    .HasForeignKey(e => e.RolId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.IslemYetki)
                    .WithMany(i => i.RolIslemYetkiler)
                    .HasForeignKey(e => e.IslemYetkiId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => new { e.RolId, e.IslemYetkiId }).IsUnique();
            });

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