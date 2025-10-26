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
        const response = await api.get('/Tatil');
        return response.data;
    },

    async create(data: Partial<Tatil>): Promise<number> {
        const response = await api.post('/Tatil', data);
        return response.data;
    },

    async delete(id: number): Promise<void> {
        await api.delete(`/Tatil/${id}`);
    },
};

export default tatilService;