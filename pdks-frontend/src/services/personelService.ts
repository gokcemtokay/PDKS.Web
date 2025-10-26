import api from './api';

export interface Personel {
  id: number;
  sicilNo: string;
  adSoyad: string;
  email: string;
  telefon: string;
  departmanAdi?: string;
  vardiyaAdi?: string;
  durum: boolean;
  iseGirisTarihi?: string;
  pozisyon?: string;
}

export interface PersonelDetay extends Personel {
  tcKimlik?: string;
  dogumTarihi?: string;
  cinsiyet?: string;
  medeniDurum?: string;
  maas?: number;
  // ... daha fazla alan
}

const personelService = {
  async getAll(): Promise<Personel[]> {
    const response = await api.get('/Personel');
    return response.data;
  },

  async getById(id: number): Promise<PersonelDetay> {
    const response = await api.get(`/Personel/${id}`);
    return response.data;
  },

  async create(data: Partial<Personel>): Promise<number> {
    const response = await api.post('/Personel', data);
    return response.data;
  },

  async update(id: number, data: Partial<Personel>): Promise<void> {
    await api.put(`/Personel/${id}`, data);
  },

  async delete(id: number): Promise<void> {
    await api.delete(`/Personel/${id}`);
  },
};

export default personelService;
