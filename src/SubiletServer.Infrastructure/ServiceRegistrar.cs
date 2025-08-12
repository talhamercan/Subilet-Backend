using GenericRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using SubiletServer.Infrastructure.Context;

namespace SubiletServer.Infrastructure
{
    public static class ServiceRegistrar
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();

            string? con = configuration.GetConnectionString("SqlServer");
            if (string.IsNullOrEmpty(con))
                throw new InvalidOperationException("Connection string 'SqlServer' not found."); 

            services.AddDbContext<ApplicationDbContext>(opt =>
            {
                opt.UseSqlServer(con, sqlOptions => 
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(60),
                        errorNumbersToAdd: null);
                    sqlOptions.CommandTimeout(120);
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorNumbersToAdd: null);
                });
            });

            services.AddScoped<IUnitOfWork>(srv => srv.GetRequiredService<ApplicationDbContext>());

            services.Scan(action => action
     // ServiceRegistrar sınıfının bulunduğu assembly'deki tüm tipleri tarar
     .FromAssemblies(typeof(ServiceRegistrar).Assembly)
     // Tüm sınıfları seçer (public olanlar ve olmayanlar dahil)
     .AddClasses(publicOnly: false)
     // Çakışma durumunda kaydı atlar (mevcut bir kayıt varsa üzerine yazmaz)
     .UsingRegistrationStrategy(RegistrationStrategy.Skip)
     // Sınıfları, implemente ettikleri interfaceleri üzerinden kaydeder
     .AsImplementedInterfaces()
     // Servislerin ömürlerini Scoped olarak ayarlar (her request için bir instance)
     .WithScopedLifetime()
 );

            return services;
        }
    }
}


