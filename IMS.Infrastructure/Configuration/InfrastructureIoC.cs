using IMS.Domain.Repository.Domain;
using IMS.Domain.Repository.Standard;
using IMS.Infrastructure.Context;
using IMS.Infrastructure.Repository.Domain;
using IMS.Infrastructure.Repository.Standard;
using IMS.Infrastructure.Repository.UnitOfWork;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System;

namespace IMS.Infrastructure.Configuration
{
    public static class InfrastructureIoC
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SqlServer");

            if (string.IsNullOrEmpty(connectionString)) throw new NullReferenceException("EF Core connection string not found");

            services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseSqlServer(connectionString, x =>
                    x.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "ims")
                        .MigrationsAssembly("IMS.Infrastructure"));
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<ITruckRepository, TruckRepository>();

            return services;
        }

        public static IApplicationBuilder ExecuteMigrations(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationContext>();

                if (context != null) context.Database.Migrate();
                else throw new NullReferenceException("Invalid context to execute migrations");
            }

            return app;
        }
    }
}
