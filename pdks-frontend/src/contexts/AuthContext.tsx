import { createContext, useContext, useState, useEffect, ReactNode } from 'react';
import authService from '../services/authService';

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
}

interface AuthContextType {
  user: User | null;
  isLoggedIn: boolean;
  login: (token: string) => void;
  logout: () => void;
  yetkiliSirketler: Sirket[];
  aktifSirket: Sirket | null;
  switchSirket: (sirketId: number) => Promise<void>;
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
      const storedAktifSirket = localStorage.getItem('aktifSirketId');
      
      if (decoded && storedUser) {
        setUser(JSON.parse(storedUser));
      }
      if (storedSirketler) {
        const sirketler = JSON.parse(storedSirketler);
        setYetkiliSirketler(sirketler);
        if (storedAktifSirket) {
          const aktif = sirketler.find((s: Sirket) => s.sirketId === parseInt(storedAktifSirket));
          setAktifSirket(aktif || sirketler[0]);
        } else if (sirketler.length > 0) {
          setAktifSirket(sirketler[0]);
          localStorage.setItem('aktifSirketId', sirketler[0].sirketId.toString());
        }
      }
    }
  }, []);

  const login = (token: string) => {
    localStorage.setItem('token', token);
    const decoded = authService.decodeToken(token);
    if (decoded) {
      const userData: User = {
        id: parseInt(decoded.sub),
        email: decoded.email,
        ad: decoded.email.split('@')[0],
        soyad: '',
        role: decoded.role,
        personelId: decoded.personelId ? parseInt(decoded.personelId) : undefined,
      };
      setUser(userData);
      localStorage.setItem('user', JSON.stringify(userData));
    }
  };

  const logout = () => {
    authService.logout();
    setUser(null);
    setYetkiliSirketler([]);
    setAktifSirket(null);
  };

  const switchSirket = async (sirketId: number) => {
    const sirket = yetkiliSirketler.find((s) => s.sirketId === sirketId);
    if (sirket) {
      setAktifSirket(sirket);
      localStorage.setItem('aktifSirketId', sirketId.toString());
    }
  };

  return (
    <AuthContext.Provider value={{
      user,
      isLoggedIn: !!user,
      login,
      logout,
      yetkiliSirketler,
      aktifSirket,
      switchSirket,
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
