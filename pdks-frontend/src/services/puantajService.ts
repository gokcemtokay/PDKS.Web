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
    const response = await api.get('/giriscikis');
    return response.data;
  },

  async create(data: Partial<GirisCikis>): Promise<number> {
      const response = await api.post('/giriscikis', data);
    return response.data;
  },

  async update(id: number, data: Partial<GirisCikis>): Promise<void> {
      await api.put(`/giriscikis/${id}`, data);
  },

  async delete(id: number): Promise<void> {
      await api.delete(`/giriscikis/${id}`);
  },
};

export default puantajService;
