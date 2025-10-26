import api from './api';

export interface Zimmet {
  id: number;
  personelId: number;
  personelAdSoyad: string;
  esyaTipi: string;
  esyaAdi: string;
  zimmetTarihi: string;
  zimmetDurumu: string;
}

const zimmetService = {
  async getByPersonel(personelId: number): Promise<Zimmet[]> {
    const response = await api.get(`/PersonelOzluk/zimmet/${personelId}`);
    return response.data;
  },
  
  async create(data: Partial<Zimmet>): Promise<any> {
    const response = await api.post('/PersonelOzluk/zimmet', data);
    return response.data;
  },

  async iade(id: number, data: any): Promise<void> {
    await api.post(`/PersonelOzluk/zimmet/${id}/iade`, data);
  },
};

export default zimmetService;
