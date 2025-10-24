// PDKS.Business/Services/SirketService.cs
using PDKS.Business.DTOs;
using PDKS.Data.Entities;
using PDKS.Data.Repositories;

namespace PDKS.Business.Services
{
    public class SirketService : ISirketService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SirketService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Şirket CRUD

        public async Task<List<SirketListDTO>> GetAllSirketlerAsync()
        {
            var sirketler = await _unitOfWork.Sirketler.GetAllAsync();

            var result = new List<SirketListDTO>();

            foreach (var sirket in sirketler)
            {
                var personelSayisi = await GetSirketPersonelSayisiAsync(sirket.Id);

                result.Add(new SirketListDTO
                {
                    Id = sirket.Id,
                    Unvan = sirket.Unvan,
                    TicariUnvan = sirket.TicariUnvan,
                    VergiNo = sirket.VergiNo,
                    VergiDairesi = sirket.VergiDairesi,
                    Telefon = sirket.Telefon,
                    Email = sirket.Email,
                    Il = sirket.Il,
                    Ilce = sirket.Ilce,  // ⭐ EKLE
                    Aktif = sirket.Aktif,
                    AnaSirket = sirket.AnaSirket,
                    AnaSirketAdi = sirket.AnaSirketId.HasValue
                        ? (await _unitOfWork.Sirketler.GetByIdAsync(sirket.AnaSirketId.Value))?.Unvan
                        : null,
                    PersonelSayisi = personelSayisi,
                    KurulusTarihi = sirket.KurulusTarihi,  // ⭐ EKLE
                    OlusturmaTarihi = sirket.OlusturmaTarihi  // ⭐ EKLE
                });
            }

            return result.OrderBy(s => s.Unvan).ToList();
        }

        public async Task<SirketDetailDTO> GetSirketByIdAsync(int id)
        {
            var sirket = await _unitOfWork.Sirketler.GetByIdAsync(id);

            if (sirket == null)
                throw new Exception($"ID: {id} olan şirket bulunamadı.");

            var personelSayisi = await GetSirketPersonelSayisiAsync(id);
            var departmanSayisi = await _unitOfWork.Departmanlar.CountAsync(d => d.SirketId == id);

            // Bağlı şirketleri getir
            var bagliSirketler = await GetBagliSirketlerAsync(id);

            return new SirketDetailDTO
            {
                Id = sirket.Id,
                Unvan = sirket.Unvan,
                TicariUnvan = sirket.TicariUnvan,
                VergiNo = sirket.VergiNo,
                VergiDairesi = sirket.VergiDairesi,
                Telefon = sirket.Telefon,
                Email = sirket.Email,
                Adres = sirket.Adres,
                Il = sirket.Il,
                Ilce = sirket.Ilce,
                PostaKodu = sirket.PostaKodu,
                Website = sirket.Website,
                LogoUrl = sirket.LogoUrl,
                KurulusTarihi = sirket.KurulusTarihi,
                Aktif = sirket.Aktif,
                ParaBirimi = sirket.ParaBirimi,
                Notlar = sirket.Notlar,
                AnaSirket = sirket.AnaSirket,
                AnaSirketId = sirket.AnaSirketId,
                AnaSirketAdi = sirket.AnaSirketId.HasValue
                    ? (await _unitOfWork.Sirketler.GetByIdAsync(sirket.AnaSirketId.Value))?.Unvan
                    : null,
                PersonelSayisi = personelSayisi,
                DepartmanSayisi = departmanSayisi,
                BagliSirketSayisi = bagliSirketler.Count,
                OlusturmaTarihi = sirket.OlusturmaTarihi,
                GuncellemeTarihi = sirket.GuncellemeTarihi
            };
        }

        public async Task<int> CreateSirketAsync(SirketCreateDTO dto)
        {
            // Vergi numarası kontrolü
            var existingVergiNo = await _unitOfWork.Sirketler
                .FirstOrDefaultAsync(s => s.VergiNo == dto.VergiNo);

            if (existingVergiNo != null)
                throw new Exception("Bu vergi numarası zaten kayıtlı.");

            // Ana şirket kontrolü
            if (dto.AnaSirketId.HasValue)
            {
                var anaSirket = await _unitOfWork.Sirketler.GetByIdAsync(dto.AnaSirketId.Value);
                if (anaSirket == null)
                    throw new Exception("Ana şirket bulunamadı.");

                if (!anaSirket.Aktif)
                    throw new Exception("Ana şirket aktif değil.");
            }

            var sirket = new Sirket
            {
                Unvan = dto.Unvan,
                TicariUnvan = dto.TicariUnvan,
                VergiNo = dto.VergiNo,
                VergiDairesi = dto.VergiDairesi,
                Telefon = dto.Telefon,
                Email = dto.Email,
                Adres = dto.Adres,
                Il = dto.Il,
                Ilce = dto.Ilce,
                PostaKodu = dto.PostaKodu,
                Website = dto.Website,
                LogoUrl = dto.LogoUrl,
                KurulusTarihi = dto.KurulusTarihi,
                Aktif = dto.Aktif,
                ParaBirimi = dto.ParaBirimi ?? "TRY",
                Notlar = dto.Notlar,
                AnaSirket = dto.AnaSirket,
                AnaSirketId = dto.AnaSirketId,
                OlusturmaTarihi = DateTime.UtcNow
            };

            await _unitOfWork.Sirketler.AddAsync(sirket);
            await _unitOfWork.SaveChangesAsync();

            return sirket.Id;
        }

        public async Task UpdateSirketAsync(SirketUpdateDTO dto)
        {
            var sirket = await _unitOfWork.Sirketler.GetByIdAsync(dto.Id);

            if (sirket == null)
                throw new Exception($"ID: {dto.Id} olan şirket bulunamadı.");

            // Vergi numarası kontrolü (kendisi hariç)
            var existingVergiNo = await _unitOfWork.Sirketler
                .FirstOrDefaultAsync(s => s.VergiNo == dto.VergiNo && s.Id != dto.Id);

            if (existingVergiNo != null)
                throw new Exception("Bu vergi numarası başka bir şirket tarafından kullanılıyor.");

            // Ana şirket kontrolü
            if (dto.AnaSirketId.HasValue)
            {
                if (dto.AnaSirketId == dto.Id)
                    throw new Exception("Şirket kendi ana şirketi olamaz.");

                var anaSirket = await _unitOfWork.Sirketler.GetByIdAsync(dto.AnaSirketId.Value);
                if (anaSirket == null)
                    throw new Exception("Ana şirket bulunamadı.");

                // Döngüsel referans kontrolü
                if (await IsCyclicReference(dto.Id, dto.AnaSirketId.Value))
                    throw new Exception("Döngüsel referans oluşturulamaz.");
            }

            sirket.Unvan = dto.Unvan;
            sirket.TicariUnvan = dto.TicariUnvan;
            sirket.VergiNo = dto.VergiNo;
            sirket.VergiDairesi = dto.VergiDairesi;
            sirket.Telefon = dto.Telefon;
            sirket.Email = dto.Email;
            sirket.Adres = dto.Adres;
            sirket.Il = dto.Il;
            sirket.Ilce = dto.Ilce;
            sirket.PostaKodu = dto.PostaKodu;
            sirket.Website = dto.Website;
            sirket.LogoUrl = dto.LogoUrl;
            sirket.KurulusTarihi = dto.KurulusTarihi;
            sirket.Aktif = dto.Aktif;
            sirket.ParaBirimi = dto.ParaBirimi;
            sirket.Notlar = dto.Notlar;
            sirket.AnaSirket = dto.AnaSirket;
            sirket.AnaSirketId = dto.AnaSirketId;
            sirket.GuncellemeTarihi = DateTime.UtcNow;

            _unitOfWork.Sirketler.Update(sirket);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteSirketAsync(int id)
        {
            var sirket = await _unitOfWork.Sirketler.GetByIdAsync(id);

            if (sirket == null)
                throw new Exception($"ID: {id} olan şirket bulunamadı.");

            // Personel kontrolü
            var personelSayisi = await GetSirketPersonelSayisiAsync(id);
            if (personelSayisi > 0)
                throw new Exception("Şirkete bağlı personel bulunmaktadır. Önce personelleri başka şirkete transfer edin.");

            // Bağlı şirket kontrolü
            var bagliSirketler = await GetBagliSirketlerAsync(id);
            if (bagliSirketler.Any())
                throw new Exception("Bu şirkete bağlı alt şirketler bulunmaktadır.");

            // Departman kontrolü
            var departmanSayisi = await _unitOfWork.Departmanlar.CountAsync(d => d.SirketId == id);
            if (departmanSayisi > 0)
                throw new Exception("Şirkete bağlı departmanlar bulunmaktadır.");

            _unitOfWork.Sirketler.Delete(sirket);
            await _unitOfWork.SaveChangesAsync();
        }

        #endregion

        #region Ana Şirket ve Bağlı Şirketler

        public async Task<List<SirketListDTO>> GetAnaSirketlerAsync()
        {
            var sirketler = await _unitOfWork.Sirketler
                .FindAsync(s => s.AnaSirket == true && s.Aktif == true);

            return sirketler.Select(s => new SirketListDTO
            {
                Id = s.Id,
                Unvan = s.Unvan,
                TicariUnvan = s.TicariUnvan,
                VergiNo = s.VergiNo,
                VergiDairesi = s.VergiDairesi,
                Email = s.Email,
                Telefon = s.Telefon,
                Adres = s.Adres,  // ⭐ EKLE
                Il = s.Il,
                Ilce = s.Ilce,  // ⭐ EKLE
                Aktif = s.Aktif,
                AnaSirket = s.AnaSirket,
                PersonelSayisi = 0  // Burada hesaplamaya gerek yok
            }).OrderBy(s => s.Unvan).ToList();
        }

        public async Task<List<SirketListDTO>> GetBagliSirketlerAsync(int anaSirketId)
        {
            var sirketler = await _unitOfWork.Sirketler
                .FindAsync(s => s.AnaSirketId == anaSirketId);

            var result = new List<SirketListDTO>();

            foreach (var sirket in sirketler)
            {
                var personelSayisi = await GetSirketPersonelSayisiAsync(sirket.Id);

                result.Add(new SirketListDTO
                {
                    Id = sirket.Id,
                    Unvan = sirket.Unvan,
                    TicariUnvan = sirket.TicariUnvan,
                    VergiNo = sirket.VergiNo,
                    VergiDairesi = sirket.VergiDairesi,
                    Email = sirket.Email,
                    Telefon = sirket.Telefon,
                    Adres = sirket.Adres,  // ⭐ EKLE
                    Il = sirket.Il,
                    Ilce = sirket.Ilce,  // ⭐ EKLE
                    Aktif = sirket.Aktif,
                    AnaSirket = sirket.AnaSirket,
                    AnaSirketAdi = (await _unitOfWork.Sirketler.GetByIdAsync(anaSirketId))?.Unvan,
                    PersonelSayisi = personelSayisi
                });
            }

            return result.OrderBy(s => s.Unvan).ToList();
        }

        #endregion

        #region Personel Transfer

        public async Task<bool> TransferPersonelAsync(PersonelTransferDTO dto, int kullaniciId)
        {
            var personel = await _unitOfWork.Personeller.GetByIdAsync(dto.PersonelId);

            if (personel == null)
                throw new Exception("Personel bulunamadı.");

            var yeniSirket = await _unitOfWork.Sirketler.GetByIdAsync(dto.YeniSirketId);

            if (yeniSirket == null)
                throw new Exception("Yeni şirket bulunamadı.");

            if (!yeniSirket.Aktif)
                throw new Exception("Hedef şirket aktif değil.");

            // Transfer geçmişi oluştur
            var transferGecmisi = new PersonelTransferGecmisi
            {
                PersonelId = dto.PersonelId,
                EskiSirketId = personel.SirketId,
                YeniSirketId = dto.YeniSirketId,
                EskiDepartmanId = personel.DepartmanId,
                YeniDepartmanId = dto.YeniDepartmanId,
                EskiUnvan = personel.Unvan,
                YeniUnvan = dto.YeniUnvan,
                EskiMaas = personel.Maas,
                YeniMaas = dto.YeniMaas,
                TransferTarihi = dto.TransferTarihi,
                TransferTipi = dto.TransferTipi,
                Sebep = dto.Sebep,
                Notlar = dto.Notlar,
                OnaylayanKullaniciId = kullaniciId,
                OlusturmaTarihi = DateTime.UtcNow
            };

            await _unitOfWork.PersonelTransferGecmisleri.AddAsync(transferGecmisi);

            // Personel bilgilerini güncelle
            personel.SirketId = dto.YeniSirketId;
            personel.DepartmanId = dto.YeniDepartmanId;
            personel.Unvan = dto.YeniUnvan ?? personel.Unvan;
            personel.Maas = dto.YeniMaas ?? personel.Maas;
            personel.GuncellemeTarihi = DateTime.UtcNow;

            _unitOfWork.Personeller.Update(personel);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<List<TransferGecmisiDTO>> GetPersonelTransferGecmisiAsync(int personelId)
        {
            var transferler = await _unitOfWork.PersonelTransferGecmisleri
                .FindAsync(t => t.PersonelId == personelId);

            var result = new List<TransferGecmisiDTO>();

            foreach (var transfer in transferler.OrderByDescending(t => t.TransferTarihi))
            {
                var eskiSirket = await _unitOfWork.Sirketler.GetByIdAsync(transfer.EskiSirketId);
                var yeniSirket = await _unitOfWork.Sirketler.GetByIdAsync(transfer.YeniSirketId);

                result.Add(new TransferGecmisiDTO
                {
                    Id = transfer.Id,
                    PersonelId = transfer.PersonelId,
                    EskiSirketAdi = eskiSirket?.Unvan,
                    YeniSirketAdi = yeniSirket?.Unvan,
                    EskiUnvan = transfer.EskiUnvan,
                    YeniUnvan = transfer.YeniUnvan,
                    EskiMaas = transfer.EskiMaas,
                    YeniMaas = transfer.YeniMaas,
                    TransferTarihi = transfer.TransferTarihi,
                    TransferTipi = transfer.TransferTipi,
                    Sebep = transfer.Sebep,
                    Notlar = transfer.Notlar
                });
            }

            return result;
        }

        public async Task<List<TransferGecmisiDTO>> GetSirketTransferGecmisiAsync(int sirketId, DateTime? baslangic = null, DateTime? bitis = null)
        {
            var allTransfers = await _unitOfWork.PersonelTransferGecmisleri.GetAllAsync();

            var transferler = allTransfers.Where(t =>
                t.EskiSirketId == sirketId || t.YeniSirketId == sirketId);

            if (baslangic.HasValue)
                transferler = transferler.Where(t => t.TransferTarihi >= baslangic.Value);

            if (bitis.HasValue)
                transferler = transferler.Where(t => t.TransferTarihi <= bitis.Value);

            var result = new List<TransferGecmisiDTO>();

            foreach (var transfer in transferler.OrderByDescending(t => t.TransferTarihi))
            {
                var personel = await _unitOfWork.Personeller.GetByIdAsync(transfer.PersonelId);
                var eskiSirket = await _unitOfWork.Sirketler.GetByIdAsync(transfer.EskiSirketId);
                var yeniSirket = await _unitOfWork.Sirketler.GetByIdAsync(transfer.YeniSirketId);

                result.Add(new TransferGecmisiDTO
                {
                    Id = transfer.Id,
                    PersonelId = transfer.PersonelId,
                    PersonelAdSoyad = personel?.AdSoyad,
                    EskiSirketAdi = eskiSirket?.Unvan,
                    YeniSirketAdi = yeniSirket?.Unvan,
                    EskiUnvan = transfer.EskiUnvan,
                    YeniUnvan = transfer.YeniUnvan,
                    TransferTarihi = transfer.TransferTarihi,
                    TransferTipi = transfer.TransferTipi,
                    Sebep = transfer.Sebep
                });
            }

            return result;
        }

        #endregion

        #region Konsolide Raporlar

        public async Task<List<KonsolideRaporDTO>> GetKonsolideRaporAsync(DateTime baslangic, DateTime bitis)
        {
            var anaSirketler = await GetAnaSirketlerAsync();
            var result = new List<KonsolideRaporDTO>();

            foreach (var anaSirket in anaSirketler)
            {
                var rapor = await GetSirketKonsolideRaporAsync(anaSirket.Id, baslangic, bitis);
                result.Add(rapor);
            }

            return result;
        }

        public async Task<KonsolideRaporDTO> GetSirketKonsolideRaporAsync(int sirketId, DateTime baslangic, DateTime bitis)
        {
            var sirket = await _unitOfWork.Sirketler.GetByIdAsync(sirketId);

            if (sirket == null)
                throw new Exception("Şirket bulunamadı.");

            var bagliSirketler = await GetBagliSirketlerAsync(sirketId);
            var tumSirketIds = new List<int> { sirketId };
            tumSirketIds.AddRange(bagliSirketler.Select(s => s.Id));

            // Toplam personel sayısı
            var allPersonel = await _unitOfWork.Personeller.GetAllAsync();
            var toplamPersonel = allPersonel.Count(p => tumSirketIds.Contains(p.SirketId) && p.Durum);

            // Transfer sayısı
            var allTransfers = await _unitOfWork.PersonelTransferGecmisleri.GetAllAsync();
            var transferSayisi = allTransfers.Count(t =>
                (tumSirketIds.Contains(t.EskiSirketId) || tumSirketIds.Contains(t.YeniSirketId))
                && t.TransferTarihi >= baslangic && t.TransferTarihi <= bitis);

            // Departman sayısı
            var allDepartman = await _unitOfWork.Departmanlar.GetAllAsync();
            var departmanSayisi = allDepartman.Count(d => tumSirketIds.Contains(d.SirketId));

            return new KonsolideRaporDTO
            {
                SirketId = sirketId,
                SirketAdi = sirket.Unvan,
                BagliSirketSayisi = bagliSirketler.Count,
                ToplamPersonelSayisi = toplamPersonel,
                ToplamDepartmanSayisi = departmanSayisi,
                TransferSayisi = transferSayisi,
                Donem = $"{baslangic:dd.MM.yyyy} - {bitis:dd.MM.yyyy}"
            };
        }

        #endregion

        #region İstatistikler ve Yardımcı Metodlar

        public async Task<bool> SirketAktifMiAsync(int sirketId)
        {
            var sirket = await _unitOfWork.Sirketler.GetByIdAsync(sirketId);
            return sirket?.Aktif ?? false;
        }

        public async Task<int> GetSirketPersonelSayisiAsync(int sirketId)
        {
            var personeller = await _unitOfWork.Personeller.FindAsync(p => p.SirketId == sirketId && p.Durum);
            return personeller.Count();
        }

        private async Task<bool> IsCyclicReference(int sirketId, int anaSirketId)
        {
            var visited = new HashSet<int>();
            var currentId = anaSirketId;

            while (currentId > 0)
            {
                if (currentId == sirketId)
                    return true;

                if (visited.Contains(currentId))
                    return true;

                visited.Add(currentId);

                var sirket = await _unitOfWork.Sirketler.GetByIdAsync(currentId);
                currentId = sirket?.AnaSirketId ?? 0;
            }

            return false;
        }

        #endregion
    }
}