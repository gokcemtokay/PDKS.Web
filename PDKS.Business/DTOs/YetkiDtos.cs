namespace PDKS.Business.DTOs
{
    public class MenuDto
    {
        public int Id { get; set; }
        public string MenuAdi { get; set; }
        public string? MenuKodu { get; set; }
        public string? Url { get; set; }
        public string? Icon { get; set; }
        public int? UstMenuId { get; set; }
        public int Sira { get; set; }
        public bool Aktif { get; set; }
        public List<MenuDto> AltMenuler { get; set; } = new();
    }

    public class MenuRolDto
    {
        public int Id { get; set; }
        public int MenuId { get; set; }
        public int RolId { get; set; }
        public bool Okuma { get; set; }
        public string? MenuAdi { get; set; }
        public string? RolAdi { get; set; }
    }

    public class IslemYetkiDto
    {
        public int Id { get; set; }
        public string IslemKodu { get; set; }
        public string IslemAdi { get; set; }
        public string? ModulAdi { get; set; }
        public string? Aciklama { get; set; }
        public bool Aktif { get; set; }
    }

    public class RolIslemYetkiDto
    {
        public int Id { get; set; }
        public int RolId { get; set; }
        public int IslemYetkiId { get; set; }
        public bool Izinli { get; set; }
        public string? IslemKodu { get; set; }
        public string? IslemAdi { get; set; }
    }

    public class RolYetkiAtamaDto
    {
        public int RolId { get; set; }
        public List<int> MenuIdler { get; set; } = new();
        public List<int> IslemYetkiIdler { get; set; } = new();
    }

    public class KullaniciYetkiDto
    {
        public List<MenuDto> Menuler { get; set; } = new();
        public List<string> IslemKodlari { get; set; } = new();
    }
}
