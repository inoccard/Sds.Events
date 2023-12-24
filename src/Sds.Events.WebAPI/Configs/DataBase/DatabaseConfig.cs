using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sds.Events.Repository.Data;

namespace Sds.Events.WebAPI.Configs.DataBase;

public static class DatabaseConfig
{
    public static void AddDatabaseConfig(this WebApplicationBuilder builder)
    {
            builder.Services
                   .AddDbContext<EventsContext>(d => 
                   d.UseSqlServer(builder.Configuration
                   .GetConnectionString("DefaultConnection")
                   ));
    }
}
