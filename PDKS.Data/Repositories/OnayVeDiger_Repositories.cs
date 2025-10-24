using PDKS.Data.Context;
using PDKS.Data.Entities;

namespace PDKS.Data.Repositories
{
    public class OnayAkisiRepository : GenericRepository<OnayAkisi>, IGenericRepository<OnayAkisi>
    {
        public OnayAkisiRepository(PDKSDbContext context) : base(context) { }
    }

    public class OnayAdimiRepository : GenericRepository<OnayAdimi>, IGenericRepository<OnayAdimi>
    {
        public OnayAdimiRepository(PDKSDbContext context) : base(context) { }
    }

    public class OnayKaydiRepository : GenericRepository<OnayKaydi>, IGenericRepository<OnayKaydi>
    {
        public OnayKaydiRepository(PDKSDbContext context) : base(context) { }
    }

    public class OnayDetayRepository : GenericRepository<OnayDetay>, IGenericRepository<OnayDetay>
    {
        public OnayDetayRepository(PDKSDbContext context) : base(context) { }
    }

    public class PersonelTransferGecmisiRepository : GenericRepository<PersonelTransferGecmisi>, IGenericRepository<PersonelTransferGecmisi>
    {
        public PersonelTransferGecmisiRepository(PDKSDbContext context) : base(context) { }
    }

    public class PrimRepository : GenericRepository<Prim>, IGenericRepository<Prim>
    {
        public PrimRepository(PDKSDbContext context) : base(context) { }
    }

    public class AvansRepository : GenericRepository<Avans>, IGenericRepository<Avans>
    {
        public AvansRepository(PDKSDbContext context) : base(context) { }
    }

    public class DeviceTokenRepository : GenericRepository<DeviceToken>, IGenericRepository<DeviceToken>
    {
        public DeviceTokenRepository(PDKSDbContext context) : base(context) { }
    }

    public class KullaniciSirketRepository : GenericRepository<KullaniciSirket>, IGenericRepository<KullaniciSirket>
    {
        public KullaniciSirketRepository(PDKSDbContext context) : base(context) { }
    }
}
