using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Sds.Events.WebAPI.Configs.Authentication;

public static class RegisterCors
{
    public static void AddCorsOrigin(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("Sds.Events.UI", builder =>
                builder.WithOrigins("http://localhost:4200")
                       .AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowCredentials());
        });
    }

    public static void UseCorsOrigin(this WebApplication app)
    {
        app.UseCors("Sds.Events.UI");
    }
}
