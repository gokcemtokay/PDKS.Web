import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-react';

// https://vitejs.dev/config/
export default defineConfig({
    plugins: [plugin()],
    server: {
        port: 56899,
        // ⭐ KRİTİK DÜZELTME: WebSocket bağlantı hatalarını gidermek için HMR ayarları
        hmr: {
            // HMR istemcisinin bağlanacağı sunucuyu açıkça belirtir.
            // Bu, proxy veya farklı ağ ortamlarında WebSocket bağlantısının stabil olmasını sağlar.
            host: 'localhost',
            port: 56899,
            protocol: 'ws', // HTTP kullanıldığı için ws (WebSocket) protokolü
        },
        proxy: {
            '/api': {
                // Backend'in çalıştığı tam adresi
                target: 'http://localhost:5104',
                changeOrigin: true,
                secure: false,
            },
        },
    }
});