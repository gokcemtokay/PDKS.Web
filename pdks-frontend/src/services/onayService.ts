import api from './api';

export interface OnayKaydi {
  onayKaydiId: number;
  modulTipi: string;
  referansId: number;
  talepEdenKisi: string;
  adimAdi: string;
  talepTarihi: string;
  beklemeSuresi: number;
  oncelikDurumu: string;
}

const onayService = {
  async getBekleyenOnaylar(): Promise<OnayKaydi[]> {
    const response = await api.get('/OnayAkisi/bekleyen-onaylar');
    return response.data;
  },

  async onayla(onayDetayId: number, not?: string): Promise<void> {
    await api.post(`/OnayAkisi/onayla/${onayDetayId}`, { not });
  },

  async reddet(onayDetayId: number, not?: string): Promise<void> {
    await api.post(`/OnayAkisi/reddet/${onayDetayId}`, { not });
  },
};

export default onayService;
