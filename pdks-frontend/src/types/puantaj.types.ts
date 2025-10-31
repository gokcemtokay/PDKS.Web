// Puantaj Types

export interface PuantajList {
  id: number;
  personelId: number;
  personelAdi: string;
  sicilNo: string;
  departman: string;
  yil: number;
  ay: number;
  donem: string;
  toplamCalismaSaati: number;
  toplamCalisilanGun: number;
  fazlaMesaiSaati: number;
  devamsizlikGunu: number;
  izinGunu: number;
  durum: string;
  onayTarihi?: string;
}

export interface PuantajDetail {
  id: number;
  personelId: number;
  personelAdi: string;
  sicilNo: string;
  departman: string;
  unvan: string;
  yil: number;
  ay: number;
  
  toplamCalismaSaati: number;
  toplamCalismaSaatFormatli: number;
  normalMesaiSaati: number;
  fazlaMesaiSaati: number;
  geceMesaiSaati: number;
  haftaSonuMesaiSaati: number;
  
  toplamCalisilanGun: number;
  devamsizlikGunu: number;
  izinGunu: number;
  raporluGun: number;
  haftaTatiliGunu: number;
  resmiTatilGunu: number;
  
  gecKalmaGunu: number;
  gecKalmaSuresi: number;
  erkenCikisGunu: number;
  erkenCikisSuresi: number;
  eksikCalismaSaati: number;
  
  durum: string;
  onayTarihi?: string;
  onaylayanKisi?: string;
  notlar?: string;
  
  gunlukDetaylar: PuantajDetay[];
}

export interface PuantajDetay {
  id: number;
  tarih: string;
  gun: string;
  vardiyaAdi?: string;
  planlananGiris?: string;
  planlananCikis?: string;
  gerceklesenGiris?: string;
  gerceklesenCikis?: string;
  gerceklesenSure?: number;
  normalMesai?: number;
  fazlaMesai?: number;
  gecKalmaSuresi?: number;
  erkenCikisSuresi?: number;
  gunDurumu: string;
  izinTuru?: string;
  haftaSonuMu: boolean;
  resmiTatilMi: boolean;
  gecKaldiMi: boolean;
  erkenCiktiMi: boolean;
  devamsizMi: boolean;
  notlar?: string;
}

export interface PuantajHesaplaRequest {
  personelId: number;
  yil: number;
  ay: number;
  yenidenHesapla?: boolean;
}

export interface TopluPuantajHesaplaRequest {
  personelIdler?: number[];
  departmanId?: number;
  yil: number;
  ay: number;
  tumPersonel?: boolean;
}

export interface PuantajOnayRequest {
  puantajId: number;
  onaylayanKullaniciId: number;
  notlar?: string;
}

export interface PuantajRaporParametre {
  baslangicTarihi: string;
  bitisTarihi: string;
  personelId?: number;
  departmanId?: number;
  personelIdler?: number[];
  raporTuru?: string;
}

export interface PuantajOzetRapor {
  donem: string;
  toplamPersonel: number;
  toplamCalismaSaati: number;
  toplamFazlaMesai: number;
  toplamDevamsizlik: number;
  personelPuantajlari: PuantajList[];
}

export interface DepartmanPuantajOzet {
  departmanId: number;
  departmanAdi: string;
  personelSayisi: number;
  toplamCalismaSaati: number;
  toplamFazlaMesai: number;
  toplamDevamsizlik: number;
  ortalamaCalismaOrani: number;
}

export interface PuantajKontrol {
  puantajMevcut: boolean;
  validasyonHatalari: string[];
  hazir: boolean;
}

// Utility Types
export type PuantajDurumu = 'Taslak' | 'Onaylandı' | 'Kapalı';
export type GunDurumu = 'Normal' | 'Tatil' | 'Izin' | 'Rapor' | 'Devamsiz' | 'HaftaSonu' | 'ResmiTatil';
