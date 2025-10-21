// src/types/dashboard.ts - KOMPLE YENÝDEN YAZ

export interface BugunkunDurum {
    toplamPersonel: number;
    bugunkuGiris: number;
    aktifPersonel: number;
    izinliPersonel: number;
    raporluPersonel: number;
    gecKalanPersonel: number;
    devamsizPersonel: number;
    girisCikisOrani: number;
}

export interface BekleyenOnay {
    onayKaydiId: number;
    modulTipi: string;
    talepEden: string;
    adimAdi: string;
    talepTarihi: string;
    beklemeSuresi: number;
    oncelikDurumu: string;
}

export interface SonAktivite {
    tip: string;
    baslik: string;
    aciklama: string;
    kullanici: string;
    tarih: string;
    icon: string;
    renk: string;
}

export interface DogumGunu {
    personelId: number;
    adSoyad: string;
    profilFoto: string;
    dogumTarihi: string;
    kacGunSonra: number;
    bugun: boolean;
}

export interface YilDonumu {
    personelId: number;
    adSoyad: string;
    profilFoto: string;
    girisTarihi: string;
    kacYil: number;
    kacGunSonra: number;
    bugun: boolean;
}

export interface AnaDashboard {
    bugunkunDurum: BugunkunDurum;
    bekleyenOnaylar: BekleyenOnay[];
    sonAktiviteler: SonAktivite[];
    dogumGunleri: DogumGunu[];
    yilDonumleri: YilDonumu[];
    duyurular: any[];
}