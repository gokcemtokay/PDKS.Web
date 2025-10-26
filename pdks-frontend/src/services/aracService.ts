import api from './api';

export interface AracTalebi {
  id: number;
  personelId: number;
  personelAdi: string;
  aracId?: number;
  gidisSehri: string;
  kalkisTarihi: string;
  donu sTarihi: string;
  onayDurumu: string;
}

const aracService = {
  async getAll(): Promise<AracTalebi[]> {
    const response = await api.get('/AracTalebi');
    return response.data;
  },
  
  async create(data: Partial<AracTalebi>): Promise<number> {
    const response = await api.post('/AracTalebi', data);
    return response.data;
  },
};

export default aracService;
