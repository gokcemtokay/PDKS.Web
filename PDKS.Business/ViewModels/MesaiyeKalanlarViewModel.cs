using System.Collections.Generic;
using PDKS.Business.DTOs;

public class MesaiyeKalanlarViewModel
{
    public RaporFiltreDTO Filtre { get; set; }
    public IEnumerable<FazlaMesaiRaporDTO> RaporSonuclari { get; set; }

    public MesaiyeKalanlarViewModel()
    {
        // Sayfanın ilk açılışında null hataları almamak için
        // bileşenleri başlangıç değerleriyle oluşturuyoruz.
        Filtre = new RaporFiltreDTO();
        RaporSonuclari = new List<FazlaMesaiRaporDTO>();
    }
}