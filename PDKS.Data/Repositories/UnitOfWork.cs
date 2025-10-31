using PDKS.Data.Context;
using PDKS.Data.Entities;

namespace PDKS.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PDKSDbContext _context;

        // Eski repository'ler
        private IRepository<Personel>? _personeller;
        private IRepository<Departman>? _departmanlar;
        private IRepository<Cihaz>? _cihazlar;
        private IRepository<Vardiya>? _vardiyalar;
        private IRepository<GirisCikis>? _girisCikislar;
        private IRepository<Izin>? _izinler;
        private IRepository<Rol>? _roller;
        private IKullaniciRepository? _kullanicilar;
        private IRepository<Log>? _loglar;
        private IRepository<Bildirim>? _bildirimler;
        private IGenericRepository<Tatil>? _tatiller;
        private IRepository<Parametre>? _parametreler;
        private IRepository<Mesai>? _mesailer;
        private IGenericRepository<Sirket>? _sirketler;
        private IGenericRepository<Avans>? _avanslar;
        private IGenericRepository<CihazLog>? _cihazLoglari;
        private IGenericRepository<OnayAkisi>? _onayAkislari;
        private IGenericRepository<OnayAdimi>? _onayAdimlari;
        private IGenericRepository<OnayKaydi>? _onayKayitlari;
        private IGenericRepository<OnayDetay>? _onayDetaylari;
        private IGenericRepository<PersonelTransferGecmisi>? _personelTransferGecmisleri;
        private IGenericRepository<Prim>? _primler;
        private IGenericRepository<KullaniciSirket>? _kullaniciSirketler;
        // Personel Özlük repository'leri
        private IPersonelAileRepository? _personelAileBilgileri;
        private IPersonelAcilDurumRepository? _personelAcilDurumlar;
        private IPersonelSaglikRepository? _personelSagliklar;
        private IPersonelEgitimRepository? _personelEgitimler;
        private IPersonelIsDeneyimiRepository? _personelIsDeneyimleri;
        private IPersonelDilRepository? _personelDiller;
        private IPersonelSertifikaRepository? _personelSertifikalar;
        private IPersonelPerformansRepository? _personelPerformanslar;
        private IPersonelDisiplinRepository? _personelDisiplinler;
        private IPersonelTerfiRepository? _personelTerfiler;
        private IPersonelUcretDegisiklikRepository? _personelUcretDegisiklikler;
        private IPersonelReferansRepository? _personelReferanslar;
        private IPersonelZimmetRepository? _personelZimmetler;
        private IPersonelYetkinlikRepository? _personelYetkinlikler;
        private IPersonelEgitimKayitRepository? _personelEgitimKayitlari;
        private IPersonelMaliBilgiRepository? _personelMaliBilgileri;
        private IPersonelEkBilgiRepository? _personelEkBilgileri;
        private IGenericRepository<DeviceToken>? _deviceTokenlari;
        private IMenuRepository? _menuler;
        private IMenuRolRepository? _menuRoller;
        private IIslemYetkiRepository? _islemYetkiler;
        private IRolIslemYetkiRepository? _rolIslemYetkiler;
        private IRepository<Puantaj> _puantajlar;
        private IRepository<PuantajDetay> _puantajDetaylar;
        public UnitOfWork(PDKSDbContext context)
        {
            _context = context;
        }

        #region Eski Repository Property'leri


        public IRepository<Personel> Personeller
        {
            get { return _personeller ??= new PersonelRepository(_context); }
        }

        public IRepository<Departman> Departmanlar
        {
            get { return _departmanlar ??= new DepartmanRepository(_context); }
        }

        public IRepository<Cihaz> Cihazlar
        {
            get { return _cihazlar ??= new CihazRepository(_context); }
        }

        public IRepository<Vardiya> Vardiyalar
        {
            get { return _vardiyalar ??= new VardiyaRepository(_context); }
        }

        public IRepository<GirisCikis> GirisCikislar
        {
            get { return _girisCikislar ??= new GirisCikisRepository(_context); }
        }

        public IRepository<Izin> Izinler
        {
            get { return _izinler ??= new IzinRepository(_context); }
        }

        public IRepository<Rol> Roller
        {
            get { return _roller ??= new RolRepository(_context); }
        }

        public IKullaniciRepository Kullanicilar
        {
            get { return _kullanicilar ??= new KullaniciRepository(_context); }
        }

        public IRepository<Log> Loglar
        {
            get { return _loglar ??= new LogRepository(_context); }
        }

        public IRepository<Bildirim> Bildirimler
        {
            get { return _bildirimler ??= new BildirimRepository(_context); }
        }

        public IGenericRepository<Tatil> Tatiller
        {
            get { return _tatiller ??= new TatilRepository(_context); }
        }


        public IRepository<Parametre> Parametreler
        {
            get { return _parametreler ??= new ParametreRepository(_context); }
        }

        public IGenericRepository<CihazLog> CihazLoglar
        {
            get
            {
                if (_cihazLoglari == null)
                    _cihazLoglari = new CihazLogRepository(_context);
                return _cihazLoglari;
            }
        }

        public IRepository<Mesai> Mesailer
        {
            get { return _mesailer ??= new MesaiRepository(_context); }
        }

        public IGenericRepository<Sirket> Sirketler
        {
            get
            {
                if (_sirketler == null)
                    _sirketler = new SirketRepository(_context);
                return _sirketler;
            }
        }

        public IGenericRepository<Avans> Avanslar
        {
            get
            {
                if (_avanslar == null)
                    _avanslar = new AvansRepository(_context);
                return _avanslar;
            }
        }

        public IGenericRepository<CihazLog> CihazLoglari
        {
            get
            {
                if (_cihazLoglari == null)
                    _cihazLoglari = new CihazLogRepository(_context);
                return _cihazLoglari;
            }
        }


        #endregion

        #region Personel Özlük Repository Property'leri

        public IPersonelAileRepository PersonelAileBilgileri
        {
            get
            {
                if (_personelAileBilgileri == null)
                    _personelAileBilgileri = new PersonelAileRepository(_context);
                return _personelAileBilgileri;
            }
        }

        public IPersonelAcilDurumRepository PersonelAcilDurumlar
        {
            get
            {
                if (_personelAcilDurumlar == null)
                    _personelAcilDurumlar = new PersonelAcilDurumRepository(_context);
                return _personelAcilDurumlar;
            }
        }

        public IPersonelSaglikRepository PersonelSagliklar
        {
            get
            {
                if (_personelSagliklar == null)
                    _personelSagliklar = new PersonelSaglikRepository(_context);
                return _personelSagliklar;
            }
        }

        public IPersonelEgitimRepository PersonelEgitimler
        {
            get
            {
                if (_personelEgitimler == null)
                    _personelEgitimler = new PersonelEgitimRepository(_context);
                return _personelEgitimler;
            }
        }

        public IPersonelIsDeneyimiRepository PersonelIsDeneyimleri
        {
            get
            {
                if (_personelIsDeneyimleri == null)
                    _personelIsDeneyimleri = new PersonelIsDeneyimiRepository(_context);
                return _personelIsDeneyimleri;
            }
        }

        public IPersonelDilRepository PersonelDiller
        {
            get
            {
                if (_personelDiller == null)
                    _personelDiller = new PersonelDilRepository(_context);
                return _personelDiller;
            }
        }

        public IPersonelSertifikaRepository PersonelSertifikalar
        {
            get
            {
                if (_personelSertifikalar == null)
                    _personelSertifikalar = new PersonelSertifikaRepository(_context);
                return _personelSertifikalar;
            }
        }

        public IPersonelPerformansRepository PersonelPerformanslar
        {
            get
            {
                if (_personelPerformanslar == null)
                    _personelPerformanslar = new PersonelPerformansRepository(_context);
                return _personelPerformanslar;
            }
        }

        public IPersonelDisiplinRepository PersonelDisiplinler
        {
            get
            {
                if (_personelDisiplinler == null)
                    _personelDisiplinler = new PersonelDisiplinRepository(_context);
                return _personelDisiplinler;
            }
        }

        public IPersonelTerfiRepository PersonelTerfiler
        {
            get
            {
                if (_personelTerfiler == null)
                    _personelTerfiler = new PersonelTerfiRepository(_context);
                return _personelTerfiler;
            }
        }

        public IPersonelUcretDegisiklikRepository PersonelUcretDegisiklikler
        {
            get
            {
                if (_personelUcretDegisiklikler == null)
                    _personelUcretDegisiklikler = new PersonelUcretDegisiklikRepository(_context);
                return _personelUcretDegisiklikler;
            }
        }

        public IPersonelReferansRepository PersonelReferanslar
        {
            get
            {
                if (_personelReferanslar == null)
                    _personelReferanslar = new PersonelReferansRepository(_context);
                return _personelReferanslar;
            }
        }

        public IPersonelZimmetRepository PersonelZimmetler
        {
            get
            {
                if (_personelZimmetler == null)
                    _personelZimmetler = new PersonelZimmetRepository(_context);
                return _personelZimmetler;
            }
        }

        public IPersonelYetkinlikRepository PersonelYetkinlikler
        {
            get
            {
                if (_personelYetkinlikler == null)
                    _personelYetkinlikler = new PersonelYetkinlikRepository(_context);
                return _personelYetkinlikler;
            }
        }

        public IPersonelEgitimKayitRepository PersonelEgitimKayitlari
        {
            get
            {
                if (_personelEgitimKayitlari == null)
                    _personelEgitimKayitlari = new PersonelEgitimKayitRepository(_context);
                return _personelEgitimKayitlari;
            }
        }

        public IPersonelMaliBilgiRepository PersonelMaliBilgileri
        {
            get
            {
                if (_personelMaliBilgileri == null)
                    _personelMaliBilgileri = new PersonelMaliBilgiRepository(_context);
                return _personelMaliBilgileri;
            }
        }

        public IPersonelEkBilgiRepository PersonelEkBilgileri
        {
            get
            {
                if (_personelEkBilgileri == null)
                    _personelEkBilgileri = new PersonelEkBilgiRepository(_context);
                return _personelEkBilgileri;
            }
        }

        public IGenericRepository<OnayAkisi> OnayAkislari
        {
            get
            {
                if (_onayAkislari == null)
                    _onayAkislari = new OnayAkisiRepository(_context);
                return _onayAkislari;
            }
        }

        public IGenericRepository<OnayAdimi> OnayAdimlari
        {
            get
            {
                if (_onayAdimlari == null)
                    _onayAdimlari = new OnayAdimiRepository(_context);
                return _onayAdimlari;
            }
        }

        public IGenericRepository<OnayKaydi> OnayKayitlari
        {
            get
            {
                if (_onayKayitlari == null)
                    _onayKayitlari = new OnayKaydiRepository(_context);
                return _onayKayitlari;
            }
        }

        public IGenericRepository<OnayDetay> OnayDetaylari
        {
            get
            {
                if (_onayDetaylari == null)
                    _onayDetaylari = new OnayDetayRepository(_context);
                return _onayDetaylari;
            }
        }

        public IGenericRepository<PersonelTransferGecmisi> PersonelTransferGecmisleri
        {
            get
            {
                if (_personelTransferGecmisleri == null)
                    _personelTransferGecmisleri = new PersonelTransferGecmisiRepository(_context);
                return _personelTransferGecmisleri;
            }
        }

        public IGenericRepository<Prim> Primler
        {
            get
            {
                if (_primler == null)
                    _primler = new PrimRepository(_context);
                return _primler;
            }
        }

        public IGenericRepository<DeviceToken> DeviceTokenlari
        {
            get
            {
                if (_deviceTokenlari == null)
                    _deviceTokenlari = new DeviceTokenRepository(_context);
                return _deviceTokenlari;
            }
        }

        public IGenericRepository<KullaniciSirket> KullaniciSirketler
        {
            get
            {
                if (_kullaniciSirketler == null)
                    _kullaniciSirketler = new KullaniciSirketRepository(_context);
                return _kullaniciSirketler;
            }
        }
        #endregion

        public IMenuRepository Menuler =>
            _menuler ??= new MenuRepository(_context);

        public IMenuRolRepository MenuRoller =>
            _menuRoller ??= new MenuRolRepository(_context);

        public IIslemYetkiRepository IslemYetkiler =>
            _islemYetkiler ??= new IslemYetkiRepository(_context);

        public IRolIslemYetkiRepository RolIslemYetkiler =>
            _rolIslemYetkiler ??= new RolIslemYetkiRepository(_context);

        public IRepository<Puantaj> Puantajlar
        {
            get
            {
                if (_puantajlar == null)
                    _puantajlar = new PuantajRepository(_context);
                return _puantajlar;
            }
        }

        public IRepository<PuantajDetay> PuantajDetaylar
        {
            get
            {
                if (_puantajDetaylar == null)
                    _puantajDetaylar = new PuantajDetayRepository(_context);
                return _puantajDetaylar;
            }
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
