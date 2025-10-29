using PDKS.Business.DTOs;

namespace PDKS.Business.Services
{
    // Report Service
    public interface IReportService
    {
        // Giriş-Çıkış Raporları
        Task<List<GirisCikisRaporDTO>> KisiBazindaGirisCikisRaporu(int personelId, DateTime baslangic, DateTime bitis);
        Task<List<GirisCikisRaporDTO>> GenelBazdaGirisCikisRaporu(DateTime baslangic, DateTime bitis);

        // Geç Kalanlar
        Task<List<GecKalanlarRaporDTO>> KisiBazindaGecKalanlarRaporu(int personelId, DateTime baslangic, DateTime bitis);
        Task<List<GecKalanlarRaporDTO>> GenelBazdaGecKalanlarRaporu(DateTime baslangic, DateTime bitis);

        // Erken Çıkanlar
        Task<List<ErkenCikanlarRaporDTO>> KisiBazindaErkenCikanlarRaporu(int personelId, DateTime baslangic, DateTime bitis);
        Task<List<ErkenCikanlarRaporDTO>> GenelBazdaErkenCikanlarRaporu(DateTime baslangic, DateTime bitis);

        // Diğer Raporlar
        Task<List<FazlaMesaiRaporDTO>> MesaiyeKalanlarRaporu(DateTime baslangic, DateTime bitis);
        Task<List<DevamsizlarRaporDTO>> DevamsizlarRaporu(DateTime baslangic, DateTime bitis);
        Task<List<IzinliPersonelRaporDTO>> IzinliPersonellerRaporu(DateTime baslangic, DateTime bitis);
        Task<List<TatilGunuCalisanlarRaporDTO>> TatilGunuCalisanlarRaporu(DateTime baslangic, DateTime bitis);
        Task<List<ElleGirisRaporDTO>> ElleGirisRaporu(DateTime baslangic, DateTime bitis);
        Task<List<KartUnutanlarRaporDTO>> KartUnutanlarRaporu(DateTime baslangic, DateTime bitis);
        Task<List<IseGirenlerRaporDTO>> IseGirenlerRaporu(DateTime baslangic, DateTime bitis);
        Task<List<IstenAyrilanlarRaporDTO>> IstenAyrilanlarRaporu(DateTime baslangic, DateTime bitis);
        Task<List<NotluKayitlarRaporDTO>> NotluKayitlarRaporu(DateTime baslangic, DateTime bitis);

        // Maaş ve Prim Raporları
        Task<MaasBordrosuDTO> KisiBazindaMaasBordrosu(int personelId, int yil, int ay);
        Task<List<MaasBordrosuDTO>> GenelBazdaMaasBordrosu(int yil, int ay);
        Task<List<AvansListDTO>> AvansListesi(DateTime baslangic, DateTime bitis);
        Task<MaasZarfiDTO> MaasZarfi(int personelId, int yil, int ay);
        Task<List<PrimListDTO>> PrimListesi(int yil, int ay);
        Task<AylikDevamCizelgesiDTO> AylikDevamCizelgesi(int personelId, int yil, int ay);
    
        //Task<IEnumerable<ReportListDTO>> GetBySirketAsync(int sirketId);
}
}
