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
        const response = await api.get('/vardiya');
        return response.data;
    },

    async create(data: Partial<Vardiya>): Promise<number> {
        const response = await api.post('/vardiya', data);
        return response.data;
    },

    async update(id: number, data: Partial<Vardiya>): Promise<void> {
        await api.put(`/vardiya/${id}`, data);
    },
};

export default vardiyaService;