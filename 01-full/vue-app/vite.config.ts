import { fileURLToPath, URL } from 'node:url';
import { defineConfig } from 'vite';
import vue from '@vitejs/plugin-vue';


const serverUrl = process.env.services__apiservice__https__0 || process.env.services__apiservice__http__0;


process.env.VITE_OTEL_EXPORTER_OTLP_ENDPOINT = process.env.OTEL_EXPORTER_OTLP_ENDPOINT;
process.env.VITE_OTEL_EXPORTER_OTLP_HEADERS = process.env.OTEL_EXPORTER_OTLP_HEADERS;
process.env.VITE_OTEL_RESOURCE_ATTRIBUTES = process.env.OTEL_RESOURCE_ATTRIBUTES;
process.env.VITE_OTEL_SERVICE_NAME = process.env.OTEL_SERVICE_NAME;

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
