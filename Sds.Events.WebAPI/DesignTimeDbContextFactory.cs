using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Sds.Events.Repository.Data;
using System.IO;

namespace Sds.Events.WebAPI
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<EventsContext>
    {
        public EventsContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<EventsContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseSqlServer(connectionString);
            return new EventsContext(builder.Options);
        }
    }
}