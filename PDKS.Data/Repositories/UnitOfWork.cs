using PDKS.Data.Context;

namespace PDKS.Data.Repositories
{
    // Unit of Work Implementation
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PDKSDbContext _context;
        private Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction _transaction;

        public UnitOfWork(PDKSDbContext context)
        {
            _context = context;
            Personeller = new GenericRepository<Entities.Personel>(_context);
            Cihazlar = new GenericRepository<Entities.Cihaz>(_context);
            Vardiyalar = new GenericRepository<Entities.Vardiya>(_context);
            GirisCikislar = new GenericRepository<Entities.GirisCikis>(_context);
            Izinler = new GenericRepository<Entities.Izin>(_context);
            Roller = new GenericRepository<Entities.Rol>(_context);
            Kullanicilar = new GenericRepository<Entities.Kullanici>(_context);
            Loglar = new GenericRepository<Entities.Log>(_context);
            Bildirimler = new GenericRepository<Entities.Bildirim>(_context);
            Tatiller = new GenericRepository<Entities.Tatil>(_context);
            Parametreler = new GenericRepository<Entities.Parametre>(_context);
            CihazLoglari = new GenericRepository<Entities.CihazLog>(_context);
            Avanslar = new GenericRepository<Entities.Avans>(_context);
            Primler = new GenericRepository<Entities.Prim>(_context);
            Departmanlar = new GenericRepository<Entities.Departman>(_context);
            Mesailer = new GenericRepository<Entities.Mesai>(_context);
        }

        public IRepository<Entities.Personel> Personeller { get; private set; }
        public IRepository<Entities.Cihaz> Cihazlar { get; private set; }
        public IRepository<Entities.Vardiya> Vardiyalar { get; private set; }
        public IRepository<Entities.GirisCikis> GirisCikislar { get; private set; }
        public IRepository<Entities.Izin> Izinler { get; private set; }
        public IRepository<Entities.Rol> Roller { get; private set; }
        public IRepository<Entities.Kullanici> Kullanicilar { get; private set; }
        public IRepository<Entities.Log> Loglar { get; private set; }
        public IRepository<Entities.Bildirim> Bildirimler { get; private set; }
        public IRepository<Entities.Tatil> Tatiller { get; private set; }
        public IRepository<Entities.Parametre> Parametreler { get; private set; }
        public IRepository<Entities.CihazLog> CihazLoglari { get; private set; }
        public IRepository<Entities.Avans> Avanslar { get; private set; }
        public IRepository<Entities.Prim> Primler { get; private set; }
        public IRepository<Entities.Departman> Departmanlar { get; private set; }
        public IRepository<Entities.Mesai> Mesailer { get; private set; }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                if (_transaction != null)
                {
                    await _transaction.CommitAsync();
                }
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context?.Dispose();
        }
    }
}
