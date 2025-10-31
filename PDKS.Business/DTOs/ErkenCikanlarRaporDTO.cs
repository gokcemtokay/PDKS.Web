using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    public class ErkenCikanlarRaporDTO
    {
        public DateTime Tarih { get; set; }
        public int PersonelId { get; set; }
        public string PersonelAdSoyad { get; set; }
        public string SicilNo { get; set; }
        public string DepartmanAdi { get; set; }
        public TimeSpan VardiyaBitis { get; set; }
        public DateTime? CikisSaati { get; set; }
        public int ErkenCikisSuresi { get; set; }
        public string ErkenCikisSuresiStr => $"{ErkenCikisSuresi / 60}:{ErkenCikisSuresi % 60:00}";
    }
}
