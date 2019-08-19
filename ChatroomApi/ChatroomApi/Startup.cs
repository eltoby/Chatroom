namespace ChatroomApi
{
    using System.Text;
    using ChatroomApi.Domain;
    using ChatroomApi.Service;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            this.Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowAnyOrigin();
            }));

            services.Configure<AppSettings>(this.Configuration.GetSection("AppSettings"));
            var sp = services.BuildServiceProvider();
            var appSettings = sp.GetService<IOptions<AppSettings>>();
            var baseUrl = appSettings.Value.BaseUrl;

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = baseUrl,
                        ValidAudience = baseUrl,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"))
                    };
                });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSignalR();

            services.Scan(scan =>
                scan.FromCallingAssembly()
                .AddClasses()
                .AsMatchingInterface());

            services.Scan(scan =>
                scan.FromAssembliesOf(typeof(IUserService))
                .AddClasses()
                .AsMatchingInterface());
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IMessageQueueHandler mqHandler)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors("CorsPolicy");

            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseMvc();

            app.UseSignalR(routes =>
            {
                routes.MapHub<ChatHub>("/chat");
            });

            mqHandler.Launch();
        }
    }
}
