using System.Globalization;
using ETicaretDalKatman�.Abstract;
using ETicaretDalKatman�.Concrete;
using ETicaretDataKatman�.Context;
using ETicaretDataKatman�.Identity;
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



//Identity kimlik do�rulama
builder.Services.AddIdentity<AppUser, AppRole>(options =>
{
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);//30 dakika sonra tekrar deneme hakk� verir
    options.Lockout.MaxFailedAccessAttempts = 5;//5 defa yanl�� giri� yaparsa hesab� kilitler
    options.Password.RequireDigit = true;//�ifrede rakam olmas� zorunlu
    options.Password.RequireLowercase = true;//�ifrede k���k harf olmas� zorunlu
    options.Password.RequireNonAlphanumeric = true;//�ifrede alfanumerik olmas� zorunlu
    options.Password.RequireUppercase = true;//�ifrede b�y�k harf olmas� zorunlu
}) //Kimlik do�rulama ayarlar�
    .AddEntityFrameworkStores<ETicaretContext>()//ETicaretContext veritaban� ile ili�kilendirildi
.AddDefaultTokenProviders();//Token olu�turuldu

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";//Giri� sayfas�
    options.LogoutPath = "/Account/Logout";//��k�� sayfas�
    options.AccessDeniedPath = "/Account/AccessDenied";//Eri�im engellendi sayfas�
    options.SlidingExpiration = true;//Kullan�c� belirli bir s�re i�lem yapmazsa oturumunu sonland�r�r
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);//30 dakika sonra oturumu sonland�r�r
    options.Cookie = new CookieBuilder
    {
        HttpOnly = false,//Javascript ile cookie'ye eri�ilemez hale getirir
        Name = "AspNetCoreIdentityExampleCookie", //Cookie ad�
        SameSite = SameSiteMode.Lax,//G�venlik �nlemleri al�r ayn� site �zerinden istek yap�lmas�n� sa�lar
        SecurePolicy = CookieSecurePolicy.Always //Cookie'yi sadece HTTPS �zerinden g�nderir
    };
    options.SlidingExpiration = true; //Kullan�c� belirli bir s�re i�lem yapmazsa oturumunu sonland�r�r
    options.ExpireTimeSpan = TimeSpan.FromMinutes(15); //15 dakika sonra oturumu sonland�r�r
});//Cookie ayarlar�



// Oturum 
builder.Services.AddSession();//Oturum y�netimi i�in kullan�l�r


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();//Http isteklerini https'e y�nlendirir
app.UseStaticFiles();//wwwroot klas�r�ndeki dosyalar� kullanmak i�in

app.UseRouting();//Y�nlendirme i�lemleri i�in kullan�l�r

app.UseAuthentication();//Kimlik do�rulama i�lemleri i�in kullan�l�r
app.UseAuthorization();//Kimlik do�rulama i�lemleri i�in kullan�l�r
app.UseSession();//Oturum y�netimi i�in kullan�l�r

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=List}/{id?}"); //Varsay�lan sayfa

app.Run();//Uygulamay� �al��t�r�r
