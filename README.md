# ETicaret Projesi - Kurulum ve Çalıştırma Rehberi

Bu doküman, **ETicaret** projesini kendi bilgisayarınızda sağlıklı bir şekilde çalıştırabilmeniz için gerekli adımları detaylı olarak açıklamaktadır.

---

## 🔧 Gereksinimler
- Visual Studio 2022 veya üzeri
- .NET 6.0 veya üzeri
- SQL Server
- Entity Framework Core

---

## 🚀 Projeyi Çalıştırma Adımları

### 1️⃣ Mevcut Migration Dosyalarını Silin
İlk olarak, `ETicaretDataKatmanı` klasörü içerisinde bulunan `Migrations` klasörünü tamamen silin.

> **Neden?**  
Bu işlem, projeyi kendi veritabanınıza göre yeniden yapılandırıp sıfırdan bir migration oluşturmanız için gereklidir.

---

### 2️⃣ Connection String'i Düzenleyin
`ETicaretDataKatmanı > Context > Context.cs` dosyasını açın.  
`optionsBuilder.UseSqlServer()` metodu içerisindeki bağlantı cümlesini kendi bilgisayarınıza ve veritabanınıza uygun şekilde düzenleyin.

#### Örnek Connection String:
```csharp
optionsBuilder.UseSqlServer("Server=YOUR_SERVER_NAME;Database=YOUR_DATABASE_NAME;Trusted_Connection=True;TrustServerCertificate=True;");
```
### 3️⃣ Package Manager Console Ayarlarını Yapın
Migration işlemlerine başlamadan önce aşağıdaki adımları takip edin:

1.Visual Studio'da View (Görünüm) menüsüne tıklayın.

2.Other Windows (Diğer Pencereler) seçeneğine gelin.

3.Package Manager Console penceresini açın.

4.Sağ üst köşede bulunan Default Project (Varsayılan Proje) kısmını ETicaretDataKatmanı olarak seçin.

### 4️⃣ Migration Oluşturun ve Veritabanını Güncelleyin
Aşağıdaki komutları sırasıyla Package Manager Console penceresine yazın:

➤ Migration Oluştur
```csharp
Add-Migration [Bir İsim Verin]
```
➤ Veritabanını Güncelle
```csharp
Update-Database
```
### 📌 Ek Bilgiler
Eğer hata alırsanız connection string ayarınızı ve SQL Server bağlantınızı kontrol edin.

TrustServerCertificate=True; parametresi geliştirme ortamları için önerilir. Canlı ortama geçmeden önce bağlantı güvenliğinizi gözden geçirmeniz tavsiye edilir.

Daha sonra yeni tablolar eklemek isterseniz yukarıdaki migration adımlarını tekrarlayarak yeni migrationlar oluşturabilirsiniz.


