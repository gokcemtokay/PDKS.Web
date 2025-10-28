import api from './api';

export interface Personel {
    id: number;
    sirketId: number;
    sirketAdi?: string;
    adSoyad: string;
    sicilNo: string;
    tcKimlikNo: string;
    profilResmi?: string;
    email: string;
    telefon?: string;
    adres?: string;
    dogumTarihi: string;
    cinsiyet?: string;
    kanGrubu?: string;
    girisTarihi: string;
    cikisTarihi?: string;
    maas?: number;
    unvan?: string;
    gorev?: string;
    avansLimiti?: number;
    durum: boolean;
    departmanId?: number;
    departmanAdi?: string;
    vardiyaId?: number;
    vardiyaAdi?: string;
    notlar?: string;
    kayitTarihi: string;
}

const personelService = {
    async getAll(): Promise<Personel[]> {
        const response = await api.get('/Personel');
        return response.data;
    },

    async getById(id: number): Promise<Personel> {
        const response = await api.get(`/Personel/${id}`);
        return response.data;
    },

    async create(data: any): Promise<number> {
        const sirketId = parseInt(localStorage.getItem('aktifSirketId') || '0');

        const dto = {
            sirketId: sirketId,
            adSoyad: data.adSoyad,
            sicilNo: data.sicilNo,
            tcKimlikNo: data.tcKimlik || data.tcKimlikNo,
            email: data.email,
            telefon: data.telefon,
            adres: data.adres,
            dogumTarihi: data.dogumTarihi,
            cinsiyet: data.cinsiyet,
            kanGrubu: data.kanGrubu,
            girisTarihi: data.girisTarihi,
            cikisTarihi: data.cikisTarihi || null,
            maas: data.maas || null,
            unvan: data.gorev || data.unvan || '',
            gorev: data.gorev || '',
            departmanId: data.departmanId ? parseInt(data.departmanId) : null,
            vardiyaId: data.vardiyaId ? parseInt(data.vardiyaId) : null,
            avansLimiti: data.avansLimiti || null,
            durum: data.durum ?? true,
            notlar: data.notlar || null
        };

        const response = await api.post('/Personel', dto);
        return response.data;
    },

    async update(id: number, data: any): Promise<void> {
        const sirketId = parseInt(localStorage.getItem('aktifSirketId') || '0');

        const dto = {
            id: id,
            sirketId: sirketId,
            adSoyad: data.adSoyad,
            sicilNo: data.sicilNo,
            tcKimlikNo: data.tcKimlik || data.tcKimlikNo,
            profilResmi: data.profilResmi || data.profilFoto || '',
            email: data.email,
            telefon: data.telefon || '',
            adres: data.adres || '',
            dogumTarihi: data.dogumTarihi,
            cinsiyet: data.cinsiyet || '',
            kanGrubu: data.kanGrubu || '',
            girisTarihi: data.girisTarihi,
            cikisTarihi: data.cikisTarihi || null,
            maas: data.maas || null,
            unvan: data.gorev || data.unvan || '',
            gorev: data.gorev || '',
            departmanId: data.departmanId ? parseInt(data.departmanId) : null,
            vardiyaId: data.vardiyaId ? parseInt(data.vardiyaId) : null,
            avansLimiti: data.avansLimiti || null,
            durum: data.durum ?? true,
            notlar: data.notlar || null
        };

        await api.put(`/Personel/${id}`, dto);

        if (data.medeniDurum) {
            localStorage.setItem(`personel_${id}_medeniDurum`, data.medeniDurum);
        }
    },

    async delete(id: number): Promise<void> {
        await api.delete(`/Personel/${id}`);
    },

    // ✅ FOTOĞRAF YÜKLEME - Native fetch API
    async uploadPhoto(id: number, file: File): Promise<string> {
        console.log('uploadPhoto called:', id, file.name);

        const formData = new FormData();
        formData.append('file', file);

        const token = localStorage.getItem('token');
        const sirketId = localStorage.getItem('aktifSirketId');

        console.log('Uploading to:', `/api/Personel/${id}/foto`);

        const response = await fetch(`/api/Personel/${id}/foto`, {
            method: 'POST',
            headers: {
                'Authorization': token ? `Bearer ${token}` : '',
                'X-Sirket-Id': sirketId || '',
            },
            body: formData,
        });

        console.log('Upload response:', response.status, response.statusText);

        if (!response.ok) {
            const errorText = await response.text();
            console.error('Upload error:', errorText);
            throw new Error(`Fotoğraf yüklenemedi: ${response.status} ${response.statusText}`);
        }

        const data = await response.json();
        console.log('Upload success:', data);
        return data.foto || data.profilResmi || data.ProfilResmi || '';
    },

    // Fotoğraf silme
    async deletePhoto(id: number): Promise<void> {
        await api.delete(`/Personel/${id}/foto`);
    },
};

export default personelService;
