namespace WCR.Web
{
    using System;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using WCR.Data;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using WCR.Models;
    using System.IO;
    using WCR.Web.Common;
    using AutoMapper;
    using WCR.Services.Moderation;
    using WCR.Services.Moderation.Interfaces;
    using WCR.Services.Administration.Interfaces;
    using WCR.Services.Administration;
    using WCR.Services.Competition.Interfaces;
    using WCR.Services.Competition;
    using WCR.Services.Statistics;
    using WCR.Services.Statistics.Interfaces;
    using WCR.Web.Extensions;

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var appLocalFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), env.ApplicationName);
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile($@"{appLocalFolder}\customSettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<WCRDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("WCRConnection")));

            //services.AddDefaultIdentity<User>()
            //    .AddRoles<IdentityRole>()
            //    .AddEntityFrameworkStores<WCRDbContext>();

            services
                .AddIdentity<User, IdentityRole>()
                .AddDefaultUI()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<WCRDbContext>();

            services.Configure<IdentityOptions>(options =>
            {
                options.User = new UserOptions()
                {
                    AllowedUserNameCharacters = null,
                    RequireUniqueEmail = true
                };

                options.Password = new PasswordOptions()
                {
                    RequiredLength = 3,
                    RequireNonAlphanumeric = false,
                    RequireLowercase = false,
                    RequireUppercase = false
                };
            });

            var fbAppId = this.Configuration.GetSection("ExternalAuthentication:Facebook:AppId").Value;
            var fbAppSecret = this.Configuration.GetSection("ExternalAuthentication:Facebook:AppSecret").Value;
            if (fbAppId != null && fbAppSecret != null)
            {
                services.AddAuthentication()
                .AddFacebook(options =>
                {
                    options.AppId = fbAppId;
                    options.AppSecret = fbAppSecret;
                });
            }

            //services.Configure<FacebookOptions>(options => Configuration.GetSection("ExternalAuthentication:Facebook").Bind(options));

            services.AddAutoMapper();

            RegisterServiceLayer(services);

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider,
            RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();

            app.SeedRoles(this.Configuration, roleManager);

            app.ConfigureExceptionHandler();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "area",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

        }

        private static void RegisterServiceLayer(IServiceCollection services)
        {
            services.AddScoped<IModerationService, ModerationService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IRoundService, RoundService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IBetService, BetService>();
            services.AddScoped<IStatisticsService, StatisticsService>();
        }
    }
}
