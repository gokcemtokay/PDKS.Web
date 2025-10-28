import axios, { AxiosError } from 'axios';

const api = axios.create({
    baseURL: '/api',
    headers: {
        'Content-Type': 'application/json',
    },
});

// Request interceptor - Her istekte token ve şirket ID ekler
api.interceptors.request.use(
    (config) => {
        // Token ekle
        const token = localStorage.getItem('token');
        if (token && config.headers) {
            config.headers.Authorization = `Bearer ${token}`;
        }

        // Aktif şirket ID'sini header'a ekle
        const aktifSirketId = localStorage.getItem('aktifSirketId');
        if (aktifSirketId && config.headers) {
            config.headers['X-Sirket-Id'] = aktifSirketId;
        }

        // ✅ FormData için Content-Type'ı kaldır (browser otomatik multipart/form-data ayarlar)
        if (config.data instanceof FormData) {
            delete config.headers['Content-Type'];
            config.transformRequest = (data) => data;
        }

        return config;
    },
    (error) => Promise.reject(error)
);

// Response interceptor - Hata yönetimi
api.interceptors.response.use(
    (response) => response,
    (error: AxiosError) => {
        // 401 Unauthorized - Token geçersiz, login'e yönlendir
        if (error.response?.status === 401) {
            localStorage.removeItem('token');
            localStorage.removeItem('user');
            localStorage.removeItem('aktifSirket');
            localStorage.removeItem('aktifSirketId');
            localStorage.removeItem('yetkiliSirketler');
            window.location.href = '/login';
        }

        // 403 Forbidden - Yetki yok
        if (error.response?.status === 403) {
            console.error('Bu işlem için yetkiniz yok!');
        }

        return Promise.reject(error);
    }
);

export default api;
