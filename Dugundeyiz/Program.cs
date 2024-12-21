using Dugundeyiz.Context;
using Dugundeyiz.Entity;
using Dugundeyiz.Extensions;
using Dugundeyiz.Identity;
using Dugundeyiz.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExtensions();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
        policy.RequireRole("Admin"));
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/"; // Yetkisiz eri�imde y�nlendirilecek adres (ana sayfa)
});


builder.Services.AddDbContext<DugundeyizContext>(context => context.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ProductService>();
builder.Services.AddAuthentication("Cookies")
    .AddCookie(options =>
    {
        options.LoginPath = "/"; // Giri� yap�lmam��sa do�rudan anasayfa
        options.AccessDeniedPath = "/"; // Yetkisiz eri�imde de anasayfa
    });

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<AppUser>>();  // AppUser kullan�yoruz
    var roleManager = services.GetRequiredService<RoleManager<AppRole>>();  // AppRole kullan�yoruz

    // Kullan�c�lara rollerin atanmas� i�lemi
}



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication(); // Kimlik do�rulama middleware
app.UseAuthorization();  // Yetkilendirme middleware (Do�ru s�rada)

app.UseStatusCodePages(async context =>
{
    if (context.HttpContext.Response.StatusCode == 403)
    {
        context.HttpContext.Response.Redirect("/");
    }
});




app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

// Default route (Ana site i�in)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
