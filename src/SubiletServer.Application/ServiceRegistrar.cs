using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MediatR;

namespace SubiletServer.Application
{
    public static class ServiceRegistrar
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfr =>
            {
                cfr.RegisterServicesFromAssembly(typeof(ServiceRegistrar).Assembly);
                cfr.AddOpenBehavior(typeof(Behaviors.ValidationBehavior<,>));
                cfr.AddOpenBehavior(typeof(Behaviors.PermissionBehavior<,>));
            });



            services.AddValidatorsFromAssembly(typeof(ServiceRegistrar).Assembly);

            return services;

        }
    }
}
