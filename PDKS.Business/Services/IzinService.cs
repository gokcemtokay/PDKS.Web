using PDKS.Business.DTOs;
using PDKS.Data.Entities;
using PDKS.Data.Repositories;

namespace PDKS.Business.Services
{
    public class IzinService : IIzinService
    {
        private readonly IUnitOfWork _unitOfWork;

        public IzinService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<IzinListDTO>> GetAllAsync()
        {
            var izinler = await _unitOfWork.Izinler.GetAllAsync();
            var personeller = await _unitOfWork.Personeller.GetAllAsync();
            var kullanicilar = await _unitOfWork.Kullanicilar.GetAllAsync();

            return izinler.Select(i => new IzinListDTO
            {
                Id = i.Id,
                PersonelId = i.PersonelId,
                PersonelAdi = personeller.FirstOrDefault(p => p.Id == i.PersonelId)?.AdSoyad ?? "",
                PersonelSicilNo = personeller.FirstOrDefault(p => p.Id == i.PersonelId)?.SicilNo ?? "",
                IzinTipi = i.IzinTipi,
                BaslangicTarihi = i.BaslangicTarihi,
                BitisTarihi = i.BitisTarihi,
                GunSayisi = i.IzinGunSayisi,
                OnayDurumu = i.OnayDurumu,
                OnaylayanKullaniciAdi = i.OnaylayanKullaniciId.HasValue
                    ? kullanicilar.FirstOrDefault(k => k.Id == i.OnaylayanKullaniciId)?.KullaniciAdi
                    : null,
                OnayTarihi = i.OnayTarihi,
                Aciklama = i.Aciklama,
                RedNedeni = i.RedNedeni,
                OlusturmaTarihi = i.OlusturmaTarihi
            }).OrderByDescending(i => i.BaslangicTarihi);
        }

        public async Task<IEnumerable<IzinListDTO>> GetBekleyenIzinlerAsync()
        {
            var izinler = await _unitOfWork.Izinler.FindAsync(i => i.OnayDurumu == "Beklemede");
            var personeller = await _unitOfWork.Personeller.GetAllAsync();

            return izinler.Select(i => new IzinListDTO
            {
                Id = i.Id,
                PersonelId = i.PersonelId,
                PersonelAdi = personeller.FirstOrDefault(p => p.Id == i.PersonelId)?.AdSoyad ?? "",
                PersonelSicilNo = personeller.FirstOrDefault(p => p.Id == i.PersonelId)?.SicilNo ?? "",
                IzinTipi = i.IzinTipi,
                BaslangicTarihi = i.BaslangicTarihi,
                BitisTarihi = i.BitisTarihi,
                GunSayisi = i.IzinGunSayisi,
                OnayDurumu = i.OnayDurumu,
                Aciklama = i.Aciklama,
                OlusturmaTarihi = i.OlusturmaTarihi
            }).OrderBy(i => i.BaslangicTarihi);
        }

        public async Task<IEnumerable<IzinListDTO>> GetByPersonelAsync(int personelId)
        {
            var izinler = await _unitOfWork.Izinler.FindAsync(i => i.PersonelId == personelId);
            var personel = await _unitOfWork.Personeller.GetByIdAsync(personelId);
            var kullanicilar = await _unitOfWork.Kullanicilar.GetAllAsync();

            return izinler.Select(i => new IzinListDTO
            {
                Id = i.Id,
                PersonelId = i.PersonelId,
                PersonelAdi = personel?.AdSoyad ?? "",
                PersonelSicilNo = personel?.SicilNo ?? "",
                IzinTipi = i.IzinTipi,
                BaslangicTarihi = i.BaslangicTarihi,
                BitisTarihi = i.BitisTarihi,
                GunSayisi = i.IzinGunSayisi,
                OnayDurumu = i.OnayDurumu,
                OnaylayanKullaniciAdi = i.OnaylayanKullaniciId.HasValue
                    ? kullanicilar.FirstOrDefault(k => k.Id == i.OnaylayanKullaniciId)?.KullaniciAdi
                    : null,
                OnayTarihi = i.OnayTarihi,
                Aciklama = i.Aciklama,
                RedNedeni = i.RedNedeni,
                OlusturmaTarihi = i.OlusturmaTarihi
            }).OrderByDescending(i => i.BaslangicTarihi);
        }

        public async Task<IzinDetailDTO> GetByIdAsync(int id)
        {
            var izin = await _unitOfWork.Izinler.GetByIdAsync(id);
            if (izin == null)
                throw new Exception("İzin kaydı bulunamadı");

            var personel = await _unitOfWork.Personeller.GetByIdAsync(izin.PersonelId);
            Kullanici? onaylayan = null;
            if (izin.OnaylayanKullaniciId.HasValue)
            {
                onaylayan = await _unitOfWork.Kullanicilar.GetByIdAsync(izin.OnaylayanKullaniciId.Value);
            }

            return new IzinDetailDTO
            {
                Id = izin.Id,
                PersonelId = izin.PersonelId,
                PersonelAdi = personel?.AdSoyad ?? "",
                PersonelSicilNo = personel?.SicilNo ?? "",
                IzinTipi = izin.IzinTipi,
                BaslangicTarihi = izin.BaslangicTarihi,
                BitisTarihi = izin.BitisTarihi,
                GunSayisi = izin.IzinGunSayisi,
                OnayDurumu = izin.OnayDurumu,
                OnaylayanKullaniciAdi = onaylayan?.KullaniciAdi,
                OnayTarihi = izin.OnayTarihi,
                Aciklama = izin.Aciklama,
                RedNedeni = izin.RedNedeni,
                TalepTarihi = izin.TalepTarihi
            };
        }

        public async Task<int> CreateAsync(IzinCreateDTO dto)
        {
            // Tarih kontrolü
            if (dto.BitisTarihi < dto.BaslangicTarihi)
                throw new Exception("Bitiş tarihi başlangıç tarihinden önce olamaz");

            // Gün sayısı hesaplama
            var gunSayisi = (dto.BitisTarihi.Date - dto.BaslangicTarihi.Date).Days + 1;

            // Çakışan izin kontrolü
            var cakisanIzin = await _unitOfWork.Izinler.FirstOrDefaultAsync(i =>
                i.PersonelId == dto.PersonelId &&
                i.OnayDurumu != "Reddedildi" &&
                ((i.BaslangicTarihi <= dto.BaslangicTarihi && i.BitisTarihi >= dto.BaslangicTarihi) ||
                 (i.BaslangicTarihi <= dto.BitisTarihi && i.BitisTarihi >= dto.BitisTarihi) ||
                 (i.BaslangicTarihi >= dto.BaslangicTarihi && i.BitisTarihi <= dto.BitisTarihi)));

            if (cakisanIzin != null)
                throw new Exception("Bu tarihler arasında zaten bir izin kaydı bulunmaktadır");

            var izin = new Izin
            {
                PersonelId = dto.PersonelId,
                IzinTipi = dto.IzinTipi,
                BaslangicTarihi = dto.BaslangicTarihi.Date,
                BitisTarihi = dto.BitisTarihi.Date,
                IzinGunSayisi = gunSayisi,
                OnayDurumu = "Beklemede",
                Aciklama = dto.Aciklama,
                TalepTarihi = DateTime.UtcNow
            };

            await _unitOfWork.Izinler.AddAsync(izin);
            await _unitOfWork.SaveChangesAsync();

            return izin.Id;
        }

        public async Task UpdateAsync(IzinUpdateDTO dto)
        {
            var izin = await _unitOfWork.Izinler.GetByIdAsync(dto.Id);
            if (izin == null)
                throw new Exception("İzin kaydı bulunamadı");

            if (izin.OnayDurumu == "Onaylandı")
                throw new Exception("Onaylanmış izin kaydı düzenlenemez");

            // Tarih kontrolü
            if (dto.BitisTarihi < dto.BaslangicTarihi)
                throw new Exception("Bitiş tarihi başlangıç tarihinden önce olamaz");

            // Gün sayısı hesaplama
            var gunSayisi = (dto.BitisTarihi.Date - dto.BaslangicTarihi.Date).Days + 1;

            // Çakışan izin kontrolü (kendisi hariç)
            var cakisanIzin = await _unitOfWork.Izinler.FirstOrDefaultAsync(i =>
                i.Id != dto.Id &&
                i.PersonelId == dto.PersonelId &&
                i.OnayDurumu != "Reddedildi" &&
                ((i.BaslangicTarihi <= dto.BaslangicTarihi && i.BitisTarihi >= dto.BaslangicTarihi) ||
                 (i.BaslangicTarihi <= dto.BitisTarihi && i.BitisTarihi >= dto.BitisTarihi) ||
                 (i.BaslangicTarihi >= dto.BaslangicTarihi && i.BitisTarihi <= dto.BitisTarihi)));

            if (cakisanIzin != null)
                throw new Exception("Bu tarihler arasında zaten bir izin kaydı bulunmaktadır");

            izin.PersonelId = dto.PersonelId;
            izin.IzinTipi = dto.IzinTipi;
            izin.BaslangicTarihi = dto.BaslangicTarihi.Date;
            izin.BitisTarihi = dto.BitisTarihi.Date;
            izin.IzinGunSayisi = gunSayisi;
            izin.Aciklama = dto.Aciklama;

            _unitOfWork.Izinler.Update(izin);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var izin = await _unitOfWork.Izinler.GetByIdAsync(id);
            if (izin == null)
                throw new Exception("İzin kaydı bulunamadı");

            if (izin.OnayDurumu == "Onaylandı")
                throw new Exception("Onaylanmış izin kaydı silinemez");

            _unitOfWork.Izinler.Remove(izin);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task OnaylaAsync(int id, int onaylayanKullaniciId)
        {
            var izin = await _unitOfWork.Izinler.GetByIdAsync(id);
            if (izin == null)
                throw new Exception("İzin kaydı bulunamadı");

            if (izin.OnayDurumu != "Beklemede")
                throw new Exception("Sadece bekleyen izin kayıtları onaylanabilir");

            izin.OnayDurumu = "Onaylandı";
            izin.OnaylayanKullaniciId = onaylayanKullaniciId;
            izin.OnayTarihi = DateTime.UtcNow;
            izin.RedNedeni = null;

            _unitOfWork.Izinler.Update(izin);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task ReddetAsync(int id, int onaylayanKullaniciId, string redNedeni)
        {
            var izin = await _unitOfWork.Izinler.GetByIdAsync(id);
            if (izin == null)
                throw new Exception("İzin kaydı bulunamadı");

            if (izin.OnayDurumu != "Beklemede")
                throw new Exception("Sadece bekleyen izin kayıtları reddedilebilir");

            if (string.IsNullOrWhiteSpace(redNedeni))
                throw new Exception("Red nedeni belirtilmelidir");

            izin.OnayDurumu = "Reddedildi";
            izin.OnaylayanKullaniciId = onaylayanKullaniciId;
            izin.OnayTarihi = DateTime.UtcNow;
            izin.RedNedeni = redNedeni;

            _unitOfWork.Izinler.Update(izin);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}