import axios from 'axios';

// API_BASE_URL art�k sadece g�receli yol '/api' olmal�d�r.
const API_BASE_URL = '/api';

const api = axios.create({
    baseURL: API_BASE_URL,
    headers: {
        'Content-Type': 'application/json; charset=utf-8',
        'Accept': 'application/json; charset=utf-8',
    },
});

// Request interceptor
api.interceptors.request.use(
    (config) => {
        const token = localStorage.getItem('token');
        // AuthContext'e g�venmek yerine burada token kontrol�n� yapabiliriz
        if (token && !config.headers.Authorization) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);

// Response interceptor  
api.interceptors.response.use(
    (response) => {
        return response;
    },
    (error) => {
        if (error.response?.status === 401) {
            // Token silme ve login sayfas�na y�nlendirme
            localStorage.removeItem('token');
            // Sayfa yenileme yerine navigate kullanmak daha iyidir ancak
            // hatay� gidermek i�in window.location.href kullan�labilir.
            window.location.href = '/login';
        }
        return Promise.reject(error);
    }
);

export default api;