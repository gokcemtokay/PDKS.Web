// src/services/dashboardService.ts - DÜZELTİLMİŞ

import api from './api';
import type {
    BugunkunDurum,    // ⬅️ Sadece bunları kullan
    BekleyenOnay,
    SonAktivite,
    DogumGunu,
    YilDonumu
} from '../types/dashboard';
// AnaDashboard import'unu SİL

class DashboardService {
    // Ana Dashboard
    async getAnaDashboard() {  // ⬅️ Return type belirtme
        const response = await api.get('/Dashboard/ana');
        return response.data;
    }

    // Widgets
    async getBugunkunDurum(): Promise<BugunkunDurum> {
        const response = await api.get('/Dashboard/widgets/bugunku-durum');
        return response.data;
    }

    async getBekleyenOnaylar(): Promise<BekleyenOnay[]> {
        const response = await api.get('/Dashboard/widgets/bekleyen-onaylar');
        return response.data;
    }

    async getSonAktiviteler(limit: number = 10): Promise<SonAktivite[]> {
        const response = await api.get('/Dashboard/widgets/son-aktiviteler', {
            params: { limit }
        });
        return response.data;
    }

    async getDogumGunleri(): Promise<DogumGunu[]> {
        const response = await api.get('/Dashboard/widgets/dogum-gunleri');
        return response.data;
    }

    async getYilDonumleri(): Promise<YilDonumu[]> {
        const response = await api.get('/Dashboard/widgets/yildonumleri');
        return response.data;
    }

    async getManagerDashboard() {
        const response = await api.get('/Dashboard/manager');
        return response.data;
    }

    async getIKDashboard() {
        const response = await api.get('/Dashboard/ik');
        return response.data;
    }

    async getExecutiveDashboard() {
        const response = await api.get('/Dashboard/executive');
        return response.data;
    }
}

export default new DashboardService();