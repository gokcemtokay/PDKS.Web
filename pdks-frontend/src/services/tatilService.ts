import api from './api';

export interface Tatil {
    id: number;
    tatilAdi: string;
    tarih: string;
    tatilTipi: string;
    aciklama?: string;
}

const tatilService = {
    async getAll(): Promise<Tatil[]> {
        const response = await api.get('/tatil');
        return response.data;
    },

    async create(data: Partial<Tatil>): Promise<number> {
        const response = await api.post('/tatil', data);
        return response.data;
    },

    async delete(id: number): Promise<void> {
        await api.delete(`/tatil/${id}`);
    },
};

export default tatilService;