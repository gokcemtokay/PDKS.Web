using PDKS.Business.DTOs;
using PDKS.Data.Entities;
using PDKS.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace PDKS.Business.Services
{
    public class PuantajService : IPuantajService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PuantajService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> HesaplaPuantajAsync(PuantajHesaplaDTO dto)
        {
            // Mevcut puantaj kontrolü
            var mevcutPuantaj = await _unitOfWork.Puantajlar
                .FindAsync(p => p.PersonelId == dto.PersonelId && p.Yil == dto.Yil && p.Ay == dto.Ay);
            
            var ilkKayit = mevcutPuantaj.FirstOrDefault();
            
            if (ilkKayit != null && !dto.YenidenHesapla)
            {
                throw new Exception("Bu dönem için puantaj zaten mevcut. Yeniden hesaplamak için YenidenHesapla=true gönderin.");
            }

            // Personel bilgilerini al
            var personel = await _unitOfWork.Personeller.GetByIdAsync(dto.PersonelId);
            if (personel == null)
                throw new Exception("Personel bulunamadı");

            // Dönem tarih aralığını belirle
            var baslangic = new DateTime(dto.Yil, dto.Ay, 1);
            var bitis = baslangic.AddMonths(1).AddDays(-1);

            // Giriş-çıkış kayıtlarını al
            var girisCikislar = await _unitOfWork.GirisCikislar
                .FindAsync(g => g.PersonelId == dto.PersonelId 
                    && g.GirisZamani.HasValue 
                    && g.GirisZamani.Value.Date >= baslangic 
                    && g.GirisZamani.Value.Date <= bitis);

            // İzin kayıtlarını al
            var izinler = await _unitOfWork.Izinler
                .FindAsync(i => i.PersonelId == dto.PersonelId 
                    && i.OnayDurumu == "Onaylandı"
                    && ((i.BaslangicTarihi >= baslangic && i.BaslangicTarihi <= bitis)
                        || (i.BitisTarihi >= baslangic && i.BitisTarihi <= bitis)));

            // Tatil günlerini al
            var tatiller = await _unitOfWork.Tatiller
                .FindAsync(t => t.Tarih >= baslangic && t.Tarih <= bitis);

            // Puantaj oluştur veya güncelle
            Puantaj puantaj;
            if (ilkKayit != null)
            {
                puantaj = ilkKayit;
                // Detayları temizle
                var eskiDetaylar = await _unitOfWork.PuantajDetaylar
                    .FindAsync(pd => pd.PuantajId == puantaj.Id);
                foreach (var detay in eskiDetaylar)
                {
                    _unitOfWork.PuantajDetaylar.Remove(detay);
                }
            }
            else
            {
                puantaj = new Puantaj
                {
                    PersonelId = dto.PersonelId,
                    SirketId = personel.SirketId,
                    Yil = dto.Yil,
                    Ay = dto.Ay,
                    OlusturmaTarihi = DateTime.UtcNow
                };
                await _unitOfWork.Puantajlar.AddAsync(puantaj);
            }

            // Günlük detayları hesapla
            var detaylar = new List<PuantajDetay>();
            var toplamCalismaDakika = 0;
            var normalMesaiDakika = 0;
            var fazlaMesaiDakika = 0;
            var geceMesaiDakika = 0;
            var haftaSonuMesaiDakika = 0;
            var calisilanGun = 0;
            var devamsizlikGunu = 0;
            var izinGunu = 0;
            var raporluGun = 0;
            var haftaTatiliGunu = 0;
            var resmiTatilGunu = 0;
            var gecKalmaGunu = 0;
            var gecKalmaDakika = 0;
            var erkenCikisGunu = 0;
            var erkenCikisDakika = 0;
            var eksikCalismaDakika = 0;

            for (var tarih = baslangic; tarih <= bitis; tarih = tarih.AddDays(1))
            {
                var detay = await HesaplaGunlukDetayAsync(
                    personel, 
                    tarih, 
                    girisCikislar.ToList(), 
                    izinler.ToList(), 
                    tatiller.ToList()
                );

                detay.PuantajId = puantaj.Id;
                detaylar.Add(detay);

                // Toplamları güncelle
                if (detay.GerceklesenSure.HasValue)
                {
                    toplamCalismaDakika += detay.GerceklesenSure.Value;
                }

                if (detay.NormalMesai.HasValue)
                    normalMesaiDakika += detay.NormalMesai.Value;

                if (detay.FazlaMesai.HasValue)
                    fazlaMesaiDakika += detay.FazlaMesai.Value;

                if (detay.GeceMesai.HasValue)
                    geceMesaiDakika += detay.GeceMesai.Value;

                if (detay.HaftaSonuMu && detay.GerceklesenSure.HasValue)
                    haftaSonuMesaiDakika += detay.GerceklesenSure.Value;

                if (detay.GunDurumu == "Normal" && detay.GerceklesenSure > 0)
                    calisilanGun++;
                else if (detay.GunDurumu == "Devamsiz")
                    devamsizlikGunu++;
                else if (detay.GunDurumu == "Izin")
                    izinGunu++;
                else if (detay.GunDurumu == "Rapor")
                    raporluGun++;
                else if (detay.GunDurumu == "HaftaSonu")
                    haftaTatiliGunu++;
                else if (detay.GunDurumu == "ResmiTatil")
                    resmiTatilGunu++;

                if (detay.GecKaldiMi)
                {
                    gecKalmaGunu++;
                    if (detay.GecKalmaSuresi.HasValue)
                        gecKalmaDakika += detay.GecKalmaSuresi.Value;
                }

                if (detay.ErkenCiktiMi)
                {
                    erkenCikisGunu++;
                    if (detay.ErkenCikisSuresi.HasValue)
                        erkenCikisDakika += detay.ErkenCikisSuresi.Value;
                }

                if (detay.EksikSure.HasValue && detay.EksikSure.Value > 0)
                    eksikCalismaDakika += detay.EksikSure.Value;
            }

            // Puantaj toplamlarını güncelle
            puantaj.ToplamCalismaSaati = toplamCalismaDakika;
            puantaj.NormalMesaiSaati = normalMesaiDakika;
            puantaj.FazlaMesaiSaati = fazlaMesaiDakika;
            puantaj.GeceMesaiSaati = geceMesaiDakika;
            puantaj.HaftaSonuMesaiSaati = haftaSonuMesaiDakika;
            puantaj.ToplamCalisilanGun = calisilanGun;
            puantaj.DevamsizlikGunu = devamsizlikGunu;
            puantaj.IzinGunu = izinGunu;
            puantaj.RaporluGun = raporluGun;
            puantaj.HaftaTatiliGunu = haftaTatiliGunu;
            puantaj.ResmiTatilGunu = resmiTatilGunu;
            puantaj.GecKalmaGunu = gecKalmaGunu;
            puantaj.GecKalmaSuresi = gecKalmaDakika;
            puantaj.ErkenCikisGunu = erkenCikisGunu;
            puantaj.ErkenCikisSuresi = erkenCikisDakika;
            puantaj.EksikCalismaSaati = eksikCalismaDakika;
            puantaj.GuncellemeTarihi = DateTime.UtcNow;

            // Detayları ekle
            foreach (var detay in detaylar)
            {
                await _unitOfWork.PuantajDetaylar.AddAsync(detay);
            }

            await _unitOfWork.SaveChangesAsync();
            return puantaj.Id;
        }

        private async Task<PuantajDetay> HesaplaGunlukDetayAsync(
            Personel personel, 
            DateTime tarih, 
            List<GirisCikis> girisCikislar,
            List<Izin> izinler,
            List<Tatil> tatiller)
        {
            var detay = new PuantajDetay
            {
                PersonelId = personel.Id,
                Tarih = tarih,
                OlusturmaTarihi = DateTime.UtcNow
            };

            // Hafta sonu kontrolü
            var haftaSonuMu = tarih.DayOfWeek == DayOfWeek.Saturday || tarih.DayOfWeek == DayOfWeek.Sunday;
            detay.HaftaSonuMu = haftaSonuMu;

            // Resmi tatil kontrolü
            var resmiTatil = tatiller.FirstOrDefault(t => t.Tarih.Date == tarih.Date);
            detay.ResmiTatilMi = resmiTatil != null;

            // İzin kontrolü
            var izin = izinler.FirstOrDefault(i => 
                i.BaslangicTarihi.Date <= tarih.Date && i.BitisTarihi.Date >= tarih.Date);

            if (izin != null)
            {
                detay.GunDurumu = "Izin";
                detay.IzinTuru = izin.IzinTipi;
                return detay;
            }

            // Vardiya bilgisi
            if (personel.VardiyaId.HasValue)
            {
                var vardiya = await _unitOfWork.Vardiyalar.GetByIdAsync(personel.VardiyaId.Value);
                if (vardiya != null)
                {
                    detay.VardiyaId = vardiya.Id;
                    detay.PlanlananGiris = vardiya.BaslangicSaati;
                    detay.PlanlananCikis = vardiya.BitisSaati;
                    
                    // Planlanan süreyi hesapla
                    var baslangic = DateTime.Today.Add(vardiya.BaslangicSaati);
                    var bitis = DateTime.Today.Add(vardiya.BitisSaati);
                    if (bitis < baslangic)
                        bitis = bitis.AddDays(1);
                    detay.PlanlananSure = (int)(bitis - baslangic).TotalMinutes;
                }
            }

            // Resmi tatil ise
            if (resmiTatil != null)
            {
                detay.GunDurumu = "ResmiTatil";
                // Giriş-çıkış varsa fazla mesai olarak değerlendir
                var gunlukGiris = girisCikislar.FirstOrDefault(g => 
                    g.GirisZamani.HasValue && g.GirisZamani.Value.Date == tarih.Date);
                
                if (gunlukGiris != null && gunlukGiris.CikisZamani.HasValue)
                {
                    detay.GerceklesenGiris = gunlukGiris.GirisZamani;
                    detay.GerceklesenCikis = gunlukGiris.CikisZamani;
                    var sure = (int)(gunlukGiris.CikisZamani.Value - gunlukGiris.GirisZamani.Value).TotalMinutes;
                    detay.GerceklesenSure = sure;
                    detay.FazlaMesai = sure;
                    detay.GirisCikisId = gunlukGiris.Id;
                }
                return detay;
            }

            // Hafta sonu ise
            if (haftaSonuMu)
            {
                detay.GunDurumu = "HaftaSonu";
                // Giriş-çıkış varsa çalışma olarak kaydet
                var gunlukGiris = girisCikislar.FirstOrDefault(g => 
                    g.GirisZamani.HasValue && g.GirisZamani.Value.Date == tarih.Date);
                
                if (gunlukGiris != null && gunlukGiris.CikisZamani.HasValue)
                {
                    detay.GerceklesenGiris = gunlukGiris.GirisZamani;
                    detay.GerceklesenCikis = gunlukGiris.CikisZamani;
                    var sure = (int)(gunlukGiris.CikisZamani.Value - gunlukGiris.GirisZamani.Value).TotalMinutes;
                    detay.GerceklesenSure = sure;
                    detay.FazlaMesai = sure;
                    detay.GirisCikisId = gunlukGiris.Id;
                }
                return detay;
            }

            // Normal çalışma günü
            var giris = girisCikislar.FirstOrDefault(g => 
                g.GirisZamani.HasValue && g.GirisZamani.Value.Date == tarih.Date);

            if (giris == null || !giris.CikisZamani.HasValue)
            {
                detay.GunDurumu = "Devamsiz";
                detay.DevamsizMi = true;
                return detay;
            }

            detay.GunDurumu = "Normal";
            detay.GerceklesenGiris = giris.GirisZamani;
            detay.GerceklesenCikis = giris.CikisZamani;
            detay.GirisCikisId = giris.Id;

            var calismaSuresi = (int)(giris.CikisZamani.Value - giris.GirisZamani.Value).TotalMinutes;
            detay.GerceklesenSure = calismaSuresi;

            // Planlanan süre varsa karşılaştır
            if (detay.PlanlananSure > 0)
            {
                if (calismaSuresi >= detay.PlanlananSure)
                {
                    detay.NormalMesai = detay.PlanlananSure;
                    detay.FazlaMesai = calismaSuresi - detay.PlanlananSure;
                }
                else
                {
                    detay.NormalMesai = calismaSuresi;
                    detay.EksikSure = detay.PlanlananSure - calismaSuresi;
                }

                // Geç kalma kontrolü
                if (detay.PlanlananGiris.HasValue)
                {
                    var planlananGirisSaat = DateTime.Today.Add(detay.PlanlananGiris.Value);
                    var gercekGirisSaat = giris.GirisZamani.Value;
                    var gecikme = (int)(gercekGirisSaat.TimeOfDay - planlananGirisSaat.TimeOfDay).TotalMinutes;
                    
                    if (gecikme > 0)
                    {
                        detay.GecKaldiMi = true;
                        detay.GecKalmaSuresi = gecikme;
                    }
                }

                // Erken çıkış kontrolü
                if (detay.PlanlananCikis.HasValue)
                {
                    var planlananCikisSaat = DateTime.Today.Add(detay.PlanlananCikis.Value);
                    var gercekCikisSaat = giris.CikisZamani.Value;
                    var erken = (int)(planlananCikisSaat.TimeOfDay - gercekCikisSaat.TimeOfDay).TotalMinutes;
                    
                    if (erken > 0)
                    {
                        detay.ErkenCiktiMi = true;
                        detay.ErkenCikisSuresi = erken;
                    }
                }
            }
            else
            {
                detay.NormalMesai = calismaSuresi;
            }

            // Gece mesai kontrolü (22:00 - 06:00 arası)
            var geceBaslangic = new TimeSpan(22, 0, 0);
            var geceBitis = new TimeSpan(6, 0, 0);
            // Basit gece mesai hesabı - detaylı hesaplama için genişletilebilir
            if (giris.GirisZamani.Value.TimeOfDay >= geceBaslangic || 
                giris.CikisZamani.Value.TimeOfDay <= geceBitis)
            {
                // Gece mesai var
                detay.GeceMesai = 0; // Detaylı hesaplama gerekli
            }

            return detay;
        }

        public async Task<List<int>> TopluPuantajHesaplaAsync(TopluPuantajHesaplaDTO dto)
        {
            var personelIdler = new List<int>();

            if (dto.TumPersonel)
            {
                var tumPersonel = await _unitOfWork.Personeller
                    .FindAsync(p => p.Durum == true);
                personelIdler = tumPersonel.Select(p => p.Id).ToList();
            }
            else if (dto.DepartmanId.HasValue)
            {
                var departmanPersoneli = await _unitOfWork.Personeller
                    .FindAsync(p => p.DepartmanId == dto.DepartmanId && p.Durum == true);
                personelIdler = departmanPersoneli.Select(p => p.Id).ToList();
            }
            else if (dto.PersonelIdler != null && dto.PersonelIdler.Any())
            {
                personelIdler = dto.PersonelIdler;
            }

            var sonuclar = new List<int>();

            foreach (var personelId in personelIdler)
            {
                try
                {
                    var hesaplaDto = new PuantajHesaplaDTO
                    {
                        PersonelId = personelId,
                        Yil = dto.Yil,
                        Ay = dto.Ay,
                        YenidenHesapla = true
                    };
                    var puantajId = await HesaplaPuantajAsync(hesaplaDto);
                    sonuclar.Add(puantajId);
                }
                catch
                {
                    // Hata durumunda devam et
                    continue;
                }
            }

            return sonuclar;
        }

        public async Task<PuantajDetailDTO> YenidenHesaplaAsync(int puantajId)
        {
            var puantaj = await _unitOfWork.Puantajlar.GetByIdAsync(puantajId);
            if (puantaj == null)
                throw new Exception("Puantaj bulunamadı");

            var dto = new PuantajHesaplaDTO
            {
                PersonelId = puantaj.PersonelId,
                Yil = puantaj.Yil,
                Ay = puantaj.Ay,
                YenidenHesapla = true
            };

            await HesaplaPuantajAsync(dto);
            return await GetByIdAsync(puantajId);
        }

        public async Task<PuantajDetailDTO> GetByIdAsync(int id)
        {
            var puantaj = await _unitOfWork.Puantajlar.GetByIdAsync(id);
            if (puantaj == null)
                return null;

            var personel = puantaj.Personel;
            var detaylar = await _unitOfWork.PuantajDetaylar
                .FindAsync(pd => pd.PuantajId == id);

            var culture = new CultureInfo("tr-TR");
            var ayAdi = culture.DateTimeFormat.GetMonthName(puantaj.Ay);

            return new PuantajDetailDTO
            {
                Id = puantaj.Id,
                PersonelId = puantaj.PersonelId,
                PersonelAdi = personel?.AdSoyad,
                SicilNo = personel?.SicilNo,
                Departman = personel?.Departman?.Ad,
                Unvan = personel?.Unvan,
                Yil = puantaj.Yil,
                Ay = puantaj.Ay,
                ToplamCalismaSaati = puantaj.ToplamCalismaSaati,
                NormalMesaiSaati = puantaj.NormalMesaiSaati,
                FazlaMesaiSaati = puantaj.FazlaMesaiSaati,
                GeceMesaiSaati = puantaj.GeceMesaiSaati,
                HaftaSonuMesaiSaati = puantaj.HaftaSonuMesaiSaati,
                ToplamCalisilanGun = puantaj.ToplamCalisilanGun,
                DevamsizlikGunu = puantaj.DevamsizlikGunu,
                IzinGunu = puantaj.IzinGunu,
                RaporluGun = puantaj.RaporluGun,
                HaftaTatiliGunu = puantaj.HaftaTatiliGunu,
                ResmiTatilGunu = puantaj.ResmiTatilGunu,
                GecKalmaGunu = puantaj.GecKalmaGunu,
                GecKalmaSuresi = puantaj.GecKalmaSuresi,
                ErkenCikisGunu = puantaj.ErkenCikisGunu,
                ErkenCikisSuresi = puantaj.ErkenCikisSuresi,
                EksikCalismaSaati = puantaj.EksikCalismaSaati,
                Durum = puantaj.Durum,
                OnayTarihi = puantaj.OnayTarihi,
                OnaylayanKisi = puantaj.OnaylayanKullanici != null ? 
                    $"{puantaj.OnaylayanKullanici.Ad} {puantaj.OnaylayanKullanici.Soyad}" : null,
                Notlar = puantaj.Notlar,
                GunlukDetaylar = detaylar.Select(d => MapToDetayDTO(d)).OrderBy(d => d.Tarih).ToList()
            };
        }

        public async Task<PuantajDetailDTO> GetByPersonelVeDonemAsync(int personelId, int yil, int ay)
        {
            var puantaj = (await _unitOfWork.Puantajlar
                .FindAsync(p => p.PersonelId == personelId && p.Yil == yil && p.Ay == ay))
                .FirstOrDefault();

            if (puantaj == null)
                return null;

            return await GetByIdAsync(puantaj.Id);
        }

        public async Task<IEnumerable<PuantajListDTO>> GetByDonemAsync(int yil, int ay, int? departmanId = null)
        {
            var query = await _unitOfWork.Puantajlar
                .FindAsync(p => p.Yil == yil && p.Ay == ay);

            var puantajlar = query.ToList();

            if (departmanId.HasValue)
            {
                puantajlar = puantajlar
                    .Where(p => p.Personel?.DepartmanId == departmanId.Value)
                    .ToList();
            }

            var culture = new CultureInfo("tr-TR");
            var ayAdi = culture.DateTimeFormat.GetMonthName(ay);

            return puantajlar.Select(p => new PuantajListDTO
            {
                Id = p.Id,
                PersonelId = p.PersonelId,
                PersonelAdi = p.Personel?.AdSoyad,
                SicilNo = p.Personel?.SicilNo,
                DepartmanId = p.Personel?.DepartmanId,
                Departman = p.Personel?.Departman?.Ad,
                Yil = p.Yil,
                Ay = p.Ay,
                Donem = $"{ayAdi} {yil}",
                ToplamCalismaSaati = p.ToplamCalismaSaati,
                ToplamCalisilanGun = p.ToplamCalisilanGun,
                FazlaMesaiSaati = p.FazlaMesaiSaati,
                DevamsizlikGunu = p.DevamsizlikGunu,
                IzinGunu = p.IzinGunu,
                Durum = p.Durum,
                OnayTarihi = p.OnayTarihi
            }).OrderBy(p => p.PersonelAdi);
        }

        public async Task<IEnumerable<PuantajListDTO>> GetByPersonelAsync(int personelId)
        {
            var puantajlar = await _unitOfWork.Puantajlar
                .FindAsync(p => p.PersonelId == personelId);

            var culture = new CultureInfo("tr-TR");

            return puantajlar.Select(p => new PuantajListDTO
            {
                Id = p.Id,
                PersonelId = p.PersonelId,
                PersonelAdi = p.Personel?.AdSoyad,
                SicilNo = p.Personel?.SicilNo,
                Departman = p.Personel?.Departman?.Ad,
                Yil = p.Yil,
                Ay = p.Ay,
                Donem = $"{culture.DateTimeFormat.GetMonthName(p.Ay)} {p.Yil}",
                ToplamCalismaSaati = p.ToplamCalismaSaati,
                ToplamCalisilanGun = p.ToplamCalisilanGun,
                FazlaMesaiSaati = p.FazlaMesaiSaati,
                DevamsizlikGunu = p.DevamsizlikGunu,
                IzinGunu = p.IzinGunu,
                Durum = p.Durum,
                OnayTarihi = p.OnayTarihi
            }).OrderByDescending(p => p.Yil).ThenByDescending(p => p.Ay);
        }

        public async Task<bool> OnaylaAsync(PuantajOnayDTO dto)
        {
            var puantaj = await _unitOfWork.Puantajlar.GetByIdAsync(dto.PuantajId);
            if (puantaj == null)
                return false;

            puantaj.Durum = "Onaylandı";
            puantaj.OnayTarihi = DateTime.UtcNow;
            puantaj.OnaylayanKullaniciId = dto.OnaylayanKullaniciId;
            if (!string.IsNullOrEmpty(dto.Notlar))
                puantaj.Notlar = dto.Notlar;
            puantaj.GuncellemeTarihi = DateTime.UtcNow;

            _unitOfWork.Puantajlar.Update(puantaj);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> OnayIptalAsync(int puantajId)
        {
            var puantaj = await _unitOfWork.Puantajlar.GetByIdAsync(puantajId);
            if (puantaj == null)
                return false;

            puantaj.Durum = "Taslak";
            puantaj.OnayTarihi = null;
            puantaj.OnaylayanKullaniciId = null;
            puantaj.GuncellemeTarihi = DateTime.UtcNow;

            _unitOfWork.Puantajlar.Update(puantaj);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var puantaj = await _unitOfWork.Puantajlar.GetByIdAsync(id);
            if (puantaj == null)
                return false;

            // Detayları sil
            var detaylar = await _unitOfWork.PuantajDetaylar
                .FindAsync(pd => pd.PuantajId == id);
            
            foreach (var detay in detaylar)
            {
                _unitOfWork.PuantajDetaylar.Remove(detay);
            }

            _unitOfWork.Puantajlar.Remove(puantaj);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<List<PuantajDetayDTO>> GetGunlukDetaylarAsync(int puantajId)
        {
            var detaylar = await _unitOfWork.PuantajDetaylar
                .FindAsync(pd => pd.PuantajId == puantajId);

            return detaylar.Select(d => MapToDetayDTO(d)).OrderBy(d => d.Tarih).ToList();
        }

        public async Task<PuantajDetayDTO> GetDetayByTarihAsync(int personelId, DateTime tarih)
        {
            var detay = (await _unitOfWork.PuantajDetaylar
                .FindAsync(pd => pd.PersonelId == personelId && pd.Tarih.Date == tarih.Date))
                .FirstOrDefault();

            return detay != null ? MapToDetayDTO(detay) : null;
        }

        public async Task<PuantajOzetRaporDTO> GetOzetRaporAsync(PuantajRaporParametreDTO parametre)
        {
            // Basitleştirilmiş rapor - genişletilebilir
            var yil = parametre.BaslangicTarihi.Year;
            var ay = parametre.BaslangicTarihi.Month;

            var puantajlar = await GetByDonemAsync(yil, ay, parametre.DepartmanId);
            var liste = puantajlar.ToList();

            var culture = new CultureInfo("tr-TR");
            var ayAdi = culture.DateTimeFormat.GetMonthName(ay);

            return new PuantajOzetRaporDTO
            {
                Donem = $"{ayAdi} {yil}",
                ToplamPersonel = liste.Count,
                ToplamCalismaSaati = liste.Sum(p => p.ToplamCalismaSaati),
                ToplamFazlaMesai = liste.Sum(p => p.FazlaMesaiSaati),
                ToplamDevamsizlik = liste.Sum(p => p.DevamsizlikGunu),
                PersonelPuantajlari = liste
            };
        }

        public async Task<List<DepartmanPuantajOzetDTO>> GetDepartmanOzetAsync(int yil, int ay)
        {
            var puantajlar = (await GetByDonemAsync(yil, ay)).ToList();


            var departmanGruplari = puantajlar
                .GroupBy(p => new { DepartmanId = p.DepartmanId, DepartmanAdi = p.Departman })
                .Select(g => new DepartmanPuantajOzetDTO
                {
                    DepartmanId = g.Key.DepartmanId ?? 0,
                    DepartmanAdi = g.Key.DepartmanAdi ?? "Tanımsız",
                    PersonelSayisi = g.Count(),
                    ToplamCalismaSaati = g.Sum(p => p.ToplamCalismaSaati),
                    ToplamFazlaMesai = g.Sum(p => p.FazlaMesaiSaati),
                    ToplamDevamsizlik = g.Sum(p => p.DevamsizlikGunu),
                    OrtalamaCalismaOrani = (decimal)g.Average(p => p.ToplamCalisilanGun)
                })
                .ToList();

            return departmanGruplari;
        }

        public async Task<byte[]> ExportToExcelAsync(PuantajRaporParametreDTO parametre)
        {
            // Excel export için ClosedXML veya EPPlus kullanılabilir
            // Şimdilik placeholder
            throw new NotImplementedException("Excel export özelliği eklenecek");
        }

        public async Task<bool> PuantajVarMiAsync(int personelId, int yil, int ay)
        {
            var puantaj = await _unitOfWork.Puantajlar
                .FindAsync(p => p.PersonelId == personelId && p.Yil == yil && p.Ay == ay);
            return puantaj.Any();
        }

        public async Task<List<string>> ValidasyonKontrolAsync(int personelId, int yil, int ay)
        {
            var hatalar = new List<string>();

            var personel = await _unitOfWork.Personeller.GetByIdAsync(personelId);
            if (personel == null)
            {
                hatalar.Add("Personel bulunamadı");
                return hatalar;
            }

            if (!personel.VardiyaId.HasValue)
                hatalar.Add("Personele vardiya atanmamış");

            var baslangic = new DateTime(yil, ay, 1);
            var bitis = baslangic.AddMonths(1).AddDays(-1);

            var girisCikislar = await _unitOfWork.GirisCikislar
                .FindAsync(g => g.PersonelId == personelId 
                    && g.GirisZamani.HasValue 
                    && g.GirisZamani.Value.Date >= baslangic 
                    && g.GirisZamani.Value.Date <= bitis);

            if (!girisCikislar.Any())
                hatalar.Add("Bu dönem için giriş-çıkış kaydı bulunamadı");

            var eksikCikislar = girisCikislar.Where(g => !g.CikisZamani.HasValue).ToList();
            if (eksikCikislar.Any())
                hatalar.Add($"{eksikCikislar.Count} adet eksik çıkış kaydı var");

            return hatalar;
        }

        private PuantajDetayDTO MapToDetayDTO(PuantajDetay detay)
        {
            var culture = new CultureInfo("tr-TR");
            var gunAdi = culture.DateTimeFormat.GetDayName(detay.Tarih.DayOfWeek);

            return new PuantajDetayDTO
            {
                Id = detay.Id,
                Tarih = detay.Tarih,
                Gun = gunAdi,
                VardiyaAdi = detay.Vardiya?.Ad,
                PlanlananGiris = detay.PlanlananGiris,
                PlanlananCikis = detay.PlanlananCikis,
                GerceklesenGiris = detay.GerceklesenGiris,
                GerceklesenCikis = detay.GerceklesenCikis,
                GerceklesenSure = detay.GerceklesenSure,
                NormalMesai = detay.NormalMesai,
                FazlaMesai = detay.FazlaMesai,
                GecKalmaSuresi = detay.GecKalmaSuresi,
                ErkenCikisSuresi = detay.ErkenCikisSuresi,
                GunDurumu = detay.GunDurumu,
                IzinTuru = detay.IzinTuru,
                HaftaSonuMu = detay.HaftaSonuMu,
                ResmiTatilMi = detay.ResmiTatilMi,
                GecKaldiMi = detay.GecKaldiMi,
                ErkenCiktiMi = detay.ErkenCiktiMi,
                DevamsizMi = detay.DevamsizMi,
                Notlar = detay.Notlar
            };
        }
    }
}
