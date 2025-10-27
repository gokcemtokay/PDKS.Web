import api from './api';

export interface Gorev {
    id: number;
    baslik: string;
    aciklama: string;
    atananPersonelId: number;
    atananPersonelAdi: string;
    durum: string;
    oncelik: string;
    baslangicTarihi: string;
    bitisTarihi: string;
}

const gorevService = {
    async getAll(): Promise<Gorev[]> {
        const response = await api.get('/gorev');
        return response.data;
    },

    async create(data: Partial<Gorev>): Promise<number> {
        const response = await api.post('/gorev', data);
        return response.data;
    },

    async updateDurum(id: number, durum: string): Promise<void> {
        await api.put(`/gorev/${id}/durum`, { durum });
    },
};

export default gorevService;