using IMS.Api.Information.Infrastructure;
using IMS.Api.Information.StartupConfig;
using IMS.Application.Configuration;
using IMS.Infrastructure.Configuration;
using IMS.Localization.Json;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Swashbuckle.AspNetCore.SwaggerUI;

using System.Text.Json;

namespace IMS.Api.Information
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        private readonly string AllowedOrigins = "AllowedOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            var allowedOriginsConfig = Configuration.GetSection(AllowedOrigins).Get<string[]>();

            services.AddCors(options =>
            {
                options.AddPolicy(AllowedOrigins,
                builder =>
                {
                    builder
                        .WithOrigins(allowedOriginsConfig)
                        .WithExposedHeaders("Content-Disposition")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
            services.AddAutoMapper();
            services.AddSecurity(Configuration);
            services.AddSwaggerServices(Configuration);
            services.AddInfrastructure(Configuration);
            services.AddApplication(Configuration);
            services.AddSupportedCultures(Configuration);
            services.AddJsonLocalization(options => options.ResourcesPath = "Resources");

            services
                .AddControllers()
                .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                .AddDataAnnotationsLocalization()
                .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);
        }

        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            ILogger<Startup> loggerFactory,
            IOptions<RequestLocalizationOptions> requestLocalizationOptions)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.ExecuteMigrations();
            app.UseGlobalExceptionHandler(loggerFactory);
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseSecurity();

            app.UseCors(AllowedOrigins);
            app.UseRequestLocalization(requestLocalizationOptions.Value);

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                var swaggerApiName = Configuration.GetValue<string>("SwaggerApiName");

                c.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: swaggerApiName);
                c.DocExpansion(DocExpansion.None);
            });

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
