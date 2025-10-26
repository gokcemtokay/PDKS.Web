import api from './api';

export interface Bildirim {
    id: number;
    baslik: string;
    mesaj: string;
    tip: string;
    okundu: boolean;
    olusturmaTarihi: string;
    link?: string;
}

const bildirimService = {
    async getAll(): Promise<Bildirim[]> {
        const response = await api.get('/Bildirim');
        return response.data;
    },

    async markAsRead(id: number): Promise<void> {
        await api.post(`/Bildirim/${id}/okundu`);
    },

    async getUnreadCount(): Promise<number> {
        const response = await api.get('/Bildirim/okunmamis-sayisi');
        return response.data;
    },
};

export default bildirimService;