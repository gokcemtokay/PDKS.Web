using PDKS.Data.Entities;
using PDKS.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PDKS.Data.Repositories
{
    // Puantaj Repository Interface
    public interface IPuantajRepository : IGenericRepository<Puantaj>
    {
        Task<Puantaj> GetWithDetailsAsync(int id);
        Task<IEnumerable<Puantaj>> GetBySirketAsync(int sirketId, int yil, int ay);
        Task<Puantaj> GetByPersonelAsync(int personelId, int yil, int ay);
    }

    // PuantajDetay Repository Interface
    public interface IPuantajDetayRepository : IGenericRepository<PuantajDetay>
    {
        Task<IEnumerable<PuantajDetay>> GetByPuantajIdAsync(int puantajId);
        Task<IEnumerable<PuantajDetay>> GetByTarihAraligAsync(int puantajId, DateTime baslangic, DateTime bitis);
    }

    // Tatil Repository Interface (if not exists)
    public interface ITatilRepository : IGenericRepository<Tatil>
    {
        Task<IEnumerable<Tatil>> GetByYilAsync(int yil);
        Task<Tatil> GetByTarihAsync(DateTime tarih);
    }
}

// IUnitOfWork'e eklenecek satırlar:
/*
public interface IUnitOfWork : IDisposable
{
    // Mevcut repository'ler...
    
    // Yeni eklenecekler:
    IGenericRepository<Puantaj> Puantajlar { get; }
    IGenericRepository<PuantajDetay> PuantajDetaylar { get; }
    IGenericRepository<Tatil> Tatiller { get; }
    
    Task<int> SaveChangesAsync();
}
*/

// UnitOfWork.cs'e eklenecek kod:
/*
private IGenericRepository<Puantaj>? _puantajlar;
private IGenericRepository<PuantajDetay>? _puantajDetaylar;
private IGenericRepository<Tatil>? _tatiller;

public IGenericRepository<Puantaj> Puantajlar => 
    _puantajlar ??= new GenericRepository<Puantaj>(_context);

public IGenericRepository<PuantajDetay> PuantajDetaylar => 
    _puantajDetaylar ??= new GenericRepository<PuantajDetay>(_context);

public IGenericRepository<Tatil> Tatiller => 
    _tatiller ??= new GenericRepository<Tatil>(_context);
*/
