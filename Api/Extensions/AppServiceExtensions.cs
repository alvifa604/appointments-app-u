using Application.Core;
using Application.Operations.Roles;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace Api.Extensions
{
    public static class AppServiceExtensions
    {

        public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration config)
        {
            //Agrega el contexto para manejar la base de datos
            services.AddDbContext<AppDbContext>(o =>
                o.UseNpgsql(config.GetConnectionString("DefaultConnection"))
            );

            //Aggrega el patrón mediator 
            services.AddMediatR(typeof(Create.Handler).Assembly);

            //Agrega automapper 
            services.AddAutoMapper(typeof(MappingProfiles).Assembly);
            
            return services;
        }
    }
}