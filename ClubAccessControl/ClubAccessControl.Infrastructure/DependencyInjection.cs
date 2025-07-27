using Microsoft.Extensions.DependencyInjection;
using ClubAccessControl.Application.Interfaces;
using ClubAccessControl.Application.Services;
using ClubAccessControl.Domain.Interfaces;
using ClubAccessControl.Infrastructure.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ClubAccessControl.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IAreaRepository, AreaRepository>();
            services.AddScoped<IPlanoRepository, PlanoRepository>();
            services.AddScoped<ISocioRepository, SocioRepository>();
            services.AddScoped<IAcessoRepository, AcessoRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IAreaService, AreaService>();
            services.AddScoped<IPlanoService, PlanoService>();
            services.AddScoped<ISocioService, SocioService>();
            services.AddScoped<IAcessoService, AcessoService>();

            return services;
        }

        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("ClubeDatabase");
            var connection = new SqliteConnection(connectionString);
            connection.Open();

            services.AddDbContext<ClubeContext>(options =>
                options.UseSqlite(connection));

            return services;
        }
    }
}
