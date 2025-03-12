# build "server" image
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine as build

WORKDIR /src

COPY AspireStandalone.LoadTest.csproj .
RUN dotnet restore

COPY . .
RUN dotnet build -c Release
RUN dotnet test -c Release
RUN dotnet publish -c Release -o /dist AspireStandalone.LoadTest.csproj


# production runtime "server" image
FROM mcr.microsoft.com/dotnet/runtime:9.0-alpine

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ApiServiceUrl=""

WORKDIR /app
COPY --from=build /dist .
CMD ["dotnet", "AspireStandalone.LoadTest.dll"]
