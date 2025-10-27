import api from './api';

export interface SeyahatTalebi {
  id: number;
  personelId: number;
  personelAdi: string;
  seyahatTipi: string;
  gidisSehri: string;
  varisSehri: string;
  kalkisTarihi: string;
  donusTarihi: string;
  onayDurumu: string;
}

const seyahatService = {
  async getAll(): Promise<SeyahatTalebi[]> {
    const response = await api.get('/seyahattalebi');
    return response.data;
  },
  
  async create(data: Partial<SeyahatTalebi>): Promise<number> {
      const response = await api.post('/seyahattalebi', data);
    return response.data;
  },
};

export default seyahatService;
