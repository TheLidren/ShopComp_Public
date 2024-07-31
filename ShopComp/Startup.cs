using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShopComp.Data;
using ShopComp.Models;
using System;
using System.Security.Claims;

namespace ShopComp
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDBContent>(options => options.UseMySql(Configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion(new Version(8, 0, 11))));
            services.AddIdentity<User, IdentityRole>(opts =>
            {
                opts.Password.RequiredLength = 8;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = true;
                opts.Password.RequireUppercase = true;
                opts.Password.RequireDigit = true;
            })
                .AddEntityFrameworkStores<AppDBContent>()
                .AddDefaultTokenProviders();
            services.AddControllersWithViews();
            services.AddMvc()
                .AddSessionStateTempDataProvider();
            services.AddRouting();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Comps/List");
                });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", builder =>
                {
                    builder.RequireClaim(ClaimTypes.Role, "Admin");
                });
                options.AddPolicy("User", builder =>
                {
                    builder.RequireClaim(ClaimTypes.Role, "User");
                });
            });
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Tovars}/{action=List}/{id?}");
            });
        }
    }
}
