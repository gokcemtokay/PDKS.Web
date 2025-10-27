import api from './api';

export interface Izin {
  id: number;
  personelId: number;
  personelAdi: string;
  izinTipi: string;
  baslangicTarihi: string;
  bitisTarihi: string;
  gunSayisi: number;
  aciklama?: string;
  onayDurumu: string;
  talepTarihi: string;
}

export interface IzinTalepDTO {
  personelId: number;
  izinTipi: string;
  baslangicTarihi: string;
  bitisTarihi: string;
  aciklama?: string;
}

const izinService = {
  async getAll(): Promise<Izin[]> {
    const response = await api.get('/izin');
    return response.data;
  },

  async getByPersonel(personelId: number): Promise<Izin[]> {
      const response = await api.get(`/izin/personel/${personelId}`);
    return response.data;
  },

  async create(data: IzinTalepDTO): Promise<number> {
      const response = await api.post('/izin', data);
    return response.data;
  },

  async getBakiye(personelId: number): Promise<any> {
      const response = await api.get(`/izin/bakiye/${personelId}`);
    return response.data;
  },
};

export default izinService;
