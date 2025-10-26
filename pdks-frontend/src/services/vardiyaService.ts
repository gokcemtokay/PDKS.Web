import api from './api';

export interface Vardiya {
    id: number;
    vardiyaAdi: string;
    baslangicSaati: string;
    bitisSaati: string;
    aciklama?: string;
}

const vardiyaService = {
    async getAll(): Promise<Vardiya[]> {
        const response = await api.get('/Vardiya');
        return response.data;
    },

    async create(data: Partial<Vardiya>): Promise<number> {
        const response = await api.post('/Vardiya', data);
        return response.data;
    },

    async update(id: number, data: Partial<Vardiya>): Promise<void> {
        await api.put(`/Vardiya/${id}`, data);
    },
};

export default vardiyaService;