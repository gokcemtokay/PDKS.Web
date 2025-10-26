import api from './api';

export interface GirisCikis {
  id: number;
  personelId: number;
  personelAdi: string;
  girisZamani?: string;
  cikisZamani?: string;
  durum: string;
}

const puantajService = {
  async getAll(): Promise<GirisCikis[]> {
    const response = await api.get('/GirisCikis');
    return response.data;
  },

  async create(data: Partial<GirisCikis>): Promise<number> {
    const response = await api.post('/GirisCikis', data);
    return response.data;
  },

  async update(id: number, data: Partial<GirisCikis>): Promise<void> {
    await api.put(`/GirisCikis/${id}`, data);
  },

  async delete(id: number): Promise<void> {
    await api.delete(`/GirisCikis/${id}`);
  },
};

export default puantajService;
