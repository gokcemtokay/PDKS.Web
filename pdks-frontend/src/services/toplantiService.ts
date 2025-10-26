import api from './api';

export interface ToplantiOdasi {
    id: number;
    odaAdi: string;
    kapasite: number;
    lokasyon: string;
    aktif: boolean;
}

export interface ToplantiRezervasyonu {
    id: number;
    odaId: number;
    odaAdi: string;
    personelId: number;
    personelAdi: string;
    baslangic: string;
    bitis: string;
    konu: string;
    durum: string;
}

const toplantiService = {
    async getAllOdalar(): Promise<ToplantiOdasi[]> {
        const response = await api.get('/ToplantiOdasi');
        return response.data;
    },

    async getAllRezervasyonlar(): Promise<ToplantiRezervasyonu[]> {
        const response = await api.get('/ToplantiRezervasyonu');
        return response.data;
    },

    async createRezervasyon(data: Partial<ToplantiRezervasyonu>): Promise<number> {
        const response = await api.post('/ToplantiRezervasyonu', data);
        return response.data;
    },
};

export default toplantiService;