import api from './api';

export interface Kullanici {
  id: number;
  email: string;
  ad: string;
  soyad: string;
  rolAdi: string;
  rolId: number;
  personelId?: number;
  personelAdi?: string;
  aktif: boolean;
  kayitTarihi: string;
}

export interface KullaniciCreateDTO {
  email: string;
  sifre: string;
  ad: string;
  soyad: string;
  rolId: number;
  personelId?: number;
}

export interface KullaniciUpdateDTO {
  id: number;
  email: string;
  ad: string;
  soyad: string;
  rolId: number;
  personelId?: number;
  aktif: boolean;
  yeniSifre?: string;
}

export interface Rol {
  id: number;
  rolAdi: string;
  aciklama?: string;
}

const kullaniciService = {
  async getAll(): Promise<Kullanici[]> {
    const response = await api.get('/Kullanici');
    return response.data;
  },

  async getById(id: number): Promise<Kullanici> {
    const response = await api.get(`/Kullanici/${id}`);
    return response.data;
  },

  async create(data: KullaniciCreateDTO): Promise<number> {
    const response = await api.post('/Kullanici', data);
    return response.data;
  },

  async update(id: number, data: KullaniciUpdateDTO): Promise<void> {
    await api.put(`/Kullanici/${id}`, data);
  },

  async delete(id: number): Promise<void> {
    await api.delete(`/Kullanici/${id}`);
  },

  async getRoller(): Promise<Rol[]> {
    const response = await api.get('/Rol');
    return response.data;
  },
};

export default kullaniciService;
