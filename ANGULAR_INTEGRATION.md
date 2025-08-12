# Angular Frontend Entegrasyonu

Bu dokümantasyon, Angular frontend'iniz için backend entegrasyonunu açıklar.

## 🚀 Port Yapılandırması

✅ **Port Ayarları:**
- **Frontend (Angular)**: `http://localhost:4200`
- **Backend (API)**: `https://localhost:7079`
- **Swagger UI**: `https://localhost:7079/swagger`
- **Health Check**: `https://localhost:7079/health`

## 📡 API Endpoint'leri

### 🔐 Kimlik Doğrulama

```typescript
// Login
POST https://localhost:7079/api/auth/login
{
  "username": "admin",
  "password": "admin123"
}

// Register
POST https://localhost:7079/api/auth/register
{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john@example.com",
  "username": "johndoe",
  "password": "password123"
}

// Mevcut kullanıcı bilgileri
GET https://localhost:7079/api/auth/me
Authorization: Bearer {token}
```

### 🎵 Müzik Etkinlikleri

```typescript
// Tüm müzik etkinlikleri
GET https://localhost:7079/api/music

// Pop müzik etkinlikleri
GET https://localhost:7079/api/music/pop

// Rock müzik etkinlikleri
GET https://localhost:7079/api/music/rock

// Jazz müzik etkinlikleri
GET https://localhost:7079/api/music/jazz

// Yeni müzik etkinliği oluştur
POST https://localhost:7079/api/music
{
  "artistName": "Test Artist",
  "description": "Test Description",
  "date": "2025-01-15T20:00:00Z",
  "location": "Test Location",
  "price": 100.0,
  "capacity": 1000,
  "imageUrl": "/images/test.jpg",
  "genre": 1
}
```

### ⚽ Spor Etkinlikleri

```typescript
// Tüm spor etkinlikleri
GET https://localhost:7079/spor

// Futbol etkinlikleri
GET https://localhost:7079/spor/futbol

// Basketbol etkinlikleri
GET https://localhost:7079/spor/basketbol

// Tenis etkinlikleri
GET https://localhost:7079/spor/tenis
```

### 🎭 Sahne Etkinlikleri

```typescript
// Tüm sahne etkinlikleri
GET https://localhost:7079/sahne

// Tiyatro etkinlikleri
GET https://localhost:7079/sahne/tiyatro

// Dans etkinlikleri
GET https://localhost:7079/sahne/dans

// Opera etkinlikleri
GET https://localhost:7079/sahne/opera
```

### 🛠️ Angular Özel Endpoint'leri

```typescript
// Backend bağlantı testi
GET https://localhost:7079/api/angular/test

// Enum değerleri
GET https://localhost:7079/api/angular/enums

// Uygulama bilgileri
GET https://localhost:7079/api/angular/app-info

// Health check
GET https://localhost:7079/api/angular/health
```

## 📊 Response Formatı

Tüm API'ler standart response formatı kullanır:

```typescript
interface ApiResponse<T> {
  success: boolean;
  message: string;
  data?: T;
  errors?: string[];
  totalCount?: number;
  pageNumber?: number;
  pageSize?: number;
}
```

### Örnek Response'lar

**Başarılı Response:**
```json
{
  "success": true,
  "message": "Müzik etkinlikleri başarıyla getirildi",
  "data": [
    {
      "id": "guid-here",
      "artistName": "Tarkan",
      "description": "Türk pop müziğinin megastarı",
      "date": "2025-07-28T20:00:00Z",
      "location": "İstanbul Harbiye Açıkhava",
      "price": 950.00,
      "capacity": 350,
      "imageUrl": "/images/tarkan.jpg",
      "genre": 1,
      "status": 1
    }
  ]
}
```

**Hata Response:**
```json
{
  "success": false,
  "message": "Kullanıcı adı veya şifre hatalı",
  "errors": ["Invalid credentials"]
}
```

## 🔧 Angular Service Örneği

```typescript
// auth.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'https://localhost:7079/api';

  constructor(private http: HttpClient) {}

  login(username: string, password: string): Observable<ApiResponse<LoginResponse>> {
    return this.http.post<ApiResponse<LoginResponse>>(`${this.apiUrl}/auth/login`, {
      username,
      password
    });
  }

  register(userData: RegisterRequest): Observable<ApiResponse<LoginResponse>> {
    return this.http.post<ApiResponse<LoginResponse>>(`${this.apiUrl}/auth/register`, userData);
  }

  getCurrentUser(): Observable<ApiResponse<UserDto>> {
    return this.http.get<ApiResponse<UserDto>>(`${this.apiUrl}/auth/me`);
  }
}

// music.service.ts
@Injectable({
  providedIn: 'root'
})
export class MusicService {
  private apiUrl = 'https://localhost:7079/api';

  constructor(private http: HttpClient) {}

  getAllEvents(): Observable<ApiResponse<MusicEventDto[]>> {
    return this.http.get<ApiResponse<MusicEventDto[]>>(`${this.apiUrl}/music`);
  }

  getPopEvents(): Observable<ApiResponse<MusicEventDto[]>> {
    return this.http.get<ApiResponse<MusicEventDto[]>>(`${this.apiUrl}/music/pop`);
  }

  createEvent(eventData: CreateMusicEventCommand): Observable<ApiResponse<Guid>> {
    return this.http.post<ApiResponse<Guid>>(`${this.apiUrl}/music`, eventData);
  }
}
```

## 🔐 JWT Token Yönetimi

```typescript
// auth.interceptor.ts
import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = localStorage.getItem('token');
    
    if (token) {
      req = req.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      });
    }
    
    return next.handle(req);
  }
}
```

## 🎨 Enum Değerleri

```typescript
// Backend'den enum değerlerini al
GET https://localhost:7079/api/angular/enums

// Response:
{
  "success": true,
  "message": "Enum değerleri başarıyla getirildi",
  "data": {
    "musicGenres": [
      { "value": 1, "name": "Pop", "displayName": "Pop" },
      { "value": 2, "name": "Rock", "displayName": "Rock" },
      { "value": 3, "name": "Jazz", "displayName": "Jazz" }
    ],
    "sportGenres": [
      { "value": 1, "name": "Football", "displayName": "Futbol" },
      { "value": 2, "name": "Basketball", "displayName": "Basketbol" }
    ]
  }
}
```

## 🚀 Hızlı Başlangıç

1. **Backend'i çalıştırın:**
```bash
cd SubiletServer
dotnet run --project src/SubiletServer.WebAPI --urls "https://localhost:7079;http://localhost:7079"
```

2. **Angular'da environment.ts dosyasını güncelleyin:**
```typescript
export const environment = {
  production: false,
  apiUrl: 'https://localhost:7079/api'
};
```

3. **Angular'ı çalıştırın:**
```bash
ng serve --port 4200
```

4. **Test endpoint'ini çağırın:**
```typescript
// Test bağlantısı
this.http.get<ApiResponse<string>>(`${environment.apiUrl}/angular/test`)
  .subscribe(response => {
    console.log('Backend bağlantısı:', response);
  });
```

## 📱 Angular Component Örneği

```typescript
// music-events.component.ts
@Component({
  selector: 'app-music-events',
  template: `
    <div *ngIf="loading">Yükleniyor...</div>
    <div *ngIf="error">{{ error }}</div>
    <div *ngFor="let event of events">
      <h3>{{ event.artistName }}</h3>
      <p>{{ event.description }}</p>
      <p>Fiyat: {{ event.price }} TL</p>
    </div>
  `
})
export class MusicEventsComponent implements OnInit {
  events: MusicEventDto[] = [];
  loading = false;
  error = '';

  constructor(private musicService: MusicService) {}

  ngOnInit() {
    this.loadEvents();
  }

  loadEvents() {
    this.loading = true;
    this.musicService.getAllEvents().subscribe({
      next: (response) => {
        if (response.success) {
          this.events = response.data || [];
        } else {
          this.error = response.message;
        }
      },
      error: (err) => {
        this.error = 'Bağlantı hatası';
      },
      complete: () => {
        this.loading = false;
      }
    });
  }
}
```

## 🔧 CORS Ayarları

Backend CORS ayarları Angular için optimize edilmiştir:

```csharp
app.UseCors(x => x
    .AllowAnyHeader()
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowCredentials()
    .SetPreflightMaxAge(TimeSpan.FromMinutes(10))
);
```

## 📊 Health Check

```typescript
// Backend durumunu kontrol et
GET https://localhost:7079/health
GET https://localhost:7079/api/angular/health
```

## 🎯 Sonraki Adımlar

1. **Angular projenizi oluşturun** (eğer yoksa)
2. **Environment dosyalarını yapılandırın**
3. **Service'leri implement edin**
4. **Interceptor'ları ekleyin**
5. **Component'leri oluşturun**
6. **Error handling ekleyin**

## 📞 Destek

Backend hazır ve çalışıyor! Angular frontend'inizi geliştirmeye başlayabilirsiniz.

**Test URL'leri:**
- Frontend: `http://localhost:4200`
- Backend: `https://localhost:7079`
- Swagger: `https://localhost:7079/swagger`
- Health: `https://localhost:7079/health`
- Angular Test: `https://localhost:7079/api/angular/test` 