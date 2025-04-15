using E_CommerceFIdentityScaff.Data;
using E_CommerceFIdentityScaff.Models;
using E_CommerceFIdentityScaff.Repository.IRepository;
using E_CommerceFIdentityScaff.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Google;
using E_CommerceFIdentityScaff.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace E_CommerceFIdentityScaff
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages(); // For scaffolded Identity

            // Database configuration
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();


            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";


            });



            builder.Services.AddAuthentication()
             .AddGoogle(googleOptions =>
             {
                 googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                 googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
             });

            // Other services
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            //builder.Services.AddScoped<IDbInitilazier, DbInitilazier>();

            builder.Services.AddSingleton<IEmailSender, DummyEmailSender>();


            var app = builder.Build();

            //using (var scope = app.Services.CreateScope())
            //{
            //    var dbInit = scope.ServiceProvider.GetRequiredService<IDbInitilazier>();
            //    await dbInit.Initialize();
            //}


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();

            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}