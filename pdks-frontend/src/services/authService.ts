import api from './api';
import { jwtDecode } from 'jwt-decode';

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  user: {
    id: number;
    email: string;
    ad: string;
    soyad: string;
    role: string;
    personelId?: number;
  };
  yetkiliSirketler: Array<{
    sirketId: number;
    sirketAdi: string;
  }>;
}

export interface TokenPayload {
  sub: string;
  email: string;
  role: string;
  personelId?: string;
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
