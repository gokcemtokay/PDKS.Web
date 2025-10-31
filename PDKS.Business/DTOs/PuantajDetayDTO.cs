
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    // ============================================
    // PUANTAJ DETAY DTO (Günlük bazda)
    // ============================================

    public class PuantajDetayDTO
    {
        public int Id { get; set; }
        public int PuantajId { get; set; }
        public DateTime Tarih { get; set; }
        public string VardiyaAdi { get; set; }
        public TimeSpan? PlanlananGirisSaati { get; set; }
        public TimeSpan? PlanlananCikisSaati { get; set; }
        public DateTime? GerceklesenGirisSaati { get; set; }
        public DateTime? GerceklesenCikisSaati { get; set; }
        public int? ToplamCalismaDakika { get; set; }
        public int? NormalMesaiDakika { get; set; }
        public int? FazlaMesaiDakika { get; set; }
        public int? GecKalmaDakika { get; set; }
        public int? ErkenCikisDakika { get; set; }
        public string Durum { get; set; }
        public string IzinTuru { get; set; }
        public bool HaftaSonuMu { get; set; }
        public bool ResmiTatilMi { get; set; }
        public string Notlar { get; set; }
    }
}
