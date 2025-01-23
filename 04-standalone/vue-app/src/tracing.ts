import { ConsoleSpanExporter, SimpleSpanProcessor } from '@opentelemetry/sdk-trace-base';
import { SpanProcessor, WebTracerProvider } from '@opentelemetry/sdk-trace-web';
import { Resource } from '@opentelemetry/resources';
import { ATTR_SERVICE_NAME } from '@opentelemetry/semantic-conventions';
import { ZoneContextManager } from '@opentelemetry/context-zone';
import { registerInstrumentations } from '@opentelemetry/instrumentation';
import { getWebAutoInstrumentations } from '@opentelemetry/auto-instrumentations-web';
/*
import { DocumentLoadInstrumentation } from '@opentelemetry/instrumentation-document-load';
import { FetchInstrumentation } from '@opentelemetry/instrumentation-fetch';
import { UserInteractionInstrumentation } from '@opentelemetry/instrumentation-user-interaction';
*/
import { OTLPTraceExporter } from '@opentelemetry/exporter-trace-otlp-proto';


export interface InitializeTelemetryData {
  otlpUrl: string;
  headers: string;
  resourceAttributes: string;
  serviceName: string;
}

let provider: WebTracerProvider;

export function initializeTelemetry(args: InitializeTelemetryData) {
  if (!args?.otlpUrl) {
    return; // OpenTelemetry is not enabled
  }

  console.log(`Initializing OpenTelemetry: ${JSON.stringify(args, null, 2)}`);

  const otlpOptions = {
    url: `${args.otlpUrl}/v1/traces`,
    headers: parseDelimitedValues(args?.headers),
  };

  const attributes: Record<string, string> = parseDelimitedValues(args.resourceAttributes);
  if (args.serviceName) {
    attributes[ATTR_SERVICE_NAME] = args.serviceName;
  }

  provider = new WebTracerProvider({
    resource: new Resource(attributes),
    spanProcessors: [
      new SimpleSpanProcessor(new ConsoleSpanExporter()) as unknown as SpanProcessor, // too noisy?
      new SimpleSpanProcessor(new OTLPTraceExporter(otlpOptions)) as unknown as SpanProcessor
    ]
  });

  provider.register({
    contextManager: new ZoneContextManager()
  });

  registerInstrumentations({
    instrumentations: [
      getWebAutoInstrumentations(), // load documentLoad, fetch, userInteraction, xmlHttpRequest
    ]
  });

  return provider;
}

function parseDelimitedValues(s: string): Record<string, string> {
  const o: Record<string, string> = {};
  if (!s) {
    return o;
  }
  const headers = s.split(','); // Split by comma, ASSUME: commas in keys or values are encoded

  headers.forEach((header) => {
    const [key, value] = header.split('='); // Split by equal sign
    if (key && value) {
      o[key.trim()] = value.trim(); // Add to the object, trimming spaces
    }
  });

  return o;
}
