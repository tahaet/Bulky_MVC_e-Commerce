using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using BulkyBook.DataAccess;
using BulkyBook.DataAccess.DbInitializer;
using BulkyBook.DataAccess.Repository;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();   
builder.Services.AddDbContext<AppDbContext>(
                options=>options.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<IdentityUser,IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
builder.Services.AddAuthentication().AddFacebook(options =>
{
    options.AppId = "1176993723547839";
    options.AppSecret = "f001ac1bd987464f893344352cc56cfc";
});

builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/identity/account/login";
    options.LogoutPath = $"/identity/account/logut";
    options.AccessDeniedPath = $"/identity/account/accessdenied";
});
builder.Services.AddDistributedMemoryCache();
builder.Services.AddScoped<IDbInitializer,DbInitializer>();  
builder.Services.AddSession(options =>
{
    options.IdleTimeout= TimeSpan.FromMinutes(100);
    options.Cookie.HttpOnly = true; 
    options.Cookie.IsEssential = true;
});
builder.Services.AddScoped<IEmailSender, EmailSender>();        
builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
builder.Services.Configure<FormOptions>(options =>
{
	options.MultipartBodyLengthLimit = 60000000; // Adjust to the size limit you need
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();
SeedDatabase();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();

void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        dbInitializer.Initialize();
    }
}