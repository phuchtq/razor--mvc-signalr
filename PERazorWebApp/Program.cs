using Microsoft.AspNetCore.Authentication.Cookies;
using PERazorWebApp.Hubs;
using Service;

namespace PERazorWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();

            builder.Services.AddSignalR();

            builder.Services.AddScoped<IStoreAccountService, StoreAccountService>();
            builder.Services.AddScoped<IManufacturerService, ManufacturerService>();
            builder.Services.AddScoped<IMedicineInformationService, MedicineInformationService>();

            builder.Services.AddAuthentication()
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.LoginPath = new PathString("/StoreAccount/Login");
        options.AccessDeniedPath = new PathString("/StoreAccount/Forbidden");
        options.LogoutPath = "/StoreAccount/Logout"; // Trang logout
        options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
    });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapHub<ChatHub>("chatHub");

            app.MapRazorPages();

            app.Run();
        }
    }
}
