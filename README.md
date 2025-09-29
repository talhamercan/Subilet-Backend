# Subilet Backend

Biletix benzeri etkinlik platformu için Clean Architecture + Modüler Monolith mimarili, .NET 8 tabanlı backend.

## Özellikler
- **Kullanıcı:** Etkinlikleri görüntüleme, koltuk/bilet seçimi, sepete ekleme, checkout (ödeme simülasyonu)
- **Admin:** Etkinlik, sanatçı, venue, koltuk, oturma planı yönetimi, kullanıcı banlama, görsel yükleme (Azure Blob)
- **Gerçek Zamanlı:** SignalR ile koltuk rezervasyon/satış güncellemesi
- **Altyapı:** Azure SQL, Azure Blob, Redis, Serilog, Rate Limiting, JWT, Swagger, Docker, CI/CD

## Mimari
- **Clean Architecture**: Domain, Application, Infrastructure, Presentation katmanları
- **Modüler Monolith**: Her iş alanı ayrı modül, mikroservise hazır
- **SignalR**: Koltuk durumu anlık güncelleme
- **Redis**: Koltuk rezervasyon süresi (TTL)
- **Azure Blob**: Etkinlik görselleri
- **Serilog**: Loglama (dosya + konsol)
- **Rate Limiting**: Abuse koruması
- **Swagger**: API dokümantasyonu
- **xUnit**: Test altyapısı
- **Docker**: Kolay container deploy
- **CI/CD**: GitHub Actions ile otomatik build, test, docker

## Hızlı Başlangıç (Geliştirme)
```sh
# Gerekli araçlar: .NET 8 SDK, Docker (opsiyonel), Redis, Azure Blob (veya local emulator)

dotnet restore ./subiletbackend/subiletbackend.sln
dotnet build ./subiletbackend/subiletbackend.sln
dotnet run --project ./subiletbackend/subiletbackend/subiletbackend.csproj
# Swagger UI: http://localhost:5000/swagger
```

## Docker ile Çalıştırma
```sh
docker build -t subilet-backend -f subiletbackend/Dockerfile .
docker run -p 8080:8080 --env-file .env subilet-backend
```

## Environment Variables (Örnek .env)
```
ConnectionStrings__DefaultConnection=... # Azure SQL
Jwt__Key=... # JWT Secret
Jwt__Issuer=...
Jwt__Audience=...
Redis__ConnectionString=localhost:6379
AzureBlob__ConnectionString=...
KeyVaultUri=... # (Production için)
```

## Production Deploy
- Azure App Service, Azure Container Apps veya Kubernetes ile kolayca deploy
- Azure Key Vault ile secret yönetimi önerilir
- CI/CD pipeline: `.github/workflows/ci-cd.yml`

## API Dokümantasyonu
- Swagger UI: `/swagger`
- JWT ile korunan endpointler için "Authorize" butonu

## Test Çalıştırma
```sh
dotnet test ./subiletbackend/subiletbackend.Tests/subiletbackend.Tests.csproj
```

## Katkı Sağlama
- PR açmadan önce testlerin geçtiğinden emin olun
- Kod standartlarına ve Clean Architecture prensiplerine uyun
- Swagger açıklamalarını ve endpoint örneklerini güncel tutun

## İletişim
- Soru, öneri veya katkı için: [your-email@example.com]

## Proje Sahibi
- `talhamercan` — GitHub: https://github.com/talhamercan
