using PDKS.Data.Entities;

namespace PDKS.Data.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        // ============================================
        // ESKİ REPOSITORY'LER
        // ============================================
        IRepository<Personel> Personeller { get; }
        IRepository<Departman> Departmanlar { get; }
        IRepository<Cihaz> Cihazlar { get; }
        IRepository<Vardiya> Vardiyalar { get; }
        IRepository<GirisCikis> GirisCikislar { get; }
        IRepository<Izin> Izinler { get; }
        IRepository<Rol> Roller { get; }
        IRepository<Kullanici> Kullanicilar { get; }
        IRepository<Log> Loglar { get; }
        IRepository<Bildirim> Bildirimler { get; }
        IGenericRepository<Tatil> Tatiller { get; }
        IRepository<Parametre> Parametreler { get; }
        IRepository<Mesai> Mesailer { get; }
        IGenericRepository<Sirket> Sirketler { get; }
        IGenericRepository<Avans> Avanslar { get; }
        IGenericRepository<CihazLog> CihazLoglari { get; }
        IGenericRepository<PersonelTransferGecmisi> PersonelTransferGecmisleri { get; }
        IGenericRepository<Prim> Primler { get; }
        IGenericRepository<KullaniciSirket> KullaniciSirketler { get; }
        // ============================================
        // PERSONEL ÖZLÜK REPOSITORY'LERİ - ÖZEL TİPLER!
        // ============================================
        IPersonelAileRepository PersonelAileBilgileri { get; }
        IPersonelAcilDurumRepository PersonelAcilDurumlar { get; }
        IPersonelSaglikRepository PersonelSagliklar { get; }
        IPersonelEgitimRepository PersonelEgitimler { get; }
        IPersonelIsDeneyimiRepository PersonelIsDeneyimleri { get; }
        IPersonelDilRepository PersonelDiller { get; }
        IPersonelSertifikaRepository PersonelSertifikalar { get; }
        IPersonelPerformansRepository PersonelPerformanslar { get; }
        IPersonelDisiplinRepository PersonelDisiplinler { get; }
        IPersonelTerfiRepository PersonelTerfiler { get; }
        IPersonelUcretDegisiklikRepository PersonelUcretDegisiklikler { get; }
        IPersonelReferansRepository PersonelReferanslar { get; }
        IPersonelZimmetRepository PersonelZimmetler { get; }
        IPersonelYetkinlikRepository PersonelYetkinlikler { get; }
        IPersonelEgitimKayitRepository PersonelEgitimKayitlari { get; }
        IPersonelMaliBilgiRepository PersonelMaliBilgileri { get; }
        IPersonelEkBilgiRepository PersonelEkBilgileri { get; }

        IGenericRepository<OnayAkisi> OnayAkislari { get; }
        IGenericRepository<OnayAdimi> OnayAdimlari { get; }
        IGenericRepository<OnayKaydi> OnayKayitlari { get; }
        IGenericRepository<OnayDetay> OnayDetaylari { get; }
        IGenericRepository<DeviceToken> DeviceTokenlari { get; }

        // ============================================
        // TEMEL METODLAR
        // ============================================
        Task<int> SaveChangesAsync();
    }
}
