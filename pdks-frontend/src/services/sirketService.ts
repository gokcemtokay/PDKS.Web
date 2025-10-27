import api from './api';

export interface Sirket {
  id: number;
  unvan: string;
  vergiNo: string;
  vergiDairesi: string;
  telefon: string;
  email: string;
  adres: string;
  logoUrl?: string;
  aktif: boolean;
  kayitTarihi: string;
}

export interface SirketCreateDTO {
  unvan: string;
  vergiNo: string;
  vergiDairesi: string;
  telefon: string;
  email: string;
  adres: string;
  logoUrl?: string;
}

export interface SirketUpdateDTO {
  id: number;
  unvan: string;
  vergiNo: string;
  vergiDairesi: string;
  telefon: string;
  email: string;
  adres: string;
  logoUrl?: string;
  aktif: boolean;
}

const sirketService = {
  async getAll(): Promise<Sirket[]> {
    const response = await api.get('/Sirket');
    return response.data;
  },

  async getById(id: number): Promise<Sirket> {
    const response = await api.get(`/Sirket/${id}`);
    return response.data;
  },

  async create(data: SirketCreateDTO): Promise<number> {
    const response = await api.post('/Sirket', data);
    return response.data;
  },

  async update(id: number, data: SirketUpdateDTO): Promise<void> {
    await api.put(`/Sirket/${id}`, data);
  },

  async delete(id: number): Promise<void> {
    await api.delete(`/Sirket/${id}`);
  },
};

export default sirketService;
