import api from './api';

export interface Masraf {
  id: number;
  personelId: number;
  personelAdi: string;
  masrafTipi: string;
  tutar: number;
  tarih: string;
  aciklama?: string;
  onayDurumu: string;
}

const masrafService = {
  async getAll(): Promise<Masraf[]> {
    const response = await api.get('/Masraf');
    return response.data;
  },
  
  async create(data: Partial<Masraf>): Promise<number> {
    const response = await api.post('/Masraf', data);
    return response.data;
  },
};

export default masrafService;
