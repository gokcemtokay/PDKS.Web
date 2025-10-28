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

    async create(data: any): Promise<void> {
        await api.post('/cihaz', {
            cihazAdi: data.cihazAdi,
            cihazTipi: data.cihazTipi,
            ipAdres: data.ipAdresi, // ✅ ipAdres
            port: data.port,
            lokasyon: data.lokasyon,
            durum: data.durum
        });
    },

    async update(id: number, data: any): Promise<void> {
        await api.put(`/cihaz/${id}`, {
            cihazAdi: data.cihazAdi,
            cihazTipi: data.cihazTipi,
            ipAdres: data.ipAdresi, // ✅ ipAdres (backend'in beklediği)
            port: data.port,
            lokasyon: data.lokasyon,
            durum: data.durum
        });
    },

    async delete(id: number): Promise<void> {
        await api.delete(`/cihaz/${id}`);
    },
};

export default cihazService;