using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace IMS.Application.Mapper
{
    public static class AutoMapperConfiguration
    {
        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new DomainDtoProfile());
                mc.AddProfile(new DtoDomainProfile());
            });

            var mapper = mappingConfig.CreateMapper();

            services.AddSingleton(mapper);

            return services;
        }
    }
}
