using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sds.Events.Repository.Data;

namespace Sds.Events.WebAPI.Configs.App;

public static class RegisterMigration
{
    public static void AddMigration(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            // Obtém o DbContext necessário para realizar a migração
            var dbContext = services.GetRequiredService<EventsContext>();

            // Aplica as migrações do banco de dados
            dbContext.Database.Migrate();
        }
    }
}
