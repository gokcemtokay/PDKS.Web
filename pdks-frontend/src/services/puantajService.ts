import api from './api';
import {
    PuantajList,
    PuantajDetail,
    PuantajDetay,
    PuantajHesaplaRequest,
    TopluPuantajHesaplaRequest,
    PuantajOnayRequest,
    PuantajRaporParametre,
    PuantajOzetRapor,
    DepartmanPuantajOzet,
    PuantajKontrol,
    GecKalanRapor,
    ErkenCikanRapor
} from '../types/puantaj.types';

const puantajService = {
    // Puantaj Sorgulama
    async getById(id: number): Promise<PuantajDetail> {
        const response = await api.get(`/puantaj/${id}`);
        return response.data;
    },

    async getByPersonel(personelId: number): Promise<PuantajList[]> {
        const response = await api.get(`/puantaj/personel/${personelId}`);
        return response.data;
    },

    async getByDonem(yil: number, ay: number, departmanId?: number): Promise<PuantajList[]> {
        const params: any = { yil, ay };
        if (departmanId) params.departmanId = departmanId;

        const response = await api.get('/puantaj', { params });  // '/puantaj/donem' değil, '/puantaj'
        return response.data;
    },

    async getByPersonelVeDonem(personelId: number, yil: number, ay: number): Promise<PuantajDetail> {
        const response = await api.get(`/puantaj/personel/${personelId}/donem`, {
            params: { yil, ay }
        });
        return response.data;
    },

    // Puantaj Hesaplama
    async hesaplaPuantaj(data: PuantajHesaplaRequest): Promise<PuantajDetail> {
        const response = await api.post('/puantaj/hesapla', data);
        return response.data;
    },

    async topluPuantajHesapla(data: TopluPuantajHesaplaRequest): Promise<{ message: string; puantajIdler: number[] }> {
        const response = await api.post('/puantaj/toplu-hesapla', data);
        return response.data;
    },

    async yenidenHesapla(id: number): Promise<PuantajDetail> {
        const response = await api.put(`/puantaj/${id}/yeniden-hesapla`);
        return response.data;
    },

    // Onaylama
    async onayla(data: PuantajOnayRequest): Promise<{ message: string }> {
        const response = await api.put('/puantaj/onayla', data);
        return response.data;
    },

    async onayIptal(id: number): Promise<{ message: string }> {
        const response = await api.put(`/puantaj/${id}/onay-iptal`);
        return response.data;
    },

    // Detaylar
    async getGunlukDetaylar(puantajId: number): Promise<PuantajDetay[]> {
        const response = await api.get(`/puantaj/${puantajId}/detaylar`);
        return response.data;
    },

    async getDetayByTarih(personelId: number, tarih: string): Promise<PuantajDetay> {
        const response = await api.get(`/puantaj/personel/${personelId}/detay`, {
            params: { tarih }
        });
        return response.data;
    },

    // Raporlar
    async getOzetRapor(parametre: PuantajRaporParametre): Promise<PuantajOzetRapor> {
        const response = await api.post('/puantaj/rapor/ozet', parametre);
        return response.data;
    },

    async getDepartmanOzet(yil: number, ay: number): Promise<DepartmanPuantajOzet[]> {
        const response = await api.get('/puantaj/rapor/departman', {
            params: { yil, ay }
        });
        return response.data;
    },

    async getGecKalanlar(baslangic: string, bitis: string): Promise<GecKalanRapor[]> {
        const response = await api.get('/puantaj/rapor/gec-kalanlar', {
            params: { baslangic, bitis }
        });
        return response.data;
    },

    async getErkenCikanlar(baslangic: string, bitis: string): Promise<ErkenCikanRapor[]> {
        const response = await api.get('/puantaj/rapor/erken-cikanlar', {
            params: { baslangic, bitis }
        });
        return response.data;
    },

    // Yardımcı
    async kontrolEt(personelId: number, yil: number, ay: number): Promise<PuantajKontrol> {
        const response = await api.get('/puantaj/kontrol', {
            params: { personelId, yil, ay }
        });
        return response.data;
    },

    async delete(id: number): Promise<void> {
        await api.delete(`/puantaj/${id}`);
    },
};

export default puantajService;