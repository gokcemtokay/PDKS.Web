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

    // Fotoðraf yükleme
    async uploadPhoto(personelId: number, file: File): Promise<any> {
        const formData = new FormData();
        formData.append('file', file);

        const response = await api.post(`/personel/${personelId}/foto`, formData, {
            headers: {
                'Content-Type': 'multipart/form-data',
            },
        });

        return response.data;
    },

    // Fotoðraf silme
    async deletePhoto(personelId: number): Promise<any> {
        const response = await api.delete(`/personel/${personelId}/foto`);
        return response.data;
    },

    // Fotoðraf URL'i alma
    getPhotoUrl(personelId: number): string {
        return `${api.defaults.baseURL}/personel/${personelId}/foto`;
    },
};



export default personelService;
