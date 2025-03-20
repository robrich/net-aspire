import { logs, SeverityNumber } from '@opentelemetry/api-logs';
import { LoggerProvider, SimpleLogRecordProcessor, ConsoleLogRecordExporter } from '@opentelemetry/sdk-logs';
import { Resource } from '@opentelemetry/resources';
import { OTLPLogExporter } from '@opentelemetry/exporter-logs-otlp-http';
import { ATTR_SERVICE_NAME } from '@opentelemetry/semantic-conventions';
import type { InitializeTelemetryData } from './types/opentelemetry';
import parseDelimitedValues from './utils/parse-delimited-values';


export function initializeLogging(args: InitializeTelemetryData) {
  if (!args?.otlpEndpoint) {
    return; // OpenTelemetry is not enabled
  }

  console.log(`Initializing OpenTelemetry Logging: ${JSON.stringify(args, null, 2)}`);

  const otlpOptions = {
    url: `${args.otlpEndpoint}/v1/logs`,
    headers: parseDelimitedValues(args?.headers),
  };

  const attributes = parseDelimitedValues(args.resourceAttributes);
  if (args.serviceName) {
    attributes[ATTR_SERVICE_NAME] = args.serviceName;
  }

  const resource = new Resource(attributes);

  const loggerProvider = new LoggerProvider({resource});
  loggerProvider.addLogRecordProcessor(
    new SimpleLogRecordProcessor(new OTLPLogExporter(otlpOptions))
  );
  loggerProvider.addLogRecordProcessor(
    new SimpleLogRecordProcessor(new ConsoleLogRecordExporter()) // too noisy?
  );

  // Setup global singleton
  logs.setGlobalLoggerProvider(loggerProvider);

  // An example of logging:
  // TODO: move to code that needs to log
  const logger = logs.getLogger('default');
  logger.emit({
    severityNumber: SeverityNumber.INFO,
    severityText: 'INFO',
    body: 'this is a log record body',
    attributes: { 'log.type': 'LogRecord' },
  });
}
