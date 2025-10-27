import api from './api';

export interface Menu {
    id: number;
    menuAdi: string;
    menuKodu?: string;
    url?: string;
    icon?: string;
    ustMenuId?: number;
    sira: number;
    aktif: boolean;
    altMenuler?: Menu[];
}

const menuService = {
    // Tüm menüleri getir
    async getAll(): Promise<Menu[]> {
        const response = await api.get('/menu');
        return response.data;
    },

    // Ana menüleri getir (hiyerarşik)
    async getAnaMenuler(): Promise<Menu[]> {
        const response = await api.get('/menu/ana-menuler');
        return response.data;
    },

    // Role göre menüleri getir
    async getMenulerByRol(rolId: number): Promise<Menu[]> {
        const response = await api.get(`/menu/rol/${rolId}`);
        return response.data;
    },

    // Menü detayı
    async getById(id: number): Promise<Menu> {
        const response = await api.get(`/menu/${id}`);
        return response.data;
    },

    // Yeni menü ekle
    async create(data: {
        menuAdi: string;
        menuKodu?: string;
        url?: string;
        icon?: string;
        ustMenuId?: number;
        sira?: number;
        aktif?: boolean;
    }): Promise<Menu> {
        const response = await api.post('/menu', data);
        return response.data;
    },

    // Menü güncelle
    async update(id: number, data: {
        menuAdi: string;
        menuKodu?: string;
        url?: string;
        icon?: string;
        ustMenuId?: number;
        sira?: number;
        aktif: boolean;
    }): Promise<Menu> {
        const response = await api.put(`/menu/${id}`, data);
        return response.data;
    },

    // Menü sil
    async delete(id: number): Promise<void> {
        await api.delete(`/menu/${id}`);
    }
};

export default menuService;
