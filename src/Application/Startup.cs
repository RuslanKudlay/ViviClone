using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Application.Server.Extentsions;
using Application.DAL;
using Application.BBL.BusinessServices;
using Application.BBLInterfaces.BusinessServicesInterfaces;
using System.Reflection;
using Application.Api.Controllers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Net;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;
using Microsoft.Extensions.PlatformAbstractions;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using Application.BBL.ModelBinders;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.AspNetCore.Identity;
using Application.EntitiesModels.Entities;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Application
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IConfiguration Configuration { get; set; }

        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            services.AddCustomDevDbContext();

            services.AddDependencies();

            services.AddCustomIdentity();

            services.AddLocalization(option =>
               option.ResourcesPath = "Resources"
            );

            services.ConfigureApplicationCookie(config =>
            {
                config.Events = new CookieAuthenticationEvents
                {
                    OnRedirectToLogin = ctx =>
                    {
                        if (ctx.Request.Path.StartsWithSegments("/api"))
                        {
                            ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        }
                        else
                        {
                            ctx.Response.Redirect(ctx.RedirectUri);
                        }
                        return Task.FromResult(0);
                    }
                };
            });

            services.AddApplicationInsightsTelemetry(Configuration);

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en"),
                    new CultureInfo("ru")
                };

                options.DefaultRequestCulture = new RequestCulture(Configuration["Localization:DefaultLanguage"], Configuration["Localization:DefaultLanguage"]);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.RequestCultureProviders = new List<IRequestCultureProvider>
                {
                    new QueryStringRequestCultureProvider(),
                    new CookieRequestCultureProvider()
                };

            });

            services.AddAuthentication();
                //.AddGoogle(googleOpt =>
                //    {
                //        var googleAuth = Configuration.GetSection("Authentication:Google");

                //        googleOpt.ClientId = googleAuth["ClientId"];
                //        googleOpt.ClientSecret = googleAuth["ClientSecret"];
                //        googleOpt.SignInScheme = IdentityConstants.ExternalScheme;
                //    })
                //.AddFacebook(facebookOpt =>
                //{
                //    var facebookAuth = Configuration.GetSection("Authentication:Facebook");

                //    facebookOpt.AppId = facebookAuth["AppId"];
                //    facebookOpt.ClientSecret = facebookAuth["AppSecret"];
                //    facebookOpt.SignInScheme = IdentityConstants.ExternalScheme;
                //});

            services.AddMvc(options =>
            {
                //options.Filters.Add(new RequireHttpsAttribute());
                options.ModelBinderProviders.Add(new SearchParamsBinderProvider());
            })
                .AddApplicationPart(typeof(AnnouncementController).Assembly)
                .AddDataAnnotationsLocalization()
                .AddViewLocalization()
                .AddJsonOptions(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Beauty Dnepr API ", Version = "v1" });
                var path = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, Configuration["Swagger:FileName"]);
                c.IncludeXmlComments(path);
                c.DescribeAllEnumsAsStrings();
            });

            services.AddDistributedMemoryCache();

            services.AddSession();

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    );
            });

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "аАбБвВгГдДеЕёЁжЖзЗиИйкКлЛмМнНоОпПрРсСтТуУфФхХцЦчЧшШщЩъыьэЭюЮяЯ " +
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 4;
                options.Password.RequireNonAlphanumeric = true;
            });
        }


        public void ConfigureDevelopment(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddSerilog();

            app.UseSerilogRequestLogging();

            app.UseDeveloperExceptionPage();

            app.UseCookiePolicy();

            app.UseCors();

            app.UseDefaultFiles();

            app.UseStaticFiles();

            app.UseSession();

            app.UseAuthentication();

            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();

            app.UseRequestLocalization(locOptions.Value);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    // Start Angular with Net Core Server
                    spa.UseAngularCliServer(npmScript: "start");
                    // Start Angular Independently
                    //spa.UseProxyToSpaDevelopmentServer("http://localhost:4200/");
                }
            });

            //app.UseSwagger();

            //app.UseSwaggerUI(c =>
            //{

            //    if(env.IsDevelopment())
            //      c.SwaggerEndpoint("/swagger/v1/swagger.json", "Jwt Security Api v1 (DEBUG)");
            //    else                 
            //      c.SwaggerEndpoint("/[]virtualDir]/swagger/v1/swagger.json", "Jwt Security Api v1 (RELEASE)");		        
            //});

        }

        public void ConfigureProductionServices(IServiceCollection services)
        {
            services.AddCustomProdDbContext();

            services.AddDependencies();

            services.AddCustomIdentity();

            services.AddLocalization(option =>
               option.ResourcesPath = "Resources"
            );

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en"),
                    new CultureInfo("ru")
                };

                options.DefaultRequestCulture = new RequestCulture(Configuration["Localization:DefaultLanguage"], Configuration["Localization:DefaultLanguage"]);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.RequestCultureProviders = new List<IRequestCultureProvider>
                {
                    new QueryStringRequestCultureProvider(),
                    new CookieRequestCultureProvider()
                };

            });

            services.ConfigureApplicationCookie(config =>
            {
                config.Events = new CookieAuthenticationEvents
                {
                    OnRedirectToLogin = ctx =>
                    {
                        if (ctx.Request.Path.StartsWithSegments("/api"))
                        {
                            ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        }
                        else
                        {
                            ctx.Response.Redirect(ctx.RedirectUri);
                        }
                        return Task.FromResult(0);
                    }
                };
            });

            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddAuthentication()
                .AddGoogle(googleOpt =>
                {
                    var googleAuth = Configuration.GetSection("Authentication:Google");

                    googleOpt.ClientId = "410624194421-k8rrimb144a2t7u3b6i8ct2jqe3ai88d.apps.googleusercontent.com";
                    googleOpt.ClientSecret = "Vxw7qGggQ_436UZT1Raj97D0";
                    googleOpt.SignInScheme = IdentityConstants.ExternalScheme;
                })
                .AddFacebook(facebookOpt =>
                {
                    var facebookAuth = Configuration.GetSection("Authentication:Facebook");

                    facebookOpt.AppId = "2632197343699224";
                    facebookOpt.ClientSecret = "f1d73224037493888ad6537838fa4559";
                    facebookOpt.SignInScheme = IdentityConstants.ExternalScheme;
                });

            services.AddMvc(options =>
            {
                //options.Filters.Add(new RequireHttpsAttribute());
            })
                .AddApplicationPart(typeof(AnnouncementController).Assembly)
                .AddDataAnnotationsLocalization()
                .AddViewLocalization()
                .AddJsonOptions(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore); ;

            services.AddSession();

            services.AddCors();

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "wwwroot/dist";
            });

            services.Configure<IISOptions>(options =>
            {
                options.ForwardClientCertificate = false;

            });
        }


        public void ConfigureProduction(IApplicationBuilder app, IHostingEnvironment env)
        {          
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseSession();

            app.UseAuthentication();

            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });          
        }
    }
}
