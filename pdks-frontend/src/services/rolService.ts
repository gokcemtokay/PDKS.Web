import api from './api';

export interface Rol {
    id: number;
    rolAdi: string;
    aciklama?: string;
    aktif: boolean;
}

export interface RolYetkiler {
    rolId: number;
    rolAdi: string;
    menuler: Array<{
        menuId: number;
        menuAdi: string;
        okuma: boolean;
    }>;
    islemler: Array<{
        islemYetkiId: number;
        islemKodu: string;
        islemAdi: string;
        izinli: boolean;
    }>;
}

const rolService = {
    // Rol CRUD işlemleri
    async getAll(): Promise<Rol[]> {
        const response = await api.get('/rolyetki'); // ✅ DEĞİŞTİ
        return response.data;
    },

    async getById(id: number): Promise<Rol> {
        const response = await api.get(`/rolyetki/${id}`); // ✅ DEĞİŞTİ
        return response.data;
    },

    async create(data: { rolAdi: string; aciklama?: string; aktif?: boolean }): Promise<Rol> {
        const response = await api.post('/rolyetki', data); // ✅ DEĞİŞTİ
        return response.data;
    },

    async update(id: number, data: { rolAdi: string; aciklama?: string; aktif: boolean }): Promise<Rol> {
        const response = await api.put(`/rolyetki/${id}`, data); // ✅ DEĞİŞTİ
        return response.data;
    },

    async delete(id: number): Promise<void> {
        await api.delete(`/rolyetki/${id}`); // ✅ DEĞİŞTİ
    },

    // Rol yetkileri
    async getRolYetkileri(id: number): Promise<RolYetkiler> {
        const response = await api.get(`/rolyetki/${id}/yetkiler`); // ✅ DEĞİŞTİ
        return response.data;
    },

    // Yetki atama
    async atama(data: {
        rolId: number;
        menuIdler: number[];
        islemYetkiIdler: number[];
    }): Promise<void> {
        await api.post('/rolyetki/ata', data); // Bu zaten doğru
    },

    // Tüm işlem yetkilerini getir
    async getAllIslemYetkileri(): Promise<Array<{
        id: number;
        islemKodu: string;
        islemAdi: string;
        modulAdi?: string;
        aciklama?: string;
        aktif: boolean;
    }>> {
        const response = await api.get('/rolyetki/islem-yetkileri'); // Bu zaten doğru
        return response.data;
    }
};

export default rolService;