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
import type { InitializeTelemetryData } from './types/opentelemetry';
import parseDelimitedValues from './utils/parse-delimited-values';


let provider: WebTracerProvider;

export function initializeTelemetry(args: InitializeTelemetryData) {
  if (!args?.otlpEndpoint) {
    return; // OpenTelemetry is not enabled
  }

  console.log(`Initializing OpenTelemetry Tracing: ${JSON.stringify(args, null, 2)}`);

  const otlpOptions = {
    url: `${args.otlpEndpoint}/v1/traces`,
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
