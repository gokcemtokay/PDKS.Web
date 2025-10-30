import api from './api';

export interface Cihaz {
    id: number;
    sirketId?: number;
    cihazAdi: string;
    cihazTipi?: string;
    ipAdres?: string;
    port?: number;
    durum: boolean;
    durumText?: string;
    lokasyon?: string;
    sonBaglantiZamani?: string;
    bugunkuOkumaSayisi?: number;
}

const cihazService = {
    async getAll(): Promise<Cihaz[]> {
        const response = await api.get('/cihaz');
        return response.data;
    },

    async create(data: any): Promise<void> {
        await api.post('/cihaz', {
            cihazAdi: data.cihazAdi,
            cihazTipi: data.cihazTipi || null,
            ipAdres: data.ipAdresi,
            port: data.port ? parseInt(data.port) : null,
            lokasyon: data.lokasyon,
            durum: data.durum
        });
    },

    async update(id: number, data: any): Promise<void> {
        await api.put(`/cihaz/${id}`, {
            id: id,
            cihazAdi: data.cihazAdi,
            cihazTipi: data.cihazTipi || null,
            ipAdres: data.ipAdresi,
            port: data.port ? parseInt(data.port) : null,
            lokasyon: data.lokasyon,
            durum: data.durum
        });
    },

    async delete(id: number): Promise<void> {
        await api.delete(`/cihaz/${id}`);
    },
};

export default cihazService;
