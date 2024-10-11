$ErrorActionPreference = "Stop"

$env:OTEL_BLRP_SCHEDULE_DELAY = "1000"
$env:OTEL_BSP_SCHEDULE_DELAY = "1000"
$env:OTEL_EXPORTER_OTLP_ENDPOINT = "http://localhost:4317"
$env:OTEL_EXPORTER_OTLP_PROTOCOL = "grpc"
$env:OTEL_METRICS_EXEMPLAR_FILTER = "trace_based"
$env:OTEL_METRIC_EXPORT_INTERVAL = "1000"
$env:OTEL_PYTHON_LOGGING_AUTO_INSTRUMENTATION_ENABLED = "true"
$env:OTEL_RESOURCE_ATTRIBUTES = "service.instance.id=python-fastapi"
$env:OTEL_SERVICE_NAME = "python-fastapi"
$env:OTEL_TRACES_SAMPLER = "always_on"

opentelemetry-instrument --traces_exporter otlp --logs_exporter console,otlp --metrics_exporter otlp uvicorn main:app --port 8001
