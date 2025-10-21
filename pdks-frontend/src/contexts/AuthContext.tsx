import React, { createContext, useContext, useState, useEffect, ReactNode } from 'react';
import axios from 'axios';
import { jwtDecode } from 'jwt-decode'; // 'npm install jwt-decode' ile y�klenmeli

// SirketController'daki JWT yap�s�na g�re token i�indeki kullan�c� bilgileri
interface UserClaims {
    sub: string;        // KullaniciId
    email: string;
    role: string;       // ClaimTypes.Role
    personelId: string;
    sirketId: string;   // Mevcut aktif �irket ID'si
    sirketAdi: string;
    jti: string;
    exp: number;        // Token biti� zaman� (Unix timestamp)
}

// Context'in d�� d�nyaya sa�lad��� de�erler
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

// Yard�mc� Fonksiyon: Token'� ��z�mle ve ge�erlili�ini kontrol et
const decodeToken = (token: string): UserClaims | null => {
    try {
        const decoded = jwtDecode<UserClaims>(token);
        // Token'�n ge�erlili�ini kontrol et (e�er s�re dolduysa null d�nd�r)
        if (decoded.exp * 1000 < Date.now()) {
            localStorage.removeItem('token');
            return null;
        }
        return decoded;
    } catch (error) {
        console.error("Token ��z�mlenemedi veya ge�ersiz:", error);
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
                // T�m API �a�r�lar�na token'� Authorization ba�l��� olarak ekle
                axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;
            } else {
                // Token ge�ersiz/s�resi dolmu�sa ��k�� yap (decodeToken zaten localStorage'dan siler)
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
        // App.tsx'teki y�nlendirme mant���n�n �al��mas� i�in yeniden y�klemeye gerek yok
    };

    const logout = () => {
        localStorage.removeItem('token');
        setToken(null);
        setUser(null);
        // ��k�� yap�ld�ktan sonra t�m uygulamay� login'e y�nlendirecek �ekilde ayarlanm��t�r.
    };

    // SirketController'daki mant��a uygun olarak JWT'den gelen de�erleri say�ya d�n��t�r
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