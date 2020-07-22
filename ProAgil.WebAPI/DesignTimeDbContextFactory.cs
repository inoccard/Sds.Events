using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using ProAgil.Repository.Data;

namespace ProAgil.WebAPI
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ProAgilContext>
{
    public ProAgilContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        var builder = new DbContextOptionsBuilder<ProAgilContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        builder.UseSqlServer(connectionString);
        return new ProAgilContext(builder.Options);
    }
}
}