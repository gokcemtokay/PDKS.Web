import axios from 'axios';

// API_BASE_URL artýk sadece göreceli yol '/api' olmalýdýr.
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
        // AuthContext'e güvenmek yerine burada token kontrolünü yapabiliriz
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
            // Token silme ve login sayfasýna yönlendirme
            localStorage.removeItem('token');
            // Sayfa yenileme yerine navigate kullanmak daha iyidir ancak
            // hatayý gidermek için window.location.href kullanýlabilir.
            window.location.href = '/login';
        }
        return Promise.reject(error);
    }
);

export default api;