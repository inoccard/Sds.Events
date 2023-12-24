using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Sds.Events.Domain.Identity;
using Sds.Events.Repository.Data;

namespace Sds.Events.WebAPI.Configs.App;

public static class RegisterAppServicesConfig
{
    public static void AddAppServices(this IServiceCollection services)
    {
        /// <summary>
        /// Injeção de Dependência do EventsRepository
        /// </summary>
        /// <returns></returns>
        services.AddScoped<IEventsRepository, EventsRepository>();

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    }
}