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
        const response = await api.get('/cihaz');
        return response.data;
    },

    async create(data: Partial<Cihaz>): Promise<number> {
        const response = await api.post('/cihaz', data);
        return response.data;
    },

    async update(id: number, data: Partial<Cihaz>): Promise<void> {
        await api.put(`/cihaz/${id}`, data);
    },

    async delete(id: number): Promise<void> {
        await api.delete(`/cihaz/${id}`);
    },
};

export default cihazService;