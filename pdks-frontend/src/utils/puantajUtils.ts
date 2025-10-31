// Puantaj Utility Functions

export const formatDakika = (dakika: number): string => {
  const saat = Math.floor(dakika / 60);
  const kalanDakika = dakika % 60;
  return `${saat}:${kalanDakika.toString().padStart(2, '0')}`;
};

export const formatSaat = (dakika: number): string => {
  return (dakika / 60).toFixed(2);
};

export const getDurumBadgeClass = (durum: string): string => {
  switch (durum) {
    case 'Onaylandı':
      return 'success';
    case 'Taslak':
      return 'warning';
    case 'Kapalı':
      return 'secondary';
    default:
      return 'primary';
  }
};

export const getGunDurumuBadgeClass = (gunDurumu: string): string => {
  switch (gunDurumu) {
    case 'Normal':
      return 'success';
    case 'Izin':
      return 'info';
    case 'Rapor':
      return 'warning';
    case 'Devamsiz':
      return 'danger';
    case 'HaftaSonu':
      return 'secondary';
    case 'ResmiTatil':
      return 'primary';
    default:
      return 'light';
  }
};

export const getGunDurumuText = (gunDurumu: string): string => {
  const durumMap: { [key: string]: string } = {
    'Normal': 'Normal',
    'Izin': 'İzinli',
    'Rapor': 'Raporlu',
    'Devamsiz': 'Devamsız',
    'HaftaSonu': 'Hafta Sonu',
    'ResmiTatil': 'Resmi Tatil'
  };
  return durumMap[gunDurumu] || gunDurumu;
};

export const getTurkceAyAdi = (ay: number): string => {
  const aylar = [
    'Ocak', 'Şubat', 'Mart', 'Nisan', 'Mayıs', 'Haziran',
    'Temmuz', 'Ağustos', 'Eylül', 'Ekim', 'Kasım', 'Aralık'
  ];
  return aylar[ay - 1] || '';
};

export const getDonemText = (yil: number, ay: number): string => {
  return `${getTurkceAyAdi(ay)} ${yil}`;
};

export const formatTarih = (tarih: string, format: 'short' | 'long' = 'short'): string => {
  const date = new Date(tarih);
  
  if (format === 'short') {
    return date.toLocaleDateString('tr-TR');
  }
  
  return date.toLocaleDateString('tr-TR', {
    weekday: 'long',
    year: 'numeric',
    month: 'long',
    day: 'numeric'
  });
};

export const formatSaat24 = (saat?: string): string => {
  if (!saat) return '-';
  const date = new Date(saat);
  return date.toLocaleTimeString('tr-TR', { hour: '2-digit', minute: '2-digit' });
};

export const formatTimeSpan = (timeSpan?: string): string => {
  if (!timeSpan) return '-';
  return timeSpan.substring(0, 5); // "HH:mm:ss" -> "HH:mm"
};

export const calculateYuzde = (deger: number, toplam: number): number => {
  if (toplam === 0) return 0;
  return Math.round((deger / toplam) * 100);
};

export const getAyListesi = (): { value: number; label: string }[] => {
  return Array.from({ length: 12 }, (_, i) => ({
    value: i + 1,
    label: getTurkceAyAdi(i + 1)
  }));
};

export const getYilListesi = (baslangic?: number, adet: number = 5): number[] => {
  const simdikiYil = baslangic || new Date().getFullYear();
  return Array.from({ length: adet }, (_, i) => simdikiYil - i);
};

export const getGunRengi = (gunDurumu: string): string => {
  switch (gunDurumu) {
    case 'Normal':
      return '#28a745';
    case 'Izin':
      return '#17a2b8';
    case 'Rapor':
      return '#ffc107';
    case 'Devamsiz':
      return '#dc3545';
    case 'HaftaSonu':
      return '#6c757d';
    case 'ResmiTatil':
      return '#007bff';
    default:
      return '#f8f9fa';
  }
};

export const exportToCSV = (data: any[], filename: string): void => {
  const headers = Object.keys(data[0]).join(',');
  const rows = data.map(row => Object.values(row).join(','));
  const csv = [headers, ...rows].join('\n');
  
  const blob = new Blob([csv], { type: 'text/csv;charset=utf-8;' });
  const link = document.createElement('a');
  const url = URL.createObjectURL(blob);
  
  link.setAttribute('href', url);
  link.setAttribute('download', `${filename}.csv`);
  link.style.visibility = 'hidden';
  
  document.body.appendChild(link);
  link.click();
  document.body.removeChild(link);
};

export const validateDonem = (yil: number, ay: number): boolean => {
  const simdikiTarih = new Date();
  const seciliTarih = new Date(yil, ay - 1);
  
  // Gelecek tarih seçilemez
  return seciliTarih <= simdikiTarih;
};

export const getSonrakiDonem = (yil: number, ay: number): { yil: number; ay: number } => {
  if (ay === 12) {
    return { yil: yil + 1, ay: 1 };
  }
  return { yil, ay: ay + 1 };
};

export const getOncekiDonem = (yil: number, ay: number): { yil: number; ay: number } => {
  if (ay === 1) {
    return { yil: yil - 1, ay: 12 };
  }
  return { yil, ay: ay - 1 };
};
