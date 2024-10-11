FROM python:slim

WORKDIR /app

COPY requirements.txt .

RUN python -m venv .venv
RUN pip install --no-cache-dir --upgrade -r requirements.txt

COPY . .

ENV OTEL_EXPORTER_OTLP_ENDPOINT=""
ENV OTEL_EXPORTER_OTLP_PROTOCOL="grpc"
ENV OTEL_METRICS_EXEMPLAR_FILTER="trace_based"
ENV OTEL_PYTHON_LOGGING_AUTO_INSTRUMENTATION_ENABLED="true"
ENV OTEL_RESOURCE_ATTRIBUTES="service.instance.id=python-fastapi"
ENV OTEL_SERVICE_NAME="python-fastapi"
ENV OTEL_TRACES_SAMPLER="always_on"

CMD ["opentelemetry-instrument", "--traces_exporter", "otlp", "--logs_exporter", "console,otlp", "--metrics_exporter", "otlp", "uvicorn", "main:app", "--host", "0.0.0.0", "--port", "8001"]
