@echo off
echo Subilet Backend Projesi Başlatılıyor...
echo.

cd /d "%~dp0subiletbackend"

echo Proje build ediliyor...
dotnet build

echo.
echo Proje başlatılıyor ve Swagger UI açılıyor...
echo.

REM Projeyi arka planda başlat
start /B dotnet run

REM 5 saniye bekle
timeout /t 5 /nobreak >nul

REM Swagger UI'yi tarayıcıda aç
echo Swagger UI tarayıcıda açılıyor...
start http://localhost:5117/swagger

echo.
echo Proje başarıyla başlatıldı!
echo API: http://localhost:5117
echo Swagger UI: http://localhost:5117/swagger
echo.
echo Projeyi durdurmak için terminal penceresinde Ctrl+C yapın
echo.
pause
