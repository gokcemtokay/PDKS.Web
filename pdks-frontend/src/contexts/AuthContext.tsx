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
    menuler: any[]; // ✅ EKLE
    islemler: string[]; // ✅ EKLE
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
      const decoded = authService.decodeToken(token);
      const storedUser = localStorage.getItem('user');
      const storedSirketler = localStorage.getItem('yetkiliSirketler');
      const storedAktifSirket = localStorage.getItem('aktifSirket');
      
      if (decoded && storedUser) {
        setUser(JSON.parse(storedUser));
      }
      
      if (storedSirketler) {
        const sirketler = JSON.parse(storedSirketler);
        setYetkiliSirketler(sirketler);
      }
      
      if (storedAktifSirket) {
        setAktifSirket(JSON.parse(storedAktifSirket));
      }
    }
  }, []);

  const login = (response: LoginResponse) => {
    // Token'ı kaydet
    localStorage.setItem('token', response.token);
    
    // Kullanıcı bilgilerini kaydet
    const userData: User = {
      id: response.kullanici.id,
      email: response.kullanici.email,
      ad: response.kullanici.email.split('@')[0], // Backend'den ad/soyad gelmezse email'den al
      soyad: '',
      role: response.kullanici.rol,
      personelId: response.kullanici.personelId,
    };
    setUser(userData);
    localStorage.setItem('user', JSON.stringify(userData));
    
    // Yetkili şirketleri kaydet
    const sirketler: Sirket[] = response.yetkiliSirketler.map(s => ({
      sirketId: s.id,
      sirketAdi: s.unvan,
      logoUrl: s.logoUrl,
    }));
    setYetkiliSirketler(sirketler);
    localStorage.setItem('yetkiliSirketler', JSON.stringify(sirketler));
    
    // Aktif şirketi kaydet
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
        const sirket = yetkiliSirketler.find((s) => s.sirketId === sirketId);
        if (!sirket) return;

        try {
            // Backend'den yeni token al
            const response = await api.post('/auth/switch-sirket', { sirketId });

            // Yeni token'ı kaydet
            localStorage.setItem('token', response.data.token);

            // Aktif şirketi güncelle
            const yeniAktifSirket: Sirket = {
                sirketId: response.data.aktifSirket.id,
                sirketAdi: response.data.aktifSirket.unvan,
                logoUrl: response.data.aktifSirket.logoUrl,
            };

            setAktifSirket(yeniAktifSirket);
            localStorage.setItem('aktifSirket', JSON.stringify(yeniAktifSirket));
            localStorage.setItem('aktifSirketId', sirketId.toString());

            // ✅ ÖNEMLİ: Mevcut URL'de kal ve sayfayı yenile
            window.location.href = window.location.pathname + window.location.search;

        } catch (error) {
            console.error('Şirket değiştirme hatası:', error);
            setAktifSirket(sirket);
            localStorage.setItem('aktifSirket', JSON.stringify(sirket));
            localStorage.setItem('aktifSirketId', sirketId.toString());
            window.location.href = window.location.pathname + window.location.search;
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
          menuler: [], // ✅ EKLE
          islemler: [], // ✅ EKLE
          hasPermission: () => false // ✅ EKLE
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
