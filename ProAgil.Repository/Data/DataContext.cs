using Microsoft.EntityFrameworkCore;
using ProAgil.Domain.Entities;

namespace ProAgil.Repository.Data {
    public class DataContext : DbContext {
        /*public DataContext(DbContextOptions<DbContext> options) : base(options)
        {
            
        }*/

        public DataContext (DbContextOptions options) : base (options) { }

        public DbSet<Event> Events { get; set; }
        public DbSet<Speaker> Speakers { get; set; }
        public DbSet<SpeakerEvent> SpeakerEvents { get; set; }
        public DbSet<Lot> Lots { get; set; }
        public DbSet<SocialNetWork> SocialNetWorks { get; set; }

        /// <summary>
        /// Indica relacionamento de n:n
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating (ModelBuilder modelBuilder) {
            modelBuilder.Entity<SpeakerEvent> ().HasKey (PE => new { PE.EventId, PE.SpeakerId });
        }
    }
}