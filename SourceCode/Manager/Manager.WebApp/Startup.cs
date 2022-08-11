using Autofac;
using Manager.SharedLibs;
using Manager.WebApp.AutofacDI;
using Manager.WebApp.Connection;
using Manager.WebApp.Hubs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using StackExchange.Redis.Extensions.Core.Abstractions;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Newtonsoft;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Manager.WebApp
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public static IContainer IocContainer { get; set; }
        public static IRedisCacheClient RedisCacheClient;

        public Startup(IConfiguration configuration)
        {
            //Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
            services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("http://localhost:3000")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                });
            });

            services.AddSingleton<IDictionary<string, UserConnection>>(opts => new Dictionary<string, UserConnection>());
            

            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddSingleton<Microsoft.AspNetCore.Http.IHttpContextAccessor, Microsoft.AspNetCore.Http.HttpContextAccessor>();

            // Add Autofac
            var autofacBuilder = new ContainerBuilder();
            autofacBuilder.RegisterModule<SqlModule>();
            autofacBuilder.RegisterModule<ServiceModule>();
            IocContainer = autofacBuilder.Build();


            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
                options =>
                {
                    options.LoginPath = new PathString("/Account/Login");
                    options.AccessDeniedPath = new PathString("/Error/Index");
                });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = AppConfiguration.GetAppsetting("Jwt:Audience"),
                    ValidIssuer = AppConfiguration.GetAppsetting("Jwt:Issuer"),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppConfiguration.GetAppsetting("Jwt:Key")))
                };
            });


            //services.ConfigureApplicationCookie(options =>
            //{
            //    options.LoginPath = $"/Account/Login";
            //    options.LogoutPath = $"/Account/Logout";
            //    options.AccessDeniedPath = $"/Account/AccessDenied";
            //});

            //We need some packages for redis cache
            //StackExchange.Redis.Extensions.Core
            //StackExchange.Redis.Extensions.AspNetCore
            //StackExchange.Redis.Extensions.Newtonsoft

            var redisConfiguration = Configuration.GetSection("Redis").Get<RedisConfiguration>();
            services.AddStackExchangeRedisExtensions<NewtonsoftSerializer>(redisConfiguration);

            services.AddControllers().AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IRedisCacheClient redisClient)
        {
            System.Web.HttpContext.Configure(app.ApplicationServices.
                GetRequiredService<Microsoft.AspNetCore.Http.IHttpContextAccessor>()
            );

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Index");
            }


            var idProvider = new CustomUserIdProvider();

            Microsoft.AspNet.SignalR.GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => idProvider);

            


            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStatusCodePagesWithRedirects("/Error/Index?StatusCode={0}");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapHub<ChatHub>("/chat");
            });

            RedisCacheClient = redisClient;
        }
    }   

    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }

    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);                
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                switch (error)
                {
                    //case AppException e:
                    //    // custom application error
                    //    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    //    break;
                    //case KeyNotFoundException e:
                    //    // not found error
                    //    response.StatusCode = (int)HttpStatusCode.NotFound;
                    //    break;
                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var result = JsonSerializer.Serialize(new { message = error?.Message });
                await response.WriteAsync(result);
            }
        }
    }
}
