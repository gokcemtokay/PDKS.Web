using PDKS.Business.DTOs;
using PDKS.Data.Entities;
using PDKS.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDKS.Business.Services
{
    public class KullaniciService : IKullaniciService
    {
        private readonly IUnitOfWork _unitOfWork;

        public KullaniciService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> CreateAsync(KullaniciCreateDTO dto)
        {
            var existingUser = await _unitOfWork.Kullanicilar.FirstOrDefaultAsync(k => k.Email == dto.Email);
            if (existingUser != null)
                throw new Exception("Bu e-posta adresi zaten kullanılıyor.");

            var personel = await _unitOfWork.Personeller.GetByIdAsync(dto.PersonelId);
            if (personel == null)
                throw new Exception("İlişkilendirilecek personel bulunamadı.");

            var kullanici = new Kullanici
            {
                PersonelId = dto.PersonelId,
                Email = dto.Email,
                SifreHash = dto.Sifre,
                RolId = dto.RolId,
                Aktif = dto.Aktif,
                OlusturmaTarihi = DateTime.UtcNow
            };

            await _unitOfWork.Kullanicilar.AddAsync(kullanici);
            await _unitOfWork.SaveChangesAsync();
            return kullanici.Id;
        }

        public async Task<IEnumerable<KullaniciListDTO>> GetAllAsync()
        {
            var kullanicilar = await _unitOfWork.Kullanicilar.GetAllAsync();
            return kullanicilar.Select(k => new KullaniciListDTO
            {
                Id = k.Id,
                PersonelAdi = k.Personel?.AdSoyad,
                Email = k.Email,
                Rol = k.Rol?.RolAdi,
                Aktif = k.Aktif,

                // --- Eski View'lar için Alanları Doldurma ---
                KullaniciAdi = k.Personel?.AdSoyad,
                PersonelSicilNo = k.Personel?.SicilNo,
                RolAdi = k.Rol?.RolAdi,
                SonGirisTarihi = k.SonGirisTarihi // Entity'de bu alan varsa doldurulur, yoksa null kalır.
            }).OrderBy(k => k.PersonelAdi);
        }

        public async Task<KullaniciUpdateDTO> GetByIdAsync(int id)
        {
            var kullanici = await _unitOfWork.Kullanicilar.GetByIdAsync(id);
            if (kullanici == null)
                return null;

            return new KullaniciUpdateDTO
            {
                Id = kullanici.Id,
                PersonelId = kullanici.PersonelId.Value,
                Email = kullanici.Email,
                RolId = kullanici.RolId,
                Aktif = kullanici.Aktif,
                KullaniciAdi = kullanici.Personel?.AdSoyad // HATAYI GİDERMEK İÇİN EKLENDİ
            };
        }

        public async Task UpdateAsync(KullaniciUpdateDTO dto)
        {
            var kullanici = await _unitOfWork.Kullanicilar.GetByIdAsync(dto.Id);

            if (kullanici == null)
                throw new Exception($"Kullanıcı bulunamadı");

            // ✅ Sadece bu alanları güncelle

            kullanici.Email = dto.Email;
            kullanici.RolId = dto.RolId;
            kullanici.Aktif = dto.Aktif;
            // ❌ PersonelId'yi GÜNCELLEME (mevcut değeri koru)

            if (!string.IsNullOrEmpty(dto.Sifre))
            {
                kullanici.SifreHash = dto.Sifre;
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}