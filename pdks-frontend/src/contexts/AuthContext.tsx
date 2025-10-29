import { createContext, useContext, useState, useEffect, ReactNode } from 'react';
import authService, { LoginResponse } from '../services/authService';
import api from '../services/api';

interface User {
    id: number;
    email: string;
    ad: string;
    soyad: string;
    role: string;
    personelId?: number;
}

interface Sirket {
    sirketId: number;
    sirketAdi: string;
    logoUrl?: string;
}

interface AuthContextType {
    user: User | null;
    isLoggedIn: boolean;
    login: (response: LoginResponse) => void;
    logout: () => void;
    yetkiliSirketler: Sirket[];
    aktifSirket: Sirket | null;
    switchSirket: (sirketId: number) => Promise<void>;
    menuler: any[];
    islemler: string[];
    hasPermission: (islemKodu: string) => boolean;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export function AuthProvider({ children }: { children: ReactNode }) {
    const [user, setUser] = useState<User | null>(null);
    const [yetkiliSirketler, setYetkiliSirketler] = useState<Sirket[]>([]);
    const [aktifSirket, setAktifSirket] = useState<Sirket | null>(null);

    useEffect(() => {
        const token = localStorage.getItem('token');
        if (token && authService.isTokenValid(token)) {
            const storedUser = localStorage.getItem('user');
            const storedSirketler = localStorage.getItem('yetkiliSirketler');
            const storedAktifSirket = localStorage.getItem('aktifSirket');
            const storedAktifSirketId = localStorage.getItem('aktifSirketId');

            console.log('🔍 useEffect - localStorage aktifSirketId:', storedAktifSirketId);
            console.log('🔍 useEffect - localStorage aktifSirket:', storedAktifSirket);

            if (storedUser) {
                setUser(JSON.parse(storedUser));
            }

            if (storedSirketler) {
                setYetkiliSirketler(JSON.parse(storedSirketler));
            }

            // KRİTİK: localStorage'dan oku, token'dan değil!
            if (storedAktifSirket) {
                const aktifSirketObj = JSON.parse(storedAktifSirket);
                console.log('✅ useEffect - aktifSirket set ediliyor:', aktifSirketObj);
                setAktifSirket(aktifSirketObj);
            }
        }
    }, []);

    const login = (response: LoginResponse) => {
        localStorage.setItem('token', response.token);

        const userData: User = {
            id: response.kullanici.id,
            email: response.kullanici.email,
            ad: response.kullanici.email.split('@')[0],
            soyad: '',
            role: response.kullanici.rol,
            personelId: response.kullanici.personelId,
        };
        setUser(userData);
        localStorage.setItem('user', JSON.stringify(userData));

        const sirketler: Sirket[] = response.yetkiliSirketler.map(s => ({
            sirketId: s.id,
            sirketAdi: s.unvan,
            logoUrl: s.logoUrl,
        }));
        setYetkiliSirketler(sirketler);
        localStorage.setItem('yetkiliSirketler', JSON.stringify(sirketler));

        const aktif: Sirket = {
            sirketId: response.aktifSirket.id,
            sirketAdi: response.aktifSirket.unvan,
            logoUrl: response.aktifSirket.logoUrl,
        };
        setAktifSirket(aktif);
        localStorage.setItem('aktifSirket', JSON.stringify(aktif));
        localStorage.setItem('aktifSirketId', aktif.sirketId.toString());
    };

    const logout = () => {
        authService.logout();
        setUser(null);
        setYetkiliSirketler([]);
        setAktifSirket(null);
        localStorage.removeItem('aktifSirket');
    };

    const switchSirket = async (sirketId: number) => {
        alert('🔄 BAŞLADI! Seçilen ID: ' + sirketId);
        console.log('🔄 SWITCH SIRKET BAŞLADI - ID:', sirketId);
        console.log('📍 Mevcut URL:', window.location.href);

        const sirket = yetkiliSirketler.find((s) => s.sirketId === sirketId);
        if (!sirket) {
            console.error('❌ Şirket bulunamadı');
            return;
        }

        const currentPath = window.location.pathname;
        const currentSearch = window.location.search;
        const currentUrl = currentPath + currentSearch;
        console.log('💾 Kaydedilen URL:', currentUrl);

        try {
            console.log('🌐 Backend çağrısı yapılıyor...');
            const response = await api.post('/auth/switch-sirket', { sirketId });
            console.log('✅ Backend yanıtı alındı');

            if (!response.data.token) {
                console.error('❌ Token bulunamadı!');
                return;
            }

            console.log('💾 Token kaydediliyor...');
            localStorage.setItem('token', response.data.token);

            const yeniAktifSirket: Sirket = {
                sirketId: response.data.aktifSirket.id,
                sirketAdi: response.data.aktifSirket.unvan,
                logoUrl: response.data.aktifSirket.logoUrl,
            };

            console.log('💾 Aktif şirket kaydediliyor:', yeniAktifSirket);
            localStorage.setItem('aktifSirket', JSON.stringify(yeniAktifSirket));
            localStorage.setItem('aktifSirketId', sirketId.toString());
            setAktifSirket(yeniAktifSirket);

            console.log('🔄 Sayfa yenileniyor, hedef URL:', currentUrl);
            window.location.href = currentUrl;

        } catch (error: any) {
            console.error('❌ HATA:', error);
            localStorage.setItem('aktifSirket', JSON.stringify(sirket));
            localStorage.setItem('aktifSirketId', sirketId.toString());
            window.location.href = currentUrl;
        }
    };

    const isLoggedIn = !!user;

    return (
        <AuthContext.Provider value={{
            user,
            isLoggedIn,
            login,
            logout,
            yetkiliSirketler,
            aktifSirket,
            switchSirket,
            menuler: [],
            islemler: [],
            hasPermission: () => false
        }}>
            {children}
        </AuthContext.Provider>
    );
}

export function useAuth() {
    const context = useContext(AuthContext);
    if (!context) {
        throw new Error('useAuth must be used within AuthProvider');
    }
    return context;
}
