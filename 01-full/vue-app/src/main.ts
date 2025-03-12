import { createApp } from 'vue';
import './style.css';
import App from './App.vue';
import { initializeTelemetry } from './tracing';
import { initializeLogging } from './logging';


const env = import.meta.env;
const otlpEndpoint: string = env.VITE_OTEL_EXPORTER_OTLP_ENDPOINT;
const headers: string = env.VITE_OTEL_EXPORTER_OTLP_HEADERS;
const resourceAttributes: string = env.VITE_OTEL_RESOURCE_ATTRIBUTES; // NAME_A=VAL_A,NAME_B=VAL_B
const serviceName: string = env.VITE_OTEL_SERVICE_NAME;

initializeTelemetry({otlpEndpoint, headers, resourceAttributes, serviceName});
initializeLogging({otlpEndpoint, headers, resourceAttributes, serviceName});

createApp(App).mount('#app');
