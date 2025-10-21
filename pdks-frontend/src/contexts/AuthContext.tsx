import React, { createContext, useContext, useState, useEffect, ReactNode } from 'react';
import axios from 'axios';
import { jwtDecode } from 'jwt-decode';

interface UserClaims {
    sub: string;
    email: string;
    role: string;
    personelId: string;
    sirketId: string;
    sirketAdi: string;
    jti: string;
    exp: number;
}

interface Sirket {
    id: number;
    unvan: string;
    logoUrl?: string;
    varsayilan: boolean;
}

interface AuthContextType {
    isLoggedIn: boolean;
    user: UserClaims | null;
    token: string | null;
    login: (token: string) => void;
    logout: () => void;
    currentSirketId: number | null;
    currentRole: string | null;
    yetkiliSirketler: Sirket[];
    aktifSirket: Sirket | null;
    switchSirket: (sirketId: number) => Promise<void>;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

const decodeToken = (token: string): UserClaims | null => {
    try {
        const decoded = jwtDecode<UserClaims>(token);
        if (decoded.exp * 1000 < Date.now()) {
            localStorage.removeItem('token');
            return null;
        }
        return decoded;
    } catch (error) {
        console.error("Token çözümlenemedi veya geçersiz:", error);
        localStorage.removeItem('token');
        return null;
    }
};

export const AuthProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
    const [token, setToken] = useState<string | null>(localStorage.getItem('token'));
    const [user, setUser] = useState<UserClaims | null>(null);
    const [yetkiliSirketler, setYetkiliSirketler] = useState<Sirket[]>([]);
    const [aktifSirket, setAktifSirket] = useState<Sirket | null>(null);

    const isLoggedIn = !!token && !!user;

    useEffect(() => {
        if (token) {
            const decodedUser = decodeToken(token);
            if (decodedUser) {
                setUser(decodedUser);
                axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;

                // Aktif þirketi token'dan set et
                setAktifSirket({
                    id: parseInt(decodedUser.sirketId),
                    unvan: decodedUser.sirketAdi,
                    varsayilan: true
                });

                // Yetkili þirketleri yükle
                loadYetkiliSirketler();
            } else {
                setUser(null);
                setYetkiliSirketler([]);
                setAktifSirket(null);
            }
        } else {
            setUser(null);
            setYetkiliSirketler([]);
            setAktifSirket(null);
            delete axios.defaults.headers.common['Authorization'];
        }
    }, [token]);

    const loadYetkiliSirketler = async () => {
        try {
            const response = await axios.get('/api/Kullanici/yetkili-sirketler');
            setYetkiliSirketler(response.data);
        } catch (error) {
            console.error('Yetkili þirketler yüklenemedi:', error);
            // Hata durumunda en azýndan aktif þirketi göster
            if (user) {
                setYetkiliSirketler([{
                    id: parseInt(user.sirketId),
                    unvan: user.sirketAdi,
                    varsayilan: true
                }]);
            }
        }
    };

    const switchSirket = async (sirketId: number) => {
        try {
            // Backend endpoint: /api/Sirket/switch/{sirketId}
            const response = await axios.post(`/api/Sirket/switch/${sirketId}`);
            const newToken = response.data.token;

            if (newToken) {
                localStorage.setItem('token', newToken);
                setToken(newToken);
                // Sayfayý yenile ki yeni token'la tüm veriler güncellensin
                window.location.reload();
            }
        } catch (error) {
            console.error('Þirket deðiþtirme hatasý:', error);
            throw error;
        }
    };

    const login = (newToken: string) => {
        localStorage.setItem('token', newToken);
        setToken(newToken);
    };

    const logout = () => {
        localStorage.removeItem('token');
        setToken(null);
        setUser(null);
        setYetkiliSirketler([]);
        setAktifSirket(null);
    };

    const currentSirketId = user?.sirketId ? parseInt(user.sirketId) : null;
    const currentRole = user?.role || null;

    return (
        <AuthContext.Provider value={{
            isLoggedIn,
            user,
            token,
            login,
            logout,
            currentSirketId,
            currentRole,
            yetkiliSirketler,
            aktifSirket,
            switchSirket
        }}>
            {children}
        </AuthContext.Provider>
    );
};

export const useAuth = () => {
    const context = useContext(AuthContext);
    if (context === undefined) {
        throw new Error('useAuth must be used within an AuthProvider');
    }
    return context;
};