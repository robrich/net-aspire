# build "server" image
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine as build

WORKDIR /src

COPY AspireStandalone.ApiService.csproj .
RUN dotnet restore

COPY . .
RUN dotnet build -c Release
RUN dotnet test -c Release
RUN dotnet publish -c Release -o /dist AspireStandalone.ApiService.csproj


# production runtime "server" image
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080
ENV ConnectionStrings__Redis=""
ENV ConnectionStrings__Postgres=""

WORKDIR /app
COPY --from=build /dist .
CMD ["dotnet", "AspireStandalone.ApiService.dll"]
