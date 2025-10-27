import api from './api';

export interface Oneri {
    id: number;
    personelId: number;
    personelAdi: string;
    baslik: string;
    aciklama: string;
    kategori: string;
    durum: string;
    anonim: boolean;
    oneriTarihi: string;
}

const oneriService = {
    async getAll(): Promise<Oneri[]> {
        const response = await api.get('/oneri');
        return response.data;
    },

    async create(data: Partial<Oneri>): Promise<number> {
        const response = await api.post('/oneri', data);
        return response.data;
    },
};

export default oneriService;