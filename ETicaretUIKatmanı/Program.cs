using System.Globalization;
using ETicaretDalKatmaný.Abstract;
using ETicaretDalKatmaný.Concrete;
using ETicaretDataKatmaný.Context;
using ETicaretDataKatmaný.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Dependency Injection
builder.Services.AddDbContext<ETicaretContext>();
builder.Services.AddScoped<IProductDal, ProductDal>();
builder.Services.AddScoped<IOrderDal, OrderDal>();
builder.Services.AddScoped<IOrderLineDal, OrderLineDal>();
builder.Services.AddScoped<ICategoryDal, CategoryDal>();



//Identity kimlik doðrulama
builder.Services.AddIdentity<AppUser, AppRole>(options =>
{
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);//30 dakika sonra tekrar deneme hakký verir
    options.Lockout.MaxFailedAccessAttempts = 5;//5 defa yanlýþ giriþ yaparsa hesabý kilitler
    options.Password.RequireDigit = true;//Þifrede rakam olmasý zorunlu
    options.Password.RequireLowercase = true;//Þifrede küçük harf olmasý zorunlu
    options.Password.RequireNonAlphanumeric = true;//Þifrede alfanumerik olmasý zorunlu
    options.Password.RequireUppercase = true;//Þifrede büyük harf olmasý zorunlu
}) //Kimlik doðrulama ayarlarý
    .AddEntityFrameworkStores<ETicaretContext>()//ETicaretContext veritabaný ile iliþkilendirildi
.AddDefaultTokenProviders();//Token oluþturuldu

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";//Giriþ sayfasý
    options.LogoutPath = "/Account/Logout";//Çýkýþ sayfasý
    options.AccessDeniedPath = "/Account/AccessDenied";//Eriþim engellendi sayfasý
    options.SlidingExpiration = true;//Kullanýcý belirli bir süre iþlem yapmazsa oturumunu sonlandýrýr
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);//30 dakika sonra oturumu sonlandýrýr
    options.Cookie = new CookieBuilder
    {
        HttpOnly = false,//Javascript ile cookie'ye eriþilemez hale getirir
        Name = "AspNetCoreIdentityExampleCookie", //Cookie adý
        SameSite = SameSiteMode.Lax,//Güvenlik önlemleri alýr ayný site üzerinden istek yapýlmasýný saðlar
        SecurePolicy = CookieSecurePolicy.Always //Cookie'yi sadece HTTPS üzerinden gönderir
    };
    options.SlidingExpiration = true; //Kullanýcý belirli bir süre iþlem yapmazsa oturumunu sonlandýrýr
    options.ExpireTimeSpan = TimeSpan.FromMinutes(15); //15 dakika sonra oturumu sonlandýrýr
});//Cookie ayarlarý



// Oturum 
builder.Services.AddSession();//Oturum yönetimi için kullanýlýr


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();//Http isteklerini https'e yönlendirir
app.UseStaticFiles();//wwwroot klasöründeki dosyalarý kullanmak için

app.UseRouting();//Yönlendirme iþlemleri için kullanýlýr

app.UseAuthentication();//Kimlik doðrulama iþlemleri için kullanýlýr
app.UseAuthorization();//Kimlik doðrulama iþlemleri için kullanýlýr
app.UseSession();//Oturum yönetimi için kullanýlýr

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=List}/{id?}"); //Varsayýlan sayfa

app.Run();//Uygulamayý çalýþtýrýr
