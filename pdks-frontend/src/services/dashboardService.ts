import api from './api';

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

export interface DogumGunu {
  personelId: number;
  adSoyad: string;
  profilFoto: string;
  dogumTarihi: string;
  kacGunSonra: number;
  bugun: boolean;
}

const dashboardService = {
  async getBugunkunDurum(): Promise<BugunkunDurum> {
    const response = await api.get('/Dashboard/widgets/bugunku-durum');
    return response.data;
  },

  async getBekleyenOnaylar(): Promise<BekleyenOnay[]> {
    const response = await api.get('/Dashboard/widgets/bekleyen-onaylar');
    return response.data;
  },

  async getDogumGunleri(): Promise<DogumGunu[]> {
    const response = await api.get('/Dashboard/widgets/dogum-gunleri');
    return response.data;
  },
};

export default dashboardService;
