import { createApp } from 'vue';
import './style.css';
import App from './App.vue';
import { initializeTelemetry } from './tracing';


const env = import.meta.env;
const otlpUrl: string = env.VITE_OTEL_EXPORTER_OTLP_ENDPOINT;
const headers: string = env.VITE_OTEL_EXPORTER_OTLP_HEADERS;
const resourceAttributes: string = env.VITE_OTEL_RESOURCE_ATTRIBUTES;
const serviceName: string = env.VITE_OTEL_SERVICE_NAME;

initializeTelemetry({otlpUrl, headers, resourceAttributes, serviceName});

createApp(App).mount('#app');
