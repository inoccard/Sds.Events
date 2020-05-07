using Microsoft.EntityFrameworkCore;
using ProAgil.Domain.Entities;

namespace ProAgil.WebAPI.Data
{
    public class DataContext : DbContext
    {
        /*public DataContext(DbContextOptions<DbContext> options) : base(options)
        {
            
        }*/

        public DataContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }
    }
}