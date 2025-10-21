import React, { createContext, useContext, useState, useEffect, ReactNode } from 'react';
import axios from 'axios';
import { jwtDecode } from 'jwt-decode'; // 'npm install jwt-decode' ile yüklenmeli

// SirketController'daki JWT yapýsýna göre token içindeki kullanýcý bilgileri
interface UserClaims {
    sub: string;        // KullaniciId
    email: string;
    role: string;       // ClaimTypes.Role
    personelId: string;
    sirketId: string;   // Mevcut aktif þirket ID'si
    sirketAdi: string;
    jti: string;
    exp: number;        // Token bitiþ zamaný (Unix timestamp)
}

// Context'in dýþ dünyaya saðladýðý deðerler
interface AuthContextType {
    isLoggedIn: boolean;
    user: UserClaims | null;
    token: string | null;
    login: (token: string) => void;
    logout: () => void;
    currentSirketId: number | null;
    currentRole: string | null;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

// Yardýmcý Fonksiyon: Token'ý çözümle ve geçerliliðini kontrol et
const decodeToken = (token: string): UserClaims | null => {
    try {
        const decoded = jwtDecode<UserClaims>(token);
        // Token'ýn geçerliliðini kontrol et (eðer süre dolduysa null döndür)
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
    const isLoggedIn = !!token && !!user;

    useEffect(() => {
        if (token) {
            const decodedUser = decodeToken(token);
            if (decodedUser) {
                setUser(decodedUser);
                // Tüm API çaðrýlarýna token'ý Authorization baþlýðý olarak ekle
                axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;
            } else {
                // Token geçersiz/süresi dolmuþsa çýkýþ yap (decodeToken zaten localStorage'dan siler)
                setUser(null);
            }
        } else {
            setUser(null);
            delete axios.defaults.headers.common['Authorization'];
        }
    }, [token]);

    const login = (newToken: string) => {
        localStorage.setItem('token', newToken);
        setToken(newToken);
        // App.tsx'teki yönlendirme mantýðýnýn çalýþmasý için yeniden yüklemeye gerek yok
    };

    const logout = () => {
        localStorage.removeItem('token');
        setToken(null);
        setUser(null);
        // Çýkýþ yapýldýktan sonra tüm uygulamayý login'e yönlendirecek þekilde ayarlanmýþtýr.
    };

    // SirketController'daki mantýða uygun olarak JWT'den gelen deðerleri sayýya dönüþtür
    const currentSirketId = user?.sirketId ? parseInt(user.sirketId) : null;
    const currentRole = user?.role || null;

    return (
        <AuthContext.Provider value={{ isLoggedIn, user, token, login, logout, currentSirketId, currentRole }}>
            {children}
        </AuthContext.Provider>
    );
};

// Custom hook
export const useAuth = () => {
    const context = useContext(AuthContext);
    if (context === undefined) {
        throw new Error('useAuth must be used within an AuthProvider');
    }
    return context;
};