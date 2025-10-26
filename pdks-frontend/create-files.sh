#!/bin/bash

# index.html
cat > index.html << 'EOF'
<!doctype html>
<html lang="tr">
  <head>
    <meta charset="UTF-8" />
    <link rel="icon" type="image/svg+xml" href="/vite.svg" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>PDKS - Personel Yönetim Sistemi</title>
  </head>
  <body>
    <div id="root"></div>
    <script type="module" src="/src/main.tsx"></script>
  </body>
</html>
EOF

# .gitignore
cat > .gitignore << 'EOF'
# Logs
logs
*.log
npm-debug.log*
yarn-debug.log*
yarn-error.log*
pnpm-debug.log*
lerna-debug.log*

node_modules
dist
dist-ssr
*.local

# Editor directories and files
.vscode/*
!.vscode/extensions.json
.idea
.DS_Store
*.suo
*.ntvs*
*.njsproj
*.sln
*.sw?
EOF

# README.md
cat > README.md << 'EOF'
# PDKS Frontend - Personel Yönetim Sistemi

Modern, profesyonel ve kullanıcı dostu IK/Personel yönetim sistemi frontend uygulaması.

## Özellikler

- ✅ Personel Özlük Yönetimi (Aile, Eğitim, Sertifika, Performans, vb.)
- ✅ İzin Yönetimi (Talep, Onay, Bakiye Takibi)
- ✅ Avans & Masraf Yönetimi  
- ✅ Seyahat & Araç Talebi
- ✅ Zimmet Yönetimi
- ✅ Gelişmiş Puantaj/Giriş-Çıkış Takibi
- ✅ Çoklu Onay Sistemi
- ✅ Dosya Yönetimi
- ✅ Dashboard & Raporlar
- ✅ Çoklu Şirket Desteği
- ✅ Rol Bazlı Yetkilendirme

## Kurulum

1. Bağımlılıkları yükleyin:
```bash
npm install
```

2. Geliştirme sunucusunu başlatın:
```bash
npm run dev
```

3. Backend servisinin çalıştığından emin olun:
- Backend: http://localhost:5104
- Frontend: http://localhost:56899

## Teknolojiler

- React 19 + TypeScript
- Material-UI (MUI) 6
- React Router v7
- Axios
- Recharts
- Date-fns

## Proje Yapısı

```
src/
├── components/       # Paylaşılan componentler
├── contexts/         # React Context (Auth vb.)
├── services/         # API servisleri
├── pages/           # Sayfa componentleri
│   ├── Dashboard/   # Ana gösterge paneli
│   ├── Personel/    # Personel yönetimi
│   ├── Izin/        # İzin yönetimi
│   ├── Avans/       # Avans yönetimi
│   ├── Masraf/      # Masraf yönetimi
│   ├── Zimmet/      # Zimmet yönetimi
│   ├── Arac/        # Araç talebi
│   ├── Seyahat/     # Seyahat talebi
│   ├── Puantaj/     # Puantaj/Giriş-Çıkış
│   ├── Onay/        # Onay işlemleri
│   └── Rapor/       # Raporlar
```

## Backend Entegrasyonu

Backend API endpoint'leri `/api` prefix'i ile proxy üzerinden çağrılır.
Örnek: `/api/Personel`, `/api/Izin`, vb.

## Geliştirme Notları

- Tüm API çağrıları `services` klasöründeki servisler üzerinden yapılır
- Yetkilendirme AuthContext ile yönetilir
- Material-UI tema ayarları `App.tsx` içinde yapılandırılmıştır

## Lisans

Proprietary - Tüm hakları saklıdır.
EOF

echo "Temel dosyalar oluşturuldu!"
