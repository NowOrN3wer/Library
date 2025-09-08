# 1. Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Solution ve projeleri kopyala
COPY *.sln ./
COPY Library.WebAPI/*.csproj ./Library.WebAPI/
COPY Library.Application/*.csproj ./Library.Application/
COPY Library.Infrastructure/*.csproj ./Library.Infrastructure/
COPY Library.Domain/*.csproj ./Library.Domain/
COPY Library.Test/*.csproj ./Library.Test/

# Gerekli NuGet paketlerini indir
RUN dotnet restore

# Tüm solution içeriğini kopyala
COPY . ./

# WebAPI projesine geç
WORKDIR /app/Library.WebAPI

# Yayınlama (publish)
RUN dotnet publish -c Release -o /app/out

# 2. Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Build aşamasından yayınlanmış çıktıyı kopyala
COPY --from=build /app/out ./

# Ortam değişkeni (isteğe bağlı)
ENV ASPNETCORE_ENVIRONMENT=Development

# Uygulama portu
ENV ASPNETCORE_URLS=http://+:9910
EXPOSE 9910

# Uygulama giriş noktası
ENTRYPOINT ["dotnet", "Library.WebAPI.dll"]
