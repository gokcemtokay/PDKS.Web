import api from './api';

export interface AracTalebi {
  id: number;
  personelId: number;
  personelAdi: string;
  aracId?: number;
  gidisSehri: string;
  kalkisTarihi: string;
  donusTarihi: string;
  onayDurumu: string;
}

const aracService = {
  async getAll(): Promise<AracTalebi[]> {
    const response = await api.get('/aractalebi');
    return response.data;
  },
  
  async create(data: Partial<AracTalebi>): Promise<number> {
    const response = await api.post('/aractalebi', data);
    return response.data;
  },
};

export default aracService;
