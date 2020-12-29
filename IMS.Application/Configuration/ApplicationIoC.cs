using IMS.Application.Interface;
using IMS.Application.Models;
using IMS.Application.Secutiry;
using IMS.Application.Service.Standard;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IMS.Application.Configuration
{
    public static class ApplicationIoC
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CryptographyConfig>(options => configuration.GetSection("Crypto").Bind(options));
            services.Configure<TemplateConfiguration>(options => configuration.GetSection("TemplateConfiguration").Bind(options));

            services.AddSingleton<ICryptography, Cryptography>();

            services.AddScoped<IValidationDictionary, ModelStateService>();

            return services;
        }
    }
}
