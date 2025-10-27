import api from './api';

export interface Rol {
    id: number;
    rolAdi: string;
    aciklama?: string;
    yetkiler: string[];
}

const rolService = {
    async getAll(): Promise<Rol[]> {
        const response = await api.get('/rol');
        return response.data;
    },

    async create(data: Partial<Rol>): Promise<number> {
        const response = await api.post('/rol', data);
        return response.data;
    },

    async update(id: number, data: Partial<Rol>): Promise<void> {
        await api.put(`/Rol/${id}`, data);
    },
};

export default rolService;