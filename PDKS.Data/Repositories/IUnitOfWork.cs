namespace PDKS.Data.Repositories
{
    // Unit of Work Interface
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Entities.Personel> Personeller { get; }
        IRepository<Entities.Cihaz> Cihazlar { get; }
        IRepository<Entities.Vardiya> Vardiyalar { get; }
        IRepository<Entities.GirisCikis> GirisCikislar { get; }
        IRepository<Entities.Izin> Izinler { get; }
        IRepository<Entities.Rol> Roller { get; }
        IRepository<Entities.Kullanici> Kullanicilar { get; }
        IRepository<Entities.Log> Loglar { get; }
        IRepository<Entities.Bildirim> Bildirimler { get; }
        IRepository<Entities.Tatil> Tatiller { get; }
        IRepository<Entities.Parametre> Parametreler { get; }
        IRepository<Entities.CihazLog> CihazLoglari { get; }
        IRepository<Entities.Avans> Avanslar { get; }
        IRepository<Entities.Prim> Primler { get; }

        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
