using MVC_ONION_PROJECT.INFRASTRUCTURE.EXTENSION;
using MVC_ONION_PROJECT.APPLICATION.Extension;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Extension;
using Microsoft.AspNetCore.Identity;
using MVC_ONION_PROJECT.INFRASTRUCTURE.APPCONTEXT;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddInfrastructureService(builder.Configuration);
builder.Services.AddPresentationService();
builder.Services.AddApplicationService();
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppDBContext>().AddDefaultTokenProviders();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("EmailConfirmationPolicy", policy =>
        policy.RequireAuthenticatedUser()
              .RequireClaim("IsEmailConfirmed", "false"));
});
builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
{
    options.TokenLifespan = TimeSpan.FromMinutes(30);
});

builder.Services.AddRazorPages();
builder.Services.AddSession();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();


// Türkiye kültürünü oluşturun
var turkishCulture = new CultureInfo("tr-TR");

var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(turkishCulture),
    SupportedCultures = new[] { turkishCulture },
    SupportedUICultures = new[] { turkishCulture }
};

app.UseRequestLocalization(localizationOptions);


app.UseAuthorization();
//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

//    //'APPUSER' rolünün varlığını kontrol edin ve oluşturun
//    if (!roleManager.RoleExistsAsync("Employee").Result)
//    {
//        var role = new IdentityRole("Employee");
//        roleManager.CreateAsync(role).Wait();
//    }
//    if (!roleManager.RoleExistsAsync("OrganizationAdmin").Result)
//    {
//        var role = new IdentityRole("OrganizationAdmin");
//        roleManager.CreateAsync(role).Wait();
//    }
//    if (!roleManager.RoleExistsAsync("SuperAdmin").Result)
//    {
//        var role = new IdentityRole("SuperAdmin");
//        roleManager.CreateAsync(role).Wait();
//    }
//}
app.UseSession();
//app.UseAuthentication();
app.MapControllerRoute(
          name: "areas",
          pattern: "{area:exists}/{controller=account}/{action=login}/{id?}");
app.MapControllerRoute(name:"default" ,
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();




