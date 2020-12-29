using AutoMapper;

using IMS.Api.Information.Infrastructure.Mapper;
using IMS.Api.Information.Infrastructure.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;

namespace IMS.Api.Information.Infrastructure
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddSwaggerServices(this IServiceCollection services, IConfiguration Configuration)
        {
            var assemblyVersion = typeof(Startup).Assembly.GetName().Version.ToString();
            var swaggerApiName = Configuration.GetValue<string>("SwaggerApiName");
            var swaggerApiVersion = "v1";

            services.AddSwaggerGen(options =>
            {
                options
                    .SwaggerDoc(name: swaggerApiVersion, new OpenApiInfo
                    {
                        Title = swaggerApiName,
                        Version = swaggerApiVersion,
                        Description = $"Build: {assemblyVersion}"
                    });

                options.OperationFilter<AddHeaderLocaleParameter>();

                var assemblyName = Assembly.GetEntryAssembly().GetName().Name;
                var fileName = Path.GetFileName($"{assemblyName}.xml");
                var basePath = AppContext.BaseDirectory;

                //options.IncludeXmlComments(Path.Combine(basePath, fileName));
                
            });

            return services;
        }

        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new DomainViewModelProfile());
                mc.AddProfile(new ViewModelDomainProfile());
            });

            var mapper = mappingConfig.CreateMapper();

            services.AddSingleton(mapper);

            return services;
        }

        public static IServiceCollection AddSupportedCultures(this IServiceCollection services, IConfiguration Configuration)
        {
            var supportedCultures = new List<CultureInfo>();
            Configuration.GetSection("SupportedCultures").Bind(supportedCultures);

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(supportedCultures.First());
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            return services;
        }

        public static void UseGlobalExceptionHandler(this IApplicationBuilder app, ILogger logger)
        {
            app.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                {
                    var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (exceptionHandlerFeature == null) return;

                    logger.LogError($"{exceptionHandlerFeature.Error}");

                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var json = new
                    {
                        context.Response.StatusCode,
                        Message = "An error occurred while processing your request",
                        Details = exceptionHandlerFeature.Error.Message
                    };

                    await context.Response.WriteAsync(JsonConvert.SerializeObject(json));
                });
            });
        }
    }
}
