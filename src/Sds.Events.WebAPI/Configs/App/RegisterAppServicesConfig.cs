using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Sds.Events.Domain.Core;
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
        services.AddScoped<INotifierMessage, NotifierMessage>();

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    }
}