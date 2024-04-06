using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ecom.Data;
using ecom.Models;
using ecom.Work.Managers;
using ecom.Work.Repository;
using MoneStore.Work.Repository;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new Microsoft.AspNetCore.Mvc.AutoValidateAntiforgeryTokenAttribute());
});

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();


builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepositoryImpl>();
builder.Services.AddTransient<ICategoryRepository, CategoryRepositoryImpl>();

builder.Services.AddScoped<IProductRepository, ProductRepositoryImpl>();
builder.Services.AddTransient<IProductRepository, ProductRepositoryImpl>();

builder.Services.AddScoped<IOrderRepository, OrderRepositoryImpl>();
builder.Services.AddTransient<IOrderRepository, OrderRepositoryImpl>();

builder.Services.AddScoped<IContactUsRepository, ContactMessageRepositoryImpl>();

builder.Services.AddScoped<CategoryManager>();
builder.Services.AddScoped<ProductManager>();
builder.Services.AddScoped<ShoppingCart>();
builder.Services.AddScoped<OrderManager>();
builder.Services.AddScoped<ContactUsManager>();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set a long timeout for testing purposes
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true; // Make the session cookie essential
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.Use(async (context, next) =>
{
    context.Response.Headers.Add(
        "Content-Security-Policy",
        "default-src 'self';" +
        "script-src 'self';" +
        "style-src 'self' https://use.fontawesome.com;" + // Allow styles from Font Awesome
        "img-src 'self' data:;" + // Allow images and base64 encoded images
        "font-src 'self' https://use.fontawesome.com;" + // Allow fonts from Font Awesome
        "object-src 'none';"
    );
    await next();
});

app.UseSession(); // Add this line before UseRouting()
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

