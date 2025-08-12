# Admin Panel API Dokümantasyonu

Bu dokümantasyon, SubiletServer admin panelinin dinamik API endpoint'lerini açıklar.

## Genel Bakış

Admin paneli tüm entity'ler (Users, MusicEvents, SportEvents, StageEvents) için dinamik CRUD işlemlerini destekler.

## Base URL
```
/api/admin
```

## Endpoint'ler

### 1. Generic CRUD İşlemleri

#### Tüm Entity'leri Listele
```http
GET /api/admin/entities/{entityType}
```

**Desteklenen entityType değerleri:**
- `users`
- `musicevents`
- `sportevents`
- `stageevents`

#### Tek Entity Getir
```http
GET /api/admin/entities/{entityType}/{id}
```

#### Entity Oluştur
```http
POST /api/admin/entities/{entityType}
Content-Type: application/json

{
  "property1": "value1",
  "property2": "value2"
}
```

#### Entity Güncelle
```http
PUT /api/admin/entities/{entityType}/{id}
Content-Type: application/json

{
  "property1": "newValue1",
  "property2": "newValue2"
}
```

#### Entity Sil
```http
DELETE /api/admin/entities/{entityType}/{id}
```

### 2. Schema Bilgileri

#### Entity Schema'sını Getir
```http
GET /api/admin/entities/{entityType}/schema
```

Bu endpoint, entity'nin hangi property'lere sahip olduğunu ve bunların tiplerini döner.

### 3. Dashboard İstatistikleri

#### Dashboard Stats
```http
GET /api/admin/dashboard
```

Bu endpoint şu bilgileri döner:
- Kullanıcı istatistikleri (toplam, aktif, pasif)
- Event istatistikleri (her kategori için toplam sayı)

### 4. Enum Değerleri

#### Tüm Enum'ları Getir
```http
GET /api/admin/enums
```

#### Belirli Enum'u Getir
```http
GET /api/admin/enums/{enumType}
```

**Desteklenen enumType değerleri:**
- `musicgenre`
- `sportgenre`
- `stagegenre`
- `eventstatus`

## Entity Örnekleri

### User Entity
```json
{
  "FirstName": "John",
  "LastName": "Doe",
  "Email": "john@example.com",
  "Username": "johndoe",
  "Password": "password123",
  "IsActive": true
}
```

### MusicEvent Entity
```json
{
  "ArtistName": "Rock Band",
  "Description": "Amazing rock concert",
  "Date": "2024-12-25T20:00:00Z",
  "Location": "Concert Hall",
  "Price": 150.00,
  "Capacity": 1000,
  "ImageUrl": "https://example.com/image.jpg",
  "Genre": 1,
  "Status": 1
}
```

### SportEvent Entity
```json
{
  "Title": "Football Match",
  "Description": "Championship final",
  "Date": "2024-12-30T15:00:00Z",
  "Location": "Stadium",
  "Price": 75.00,
  "Capacity": 50000,
  "ImageUrl": "https://example.com/football.jpg",
  "Genre": 1,
  "Status": 1
}
```

### StageEvent Entity
```json
{
  "Title": "Hamlet",
  "Description": "Shakespeare's classic",
  "Date": "2024-12-20T19:00:00Z",
  "Location": "Theatre",
  "Price": 200.00,
  "Capacity": 500,
  "ImageUrl": "https://example.com/hamlet.jpg",
  "Genre": 1,
  "Status": 1
}
```

## Enum Değerleri

### MusicGenre
- 1: Pop
- 2: Rock
- 3: Jazz
- 4: Classical
- 5: Folk
- 6: Electronic
- 7: Other

### SportGenre
- 1: Football
- 2: Basketball
- 3: Tennis
- 4: Volleyball
- 5: Swimming
- 6: Athletics
- 7: Other

### StageGenre
- 1: Theatre
- 2: Dance
- 3: Opera
- 4: Ballet
- 5: Musical
- 6: Comedy
- 7: Other

### EventStatus
- 1: Active
- 2: Inactive
- 3: Cancelled
- 4: SoldOut

## Hata Yönetimi

Tüm endpoint'ler tutarlı hata yanıtları döner:

```json
{
  "message": "Hata mesajı",
  "error": "Detaylı hata bilgisi"
}
```

## Güvenlik

- Tüm admin endpoint'leri `[Authorize]` attribute'u ile korunur
- Admin kullanıcısı silinemez
- Tüm işlemler try-catch blokları ile korunur

## Kullanım Örnekleri

### 1. Tüm müzik etkinliklerini getir
```bash
curl -X GET "https://api.example.com/api/admin/entities/musicevents" \
  -H "Authorization: Bearer YOUR_TOKEN"
```

### 2. Yeni kullanıcı oluştur
```bash
curl -X POST "https://api.example.com/api/admin/entities/users" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -d '{
    "FirstName": "Jane",
    "LastName": "Smith",
    "Email": "jane@example.com",
    "Username": "janesmith",
    "Password": "password123"
  }'
```

### 3. Dashboard istatistiklerini getir
```bash
curl -X GET "https://api.example.com/api/admin/dashboard" \
  -H "Authorization: Bearer YOUR_TOKEN"
```

### 4. Entity schema'sını getir
```bash
curl -X GET "https://api.example.com/api/admin/entities/users/schema" \
  -H "Authorization: Bearer YOUR_TOKEN"
```

Bu dinamik admin paneli sayesinde yeni entity'ler eklemek için sadece repository'leri ve schema bilgilerini güncellemeniz yeterli olacaktır. 