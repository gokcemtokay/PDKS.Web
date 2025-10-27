import api from './api';
import { jwtDecode } from 'jwt-decode';

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  kullanici: {
    id: number;
    email: string;
    rol: string;
    personelId?: number;
  };
  aktifSirket: {
    id: number;
    unvan: string;
    logoUrl?: string;
  };
  yetkiliSirketler: Array<{
    id: number;
    unvan: string;
    logoUrl?: string;
    varsayilan?: boolean;
  }>;
}

export interface TokenPayload {
  sub: string;
  email: string;
  role: string;
  personelId?: string;
  sirketId?: string;
  sirketAdi?: string;
  exp: number;
}

const authService = {
  async login(credentials: LoginRequest): Promise<LoginResponse> {
    const response = await api.post('/auth/login', credentials);
    return response.data;
  },

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    localStorage.removeItem('aktifSirketId');
    localStorage.removeItem('aktifSirket');
    localStorage.removeItem('yetkiliSirketler');
  },

  decodeToken(token: string): TokenPayload | null {
    try {
      return jwtDecode<TokenPayload>(token);
    } catch {
      return null;
    }
  },

  isTokenValid(token: string): boolean {
    const decoded = this.decodeToken(token);
    if (!decoded) return false;
    return decoded.exp * 1000 > Date.now();
  },
};

export default authService;
