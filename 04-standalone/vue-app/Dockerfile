FROM node:alpine as build

WORKDIR /src

COPY package.json package.json

RUN npm install

COPY . .

# build .env
ARG VITE_OTEL_EXPORTER_OTLP_ENDPOINT
ARG VITE_OTEL_EXPORTER_OTLP_HEADERS
ARG VITE_OTEL_RESOURCE_ATTRIBUTES
ARG VITE_OTEL_SERVICE_NAME
RUN echo "VITE_OTEL_EXPORTER_OTLP_ENDPOINT=$VITE_OTEL_EXPORTER_OTLP_ENDPOINT" > .env
RUN echo "VITE_OTEL_EXPORTER_OTLP_HEADERS=$VITE_OTEL_EXPORTER_OTLP_HEADERS" >> .env
RUN echo "VITE_OTEL_RESOURCE_ATTRIBUTES=$VITE_OTEL_RESOURCE_ATTRIBUTES" >> .env
RUN echo "VITE_OTEL_SERVICE_NAME=$VITE_OTEL_SERVICE_NAME" >> .env

RUN npm run build


FROM nginx:alpine

# Install OpenTelemetry
# https://github.com/open-telemetry/opentelemetry-cpp-contrib/issues/302#issuecomment-2236282685
# https://nginx.org/packages/mainline/alpine/v3.20/main/x86_64/

RUN apk add --no-cache libstdc++

ARG ARCH="x86_64"

RUN ALPINE_VERSION=$(cat /etc/alpine-release | cut -d. -f1-2 | sed 's/^/v/') && \
    NGINX_VERSION=$(nginx -v 2>&1 | cut -d/ -f2) && \
    MODULE_URL_BASE=https://nginx.org/packages/mainline/alpine/$ALPINE_VERSION/main/${ARCH}/ && \
    wget -qO- $MODULE_URL_BASE | \
    grep $NGINX_VERSION | \
    grep -o 'nginx-module-otel-[0-9\.]*-r[0-9]*\.apk' | \
    sort -Vr | \
    head -n 1 | \
    xargs -I {} wget ${MODULE_URL_BASE}{} && \
    tar -xzf ./nginx-module-otel-*.apk && \
    rm nginx-module-otel-*.apk

RUN echo "load_module modules/ngx_otel_module.so;" | cat - /etc/nginx/nginx.conf > /tmp/nginx.conf; mv /tmp/nginx.conf /etc/nginx/nginx.conf
COPY nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /src/dist /usr/share/nginx/html

# nginx:alpine has a CMD already
