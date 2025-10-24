using PDKS.Business.DTOs;
using PDKS.Data.Entities;
using PDKS.Data.Repositories;

namespace PDKS.Business.Services
{
    public class PersonelOzlukService : IPersonelOzlukService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PersonelOzlukService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Aile Bilgileri

        public async Task<IEnumerable<PersonelAileDTO>> GetAileBilgileriAsync(int personelId)
        {
            // ✅ Özel metod - interface'den geliyor
            var aile = await _unitOfWork.PersonelAileBilgileri.GetByPersonelIdAsync(personelId);
            return aile.Select(MapToAileDTO);
        }

        public async Task<PersonelAileDTO> GetAileBilgisiByIdAsync(int id)
        {
            // ✅ IRepository'den gelen metod
            var aile = await _unitOfWork.PersonelAileBilgileri.GetByIdAsync(id);
            return aile == null ? throw new Exception("Aile bilgisi bulunamadı") : MapToAileDTO(aile);
        }

        public async Task<PersonelAileDTO> CreateAileBilgisiAsync(PersonelAileCreateDTO dto)
        {
            var aile = new PersonelAile
            {
                PersonelId = dto.PersonelId,
                YakinlikDerecesi = dto.YakinlikDerecesi,
                AdSoyad = dto.AdSoyad,
                TcKimlikNo = dto.TcKimlikNo,
                DogumTarihi = dto.DogumTarihi,
                Meslek = dto.Meslek,
                CalisiyorMu = dto.CalisiyorMu,
                Telefon = dto.Telefon,
                OgrenciMi = dto.OgrenciMi,
                SGKBagimlisi = dto.SGKBagimlisi,
                Notlar = dto.Notlar,
                KayitTarihi = DateTime.UtcNow
            };

            // ✅ IRepository'den gelen metod
            await _unitOfWork.PersonelAileBilgileri.AddAsync(aile);
            await _unitOfWork.SaveChangesAsync();

            return MapToAileDTO(aile);
        }

        public async Task<bool> UpdateAileBilgisiAsync(int id, PersonelAileCreateDTO dto)
        {
            // ✅ IRepository'den gelen metod
            var aile = await _unitOfWork.PersonelAileBilgileri.GetByIdAsync(id);
            if (aile == null) return false;

            aile.YakinlikDerecesi = dto.YakinlikDerecesi;
            aile.AdSoyad = dto.AdSoyad;
            aile.TcKimlikNo = dto.TcKimlikNo;
            aile.DogumTarihi = dto.DogumTarihi;
            aile.Meslek = dto.Meslek;
            aile.CalisiyorMu = dto.CalisiyorMu;
            aile.Telefon = dto.Telefon;
            aile.OgrenciMi = dto.OgrenciMi;
            aile.SGKBagimlisi = dto.SGKBagimlisi;
            aile.Notlar = dto.Notlar;
            aile.GuncellemeTarihi = DateTime.UtcNow;

            // ✅ IRepository'den gelen metod
            _unitOfWork.PersonelAileBilgileri.Update(aile);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAileBilgisiAsync(int id)
        {
            // ✅ IRepository'den gelen metod
            var aile = await _unitOfWork.PersonelAileBilgileri.GetByIdAsync(id);
            if (aile == null) return false;

            // ✅ Delete değil Remove kullan!
            ((IRepository<PersonelAile>)_unitOfWork.PersonelAileBilgileri).Remove(aile);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        #endregion

        #region Acil Durum

        public async Task<IEnumerable<PersonelAcilDurumDTO>> GetAcilDurumBilgileriAsync(int personelId)
        {
            // ✅ Özel metod - interface'den geliyor
            var acilDurum = await _unitOfWork.PersonelAcilDurumlar.GetByPersonelIdAsync(personelId);
            return acilDurum.Select(MapToAcilDurumDTO);
        }

        public async Task<PersonelAcilDurumDTO> GetAcilDurumByIdAsync(int id)
        {
            // ✅ IRepository'den gelen metod
            var acilDurum = await _unitOfWork.PersonelAcilDurumlar.GetByIdAsync(id);
            return acilDurum == null ? throw new Exception("Acil durum bilgisi bulunamadı") : MapToAcilDurumDTO(acilDurum);
        }

        public async Task<PersonelAcilDurumDTO> CreateAcilDurumAsync(PersonelAcilDurumCreateDTO dto)
        {
            var acilDurum = new PersonelAcilDurum
            {
                PersonelId = dto.PersonelId,
                IletisimTipi = dto.IletisimTipi,
                AdSoyad = dto.AdSoyad,
                YakinlikDerecesi = dto.YakinlikDerecesi,
                Telefon1 = dto.Telefon1,
                Telefon2 = dto.Telefon2,
                Adres = dto.Adres,
                Notlar = dto.Notlar,
                KayitTarihi = DateTime.UtcNow
            };

            // ✅ IRepository'den gelen metod
            await _unitOfWork.PersonelAcilDurumlar.AddAsync(acilDurum);
            await _unitOfWork.SaveChangesAsync();

            return MapToAcilDurumDTO(acilDurum);
        }

        public async Task<bool> UpdateAcilDurumAsync(int id, PersonelAcilDurumCreateDTO dto)
        {
            // ✅ IRepository'den gelen metod
            var acilDurum = await _unitOfWork.PersonelAcilDurumlar.GetByIdAsync(id);
            if (acilDurum == null) return false;

            acilDurum.IletisimTipi = dto.IletisimTipi;
            acilDurum.AdSoyad = dto.AdSoyad;
            acilDurum.YakinlikDerecesi = dto.YakinlikDerecesi;
            acilDurum.Telefon1 = dto.Telefon1;
            acilDurum.Telefon2 = dto.Telefon2;
            acilDurum.Adres = dto.Adres;
            acilDurum.Notlar = dto.Notlar;
            acilDurum.GuncellemeTarihi = DateTime.UtcNow;

            // ✅ IRepository'den gelen metod
            _unitOfWork.PersonelAcilDurumlar.Update(acilDurum);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAcilDurumAsync(int id)
        {
            // ✅ IRepository'den gelen metod
            var acilDurum = await _unitOfWork.PersonelAcilDurumlar.GetByIdAsync(id);
            if (acilDurum == null) return false;

            // ✅ Delete değil Remove kullan!
            ((IRepository<PersonelAcilDurum>)_unitOfWork.PersonelAcilDurumlar).Remove(acilDurum);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        #endregion

        #region Sağlık

        public async Task<PersonelSaglikDTO?> GetSaglikBilgisiAsync(int personelId)
        {
            // ✅ Özel metod - interface'den geliyor
            var saglik = await _unitOfWork.PersonelSagliklar.GetByPersonelIdAsync(personelId);
            return saglik == null ? null : MapToSaglikDTO(saglik);
        }

        public async Task<PersonelSaglikDTO> CreateOrUpdateSaglikBilgisiAsync(PersonelSaglikDTO dto)
        {
            // ✅ Özel metod - interface'den geliyor
            var mevcut = await _unitOfWork.PersonelSagliklar.GetByPersonelIdAsync(dto.PersonelId);

            if (mevcut == null)
            {
                var saglik = new PersonelSaglik
                {
                    PersonelId = dto.PersonelId,
                    KanGrubu = dto.KanGrubu,
                    Boy = dto.Boy,
                    Kilo = dto.Kilo,
                    KronikHastaliklar = dto.KronikHastaliklar,
                    Alerjiler = dto.Alerjiler,
                    SurekliKullanilanIlaclar = dto.SurekliKullanilanIlaclar,
                    EngelDurumuVarMi = dto.EngelDurumuVarMi,
                    EngelYuzdesi = dto.EngelYuzdesi,
                    EngelAciklama = dto.EngelAciklama,
                    SaglikRaporlari = dto.SaglikRaporlari,
                    SonPeriyodikMuayeneTarihi = dto.SonPeriyodikMuayeneTarihi,
                    SonradakiPeriyodikMuayeneTarihi = dto.SonradakiPeriyodikMuayeneTarihi,
                    IsGuvenligiEgitimiTarihi = dto.IsGuvenligiEgitimiTarihi,
                    Notlar = dto.Notlar,
                    KayitTarihi = DateTime.UtcNow
                };

                // ✅ IRepository'den gelen metod
                await _unitOfWork.PersonelSagliklar.AddAsync(saglik);
                await _unitOfWork.SaveChangesAsync();

                return MapToSaglikDTO(saglik);
            }
            else
            {
                mevcut.KanGrubu = dto.KanGrubu;
                mevcut.Boy = dto.Boy;
                mevcut.Kilo = dto.Kilo;
                mevcut.KronikHastaliklar = dto.KronikHastaliklar;
                mevcut.Alerjiler = dto.Alerjiler;
                mevcut.SurekliKullanilanIlaclar = dto.SurekliKullanilanIlaclar;
                mevcut.EngelDurumuVarMi = dto.EngelDurumuVarMi;
                mevcut.EngelYuzdesi = dto.EngelYuzdesi;
                mevcut.EngelAciklama = dto.EngelAciklama;
                mevcut.SaglikRaporlari = dto.SaglikRaporlari;
                mevcut.SonPeriyodikMuayeneTarihi = dto.SonPeriyodikMuayeneTarihi;
                mevcut.SonradakiPeriyodikMuayeneTarihi = dto.SonradakiPeriyodikMuayeneTarihi;
                mevcut.IsGuvenligiEgitimiTarihi = dto.IsGuvenligiEgitimiTarihi;
                mevcut.Notlar = dto.Notlar;
                mevcut.GuncellemeTarihi = DateTime.UtcNow;

                // ✅ IRepository'den gelen metod
                _unitOfWork.PersonelSagliklar.Update(mevcut);
                await _unitOfWork.SaveChangesAsync();

                return MapToSaglikDTO(mevcut);
            }
        }

        #endregion

        #region Mapping Metodları

        private PersonelAileDTO MapToAileDTO(PersonelAile entity)
        {
            return new PersonelAileDTO
            {
                Id = entity.Id,
                PersonelId = entity.PersonelId,
                YakinlikDerecesi = entity.YakinlikDerecesi,
                AdSoyad = entity.AdSoyad,
                TcKimlikNo = entity.TcKimlikNo,
                DogumTarihi = entity.DogumTarihi,
                Meslek = entity.Meslek,
                CalisiyorMu = entity.CalisiyorMu,
                Telefon = entity.Telefon,
                OgrenciMi = entity.OgrenciMi,
                SGKBagimlisi = entity.SGKBagimlisi,
                Notlar = entity.Notlar
            };
        }

        private PersonelAcilDurumDTO MapToAcilDurumDTO(PersonelAcilDurum entity)
        {
            return new PersonelAcilDurumDTO
            {
                Id = entity.Id,
                PersonelId = entity.PersonelId,
                IletisimTipi = entity.IletisimTipi,
                AdSoyad = entity.AdSoyad,
                YakinlikDerecesi = entity.YakinlikDerecesi,
                Telefon1 = entity.Telefon1,
                Telefon2 = entity.Telefon2,
                Adres = entity.Adres,
                Notlar = entity.Notlar
            };
        }

        private PersonelSaglikDTO MapToSaglikDTO(PersonelSaglik entity)
        {
            return new PersonelSaglikDTO
            {
                Id = entity.Id,
                PersonelId = entity.PersonelId,
                KanGrubu = entity.KanGrubu,
                Boy = entity.Boy,
                Kilo = entity.Kilo,
                KronikHastaliklar = entity.KronikHastaliklar,
                Alerjiler = entity.Alerjiler,
                SurekliKullanilanIlaclar = entity.SurekliKullanilanIlaclar,
                EngelDurumuVarMi = entity.EngelDurumuVarMi,
                EngelYuzdesi = entity.EngelYuzdesi,
                EngelAciklama = entity.EngelAciklama,
                SaglikRaporlari = entity.SaglikRaporlari,
                SonPeriyodikMuayeneTarihi = entity.SonPeriyodikMuayeneTarihi,
                SonradakiPeriyodikMuayeneTarihi = entity.SonradakiPeriyodikMuayeneTarihi,
                IsGuvenligiEgitimiTarihi = entity.IsGuvenligiEgitimiTarihi,
                Notlar = entity.Notlar
            };
        }

        #endregion


        #region Eğitim Geçmişi

        public async Task<IEnumerable<PersonelEgitimDTO>> GetEgitimGecmisiAsync(int personelId)
        {
            var egitimler = await _unitOfWork.PersonelEgitimler.GetByPersonelIdAsync(personelId);
            return egitimler.Select(MapToEgitimDTO);
        }

        public async Task<PersonelEgitimDTO> GetEgitimByIdAsync(int id)
        {
            var egitim = await _unitOfWork.PersonelEgitimler.GetByIdAsync(id);
            return egitim == null ? throw new Exception("Eğitim bilgisi bulunamadı") : MapToEgitimDTO(egitim);
        }

        public async Task<PersonelEgitimDTO> CreateEgitimAsync(PersonelEgitimCreateDTO dto)
        {
            var egitim = new PersonelEgitim
            {
                PersonelId = dto.PersonelId,
                EgitimSeviyesi = dto.EgitimSeviyesi,
                OkulAdi = dto.OkulAdi,
                Bolum = dto.Bolum,
                BaslangicYili = dto.BaslangicYili,
                BitisYili = dto.BitisYili,
                MezuniyetDurumu = dto.MezuniyetDurumu,
                MezuniyetNotu = dto.MezuniyetNotu,
                DiplomaDosyasi = dto.DiplomaDosyasi,
                Notlar = dto.Notlar,
                KayitTarihi = DateTime.UtcNow
            };

            await _unitOfWork.PersonelEgitimler.AddAsync(egitim);
            await _unitOfWork.SaveChangesAsync();

            return MapToEgitimDTO(egitim);
        }

        public async Task<bool> UpdateEgitimAsync(int id, PersonelEgitimCreateDTO dto)
        {
            var egitim = await _unitOfWork.PersonelEgitimler.GetByIdAsync(id);
            if (egitim == null) return false;

            egitim.EgitimSeviyesi = dto.EgitimSeviyesi;
            egitim.OkulAdi = dto.OkulAdi;
            egitim.Bolum = dto.Bolum;
            egitim.BaslangicYili = dto.BaslangicYili;
            egitim.BitisYili = dto.BitisYili;
            egitim.MezuniyetDurumu = dto.MezuniyetDurumu;
            egitim.MezuniyetNotu = dto.MezuniyetNotu;
            egitim.DiplomaDosyasi = dto.DiplomaDosyasi;
            egitim.Notlar = dto.Notlar;
            egitim.GuncellemeTarihi = DateTime.UtcNow;

            _unitOfWork.PersonelEgitimler.Update(egitim);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteEgitimAsync(int id)
        {
            var egitim = await _unitOfWork.PersonelEgitimler.GetByIdAsync(id);
            if (egitim == null) return false;

            ((IRepository<PersonelEgitim>)_unitOfWork.PersonelEgitimler).Remove(egitim);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        #endregion

        #region İş Deneyimi

        public async Task<IEnumerable<PersonelIsDeneyimiDTO>> GetIsDeneyimleriAsync(int personelId)
        {
            var deneyimler = await _unitOfWork.PersonelIsDeneyimleri.GetByPersonelIdAsync(personelId);
            return deneyimler.Select(MapToIsDeneyimiDTO);
        }

        public async Task<PersonelIsDeneyimiDTO> GetIsDeneyimiByIdAsync(int id)
        {
            var deneyim = await _unitOfWork.PersonelIsDeneyimleri.GetByIdAsync(id);
            return deneyim == null ? throw new Exception("İş deneyimi bulunamadı") : MapToIsDeneyimiDTO(deneyim);
        }

        public async Task<PersonelIsDeneyimiDTO> CreateIsDeneyimiAsync(PersonelIsDeneyimiCreateDTO dto)
        {
            var deneyim = new PersonelIsDeneyimi
            {
                PersonelId = dto.PersonelId,
                SirketAdi = dto.SirketAdi,
                Pozisyon = dto.Pozisyon,
                BaslangicTarihi = dto.BaslangicTarihi,
                BitisTarihi = dto.BitisTarihi,
                HalenCalisiyor = dto.HalenCalisiyor,
                IsTanimi = dto.IsTanimi,
                AyrilmaNedeni = dto.AyrilmaNedeni,
                ReferansKisiAdi = dto.ReferansKisiAdi,
                ReferansKisiTelefon = dto.ReferansKisiTelefon,
                ReferansKisiEmail = dto.ReferansKisiEmail,
                SGKTescilliMi = dto.SGKTescilliMi,
                Notlar = dto.Notlar,
                KayitTarihi = DateTime.UtcNow
            };

            await _unitOfWork.PersonelIsDeneyimleri.AddAsync(deneyim);
            await _unitOfWork.SaveChangesAsync();

            return MapToIsDeneyimiDTO(deneyim);
        }

        public async Task<bool> UpdateIsDeneyimiAsync(int id, PersonelIsDeneyimiCreateDTO dto)
        {
            var deneyim = await _unitOfWork.PersonelIsDeneyimleri.GetByIdAsync(id);
            if (deneyim == null) return false;

            deneyim.SirketAdi = dto.SirketAdi;
            deneyim.Pozisyon = dto.Pozisyon;
            deneyim.BaslangicTarihi = dto.BaslangicTarihi;
            deneyim.BitisTarihi = dto.BitisTarihi;
            deneyim.HalenCalisiyor = dto.HalenCalisiyor;
            deneyim.IsTanimi = dto.IsTanimi;
            deneyim.AyrilmaNedeni = dto.AyrilmaNedeni;
            deneyim.ReferansKisiAdi = dto.ReferansKisiAdi;
            deneyim.ReferansKisiTelefon = dto.ReferansKisiTelefon;
            deneyim.ReferansKisiEmail = dto.ReferansKisiEmail;
            deneyim.SGKTescilliMi = dto.SGKTescilliMi;
            deneyim.Notlar = dto.Notlar;
            deneyim.GuncellemeTarihi = DateTime.UtcNow;

            _unitOfWork.PersonelIsDeneyimleri.Update(deneyim);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteIsDeneyimiAsync(int id)
        {
            var deneyim = await _unitOfWork.PersonelIsDeneyimleri.GetByIdAsync(id);
            if (deneyim == null) return false;

            ((IRepository<PersonelIsDeneyimi>)_unitOfWork.PersonelIsDeneyimleri).Remove(deneyim);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        #endregion

        #region Dil Becerileri

        public async Task<IEnumerable<PersonelDilDTO>> GetDilBecerileriAsync(int personelId)
        {
            var diller = await _unitOfWork.PersonelDiller.GetByPersonelIdAsync(personelId);
            return diller.Select(MapToDilDTO);
        }

        public async Task<PersonelDilDTO> GetDilByIdAsync(int id)
        {
            var dil = await _unitOfWork.PersonelDiller.GetByIdAsync(id);
            return dil == null ? throw new Exception("Dil bilgisi bulunamadı") : MapToDilDTO(dil);
        }

        public async Task<PersonelDilDTO> CreateDilAsync(PersonelDilCreateDTO dto)
        {
            var dil = new PersonelDil
            {
                PersonelId = dto.PersonelId,
                DilAdi = dto.DilAdi,
                Seviye = dto.Seviye,
                OkumaSeviyesi = dto.OkumaSeviyesi,
                YazmaSeviyesi = dto.YazmaSeviyesi,
                KonusmaSeviyesi = dto.KonusmaSeviyesi,
                SertifikaTuru = dto.SertifikaTuru,
                SertifikaPuani = dto.SertifikaPuani,
                SertifikaTarihi = dto.SertifikaTarihi,
                SertifikaDosyasi = dto.SertifikaDosyasi,
                Notlar = dto.Notlar,
                KayitTarihi = DateTime.UtcNow
            };

            await _unitOfWork.PersonelDiller.AddAsync(dil);
            await _unitOfWork.SaveChangesAsync();

            return MapToDilDTO(dil);
        }

        public async Task<bool> UpdateDilAsync(int id, PersonelDilCreateDTO dto)
        {
            var dil = await _unitOfWork.PersonelDiller.GetByIdAsync(id);
            if (dil == null) return false;

            dil.DilAdi = dto.DilAdi;
            dil.Seviye = dto.Seviye;
            dil.OkumaSeviyesi = dto.OkumaSeviyesi;
            dil.YazmaSeviyesi = dto.YazmaSeviyesi;
            dil.KonusmaSeviyesi = dto.KonusmaSeviyesi;
            dil.SertifikaTuru = dto.SertifikaTuru;
            dil.SertifikaPuani = dto.SertifikaPuani;
            dil.SertifikaTarihi = dto.SertifikaTarihi;
            dil.SertifikaDosyasi = dto.SertifikaDosyasi;
            dil.Notlar = dto.Notlar;
            dil.GuncellemeTarihi = DateTime.UtcNow;

            _unitOfWork.PersonelDiller.Update(dil);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteDilAsync(int id)
        {
            var dil = await _unitOfWork.PersonelDiller.GetByIdAsync(id);
            if (dil == null) return false;

            ((IRepository<PersonelDil>)_unitOfWork.PersonelDiller).Remove(dil);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        #endregion

        #region Sertifikalar

        public async Task<IEnumerable<PersonelSertifikaDTO>> GetSertifikalarAsync(int personelId)
        {
            var sertifikalar = await _unitOfWork.PersonelSertifikalar.GetByPersonelIdAsync(personelId);
            return sertifikalar.Select(s => MapToSertifikaDTO(s));
        }

        public async Task<PersonelSertifikaDTO> GetSertifikaByIdAsync(int id)
        {
            var sertifika = await _unitOfWork.PersonelSertifikalar.GetByIdAsync(id);
            return sertifika == null ? throw new Exception("Sertifika bulunamadı") : MapToSertifikaDTO(sertifika);
        }

        public async Task<PersonelSertifikaDTO> CreateSertifikaAsync(PersonelSertifikaCreateDTO dto)
        {
            var sertifika = new PersonelSertifika
            {
                PersonelId = dto.PersonelId,
                SertifikaAdi = dto.SertifikaAdi,
                VerenKurum = dto.VerenKurum,
                AlimTarihi = dto.AlimTarihi,
                GecerlilikTarihi = dto.GecerlilikTarihi,
                SureliMi = dto.SureliMi,
                SertifikaNumarasi = dto.SertifikaNumarasi,
                SertifikaDosyasi = dto.SertifikaDosyasi,
                Durum = "Geçerli",
                HatirlatmaGonderildiMi = false,
                Notlar = dto.Notlar,
                KayitTarihi = DateTime.UtcNow
            };

            await _unitOfWork.PersonelSertifikalar.AddAsync(sertifika);
            await _unitOfWork.SaveChangesAsync();

            return MapToSertifikaDTO(sertifika);
        }

        public async Task<bool> UpdateSertifikaAsync(int id, PersonelSertifikaCreateDTO dto)
        {
            var sertifika = await _unitOfWork.PersonelSertifikalar.GetByIdAsync(id);
            if (sertifika == null) return false;

            sertifika.SertifikaAdi = dto.SertifikaAdi;
            sertifika.VerenKurum = dto.VerenKurum;
            sertifika.AlimTarihi = dto.AlimTarihi;
            sertifika.GecerlilikTarihi = dto.GecerlilikTarihi;
            sertifika.SureliMi = dto.SureliMi;
            sertifika.SertifikaNumarasi = dto.SertifikaNumarasi;
            sertifika.SertifikaDosyasi = dto.SertifikaDosyasi;
            sertifika.Notlar = dto.Notlar;
            sertifika.GuncellemeTarihi = DateTime.UtcNow;

            _unitOfWork.PersonelSertifikalar.Update(sertifika);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteSertifikaAsync(int id)
        {
            var sertifika = await _unitOfWork.PersonelSertifikalar.GetByIdAsync(id);
            if (sertifika == null) return false;

            ((IRepository<PersonelSertifika>)_unitOfWork.PersonelSertifikalar).Remove(sertifika);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<PersonelSertifikaDTO>> GetExpiringSertifikalarAsync(int daysBeforeExpiry = 30)
        {
            var sertifikalar = await _unitOfWork.PersonelSertifikalar.GetExpiringSertifikalarAsync(daysBeforeExpiry);
            return sertifikalar.Select(s => MapToSertifikaDTO(s));
        }

        public async Task<IEnumerable<PersonelSertifikaDTO>> GetExpiredSertifikalarAsync()
        {
            var sertifikalar = await _unitOfWork.PersonelSertifikalar.GetExpiredSertifikalarAsync();
            return sertifikalar.Select(s => MapToSertifikaDTO(s));
        }

        #endregion

        #region Performans

        public async Task<IEnumerable<PersonelPerformansDTO>> GetPerformansKayitlariAsync(int personelId)
        {
            var performanslar = await _unitOfWork.PersonelPerformanslar.GetByPersonelIdAsync(personelId);
            return performanslar.Select(MapToPerformansDTO);
        }

        public async Task<PersonelPerformansDTO> GetPerformansByIdAsync(int id)
        {
            var performans = await _unitOfWork.PersonelPerformanslar.GetByIdAsync(id);
            return performans == null ? throw new Exception("Performans kaydı bulunamadı") : MapToPerformansDTO(performans);
        }

        public async Task<PersonelPerformansDTO> CreatePerformansAsync(PersonelPerformansCreateDTO dto)
        {
            var performans = new PersonelPerformans
            {
                PersonelId = dto.PersonelId,
                DegerlendirmeTarihi = dto.DegerlendirmeTarihi,
                Donem = dto.Donem,
                DegerlendiriciKullaniciId = dto.DegerlendiriciKullaniciId,
                PerformansNotu = dto.PerformansNotu,
                NotSkalasi = dto.NotSkalasi,
                Hedefler = dto.Hedefler,
                HedefBasariOrani = dto.HedefBasariOrani,
                GucluYonler = dto.GucluYonler,
                GelisimAlanlari = dto.GelisimAlanlari,
                Yorumlar = dto.Yorumlar,
                AksiyonPlanlari = dto.AksiyonPlanlari,
                Durum = "Beklemede",
                EkDosyalar = dto.EkDosyalar,
                Notlar = dto.Notlar,
                KayitTarihi = DateTime.UtcNow
            };

            await _unitOfWork.PersonelPerformanslar.AddAsync(performans);
            await _unitOfWork.SaveChangesAsync();

            return MapToPerformansDTO(performans);
        }

        public async Task<bool> UpdatePerformansAsync(int id, PersonelPerformansCreateDTO dto)
        {
            var performans = await _unitOfWork.PersonelPerformanslar.GetByIdAsync(id);
            if (performans == null) return false;

            performans.DegerlendirmeTarihi = dto.DegerlendirmeTarihi;
            performans.Donem = dto.Donem;
            performans.DegerlendiriciKullaniciId = dto.DegerlendiriciKullaniciId;
            performans.PerformansNotu = dto.PerformansNotu;
            performans.NotSkalasi = dto.NotSkalasi;
            performans.Hedefler = dto.Hedefler;
            performans.HedefBasariOrani = dto.HedefBasariOrani;
            performans.GucluYonler = dto.GucluYonler;
            performans.GelisimAlanlari = dto.GelisimAlanlari;
            performans.Yorumlar = dto.Yorumlar;
            performans.AksiyonPlanlari = dto.AksiyonPlanlari;
            performans.EkDosyalar = dto.EkDosyalar;
            performans.Notlar = dto.Notlar;
            performans.GuncellemeTarihi = DateTime.UtcNow;

            _unitOfWork.PersonelPerformanslar.Update(performans);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeletePerformansAsync(int id)
        {
            var performans = await _unitOfWork.PersonelPerformanslar.GetByIdAsync(id);
            if (performans == null) return false;

            ((IRepository<PersonelPerformans>)_unitOfWork.PersonelPerformanslar).Remove(performans);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> OnaylaPerformansAsync(int id, int onaylayanKullaniciId)
        {
            var performans = await _unitOfWork.PersonelPerformanslar.GetByIdAsync(id);
            if (performans == null) return false;

            performans.Durum = "Onaylandı";
            performans.OnaylayanKullaniciId = onaylayanKullaniciId;
            performans.OnayTarihi = DateTime.UtcNow;

            _unitOfWork.PersonelPerformanslar.Update(performans);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        #endregion

        #region Disiplin

        public async Task<IEnumerable<PersonelDisiplinDTO>> GetDisiplinKayitlariAsync(int personelId)
        {
            var disiplinler = await _unitOfWork.PersonelDisiplinler.GetByPersonelIdAsync(personelId);
            return disiplinler.Select(MapToDisiplinDTO);
        }

        public async Task<PersonelDisiplinDTO> GetDisiplinByIdAsync(int id)
        {
            var disiplin = await _unitOfWork.PersonelDisiplinler.GetByIdAsync(id);
            return disiplin == null ? throw new Exception("Disiplin kaydı bulunamadı") : MapToDisiplinDTO(disiplin);
        }

        public async Task<PersonelDisiplinDTO> CreateDisiplinAsync(PersonelDisiplinCreateDTO dto)
        {
            var disiplin = new PersonelDisiplin
            {
                PersonelId = dto.PersonelId,
                DisiplinTuru = dto.DisiplinTuru,
                OlayTarihi = dto.OlayTarihi,
                Aciklama = dto.Aciklama,
                UygulananCeza = dto.UygulananCeza,
                KararVerenYetkiliId = dto.KararVerenYetkiliId,
                IlgiliDokumanlar = dto.IlgiliDokumanlar,
                Durum = "Aktif",
                Notlar = dto.Notlar,
                KayitTarihi = DateTime.UtcNow
            };

            await _unitOfWork.PersonelDisiplinler.AddAsync(disiplin);
            await _unitOfWork.SaveChangesAsync();

            return MapToDisiplinDTO(disiplin);
        }

        public async Task<bool> UpdateDisiplinAsync(int id, PersonelDisiplinCreateDTO dto)
        {
            var disiplin = await _unitOfWork.PersonelDisiplinler.GetByIdAsync(id);
            if (disiplin == null) return false;

            disiplin.DisiplinTuru = dto.DisiplinTuru;
            disiplin.OlayTarihi = dto.OlayTarihi;
            disiplin.Aciklama = dto.Aciklama;
            disiplin.UygulananCeza = dto.UygulananCeza;
            disiplin.KararVerenYetkiliId = dto.KararVerenYetkiliId;
            disiplin.IlgiliDokumanlar = dto.IlgiliDokumanlar;
            disiplin.Notlar = dto.Notlar;
            disiplin.GuncellemeTarihi = DateTime.UtcNow;

            _unitOfWork.PersonelDisiplinler.Update(disiplin);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> IptalDisiplinAsync(int id, int iptalEdenKullaniciId, string iptalNedeni)
        {
            var disiplin = await _unitOfWork.PersonelDisiplinler.GetByIdAsync(id);
            if (disiplin == null) return false;

            disiplin.Durum = "İptal";
            disiplin.IptalEdenKullaniciId = iptalEdenKullaniciId;
            disiplin.IptalTarihi = DateTime.UtcNow;
            disiplin.IptalNedeni = iptalNedeni;

            _unitOfWork.PersonelDisiplinler.Update(disiplin);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        #endregion

        #region Terfi

        public async Task<IEnumerable<PersonelTerfiDTO>> GetTerfiGecmisiAsync(int personelId)
        {
            var terfiler = await _unitOfWork.PersonelTerfiler.GetByPersonelIdAsync(personelId);
            return terfiler.Select(MapToTerfiDTO);
        }

        public async Task<PersonelTerfiDTO> GetTerfiByIdAsync(int id)
        {
            var terfi = await _unitOfWork.PersonelTerfiler.GetByIdAsync(id);
            return terfi == null ? throw new Exception("Terfi kaydı bulunamadı") : MapToTerfiDTO(terfi);
        }

        public async Task<PersonelTerfiDTO> CreateTerfiAsync(PersonelTerfiCreateDTO dto)
        {
            var terfi = new PersonelTerfi
            {
                PersonelId = dto.PersonelId,
                TerfiTarihi = dto.TerfiTarihi,
                EskiPozisyon = dto.EskiPozisyon,
                YeniPozisyon = dto.YeniPozisyon,
                EskiUnvan = dto.EskiUnvan,
                YeniUnvan = dto.YeniUnvan,
                EskiDepartmanId = dto.EskiDepartmanId,
                YeniDepartmanId = dto.YeniDepartmanId,
                TerfiNedeni = dto.TerfiNedeni,
                OnaylayanKullaniciId = dto.OnaylayanKullaniciId,
                OnayTarihi = DateTime.UtcNow,
                EkDosyalar = dto.EkDosyalar,
                Notlar = dto.Notlar,
                KayitTarihi = DateTime.UtcNow
            };

            await _unitOfWork.PersonelTerfiler.AddAsync(terfi);
            await _unitOfWork.SaveChangesAsync();

            return MapToTerfiDTO(terfi);
        }

        public async Task<bool> UpdateTerfiAsync(int id, PersonelTerfiCreateDTO dto)
        {
            var terfi = await _unitOfWork.PersonelTerfiler.GetByIdAsync(id);
            if (terfi == null) return false;

            terfi.TerfiTarihi = dto.TerfiTarihi;
            terfi.EskiPozisyon = dto.EskiPozisyon;
            terfi.YeniPozisyon = dto.YeniPozisyon;
            terfi.EskiUnvan = dto.EskiUnvan;
            terfi.YeniUnvan = dto.YeniUnvan;
            terfi.EskiDepartmanId = dto.EskiDepartmanId;
            terfi.YeniDepartmanId = dto.YeniDepartmanId;
            terfi.TerfiNedeni = dto.TerfiNedeni;
            terfi.OnaylayanKullaniciId = dto.OnaylayanKullaniciId;
            terfi.EkDosyalar = dto.EkDosyalar;
            terfi.Notlar = dto.Notlar;
            terfi.GuncellemeTarihi = DateTime.UtcNow;

            _unitOfWork.PersonelTerfiler.Update(terfi);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        #endregion

        #region Ücret Değişiklik

        public async Task<IEnumerable<PersonelUcretDegisiklikDTO>> GetUcretDegisiklikleriAsync(int personelId)
        {
            var ucretDegisiklikler = await _unitOfWork.PersonelUcretDegisiklikler.GetByPersonelIdAsync(personelId);
            return ucretDegisiklikler.Select(MapToUcretDegisiklikDTO);
        }

        public async Task<PersonelUcretDegisiklikDTO> GetUcretDegisiklikByIdAsync(int id)
        {
            var ucretDegisiklik = await _unitOfWork.PersonelUcretDegisiklikler.GetByIdAsync(id);
            return ucretDegisiklik == null ? throw new Exception("Ücret değişiklik kaydı bulunamadı") : MapToUcretDegisiklikDTO(ucretDegisiklik);
        }

        public async Task<PersonelUcretDegisiklikDTO> CreateUcretDegisiklikAsync(PersonelUcretDegisiklikCreateDTO dto)
        {
            // Otomatik hesaplamalar
            var farkTutari = dto.YeniMaas - dto.EskiMaas;
            var degisimYuzdesi = dto.EskiMaas > 0 ? (farkTutari / dto.EskiMaas) * 100 : 0;

            var ucretDegisiklik = new PersonelUcretDegisiklik
            {
                PersonelId = dto.PersonelId,
                DegisiklikTarihi = dto.DegisiklikTarihi,
                EskiMaas = dto.EskiMaas,
                YeniMaas = dto.YeniMaas,
                FarkTutari = farkTutari,
                DegisimYuzdesi = degisimYuzdesi,
                DegisimNedeni = dto.DegisimNedeni,
                Aciklama = dto.Aciklama,
                OnaylayanKullaniciId = dto.OnaylayanKullaniciId,
                OnayTarihi = DateTime.UtcNow,
                EkDosyalar = dto.EkDosyalar,
                Notlar = dto.Notlar,
                KayitTarihi = DateTime.UtcNow
            };

            await _unitOfWork.PersonelUcretDegisiklikler.AddAsync(ucretDegisiklik);
            await _unitOfWork.SaveChangesAsync();

            return MapToUcretDegisiklikDTO(ucretDegisiklik);
        }

        public async Task<bool> UpdateUcretDegisiklikAsync(int id, PersonelUcretDegisiklikCreateDTO dto)
        {
            var ucretDegisiklik = await _unitOfWork.PersonelUcretDegisiklikler.GetByIdAsync(id);
            if (ucretDegisiklik == null) return false;

            // Otomatik hesaplamalar
            var farkTutari = dto.YeniMaas - dto.EskiMaas;
            var degisimYuzdesi = dto.EskiMaas > 0 ? (farkTutari / dto.EskiMaas) * 100 : 0;

            ucretDegisiklik.DegisiklikTarihi = dto.DegisiklikTarihi;
            ucretDegisiklik.EskiMaas = dto.EskiMaas;
            ucretDegisiklik.YeniMaas = dto.YeniMaas;
            ucretDegisiklik.FarkTutari = farkTutari;
            ucretDegisiklik.DegisimYuzdesi = degisimYuzdesi;
            ucretDegisiklik.DegisimNedeni = dto.DegisimNedeni;
            ucretDegisiklik.Aciklama = dto.Aciklama;
            ucretDegisiklik.OnaylayanKullaniciId = dto.OnaylayanKullaniciId;
            ucretDegisiklik.EkDosyalar = dto.EkDosyalar;
            ucretDegisiklik.Notlar = dto.Notlar;
            ucretDegisiklik.GuncellemeTarihi = DateTime.UtcNow;

            _unitOfWork.PersonelUcretDegisiklikler.Update(ucretDegisiklik);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        #endregion

        #region Referans

        public async Task<IEnumerable<PersonelReferansDTO>> GetReferanslarAsync(int personelId)
        {
            var referanslar = await _unitOfWork.PersonelReferanslar.GetByPersonelIdAsync(personelId);
            return referanslar.Select(MapToReferansDTO);
        }

        public async Task<PersonelReferansDTO> GetReferansByIdAsync(int id)
        {
            var referans = await _unitOfWork.PersonelReferanslar.GetByIdAsync(id);
            return referans == null ? throw new Exception("Referans bulunamadı") : MapToReferansDTO(referans);
        }

        public async Task<PersonelReferansDTO> CreateReferansAsync(PersonelReferansCreateDTO dto)
        {
            var referans = new PersonelReferans
            {
                PersonelId = dto.PersonelId,
                AdSoyad = dto.AdSoyad,
                SirketKurum = dto.SirketKurum,
                Pozisyon = dto.Pozisyon,
                Iliski = dto.Iliski,
                Telefon = dto.Telefon,
                Email = dto.Email,
                Notlar = dto.Notlar,
                KayitTarihi = DateTime.UtcNow
            };

            await _unitOfWork.PersonelReferanslar.AddAsync(referans);
            await _unitOfWork.SaveChangesAsync();

            return MapToReferansDTO(referans);
        }

        public async Task<bool> UpdateReferansAsync(int id, PersonelReferansCreateDTO dto)
        {
            var referans = await _unitOfWork.PersonelReferanslar.GetByIdAsync(id);
            if (referans == null) return false;

            referans.AdSoyad = dto.AdSoyad;
            referans.SirketKurum = dto.SirketKurum;
            referans.Pozisyon = dto.Pozisyon;
            referans.Iliski = dto.Iliski;
            referans.Telefon = dto.Telefon;
            referans.Email = dto.Email;
            referans.Notlar = dto.Notlar;
            referans.GuncellemeTarihi = DateTime.UtcNow;

            _unitOfWork.PersonelReferanslar.Update(referans);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteReferansAsync(int id)
        {
            var referans = await _unitOfWork.PersonelReferanslar.GetByIdAsync(id);
            if (referans == null) return false;

            ((IRepository<PersonelReferans>)_unitOfWork.PersonelReferanslar).Remove(referans);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        #endregion

        #region Zimmet

        public async Task<IEnumerable<PersonelZimmetDTO>> GetZimmetlerAsync(int personelId)
        {
            var zimmetler = await _unitOfWork.PersonelZimmetler.GetByPersonelIdAsync(personelId);
            return zimmetler.Select(MapToZimmetDTO);
        }

        public async Task<PersonelZimmetDTO> GetZimmetByIdAsync(int id)
        {
            var zimmet = await _unitOfWork.PersonelZimmetler.GetByIdAsync(id);
            return zimmet == null ? throw new Exception("Zimmet kaydı bulunamadı") : MapToZimmetDTO(zimmet);
        }

        public async Task<PersonelZimmetDTO> CreateZimmetAsync(PersonelZimmetCreateDTO dto)
        {
            var zimmet = new PersonelZimmet
            {
                PersonelId = dto.PersonelId,
                EsyaTipi = dto.EsyaTipi,
                EsyaAdi = dto.EsyaAdi,
                MarkaModel = dto.MarkaModel,
                SeriNumarasi = dto.SeriNumarasi,
                ZimmetTarihi = dto.ZimmetTarihi,
                ZimmetDurumu = "Aktif",
                Degeri = dto.Degeri,
                ZimmetSozlesmesi = dto.ZimmetSozlesmesi,
                ZimmetFotografi = dto.ZimmetFotografi,
                Aciklama = dto.Aciklama,
                ZimmetVerenKullaniciId = dto.ZimmetVerenKullaniciId,
                Notlar = dto.Notlar,
                KayitTarihi = DateTime.UtcNow
            };

            await _unitOfWork.PersonelZimmetler.AddAsync(zimmet);
            await _unitOfWork.SaveChangesAsync();

            return MapToZimmetDTO(zimmet);
        }

        public async Task<bool> IadeZimmetAsync(int id, int iadeTeslimAlanKullaniciId, DateTime iadeTarihi)
        {
            var zimmet = await _unitOfWork.PersonelZimmetler.GetByIdAsync(id);
            if (zimmet == null) return false;

            zimmet.ZimmetDurumu = "İade Edildi";
            zimmet.IadeTarihi = iadeTarihi;
            zimmet.IadeTeslimAlanKullaniciId = iadeTeslimAlanKullaniciId;
            zimmet.GuncellemeTarihi = DateTime.UtcNow;

            _unitOfWork.PersonelZimmetler.Update(zimmet);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteZimmetAsync(int id)
        {
            var zimmet = await _unitOfWork.PersonelZimmetler.GetByIdAsync(id);
            if (zimmet == null) return false;

            ((IRepository<PersonelZimmet>)_unitOfWork.PersonelZimmetler).Remove(zimmet);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        #endregion

        #region Yetkinlik

        public async Task<IEnumerable<PersonelYetkinlikDTO>> GetYetkinliklerAsync(int personelId)
        {
            var yetkinlikler = await _unitOfWork.PersonelYetkinlikler.GetByPersonelIdAsync(personelId);
            return yetkinlikler.Select(MapToYetkinlikDTO);
        }

        public async Task<PersonelYetkinlikDTO> GetYetkinlikByIdAsync(int id)
        {
            var yetkinlik = await _unitOfWork.PersonelYetkinlikler.GetByIdAsync(id);
            return yetkinlik == null ? throw new Exception("Yetkinlik bulunamadı") : MapToYetkinlikDTO(yetkinlik);
        }

        public async Task<PersonelYetkinlikDTO> CreateYetkinlikAsync(PersonelYetkinlikCreateDTO dto)
        {
            var yetkinlik = new PersonelYetkinlik
            {
                PersonelId = dto.PersonelId,
                YetkinlikTipi = dto.YetkinlikTipi,
                YetkinlikAdi = dto.YetkinlikAdi,
                Seviye = dto.Seviye,
                SeviyePuani = dto.SeviyePuani,
                SonKullanimTarihi = dto.SonKullanimTarihi,
                SelfAssessment = dto.SelfAssessment,
                DegerlendiriciKullaniciId = dto.DegerlendiriciKullaniciId,
                DegerlendirmeTarihi = dto.DegerlendiriciKullaniciId.HasValue ? DateTime.UtcNow : (DateTime?)null,
                BelgelendirenDokumanlar = dto.BelgelendirenDokumanlar,
                Notlar = dto.Notlar,
                KayitTarihi = DateTime.UtcNow
            };

            await _unitOfWork.PersonelYetkinlikler.AddAsync(yetkinlik);
            await _unitOfWork.SaveChangesAsync();

            return MapToYetkinlikDTO(yetkinlik);
        }

        public async Task<bool> UpdateYetkinlikAsync(int id, PersonelYetkinlikCreateDTO dto)
        {
            var yetkinlik = await _unitOfWork.PersonelYetkinlikler.GetByIdAsync(id);
            if (yetkinlik == null) return false;

            yetkinlik.YetkinlikTipi = dto.YetkinlikTipi;
            yetkinlik.YetkinlikAdi = dto.YetkinlikAdi;
            yetkinlik.Seviye = dto.Seviye;
            yetkinlik.SeviyePuani = dto.SeviyePuani;
            yetkinlik.SonKullanimTarihi = dto.SonKullanimTarihi;
            yetkinlik.SelfAssessment = dto.SelfAssessment;
            yetkinlik.DegerlendiriciKullaniciId = dto.DegerlendiriciKullaniciId;
            yetkinlik.BelgelendirenDokumanlar = dto.BelgelendirenDokumanlar;
            yetkinlik.Notlar = dto.Notlar;
            yetkinlik.GuncellemeTarihi = DateTime.UtcNow;

            _unitOfWork.PersonelYetkinlikler.Update(yetkinlik);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteYetkinlikAsync(int id)
        {
            var yetkinlik = await _unitOfWork.PersonelYetkinlikler.GetByIdAsync(id);
            if (yetkinlik == null) return false;

            ((IRepository<PersonelYetkinlik>)_unitOfWork.PersonelYetkinlikler).Remove(yetkinlik);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        #endregion

        #region Eğitim Kayıt

        public async Task<IEnumerable<PersonelEgitimKayitDTO>> GetEgitimKayitlariAsync(int personelId)
        {
            var egitimKayitlari = await _unitOfWork.PersonelEgitimKayitlari.GetByPersonelIdAsync(personelId);
            return egitimKayitlari.Select(MapToEgitimKayitDTO);
        }

        public async Task<PersonelEgitimKayitDTO> GetEgitimKayitByIdAsync(int id)
        {
            var egitimKayit = await _unitOfWork.PersonelEgitimKayitlari.GetByIdAsync(id);
            return egitimKayit == null ? throw new Exception("Eğitim kaydı bulunamadı") : MapToEgitimKayitDTO(egitimKayit);
        }

        public async Task<PersonelEgitimKayitDTO> CreateEgitimKayitAsync(PersonelEgitimKayitCreateDTO dto)
        {
            var egitimKayit = new PersonelEgitimKayit
            {
                PersonelId = dto.PersonelId,
                EgitimAdi = dto.EgitimAdi,
                EgitmenKurum = dto.EgitmenKurum,
                EgitimTarihi = dto.EgitimTarihi,
                BitisTarihi = dto.BitisTarihi,
                EgitimSuresiSaat = dto.EgitimSuresiSaat,
                TamamlanmaDurumu = dto.TamamlanmaDurumu,
                EgitimMaliyeti = dto.EgitimMaliyeti,
                EgitimSertifikasi = dto.EgitimSertifikasi,
                SertifikaAldiMi = dto.SertifikaAldiMi,
                EgitimTuru = dto.EgitimTuru,
                EgitimKategorisi = dto.EgitimKategorisi,
                EgitimIcerigi = dto.EgitimIcerigi,
                DegerlendirmePuani = dto.DegerlendirmePuani,
                PersonelGeribildirimi = dto.PersonelGeribildirimi,
                EkDosyalar = dto.EkDosyalar,
                Notlar = dto.Notlar,
                KayitTarihi = DateTime.UtcNow
            };

            await _unitOfWork.PersonelEgitimKayitlari.AddAsync(egitimKayit);
            await _unitOfWork.SaveChangesAsync();

            return MapToEgitimKayitDTO(egitimKayit);
        }

        public async Task<bool> UpdateEgitimKayitAsync(int id, PersonelEgitimKayitCreateDTO dto)
        {
            var egitimKayit = await _unitOfWork.PersonelEgitimKayitlari.GetByIdAsync(id);
            if (egitimKayit == null) return false;

            egitimKayit.EgitimAdi = dto.EgitimAdi;
            egitimKayit.EgitmenKurum = dto.EgitmenKurum;
            egitimKayit.EgitimTarihi = dto.EgitimTarihi;
            egitimKayit.BitisTarihi = dto.BitisTarihi;
            egitimKayit.EgitimSuresiSaat = dto.EgitimSuresiSaat;
            egitimKayit.TamamlanmaDurumu = dto.TamamlanmaDurumu;
            egitimKayit.EgitimMaliyeti = dto.EgitimMaliyeti;
            egitimKayit.EgitimSertifikasi = dto.EgitimSertifikasi;
            egitimKayit.SertifikaAldiMi = dto.SertifikaAldiMi;
            egitimKayit.EgitimTuru = dto.EgitimTuru;
            egitimKayit.EgitimKategorisi = dto.EgitimKategorisi;
            egitimKayit.EgitimIcerigi = dto.EgitimIcerigi;
            egitimKayit.DegerlendirmePuani = dto.DegerlendirmePuani;
            egitimKayit.PersonelGeribildirimi = dto.PersonelGeribildirimi;
            egitimKayit.EkDosyalar = dto.EkDosyalar;
            egitimKayit.Notlar = dto.Notlar;
            egitimKayit.GuncellemeTarihi = DateTime.UtcNow;

            _unitOfWork.PersonelEgitimKayitlari.Update(egitimKayit);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteEgitimKayitAsync(int id)
        {
            var egitimKayit = await _unitOfWork.PersonelEgitimKayitlari.GetByIdAsync(id);
            if (egitimKayit == null) return false;

            ((IRepository<PersonelEgitimKayit>)_unitOfWork.PersonelEgitimKayitlari).Remove(egitimKayit);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        #endregion

        #region Mali Bilgi

        public async Task<PersonelMaliBilgiDTO?> GetMaliBilgiAsync(int personelId)
        {
            var maliBilgi = await _unitOfWork.PersonelMaliBilgileri.GetByPersonelIdAsync(personelId);
            return maliBilgi == null ? null : MapToMaliBilgiDTO(maliBilgi);
        }

        public async Task<PersonelMaliBilgiDTO> CreateOrUpdateMaliBilgiAsync(PersonelMaliBilgiDTO dto)
        {
            var mevcut = await _unitOfWork.PersonelMaliBilgileri.GetByPersonelIdAsync(dto.PersonelId);

            if (mevcut == null)
            {
                var maliBilgi = new PersonelMaliBilgi
                {
                    PersonelId = dto.PersonelId,
                    BankaAdi = dto.BankaAdi,
                    IBAN = dto.IBAN,
                    HesapTuru = dto.HesapTuru,
                    VergiNo = dto.VergiNo,
                    VergiDairesi = dto.VergiDairesi,
                    SGKNo = dto.SGKNo,
                    SGKBaslangicTarihi = dto.SGKBaslangicTarihi,
                    AsgariUcretMuafiyeti = dto.AsgariUcretMuafiyeti,
                    GelirVergisiOrani = dto.GelirVergisiOrani,
                    EmekliSandigi = dto.EmekliSandigi,
                    OdemeYontemi = dto.OdemeYontemi,
                    Notlar = dto.Notlar,
                    KayitTarihi = DateTime.UtcNow
                };

                await _unitOfWork.PersonelMaliBilgileri.AddAsync(maliBilgi);
                await _unitOfWork.SaveChangesAsync();

                return MapToMaliBilgiDTO(maliBilgi);
            }
            else
            {
                mevcut.BankaAdi = dto.BankaAdi;
                mevcut.IBAN = dto.IBAN;
                mevcut.HesapTuru = dto.HesapTuru;
                mevcut.VergiNo = dto.VergiNo;
                mevcut.VergiDairesi = dto.VergiDairesi;
                mevcut.SGKNo = dto.SGKNo;
                mevcut.SGKBaslangicTarihi = dto.SGKBaslangicTarihi;
                mevcut.AsgariUcretMuafiyeti = dto.AsgariUcretMuafiyeti;
                mevcut.GelirVergisiOrani = dto.GelirVergisiOrani;
                mevcut.EmekliSandigi = dto.EmekliSandigi;
                mevcut.OdemeYontemi = dto.OdemeYontemi;
                mevcut.Notlar = dto.Notlar;
                mevcut.GuncellemeTarihi = DateTime.UtcNow;

                _unitOfWork.PersonelMaliBilgileri.Update(mevcut);
                await _unitOfWork.SaveChangesAsync();

                return MapToMaliBilgiDTO(mevcut);
            }
        }

        #endregion

        #region Ek Bilgi

        public async Task<PersonelEkBilgiDTO?> GetEkBilgiAsync(int personelId)
        {
            var ekBilgi = await _unitOfWork.PersonelEkBilgileri.GetByPersonelIdAsync(personelId);
            return ekBilgi == null ? null : MapToEkBilgiDTO(ekBilgi);
        }

        public async Task<PersonelEkBilgiDTO> CreateOrUpdateEkBilgiAsync(PersonelEkBilgiDTO dto)
        {
            var mevcut = await _unitOfWork.PersonelEkBilgileri.GetByPersonelIdAsync(dto.PersonelId);

            if (mevcut == null)
            {
                var ekBilgi = new PersonelEkBilgi
                {
                    PersonelId = dto.PersonelId,
                    MedeniDurum = dto.MedeniDurum,
                    AskerlikDurumu = dto.AskerlikDurumu,
                    AskerlikBaslangicTarihi = dto.AskerlikBaslangicTarihi,
                    AskerlikBitisTarihi = dto.AskerlikBitisTarihi,
                    AskerlikYeri = dto.AskerlikYeri,
                    AskerlikRutbesi = dto.AskerlikRutbesi,
                    EhliyetSiniflari = dto.EhliyetSiniflari,
                    EhliyetAlisTarihi = dto.EhliyetAlisTarihi,
                    EhliyetGecerlilikTarihi = dto.EhliyetGecerlilikTarihi,
                    Uyruk = dto.Uyruk,
                    IkametIli = dto.IkametIli,
                    IkametIlce = dto.IkametIlce,
                    IkametAdresi = dto.IkametAdresi,
                    DogumYeri = dto.DogumYeri,
                    AnneAdi = dto.AnneAdi,
                    BabaAdi = dto.BabaAdi,
                    CocukSayisi = dto.CocukSayisi,
                    HobiIlgiAlanlari = dto.HobiIlgiAlanlari,
                    SosyalGuvence = dto.SosyalGuvence,
                    SigortaliMi = dto.SigortaliMi,
                    SigortaSirketi = dto.SigortaSirketi,
                    SigortaPoliceNo = dto.SigortaPoliceNo,
                    Notlar = dto.Notlar,
                    KayitTarihi = DateTime.UtcNow
                };

                await _unitOfWork.PersonelEkBilgileri.AddAsync(ekBilgi);
                await _unitOfWork.SaveChangesAsync();

                return MapToEkBilgiDTO(ekBilgi);
            }
            else
            {
                mevcut.MedeniDurum = dto.MedeniDurum;
                mevcut.AskerlikDurumu = dto.AskerlikDurumu;
                mevcut.AskerlikBaslangicTarihi = dto.AskerlikBaslangicTarihi;
                mevcut.AskerlikBitisTarihi = dto.AskerlikBitisTarihi;
                mevcut.AskerlikYeri = dto.AskerlikYeri;
                mevcut.AskerlikRutbesi = dto.AskerlikRutbesi;
                mevcut.EhliyetSiniflari = dto.EhliyetSiniflari;
                mevcut.EhliyetAlisTarihi = dto.EhliyetAlisTarihi;
                mevcut.EhliyetGecerlilikTarihi = dto.EhliyetGecerlilikTarihi;
                mevcut.Uyruk = dto.Uyruk;
                mevcut.IkametIli = dto.IkametIli;
                mevcut.IkametIlce = dto.IkametIlce;
                mevcut.IkametAdresi = dto.IkametAdresi;
                mevcut.DogumYeri = dto.DogumYeri;
                mevcut.AnneAdi = dto.AnneAdi;
                mevcut.BabaAdi = dto.BabaAdi;
                mevcut.CocukSayisi = dto.CocukSayisi;
                mevcut.HobiIlgiAlanlari = dto.HobiIlgiAlanlari;
                mevcut.SosyalGuvence = dto.SosyalGuvence;
                mevcut.SigortaliMi = dto.SigortaliMi;
                mevcut.SigortaSirketi = dto.SigortaSirketi;
                mevcut.SigortaPoliceNo = dto.SigortaPoliceNo;
                mevcut.Notlar = dto.Notlar;
                mevcut.GuncellemeTarihi = DateTime.UtcNow;

                _unitOfWork.PersonelEkBilgileri.Update(mevcut);
                await _unitOfWork.SaveChangesAsync();

                return MapToEkBilgiDTO(mevcut);
            }
        }

        #endregion

        #region Kombine Özlük Detay

        public async Task<PersonelOzlukDetayDTO> GetPersonelOzlukDetayAsync(int personelId)
        {
            var personel = await _unitOfWork.Personeller.GetByIdAsync(personelId);
            if (personel == null) throw new Exception("Personel bulunamadı");

            var ozlukDetay = new PersonelOzlukDetayDTO
            {
                PersonelId = personelId,
                AdSoyad = personel.AdSoyad,
                AileBilgileri = (await GetAileBilgileriAsync(personelId)).ToList(),
                AcilDurumBilgileri = (await GetAcilDurumBilgileriAsync(personelId)).ToList(),
                SaglikBilgisi = await GetSaglikBilgisiAsync(personelId),
                EgitimGecmisi = (await GetEgitimGecmisiAsync(personelId)).ToList(),
                IsDeneyimleri = (await GetIsDeneyimleriAsync(personelId)).ToList(),
                DilBecerileri = (await GetDilBecerileriAsync(personelId)).ToList(),
                Sertifikalar = (await GetSertifikalarAsync(personelId)).ToList(),
                PerformansKayitlari = (await GetPerformansKayitlariAsync(personelId)).ToList(),
                DisiplinKayitlari = (await GetDisiplinKayitlariAsync(personelId)).ToList(),
                TerfiGecmisi = (await GetTerfiGecmisiAsync(personelId)).ToList(),
                UcretDegisiklikleri = (await GetUcretDegisiklikleriAsync(personelId)).ToList(),
                Referanslar = (await GetReferanslarAsync(personelId)).ToList(),
                ZimmetliEsyalar = (await GetZimmetlerAsync(personelId)).ToList(),
                Yetkinlikler = (await GetYetkinliklerAsync(personelId)).ToList(),
                EgitimKayitlari = (await GetEgitimKayitlariAsync(personelId)).ToList(),
                MaliBilgi = await GetMaliBilgiAsync(personelId),
                EkBilgi = await GetEkBilgiAsync(personelId)
            };

            return ozlukDetay;
        }

        #endregion

        #region Mapping Metodları - Part 2

        private PersonelEgitimDTO MapToEgitimDTO(PersonelEgitim entity)
        {
            return new PersonelEgitimDTO
            {
                Id = entity.Id,
                PersonelId = entity.PersonelId,
                EgitimSeviyesi = entity.EgitimSeviyesi,
                OkulAdi = entity.OkulAdi,
                Bolum = entity.Bolum,
                BaslangicYili = entity.BaslangicYili,
                BitisYili = entity.BitisYili,
                MezuniyetDurumu = entity.MezuniyetDurumu,
                MezuniyetNotu = entity.MezuniyetNotu,
                DiplomaDosyasi = entity.DiplomaDosyasi,
                Notlar = entity.Notlar
            };
        }

        private PersonelIsDeneyimiDTO MapToIsDeneyimiDTO(PersonelIsDeneyimi entity)
        {
            return new PersonelIsDeneyimiDTO
            {
                Id = entity.Id,
                PersonelId = entity.PersonelId,
                SirketAdi = entity.SirketAdi,
                Pozisyon = entity.Pozisyon,
                BaslangicTarihi = entity.BaslangicTarihi,
                BitisTarihi = entity.BitisTarihi,
                HalenCalisiyor = entity.HalenCalisiyor,
                IsTanimi = entity.IsTanimi,
                AyrilmaNedeni = entity.AyrilmaNedeni,
                ReferansKisiAdi = entity.ReferansKisiAdi,
                ReferansKisiTelefon = entity.ReferansKisiTelefon,
                ReferansKisiEmail = entity.ReferansKisiEmail,
                SGKTescilliMi = entity.SGKTescilliMi,
                Notlar = entity.Notlar
            };
        }

        private PersonelDilDTO MapToDilDTO(PersonelDil entity)
        {
            return new PersonelDilDTO
            {
                Id = entity.Id,
                PersonelId = entity.PersonelId,
                DilAdi = entity.DilAdi,
                Seviye = entity.Seviye,
                OkumaSeviyesi = entity.OkumaSeviyesi,
                YazmaSeviyesi = entity.YazmaSeviyesi,
                KonusmaSeviyesi = entity.KonusmaSeviyesi,
                SertifikaTuru = entity.SertifikaTuru,
                SertifikaPuani = entity.SertifikaPuani,
                SertifikaTarihi = entity.SertifikaTarihi,
                SertifikaDosyasi = entity.SertifikaDosyasi,
                Notlar = entity.Notlar
            };
        }

        private PersonelSertifikaDTO MapToSertifikaDTO(PersonelSertifika entity)
        {
            int? kalanGun = null;
            if (entity.SureliMi && entity.GecerlilikTarihi.HasValue)
            {
                kalanGun = (int)(entity.GecerlilikTarihi.Value - DateTime.UtcNow).TotalDays;
            }

            return new PersonelSertifikaDTO
            {
                Id = entity.Id,
                PersonelId = entity.PersonelId,
                PersonelAdSoyad = entity.Personel?.AdSoyad ?? "",
                SertifikaAdi = entity.SertifikaAdi,
                VerenKurum = entity.VerenKurum,
                AlimTarihi = entity.AlimTarihi,
                GecerlilikTarihi = entity.GecerlilikTarihi,
                SureliMi = entity.SureliMi,
                SertifikaNumarasi = entity.SertifikaNumarasi,
                SertifikaDosyasi = entity.SertifikaDosyasi,
                Durum = entity.Durum,
                HatirlatmaGonderildiMi = entity.HatirlatmaGonderildiMi,
                HatirlatmaTarihi = entity.HatirlatmaTarihi,
                Notlar = entity.Notlar,
                KalanGunSayisi = kalanGun
            };
        }

        private PersonelPerformansDTO MapToPerformansDTO(PersonelPerformans entity)
        {
            return new PersonelPerformansDTO
            {
                Id = entity.Id,
                PersonelId = entity.PersonelId,
                PersonelAdSoyad = entity.Personel?.AdSoyad ?? "",
                DegerlendirmeTarihi = entity.DegerlendirmeTarihi,
                Donem = entity.Donem,
                DegerlendiriciKullaniciId = entity.DegerlendiriciKullaniciId,
                DegerlendiriciAdSoyad = entity.DegerlendiriciKullanici?.Ad ?? "" + " " + entity.DegerlendiriciKullanici?.Soyad,
                PerformansNotu = entity.PerformansNotu,
                NotSkalasi = entity.NotSkalasi,
                Hedefler = entity.Hedefler,
                HedefBasariOrani = entity.HedefBasariOrani,
                GucluYonler = entity.GucluYonler,
                GelisimAlanlari = entity.GelisimAlanlari,
                Yorumlar = entity.Yorumlar,
                AksiyonPlanlari = entity.AksiyonPlanlari,
                Durum = entity.Durum,
                OnaylayanKullaniciId = entity.OnaylayanKullaniciId,
                OnaylayanAdSoyad = entity.DegerlendiriciKullanici?.Ad ?? "" + " " + entity.DegerlendiriciKullanici?.Soyad,
                OnayTarihi = entity.OnayTarihi,
                EkDosyalar = entity.EkDosyalar,
                Notlar = entity.Notlar
            };
        }

        private PersonelDisiplinDTO MapToDisiplinDTO(PersonelDisiplin entity)
        {
            return new PersonelDisiplinDTO
            {
                Id = entity.Id,
                PersonelId = entity.PersonelId,
                PersonelAdSoyad = entity.Personel?.AdSoyad ?? "",
                DisiplinTuru = entity.DisiplinTuru,
                OlayTarihi = entity.OlayTarihi,
                Aciklama = entity.Aciklama,
                UygulananCeza = entity.UygulananCeza,
                KararVerenYetkiliId = entity.KararVerenYetkiliId,
                KararVerenYetkiliAdi = entity.KararVerenYetkili?.Ad ?? "" + " " + entity.KararVerenYetkili?.Soyad,
                IlgiliDokumanlar = entity.IlgiliDokumanlar,
                Durum = entity.Durum,
                IptalTarihi = entity.IptalTarihi,
                IptalNedeni = entity.IptalNedeni,
                Notlar = entity.Notlar
            };
        }

        private PersonelTerfiDTO MapToTerfiDTO(PersonelTerfi entity)
        {
            return new PersonelTerfiDTO
            {
                Id = entity.Id,
                PersonelId = entity.PersonelId,
                PersonelAdSoyad = entity.Personel?.AdSoyad ?? "",
                TerfiTarihi = entity.TerfiTarihi,
                EskiPozisyon = entity.EskiPozisyon,
                YeniPozisyon = entity.YeniPozisyon,
                EskiUnvan = entity.EskiUnvan,
                YeniUnvan = entity.YeniUnvan,
                EskiDepartmanId = entity.EskiDepartmanId,
                EskiDepartmanAdi = entity.EskiDepartman?.Ad,
                YeniDepartmanId = entity.YeniDepartmanId,
                YeniDepartmanAdi = entity.YeniDepartman?.Ad,
                TerfiNedeni = entity.TerfiNedeni,
                OnaylayanKullaniciId = entity.OnaylayanKullaniciId,
                OnaylayanAdSoyad = entity.OnaylayanKullanici?.Ad ?? "" + " " + entity.OnaylayanKullanici?.Soyad,
                OnayTarihi = entity.OnayTarihi,
                EkDosyalar = entity.EkDosyalar,
                Notlar = entity.Notlar
            };
        }

        private PersonelUcretDegisiklikDTO MapToUcretDegisiklikDTO(PersonelUcretDegisiklik entity)
        {
            return new PersonelUcretDegisiklikDTO
            {
                Id = entity.Id,
                PersonelId = entity.PersonelId,
                PersonelAdSoyad = entity.Personel?.AdSoyad ?? "",
                DegisiklikTarihi = entity.DegisiklikTarihi,
                EskiMaas = entity.EskiMaas,
                YeniMaas = entity.YeniMaas,
                DegisimYuzdesi = entity.DegisimYuzdesi,
                FarkTutari = entity.FarkTutari,
                DegisimNedeni = entity.DegisimNedeni,
                Aciklama = entity.Aciklama,
                OnaylayanKullaniciId = entity.OnaylayanKullaniciId,
                OnaylayanAdSoyad = entity.OnaylayanKullanici?.Ad ?? "" + " " + entity.OnaylayanKullanici?.Soyad,
                OnayTarihi = entity.OnayTarihi,
                EkDosyalar = entity.EkDosyalar,
                Notlar = entity.Notlar
            };
        }

        private PersonelReferansDTO MapToReferansDTO(PersonelReferans entity)
        {
            return new PersonelReferansDTO
            {
                Id = entity.Id,
                PersonelId = entity.PersonelId,
                AdSoyad = entity.AdSoyad,
                SirketKurum = entity.SirketKurum,
                Pozisyon = entity.Pozisyon,
                Iliski = entity.Iliski,
                Telefon = entity.Telefon,
                Email = entity.Email,
                Notlar = entity.Notlar
            };
        }

        private PersonelZimmetDTO MapToZimmetDTO(PersonelZimmet entity)
        {
            return new PersonelZimmetDTO
            {
                Id = entity.Id,
                PersonelId = entity.PersonelId,
                PersonelAdSoyad = entity.Personel?.AdSoyad ?? "",
                EsyaTipi = entity.EsyaTipi,
                EsyaAdi = entity.EsyaAdi,
                MarkaModel = entity.MarkaModel,
                SeriNumarasi = entity.SeriNumarasi,
                ZimmetTarihi = entity.ZimmetTarihi,
                IadeTarihi = entity.IadeTarihi,
                ZimmetDurumu = entity.ZimmetDurumu,
                Degeri = entity.Degeri,
                ZimmetSozlesmesi = entity.ZimmetSozlesmesi,
                ZimmetFotografi = entity.ZimmetFotografi,
                Aciklama = entity.Aciklama,
                ZimmetVerenKullaniciId = entity.ZimmetVerenKullaniciId,
                ZimmetVerenAdSoyad = entity.ZimmetVerenKullanici?.Ad ?? "" + " " + entity.ZimmetVerenKullanici?.Soyad,
                IadeTeslimAlanKullaniciId = entity.IadeTeslimAlanKullaniciId,
                IadeTeslimAlanAdSoyad = entity.IadeTeslimAlanKullanici?.Ad ?? "" + " " + entity.IadeTeslimAlanKullanici?.Soyad,
                Notlar = entity.Notlar
            };
        }

        private PersonelYetkinlikDTO MapToYetkinlikDTO(PersonelYetkinlik entity)
        {
            return new PersonelYetkinlikDTO
            {
                Id = entity.Id,
                PersonelId = entity.PersonelId,
                YetkinlikTipi = entity.YetkinlikTipi,
                YetkinlikAdi = entity.YetkinlikAdi,
                Seviye = entity.Seviye,
                SeviyePuani = entity.SeviyePuani,
                SonKullanimTarihi = entity.SonKullanimTarihi,
                SelfAssessment = entity.SelfAssessment,
                DegerlendiriciKullaniciId = entity.DegerlendiriciKullaniciId,
                DegerlendiriciAdSoyad = entity.DegerlendiriciKullanici?.Ad ?? "" + " " + entity.DegerlendiriciKullanici?.Soyad,
                DegerlendirmeTarihi = entity.DegerlendirmeTarihi,
                BelgelendirenDokumanlar = entity.BelgelendirenDokumanlar,
                Notlar = entity.Notlar
            };
        }

        private PersonelEgitimKayitDTO MapToEgitimKayitDTO(PersonelEgitimKayit entity)
        {
            return new PersonelEgitimKayitDTO
            {
                Id = entity.Id,
                PersonelId = entity.PersonelId,
                EgitimAdi = entity.EgitimAdi,
                EgitmenKurum = entity.EgitmenKurum,
                EgitimTarihi = entity.EgitimTarihi,
                BitisTarihi = entity.BitisTarihi,
                EgitimSuresiSaat = entity.EgitimSuresiSaat,
                TamamlanmaDurumu = entity.TamamlanmaDurumu,
                EgitimMaliyeti = entity.EgitimMaliyeti,
                EgitimSertifikasi = entity.EgitimSertifikasi,
                SertifikaAldiMi = entity.SertifikaAldiMi,
                EgitimTuru = entity.EgitimTuru,
                EgitimKategorisi = entity.EgitimKategorisi,
                EgitimIcerigi = entity.EgitimIcerigi,
                DegerlendirmePuani = entity.DegerlendirmePuani,
                PersonelGeribildirimi = entity.PersonelGeribildirimi,
                EkDosyalar = entity.EkDosyalar,
                Notlar = entity.Notlar
            };
        }

        private PersonelMaliBilgiDTO MapToMaliBilgiDTO(PersonelMaliBilgi entity)
        {
            return new PersonelMaliBilgiDTO
            {
                Id = entity.Id,
                PersonelId = entity.PersonelId,
                BankaAdi = entity.BankaAdi,
                IBAN = entity.IBAN,
                HesapTuru = entity.HesapTuru,
                VergiNo = entity.VergiNo,
                VergiDairesi = entity.VergiDairesi,
                SGKNo = entity.SGKNo,
                SGKBaslangicTarihi = entity.SGKBaslangicTarihi,
                AsgariUcretMuafiyeti = entity.AsgariUcretMuafiyeti,
                GelirVergisiOrani = entity.GelirVergisiOrani,
                EmekliSandigi = entity.EmekliSandigi,
                OdemeYontemi = entity.OdemeYontemi,
                Notlar = entity.Notlar
            };
        }

        private PersonelEkBilgiDTO MapToEkBilgiDTO(PersonelEkBilgi entity)
        {
            return new PersonelEkBilgiDTO
            {
                Id = entity.Id,
                PersonelId = entity.PersonelId,
                MedeniDurum = entity.MedeniDurum,
                AskerlikDurumu = entity.AskerlikDurumu,
                AskerlikBaslangicTarihi = entity.AskerlikBaslangicTarihi,
                AskerlikBitisTarihi = entity.AskerlikBitisTarihi,
                AskerlikYeri = entity.AskerlikYeri,
                AskerlikRutbesi = entity.AskerlikRutbesi,
                EhliyetSiniflari = entity.EhliyetSiniflari,
                EhliyetAlisTarihi = entity.EhliyetAlisTarihi,
                EhliyetGecerlilikTarihi = entity.EhliyetGecerlilikTarihi,
                Uyruk = entity.Uyruk,
                IkametIli = entity.IkametIli,
                IkametIlce = entity.IkametIlce,
                IkametAdresi = entity.IkametAdresi,
                DogumYeri = entity.DogumYeri,
                AnneAdi = entity.AnneAdi,
                BabaAdi = entity.BabaAdi,
                CocukSayisi = entity.CocukSayisi,
                HobiIlgiAlanlari = entity.HobiIlgiAlanlari,
                SosyalGuvence = entity.SosyalGuvence,
                SigortaliMi = entity.SigortaliMi,
                SigortaSirketi = entity.SigortaSirketi,
                SigortaPoliceNo = entity.SigortaPoliceNo,
                Notlar = entity.Notlar
            };
        }

        #endregion
    }
}
