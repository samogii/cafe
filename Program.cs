using Cafe.Data;
using Cafe.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var services = builder.Services;
var env = builder.Environment;
var config = builder.Configuration;
var local = builder.Configuration.GetConnectionString("localsql");
    
services.AddDbContext<DataContext>(x => x.UseSqlServer(local));

services.AddTransient<IJwtUtils, JwtUtils>();
services.AddScoped<Cafe.Models.User>();

services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
{
    option.Cookie.Name = "AuthToken";
    option.LoginPath = "/users/login";
    option.AccessDeniedPath = "/users/login";
});


//var serviceProvider = new ServiceCollection()
//                .AddDbContext<DataContext>(options =>
//                    options.UseSqlServer(local))
//                .BuildServiceProvider();

//// Get an instance of your DbContext from the service provider
//using (var scope = serviceProvider.CreateScope())
//{
//    var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();

//    try
//    {
//        // Apply pending migrations
//        dbContext.Database.Migrate();
//    }
//    catch (Exception ex)
//    {
//        // Handle migration errors
//        Console.WriteLine($"Error applying migrations: {ex.Message}");
//    }
//}

services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/404");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}
//async void Configure(IApplicationBuilder app)
//    {
//        // Other configurations...

//        await CreateRoles(app.ApplicationServices);

//        // Other configurations...
//    }
//app.UseExceptionHandler("/Home/404");

//app.UseStatusCodePagesWithReExecute("/Home/404", "?statusCode={0}");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

app.UseMiddleware<JwtMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

