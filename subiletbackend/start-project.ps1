Write-Host "Subilet Backend Projesi Başlatılıyor..." -ForegroundColor Green
Write-Host ""

# Proje dizinine git
Set-Location ".\subiletbackend"

Write-Host "Proje build ediliyor..." -ForegroundColor Yellow
dotnet build

Write-Host ""
Write-Host "Proje başlatılıyor ve Swagger UI açılıyor..." -ForegroundColor Yellow
Write-Host ""

# Projeyi arka planda başlat
$job = Start-Job -ScriptBlock {
    Set-Location $using:PWD
    dotnet run
}

# 5 saniye bekle
Start-Sleep -Seconds 5

# Swagger UI'yi tarayıcıda aç
Write-Host "Swagger UI tarayıcıda açılıyor..." -ForegroundColor Green
Start-Process "http://localhost:5117/swagger"

Write-Host ""
Write-Host "Proje başarıyla başlatıldı!" -ForegroundColor Green
Write-Host "API: http://localhost:5117" -ForegroundColor Cyan
Write-Host "Swagger UI: http://localhost:5117/swagger" -ForegroundColor Cyan
Write-Host ""
Write-Host "Projeyi durdurmak için: Stop-Job -Job $($job.Id)" -ForegroundColor Red
Write-Host ""

# Job durumunu göster
Get-Job $job.Id | Format-Table

Write-Host "Devam etmek için Enter'a basın..." -ForegroundColor Yellow
Read-Host
