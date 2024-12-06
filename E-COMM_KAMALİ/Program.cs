using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ECOMM.Business.Configuration;
using ECOMM.Core.Models;
using ECOMM.Data.Shared.Abstract;
using ECOMM.Data.Shared.Concrete;
using ECOMM.Data;
using ECOMM.Business.Abstract;
using ECOMM.Business.Concrete;
using Microsoft.AspNetCore.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Repositories için hizmet kaydı
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// HTTP context erişimi için
builder.Services.AddHttpContextAccessor();

// Controller ve View'lar için gerekli hizmetlerin kaydı
builder.Services.AddControllersWithViews();

// Cookie ayarları
builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/account/login";
    options.LoginPath = "/account/login";
});

// Veritabanı bağlantısı için SQL Server kullanımı
builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    sqlOptions => sqlOptions.EnableRetryOnFailure()));

// Identity yapılandırması
builder.Services.AddIdentity<User, ApplicationRole>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedPhoneNumber = false;
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDBContext>()
.AddDefaultTokenProviders();

// RoleManager ve RoleService için hizmet kaydı
builder.Services.AddScoped<RoleManager<ApplicationRole>>();
builder.Services.AddScoped<IRoleService, RoleService>();

// Business ve Repository DI işlemleri
builder.Services.BusinessDI();
builder.Services.RepositoryDI();

//// E-posta servisi kaydı
//builder.Services.AddScoped<IEmailService, SmtpEmailService>();


// Oturum (Session) yapılandırması
builder.Services.AddDistributedMemoryCache(); // Oturum verilerini saklamak için bellek önbelleği ekleyin
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Oturum zaman aşımını ayarlayın
    options.Cookie.HttpOnly = true; // Çerezlerin sadece HTTP üzerinden erişilebilir olmasını sağlayın
    options.Cookie.IsEssential = true; // Çerezlerin gerekli olduğunu belirtin
});

var app = builder.Build();

// Hata ayıklama için geliştirici modunda olduğundan emin ol
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Geliştirici hata sayfasını etkinleştir
}
else
{
    app.UseExceptionHandler("/Home/Error"); // Hata sayfası için yönlendirme
    app.UseHsts(); // HSTS etkinleştir
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession(); // Oturum middleware'ini ekleyin

// Yönlendirme ayarları
app.MapControllerRoute(
    name: "PostDetail",
    pattern: "postdetail/{category}/{tag}/{id}",
    defaults: new { controller = "PostDetail", action = "Index" }
);

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

// Varsayılan yönlendirme
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
