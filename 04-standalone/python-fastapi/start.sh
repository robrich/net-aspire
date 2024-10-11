#!/bin/sh

export OTEL_BLRP_SCHEDULE_DELAY="1000"
export OTEL_BSP_SCHEDULE_DELAY="1000"
export OTEL_EXPORTER_OTLP_ENDPOINT="http://localhost:4317"
export OTEL_EXPORTER_OTLP_PROTOCOL="grpc"
export OTEL_METRICS_EXEMPLAR_FILTER="trace_based"
export OTEL_METRIC_EXPORT_INTERVAL="1000"
export OTEL_PYTHON_LOGGING_AUTO_INSTRUMENTATION_ENABLED="true"
export OTEL_RESOURCE_ATTRIBUTES="service.instance.id=python-fastapi"
export OTEL_SERVICE_NAME="python-fastapi"
export OTEL_TRACES_SAMPLER="always_on"

opentelemetry-instrument --traces_exporter otlp --logs_exporter console,otlp --metrics_exporter otlp uvicorn main:app --port 8001
