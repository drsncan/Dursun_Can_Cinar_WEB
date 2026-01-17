# 📄 Belge Yönetim Sistemi (Document Management System - DMS)

## 🔖 Proje Tanımı

Bu proje, kurum içi doküman süreçlerinin dijital ortamda güvenli, kontrollü ve izlenebilir bir şekilde yönetilebilmesini sağlayan web tabanlı bir **Belge Yönetim Sistemi (Document Management System – DMS)** uygulamasıdır.

Sistem; kullanıcı ve rol yönetimi, belge yükleme, onay süreçleri ve belge durum takibini içeren modern bir web mimarisi ile geliştirilmiştir.

---

## 🎯 Projenin Amacı

Projenin temel amacı, kurum içi belge akışını manuel yöntemlerden kurtararak dijital ortama taşımak ve belge süreçlerini daha verimli hale getirmektir.

Bu kapsamda geliştirilen sistem ile:

- Belgeler güvenli şekilde saklanır,
- Yetkilendirme mekanizması ile erişimler kontrol edilir,
- Onay süreçleri izlenebilir hale gelir,
- Kullanıcı dostu bir arayüz ile belge yönetimi kolaylaştırılır.

---

## 📦 Proje Kapsamı

Proje aşağıdaki temel özellikleri içermektedir:

- Kullanıcı kayıt ve giriş işlemleri (ASP.NET Identity)
- Rol bazlı erişim kontrolü (Admin, Kullanıcı, Onaycı)
- Belge yükleme, silme ve görüntüleme
- Belge durum yönetimi (Beklemede / Onaylandı / Reddedildi)
- Belge açıklamaları ve metadata kaydı
- Belge listeleme ve filtreleme
- Onay ve red işlemleri
- İşlem loglama altyapısı

---

## 🛠️ Kullanılan Teknolojiler

| Katman           | Teknoloji             | Açıklama                                                                     |
| ---------------- | --------------------- | ---------------------------------------------------------------------------- |
| Backend          | ASP.NET Core 8 (MVC)  | Katmanlı mimari kullanılarak uygulamanın sunucu tarafı geliştirilmiştir.     |
| API              | RESTful API           | Frontend ile backend arasındaki veri iletişimi JSON formatında sağlanmıştır. |
| ORM              | Entity Framework Core | Veritabanı işlemleri nesne tabanlı olarak gerçekleştirilmiştir.              |
| Veritabanı       | SQL Server (LocalDB)  | Kullanıcı, belge ve onay verileri ilişkisel yapıda saklanmaktadır.           |
| Frontend         | HTML, CSS, Bootstrap  | Kullanıcı arayüzü responsive ve sade bir yapıda tasarlanmıştır.              |
| UI Template      | AdminLTE              | Yönetim paneli için kullanılmıştır.                                          |
| Versiyon Kontrol | Git & GitHub          | Proje sürüm takibi ve paylaşımı sağlanmıştır.                                |
| Yapay Zeka       | ChatGPT               | Kod taslakları ve hata analizlerinde destek amaçlı kullanılmıştır.           |

---

## 🗂️ Proje Yapısı

```
Dursun_Can_Cinar_WEB/
│
├── Controllers/      → API ve MVC controller'ları
├── Models/           → Veritabanı entity sınıfları
├── Data/             → DbContext ve veritabanı yapılandırması
├── Views/            → Kullanıcı arayüzü sayfaları
├── wwwroot/          → CSS, JS ve yüklenen dosyalar
├── Migrations/       → EF Core migration dosyaları
└── README.md         → Proje dokümantasyonu
```

---

## 🔗 Sistem Mimarisi

Uygulama üç katmanlı mimari ile geliştirilmiştir:

- **Frontend:** Kullanıcı arayüzü
- **Backend:** İş mantığı ve API katmanı
- **Veritabanı:** Kalıcı veri saklama katmanı

Tüm veri alışverişi frontend ile backend arasında RESTful API aracılığıyla gerçekleştirilmiştir.

---

## 🧩 Veritabanı Yapısı

Sistemde aşağıdaki temel tablolar yer almaktadır:

- Users
- Roles
- Documents
- DocumentFiles
- ApprovalRequests
- ApprovalActions
- AuditLogs
- SystemSettings

Bu yapı sayesinde belge süreçleri, kullanıcı işlemleri ve onay mekanizmaları izlenebilir hale getirilmiştir.

---

## 🔄 Uygulama Çalışma Akışı (Özet)

1. Kullanıcı sisteme giriş yapar.
2. Yetkisine göre işlem yapabileceği sayfalar görüntülenir.
3. Belge yükleme işlemi frontend üzerinden API'ye iletilir.
4. Backend gerekli kontrolleri yapar ve veriyi veritabanına kaydeder.
5. Onay süreci başlatılır ve belge durumu güncellenir.
6. Tüm işlemler loglanır.

---

## 🎥 Proje Tanıtım Videosu

📌 **Video Linki: https://youtu.be/IIHZu3TPz3w


---

## 👤 Geliştirici

**Dursun Can Çınar**\
Ankara Üniversitesi – Bilgisayar Mühendisliği

---

## 📌 Notlar

- Proje akademik amaçlı geliştirilmiştir.
- Tüm kodlar manuel olarak yazılmış ve test edilmiştir.
- Yapay zeka yalnızca destekleyici araç olarak kullanılmıştır.

