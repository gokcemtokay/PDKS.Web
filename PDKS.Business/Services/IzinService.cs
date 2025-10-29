using PDKS.Business.DTOs;
using PDKS.Data.Entities;
using PDKS.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDKS.Business.Services
{
    public class IzinService : IIzinService
    {
        private readonly IUnitOfWork _unitOfWork;

        public IzinService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> CreateAsync(IzinCreateDTO dto)
        {
            var personel = await _unitOfWork.Personeller.GetByIdAsync(dto.PersonelId);
            if (personel == null)
                throw new Exception("Personel bulunamadı");

            var izin = new Izin
            {
                PersonelId = dto.PersonelId,
                IzinTipi = dto.IzinTipi,
                BaslangicTarihi = dto.BaslangicTarihi,
                BitisTarihi = dto.BitisTarihi,
                Aciklama = dto.Aciklama,
                OnayDurumu = "Beklemede", // Yeni izin talepleri her zaman "Beklemede" başlar.
                OlusturmaTarihi = DateTime.UtcNow
            };

            await _unitOfWork.Izinler.AddAsync(izin);
            await _unitOfWork.SaveChangesAsync();
            return izin.Id;
        }

        public async Task DeleteAsync(int id)
        {
            var izin = await _unitOfWork.Izinler.GetByIdAsync(id);
            if (izin == null)
                throw new Exception("İzin kaydı bulunamadı");

            _unitOfWork.Izinler.Remove(izin);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<IzinListDTO>> GetAllAsync()
        {
            var izinler = await _unitOfWork.Izinler.GetAllAsync();
            return izinler.Select(i => new IzinListDTO
            {
                Id = i.Id,
                PersonelId = i.PersonelId,
                PersonelAdi = i.Personel?.AdSoyad,
                IzinTipi = i.IzinTipi,
                BaslangicTarihi = i.BaslangicTarihi,
                BitisTarihi = i.BitisTarihi,
                GunSayisi = i.IzinGunSayisi,
                OnayDurumu = i.OnayDurumu,
                OlusturmaTarihi = i.OlusturmaTarihi
            }).OrderByDescending(i => i.OlusturmaTarihi);
        }

        public async Task<IEnumerable<IzinListDTO>> GetBekleyenIzinlerAsync()
        {
            var izinler = await _unitOfWork.Izinler.FindAsync(i => i.OnayDurumu == "Beklemede");
            return izinler.Select(i => new IzinListDTO
            {
                Id = i.Id,
                PersonelId = i.PersonelId,
                PersonelAdi = i.Personel?.AdSoyad,
                IzinTipi = i.IzinTipi,
                BaslangicTarihi = i.BaslangicTarihi,
                BitisTarihi = i.BitisTarihi,
                GunSayisi = i.IzinGunSayisi,
                OnayDurumu = i.OnayDurumu,
                OlusturmaTarihi = i.OlusturmaTarihi
            }).OrderByDescending(i => i.OlusturmaTarihi);
        }

        // PDKS.Business/Services/IzinService.cs dosyasının içindeki GetByIdAsync metodunu bulun
        // ve aşağıdaki kod ile değiştirin.

        public async Task<IzinDetailDTO> GetByIdAsync(int id)
        {
            var izin = await _unitOfWork.Izinler.GetByIdAsync(id);
            if (izin == null)
                return null;

            var onaylayan = izin.OnaylayanKullaniciId.HasValue
                ? await _unitOfWork.Kullanicilar.GetByIdAsync(izin.OnaylayanKullaniciId.Value)
                : null;

            return new IzinDetailDTO
            {
                Id = izin.Id,
                PersonelId = izin.PersonelId,
                PersonelAdi = izin.Personel?.AdSoyad,
                IzinTipi = izin.IzinTipi,
                BaslangicTarihi = izin.BaslangicTarihi,
                BitisTarihi = izin.BitisTarihi,
                GunSayisi = izin.IzinGunSayisi,
                Aciklama = izin.Aciklama,
                OnayDurumu = izin.OnayDurumu,
                OnaylayanKullaniciId = izin.OnaylayanKullaniciId,
                OnaylayanAdi = onaylayan?.Personel?.AdSoyad,
                OnayTarihi = izin.OnayTarihi,
                RedNedeni = izin.RedNedeni,
                OlusturmaTarihi = izin.OlusturmaTarihi,

                // YENİ EKLENEN MAPPING'LER
                PersonelSicilNo = izin.Personel?.SicilNo,
                TalepTarihi = izin.OlusturmaTarihi, // TalepTarihi alanını OlusturmaTarihi ile dolduruyoruz.
                OnaylayanKullaniciAdi = onaylayan?.Personel?.AdSoyad // OnaylayanKullaniciAdi'ni OnaylayanAdi ile dolduruyoruz.
            };
        }

        // YENİ EKLENEN METOT
        public async Task OnaylaReddetAsync(int izinId, string onayDurumu, int onaylayanKullaniciId, string redNedeni)
        {
            var izin = await _unitOfWork.Izinler.GetByIdAsync(izinId);
            if (izin == null)
                throw new Exception("İzin talebi bulunamadı.");

            if (izin.OnayDurumu != "Beklemede")
                throw new Exception("Bu izin talebi zaten daha önce işleme alınmış.");

            if (onayDurumu != "Onaylandı" && onayDurumu != "Reddedildi")
                throw new ArgumentException("Geçersiz onay durumu. Sadece 'Onaylandı' veya 'Reddedildi' olabilir.");

            if (onayDurumu == "Reddedildi" && string.IsNullOrWhiteSpace(redNedeni))
                throw new ArgumentException("İzin talebi reddediliyorsa, red nedeni belirtilmelidir.");

            izin.OnayDurumu = onayDurumu;
            izin.OnaylayanKullaniciId = onaylayanKullaniciId;
            izin.OnayTarihi = DateTime.UtcNow;
            izin.RedNedeni = onayDurumu == "Reddedildi" ? redNedeni : null;

            _unitOfWork.Izinler.Update(izin);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(IzinUpdateDTO dto)
        {
            var izin = await _unitOfWork.Izinler.GetByIdAsync(dto.Id);
            if (izin == null)
                throw new Exception("İzin kaydı bulunamadı");

            // Sadece belirli alanların güncellenmesine izin verelim.
            izin.IzinTipi = dto.IzinTipi;
            izin.BaslangicTarihi = dto.BaslangicTarihi;
            izin.BitisTarihi = dto.BitisTarihi;
            izin.Aciklama = dto.Aciklama;

            _unitOfWork.Izinler.Update(izin);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<IzinListDTO>> GetBySirketAsync(int sirketId)
        {
            // Personelleri şirkete göre filtrele
            var personeller = await _unitOfWork.Personeller.FindAsync(p => p.SirketId == sirketId);
            var personelIds = personeller.Select(p => p.Id).ToList();

            if (!personelIds.Any())
                return Enumerable.Empty<IzinListDTO>();

            // İzinleri o şirketin personellerine göre getir
            var izinler = await _unitOfWork.Izinler.FindAsync(i => personelIds.Contains(i.PersonelId));

            return izinler.Select(i => new IzinListDTO
            {
                Id = i.Id,
                PersonelId = i.PersonelId,
                PersonelAdi = i.Personel?.AdSoyad,
                IzinTipi = i.IzinTipi,
                BaslangicTarihi = i.BaslangicTarihi,
                BitisTarihi = i.BitisTarihi,
                GunSayisi = i.IzinGunSayisi,
                OnayDurumu = i.OnayDurumu,
                Aciklama = i.Aciklama
            }).OrderByDescending(i => i.BaslangicTarihi);
        }
    }
}
