import api from './api';

export interface Cihaz {
    id: number;
    cihazAdi: string;
    cihazTipi: string;
    ipAdresi: string;
    port: number;
    durum: boolean;
    lokasyon?: string;
}

const cihazService = {
    async getAll(): Promise<Cihaz[]> {
        const response = await api.get('/Cihaz');
        return response.data;
    },

    async create(data: Partial<Cihaz>): Promise<number> {
        const response = await api.post('/Cihaz', data);
        return response.data;
    },

    async update(id: number, data: Partial<Cihaz>): Promise<void> {
        await api.put(`/Cihaz/${id}`, data);
    },

    async delete(id: number): Promise<void> {
        await api.delete(`/Cihaz/${id}`);
    },
};

export default cihazService;