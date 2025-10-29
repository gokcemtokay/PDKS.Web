import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

export default defineConfig({
  plugins: [react()],
  server: {
    port: 56899,
    hmr: {
      host: 'localhost',
      port: 56899,
      protocol: 'ws',
    },
    proxy: {
      '/api': {
        target: 'http://localhost:5104',
        changeOrigin: true,
      },
      '/uploads': {  // ✅ BU SATIR EKSİK OLABİLİR!
        target: 'http://localhost:5104',
        changeOrigin: true,
      }
    }
  },
});
