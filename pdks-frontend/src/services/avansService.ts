import api from './api';

export interface Avans {
  id: number;
  personelId: number;
  personelAdi: string;
  tutar: number;
  talepTarihi: string;
  durum: string;
  aciklama?: string;
}

const avansService = {
  async getAll(): Promise<Avans[]> {
    const response = await api.get('/avans');
    return response.data;
  },
  
  async create(data: Partial<Avans>): Promise<number> {
    const response = await api.post('/avans', data);
    return response.data;
  },
};

export default avansService;
