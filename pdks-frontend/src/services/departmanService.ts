import api from './api';

export interface Departman {
    id: number;
    sirketId: number;
    sirketAdi?: string;
    departmanAdi: string;
    kod?: string;
    aciklama?: string;
    ustDepartmanId?: number;
    ustDepartmanAdi?: string;
    personelSayisi: number;
    durum: boolean;
}

export interface DepartmanCreateDTO {
    sirketId: number;
    departmanAdi: string;
    kod?: string;
    aciklama?: string;
    ustDepartmanId?: number;
    durum?: boolean;
}

export interface DepartmanUpdateDTO {
    id: number;
    sirketId: number;
    departmanAdi: string;
    kod?: string;
    aciklama?: string;
    ustDepartmanId?: number;
    durum: boolean;
}

const departmanService = {
    async getAll(): Promise<Departman[]> {
        const response = await api.get('/Departman');
        return response.data;
    },

    async getById(id: number): Promise<Departman> {
        const response = await api.get(`/Departman/${id}`);
        return response.data;
    },

    async create(data: DepartmanCreateDTO): Promise<number> {
        const sirketId = parseInt(localStorage.getItem('aktifSirketId') || '0');
        const payload = {
            ...data,
            sirketId: sirketId
        };
        const response = await api.post('/Departman', payload);
        return response.data;
    },

    async update(id: number, data: DepartmanUpdateDTO): Promise<void> {
        const sirketId = parseInt(localStorage.getItem('aktifSirketId') || '0');
        const payload = {
            ...data,
            id: id,
            sirketId: sirketId
        };
        await api.put(`/Departman/${id}`, payload);
    },

    async delete(id: number): Promise<void> {
        await api.delete(`/Departman/${id}`);
    },
};

export default departmanService;
