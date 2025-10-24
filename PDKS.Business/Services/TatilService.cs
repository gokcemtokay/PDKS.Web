using PDKS.Business.DTOs;
using PDKS.Data.Entities;
using PDKS.Data.Repositories;


namespace PDKS.Business.Services
{
    public class TatilService : ITatilService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TatilService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<TatilListDTO>> GetAllAsync()
        {
            var tatiller = await _unitOfWork.Tatiller.GetAllAsync();
            return tatiller.Select(t => new TatilListDTO
            {
                Id = t.Id,
                Ad = t.Ad,
                Tarih = t.Tarih,
                Aciklama = t.Aciklama
            }).OrderBy(t => t.Tarih);
        }

        public async Task<TatilListDTO> GetByIdAsync(int id)
        {
            var tatil = await _unitOfWork.Tatiller.GetByIdAsync(id);
            if (tatil == null)
                throw new Exception("Tatil bulunamadı");

            return new TatilListDTO
            {
                Id = tatil.Id,
                Ad = tatil.Ad,
                Tarih = tatil.Tarih,
                Aciklama = tatil.Aciklama
            };
        }

        public async Task<int> CreateAsync(TatilCreateDTO dto)
        {
            // Aynı tarihte tatil var mı kontrol et
            var mevcutTatil = await _unitOfWork.Tatiller.FindAsync(t => t.Tarih.Date == dto.Tarih.Date);
            if (mevcutTatil.Any())
                throw new Exception("Bu tarihte zaten bir tatil günü bulunmaktadır");

            var tatil = new Tatil
            {
                Ad = dto.Ad,
                Tarih = dto.Tarih.Date,
                Aciklama = dto.Aciklama
            };

            await _unitOfWork.Tatiller.AddAsync(tatil);
            await _unitOfWork.SaveChangesAsync();

            return tatil.Id;
        }

        public async Task UpdateAsync(TatilUpdateDTO dto)
        {
            var tatil = await _unitOfWork.Tatiller.GetByIdAsync(dto.Id);
            if (tatil == null)
                throw new Exception("Tatil bulunamadı");

            // Aynı tarihte başka tatil var mı kontrol et (kendisi hariç)
            var mevcutTatil = await _unitOfWork.Tatiller.FindAsync(t =>
                t.Tarih.Date == dto.Tarih.Date && t.Id != dto.Id);
            if (mevcutTatil.Any())
                throw new Exception("Bu tarihte zaten bir tatil günü bulunmaktadır");

            tatil.Ad = dto.Ad;
            tatil.Tarih = dto.Tarih.Date;
            tatil.Aciklama = dto.Aciklama;

            _unitOfWork.Tatiller.Update(tatil);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var tatil = await _unitOfWork.Tatiller.GetByIdAsync(id);
            if (tatil == null)
                throw new Exception("Tatil bulunamadı");

            _unitOfWork.Tatiller.Delete(tatil);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> IsTatilAsync(DateTime tarih)
        {
            var tatiller = await _unitOfWork.Tatiller.FindAsync(t => t.Tarih.Date == tarih.Date);
            return tatiller.Any();
        }

        public async Task ResmiTatilleriEkleAsync(int yil)
        {
            var resmiTatiller = new List<(string Ad, int Ay, int Gun, string Aciklama)>
            {
                ("Yılbaşı", 1, 1, "Resmi Tatil"),
                ("Ulusal Egemenlik ve Çocuk Bayramı", 4, 23, "Resmi Tatil"),
                ("Emek ve Dayanışma Günü", 5, 1, "Resmi Tatil"),
                ("Gençlik ve Spor Bayramı", 5, 19, "Resmi Tatil"),
                ("Zafer Bayramı", 8, 30, "Resmi Tatil"),
                ("Cumhuriyet Bayramı", 10, 29, "Resmi Tatil")
            };

            foreach (var (ad, ay, gun, aciklama) in resmiTatiller)
            {
                var tarih = new DateTime(yil, ay, gun);

                // Zaten varsa ekleme
                var mevcutTatil = await _unitOfWork.Tatiller.FindAsync(t => t.Tarih.Date == tarih.Date);
                if (!mevcutTatil.Any())
                {
                    await _unitOfWork.Tatiller.AddAsync(new Tatil
                    {
                        Ad = ad,
                        Tarih = tarih,
                        Aciklama = aciklama
                    });
                }
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}