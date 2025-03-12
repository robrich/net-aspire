import { fileURLToPath, URL } from 'node:url';
import { defineConfig } from 'vite';
import vue from '@vitejs/plugin-vue';


const serverUrl = process.env.SERVER_URL || 'https://localhost:7364';
console.log('proxying /api/* to server:', serverUrl);

// VITE_OTEL_* env vars set in .env

export default defineConfig({
  plugins: [
    vue(),
  ],
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url))
    }
  },
  server: {
    host: true,
    port: parseInt(process.env.PORT ?? '3000', 10),
    proxy: {
      '/api': {
        // ASSUME: k8s ingress will split traffic and avoid CORS in production
        target: serverUrl,
        secure: false
      }
    }
  }
});
