using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OnLineBookStore.Data;
using OnLineBookStore.Models;
using Microsoft.AspNetCore.Identity;
namespace OnLineBookStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<OnLineBookStoreContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("OnLineBookStoreContext") ?? throw new InvalidOperationException("Connection string 'OnLineBookStoreContext' not found.")));
            builder.Services.AddRazorPages();
            builder.Services.AddDefaultIdentity<AppUser>().AddRoles<IdentityRole>().AddEntityFrameworkStores<OnLineBookStoreContext>();
            //builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<OnLineBookStoreContext>();

            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddScoped<Cart>(sp=> Cart.GetCart(sp));
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.IdleTimeout = TimeSpan.FromSeconds(10);
            });
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();
            // Seed the database with initial data
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<OnLineBookStoreContext>();
                UserRoleInitializer.InitializeAsync(services).Wait();
                //SeedData.Initialize(services); // Call the seed data method
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
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Store}/{action=Index}/{id?}");
            app.MapRazorPages();
            app.Run();
        }
    }
}
