using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using StorageVangers.Api.Data;
using StorageVangers.Api.Services;

namespace StorageVangers.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            services.AddHttpContextAccessor();

            var pgsqlConnectionString = Configuration["PostgreSql:ConnectionString"];
            var pgsqlDbPassword = Configuration["PostgreSql:DbPassword"];

            var builder = new NpgsqlConnectionStringBuilder(pgsqlConnectionString)
            {
                Password = pgsqlDbPassword
            };

            // Configures Ef Core
            services.AddDbContext<AppDbContext>(
                options => options.UseNpgsql(builder.ConnectionString)
            );

            // Add Identitty
            services.AddIdentity<AppUser, IdentityRole>()
            // Configure EF for Identity
            .AddEntityFrameworkStores<AppDbContext>()
            // To handle token generation for things like confirmation, forgot pass, etc.
            .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })
            .AddGoogle("Google", options =>
            {
                var googleAuthSection = Configuration.GetSection("Authentication:Google");
                options.ClientId = googleAuthSection["ClientId"];
                options.ClientSecret = googleAuthSection["ClientSecret"];
                options.Scope.Add("https://www.googleapis.com/auth/drive");
                options.Scope.Add("https://www.googleapis.com/auth/userinfo.profile");
                options.SaveTokens = true;
            });

            // Configures Mvc Services
            services.AddControllers().SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.AddScoped<IStorageService, StorageService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseHttpsRedirection();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.Use((context, next) =>
            {
                if (context.Request.Headers["x-forwarded-proto"] == "https" || context.Request.Headers["x-forwarded-for"] == "https")
                {
                    context.Request.Scheme = "https";
                }

                return next();
            });

            app.UseFileServer();

            app.UseRouting();

            //app.UseResponseCaching();

            //app.Use(async (context, next) =>
            //{
            //    context.Response.GetTypedHeaders().CacheControl =
            //        new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
            //        {
            //            Public = true,
            //            MaxAge = TimeSpan.FromSeconds(30)
            //        };
            //    context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] =
            //        new string[] { "Accept-Encoding" };

            //    await next();
            //});

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All,
                RequireHeaderSymmetry = false,
                ForwardLimit = null,
                KnownNetworks = { new IPNetwork(IPAddress.Parse($"::ffff:{Configuration["HostIP"]}"), 104) }
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("Default", "api/{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
