using PDKS.Data.Context;
using PDKS.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace PDKS.Data.Repositories
{
    // ============================================
    // PERSONEL ÖZLÜK REPOSITORY CLASS'LARI
    // GenericRepository<T> base class'ını kullanıyor
    // IPersonelXRepository interface'lerini implement ediyor
    // ============================================

    public class PersonelAileRepository : GenericRepository<PersonelAile>, IPersonelAileRepository
    {
        public PersonelAileRepository(PDKSDbContext context) : base(context) { }

        public async Task<IEnumerable<PersonelAile>> GetByPersonelIdAsync(int personelId)
        {
            return await _dbSet
                .Where(x => x.PersonelId == personelId)
                .OrderBy(x => x.Id)
                .ToListAsync();
        }
    }

    public class PersonelAcilDurumRepository : GenericRepository<PersonelAcilDurum>, IPersonelAcilDurumRepository
    {
        public PersonelAcilDurumRepository(PDKSDbContext context) : base(context) { }

        public async Task<IEnumerable<PersonelAcilDurum>> GetByPersonelIdAsync(int personelId)
        {
            return await _dbSet
                .Where(x => x.PersonelId == personelId)
                .OrderBy(x => x.Id)
                .ToListAsync();
        }
    }

    public class PersonelSaglikRepository : GenericRepository<PersonelSaglik>, IPersonelSaglikRepository
    {
        public PersonelSaglikRepository(PDKSDbContext context) : base(context) { }

        public async Task<PersonelSaglik?> GetByPersonelIdAsync(int personelId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(x => x.PersonelId == personelId);
        }
    }

    public class PersonelEgitimRepository : GenericRepository<PersonelEgitim>, IPersonelEgitimRepository
    {
        public PersonelEgitimRepository(PDKSDbContext context) : base(context) { }

        public async Task<IEnumerable<PersonelEgitim>> GetByPersonelIdAsync(int personelId)
        {
            return await _dbSet
                .Where(x => x.PersonelId == personelId)
                .OrderByDescending(x => x.Id)
                .ToListAsync();
        }
    }

    public class PersonelIsDeneyimiRepository : GenericRepository<PersonelIsDeneyimi>, IPersonelIsDeneyimiRepository
    {
        public PersonelIsDeneyimiRepository(PDKSDbContext context) : base(context) { }

        public async Task<IEnumerable<PersonelIsDeneyimi>> GetByPersonelIdAsync(int personelId)
        {
            return await _dbSet
                .Where(x => x.PersonelId == personelId)
                .OrderByDescending(x => x.BaslangicTarihi)
                .ToListAsync();
        }
    }

    public class PersonelDilRepository : GenericRepository<PersonelDil>, IPersonelDilRepository
    {
        public PersonelDilRepository(PDKSDbContext context) : base(context) { }

        public async Task<IEnumerable<PersonelDil>> GetByPersonelIdAsync(int personelId)
        {
            return await _dbSet
                .Where(x => x.PersonelId == personelId)
                .OrderBy(x => x.DilAdi)
                .ToListAsync();
        }
    }

    public class PersonelSertifikaRepository : GenericRepository<PersonelSertifika>, IPersonelSertifikaRepository
    {
        public PersonelSertifikaRepository(PDKSDbContext context) : base(context) { }

        public async Task<IEnumerable<PersonelSertifika>> GetByPersonelIdAsync(int personelId)
        {
            return await _dbSet
                .Where(x => x.PersonelId == personelId)
                .OrderByDescending(x => x.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<PersonelSertifika>> GetExpiringSertifikalarAsync(int daysBeforeExpiry = 30)
        {
            var expiryDate = DateTime.UtcNow.AddDays(daysBeforeExpiry);
            return await _dbSet
                .Where(x => x.GecerlilikTarihi.HasValue &&
                           x.GecerlilikTarihi.Value <= expiryDate &&
                           x.GecerlilikTarihi.Value >= DateTime.UtcNow)
                .OrderBy(x => x.GecerlilikTarihi)
                .ToListAsync();
        }

        public async Task<IEnumerable<PersonelSertifika>> GetExpiredSertifikalarAsync()
        {
            return await _dbSet
                .Where(x => x.GecerlilikTarihi.HasValue &&
                           x.GecerlilikTarihi.Value < DateTime.UtcNow)
                .OrderByDescending(x => x.GecerlilikTarihi)
                .ToListAsync();
        }
    }

    public class PersonelPerformansRepository : GenericRepository<PersonelPerformans>, IPersonelPerformansRepository
    {
        public PersonelPerformansRepository(PDKSDbContext context) : base(context) { }

        public async Task<IEnumerable<PersonelPerformans>> GetByPersonelIdAsync(int personelId)
        {
            return await _dbSet
                .Where(x => x.PersonelId == personelId)
                .OrderByDescending(x => x.DegerlendirmeTarihi)
                .ToListAsync();
        }

        public async Task<IEnumerable<PersonelPerformans>> GetByDonemAsync(string donem)
        {
            return await _dbSet
                .Where(x => x.Donem == donem)
                .OrderBy(x => x.PersonelId)
                .ToListAsync();
        }
    }

    public class PersonelDisiplinRepository : GenericRepository<PersonelDisiplin>, IPersonelDisiplinRepository
    {
        public PersonelDisiplinRepository(PDKSDbContext context) : base(context) { }

        public async Task<IEnumerable<PersonelDisiplin>> GetByPersonelIdAsync(int personelId)
        {
            return await _dbSet
                .Where(x => x.PersonelId == personelId)
                .OrderByDescending(x => x.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<PersonelDisiplin>> GetAktifDisiplinlerAsync(int personelId)
        {
            return await _dbSet
                .Where(x => x.PersonelId == personelId)
                .OrderByDescending(x => x.Id)
                .ToListAsync();
        }
    }

    public class PersonelTerfiRepository : GenericRepository<PersonelTerfi>, IPersonelTerfiRepository
    {
        public PersonelTerfiRepository(PDKSDbContext context) : base(context) { }

        public async Task<IEnumerable<PersonelTerfi>> GetByPersonelIdAsync(int personelId)
        {
            return await _dbSet
                .Where(x => x.PersonelId == personelId)
                .OrderByDescending(x => x.TerfiTarihi)
                .ToListAsync();
        }

        public async Task<PersonelTerfi?> GetLastTerfiAsync(int personelId)
        {
            return await _dbSet
                .Where(x => x.PersonelId == personelId)
                .OrderByDescending(x => x.TerfiTarihi)
                .FirstOrDefaultAsync();
        }
    }

    public class PersonelUcretDegisiklikRepository : GenericRepository<PersonelUcretDegisiklik>, IPersonelUcretDegisiklikRepository
    {
        public PersonelUcretDegisiklikRepository(PDKSDbContext context) : base(context) { }

        public async Task<IEnumerable<PersonelUcretDegisiklik>> GetByPersonelIdAsync(int personelId)
        {
            return await _dbSet
                .Where(x => x.PersonelId == personelId)
                .OrderByDescending(x => x.Id)
                .ToListAsync();
        }

        public async Task<PersonelUcretDegisiklik?> GetLastUcretDegisiklikAsync(int personelId)
        {
            return await _dbSet
                .Where(x => x.PersonelId == personelId)
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync();
        }
    }

    public class PersonelReferansRepository : GenericRepository<PersonelReferans>, IPersonelReferansRepository
    {
        public PersonelReferansRepository(PDKSDbContext context) : base(context) { }

        public async Task<IEnumerable<PersonelReferans>> GetByPersonelIdAsync(int personelId)
        {
            return await _dbSet
                .Where(x => x.PersonelId == personelId)
                .OrderBy(x => x.AdSoyad)
                .ToListAsync();
        }
    }

    public class PersonelZimmetRepository : GenericRepository<PersonelZimmet>, IPersonelZimmetRepository
    {
        public PersonelZimmetRepository(PDKSDbContext context) : base(context) { }

        public async Task<IEnumerable<PersonelZimmet>> GetByPersonelIdAsync(int personelId)
        {
            return await _dbSet
                .Where(x => x.PersonelId == personelId)
                .OrderByDescending(x => x.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<PersonelZimmet>> GetAktifZimmetlerAsync(int personelId)
        {
            return await _dbSet
                .Where(x => x.PersonelId == personelId && x.IadeTarihi == null)
                .OrderByDescending(x => x.Id)
                .ToListAsync();
        }
    }

    public class PersonelYetkinlikRepository : GenericRepository<PersonelYetkinlik>, IPersonelYetkinlikRepository
    {
        public PersonelYetkinlikRepository(PDKSDbContext context) : base(context) { }

        public async Task<IEnumerable<PersonelYetkinlik>> GetByPersonelIdAsync(int personelId)
        {
            return await _dbSet
                .Where(x => x.PersonelId == personelId)
                .OrderBy(x => x.YetkinlikAdi)
                .ToListAsync();
        }

        public async Task<IEnumerable<PersonelYetkinlik>> GetByYetkinlikTipiAsync(int personelId, string yetkinlikTipi)
        {
            return await _dbSet
                .Where(x => x.PersonelId == personelId && x.YetkinlikTipi == yetkinlikTipi)
                .OrderBy(x => x.YetkinlikAdi)
                .ToListAsync();
        }
    }

    public class PersonelEgitimKayitRepository : GenericRepository<PersonelEgitimKayit>, IPersonelEgitimKayitRepository
    {
        public PersonelEgitimKayitRepository(PDKSDbContext context) : base(context) { }

        public async Task<IEnumerable<PersonelEgitimKayit>> GetByPersonelIdAsync(int personelId)
        {
            return await _dbSet
                .Where(x => x.PersonelId == personelId)
                .OrderByDescending(x => x.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<PersonelEgitimKayit>> GetTamamlananEgitimlerAsync(int personelId)
        {
            return await _dbSet
                .Where(x => x.PersonelId == personelId)
                .OrderByDescending(x => x.Id)
                .ToListAsync();
        }
    }

    public class PersonelMaliBilgiRepository : GenericRepository<PersonelMaliBilgi>, IPersonelMaliBilgiRepository
    {
        public PersonelMaliBilgiRepository(PDKSDbContext context) : base(context) { }

        public async Task<PersonelMaliBilgi?> GetByPersonelIdAsync(int personelId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(x => x.PersonelId == personelId);
        }
    }

    public class PersonelEkBilgiRepository : GenericRepository<PersonelEkBilgi>, IPersonelEkBilgiRepository
    {
        public PersonelEkBilgiRepository(PDKSDbContext context) : base(context) { }

        public async Task<PersonelEkBilgi?> GetByPersonelIdAsync(int personelId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(x => x.PersonelId == personelId);
        }
    }



    // ============================================
    // EKSİK BASİT REPOSITORY - TatilRepository
    // ============================================
    public class TatilRepository : GenericRepository<Tatil>, IGenericRepository<Tatil>
    {
        public TatilRepository(PDKSDbContext context) : base(context) { }
    }

    public class SirketRepository : GenericRepository<Sirket>, IGenericRepository<Sirket>
    {
        public SirketRepository(PDKSDbContext context) : base(context) { }
    }
}
