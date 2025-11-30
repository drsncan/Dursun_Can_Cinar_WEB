📄 Döküman Yönetim Sistemi (DMS)

Kurumsal doküman süreçlerini kolaylaştırmak için geliştirilmiş, güvenli ve modern bir Document Management System.

<div align="center"> <img src="https://raw.githubusercontent.com/github/explore/main/topics/aspnet/aspnet.png" width="90" /> <img src="https://raw.githubusercontent.com/github/explore/main/topics/sql-server/sql-server.png" width="90" /> <img src="https://raw.githubusercontent.com/github/explore/main/topics/bootstrap/bootstrap.png" width="90" /> </div> <br> <div align="center">
</div>
🚀 Özellikler
🔐 Kullanıcı ve Rol Yönetimi

Admin / User rolleri

Kullanıcı oluşturma, aktif/pasif yapma

Rol değiştirme (User → Admin)

Kullanıcı belge geçmişi görüntüleme

📁 Belge Yönetimi

Dosya yükleme / indirme

Belge düzenleme / silme / detay

Belgeleri filtreleme ve arama

"Sadece benim belgelerim" modu

Durumlar: Taslak, Onay Bekliyor, Onaylandı, Reddedildi

Public / Private belge seçenekleri

✔️ Onay Süreçleri

Admin panelinde bekleyen belgeleri listeleme

Belgeyi inceleme – Onaylama – Reddetme

Durum geçmişi

🌐 API Entegrasyonu

Ücretsiz dış API’dan belge çekme (JSONPlaceholder)

API’den gelen belgeleri tabloya ekleme

API belgelerini sisteme kaydetme (dosyasız belge)

⚙️ Sistem Ayarları

Sistem adı

Kurum adı

Tema seçimi (Açık / Koyu)

Logo yükleme

Maksimum dosya boyutu ayarı

İzin verilen dosya uzantıları

🎨 Arayüz Özellikleri

AdminLTE 3 tabanlı modern panel

Responsive mobil uyumlu tasarım

Sol menüde logo + kurum adı görüntüleme

Login ekranında açık mavi tema

| Katman     | Teknoloji                       |
| ---------- | ------------------------------- |
| Backend    | ASP.NET Core 8 (MVC)            |
| Veritabanı | SQL Server / LocalDB            |
| ORM        | Entity Framework Core 8         |
| Frontend   | Bootstrap 4.6, AdminLTE, jQuery |
| API        | REST JSON API (JSONPlaceholder) |
| Depolama   | wwwroot/uploads                 |

DmsWeb/
 ├── Controllers/
 ├── Data/
 ├── Models/
 ├── Migrations/
 ├── Views/
 ├── wwwroot/
 └── README.md

 Kurulum
1️⃣ Depoyu Klonla
git clone https://github.com/kullaniciadi/DMS.git

2️⃣ Bağımlılıkları Yükle
dotnet restore

3️⃣ Veritabanını Oluştur
dotnet ef database update

4️⃣ Projeyi Çalıştır
dotnet run

🔐 Varsayılan Giriş Bilgileri
Admin
admin / 1234

User
user / 1234

🧭 Yol Haritası

🔍 Full-text arama

🗂 Belge klasör yapısı

🕒 Versiyonlama

🔐 JWT API desteği

📜 Log & Audit sistemi

🧩 Modüler microservice yapı

👨‍💻 Geliştirici

Dursun Can Çınar
Computer Engineering – Ankara University