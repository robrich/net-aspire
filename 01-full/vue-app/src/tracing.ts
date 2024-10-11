import { WebTracerProvider } from '@opentelemetry/sdk-trace-web';
import { SimpleSpanProcessor } from '@opentelemetry/sdk-trace-base';
import { ZoneContextManager } from '@opentelemetry/context-zone';
import { registerInstrumentations } from '@opentelemetry/instrumentation';
import { getWebAutoInstrumentations } from '@opentelemetry/auto-instrumentations-web';
import { Resource } from '@opentelemetry/resources';
import { OTLPTraceExporter } from '@opentelemetry/exporter-trace-otlp-http';


const url = (import.meta.env.VITE_OTEL_EXPORTER_OTLP_ENDPOINT || 'https://localhost:21097')+'/v1/traces';

export const provider = new WebTracerProvider({
  resource: Resource.default().merge(new Resource({
    'service.name': 'vue-app'
  }))
});

const exporterOptions = { url };

const traceExporter = new OTLPTraceExporter(exporterOptions);

provider.addSpanProcessor(new SimpleSpanProcessor(traceExporter));

provider.register({
  contextManager: new ZoneContextManager()
});

// Registering instrumentations
registerInstrumentations({
  instrumentations: [
    getWebAutoInstrumentations()
  ]
});

console.log('OpenTelemetry sending data to', url);
