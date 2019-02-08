using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorCore.Infrastructure.Data;
using BlazorCore.Core.Models;
using BlazorCore.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BlazorCore.Server
{
    public class Startup
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfiguration _config;

        public Startup(IHostingEnvironment env, IConfiguration config)
        {
            _env = env;
            _config = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            if (_env.IsDevelopment())
            {
                services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(
                        _config.GetConnectionString("DevConnection")));
            }
            else
            {
                services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(
                        _config.GetConnectionString("ReleaseConnection")));

                //services.AddHttpsRedirection(options =>
                //{
                //    options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
                //    options.HttpsPort = 443;
                //});
            }

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            // Configure Identity
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                options.User.RequireUniqueEmail = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromHours(24);

                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            //services.Configure<StripeSettings>(_config.GetSection("Stripe"));

            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                     .RequireAuthenticatedUser()
                     .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            }).AddNewtonsoftJson();

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy(ConstantStrings.CreatePermissionName, policy =>
            //        policy.Requirements.Add(new CreatePermission()));

            //});
            //services.AddScoped<IUserData, UserData>();
            //services.AddScoped<IAuthorizationHandler, SalesRecordHandler>();
            //services.AddScoped<IAuthorizationHandler, EscalationRecordHandler>();
            //services.AddScoped<IAuthorizationHandler, DisconnectRecordHandler>();
            //services.AddScoped<IAuthorizationHandler, CreateHandler>();

            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

            //services.AddAuthentication().AddFacebook(facebookOptions =>
            //{
            //    facebookOptions.AppId = "558371061196915";
            //    facebookOptions.AppSecret = "958ab8200b41649130fd7a74dc1264b9";
            //});

            //services.AddAuthentication().AddGoogle(googleOptions =>
            //{
            //    googleOptions.ClientId = "226283670042-jb4t0a2ksf12tq3tpvk285vrfvvrniu4.apps.googleusercontent.com";
            //    googleOptions.ClientSecret = "Z1usGDLXwmIkLJY-lmAE-EYa";
            //});

            services.AddSingleton<IEmailSender, EmailSender>();
            //services.Configure<AuthMessageSenderOptions>(Configuration);

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
            });

            services.AddTransient<DbInitializer>();

            services.AddRazorComponents<App.Startup>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting(routes =>
            {
                routes.MapApplication();
            });

            app.UseCookiePolicy();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseRazorComponents<App.Startup>();
        }
    }
}
