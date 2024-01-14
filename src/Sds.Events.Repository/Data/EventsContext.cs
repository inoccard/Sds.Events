using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sds.Events.Domain.Entities;

/// <summary>
/// Entities
/// </summary>
using Sds.Events.Domain.Identity;

/// <summary>
/// Conext DB Class
/// </summary>
namespace Sds.Events.Repository.Data
{
    public class EventsContext : IdentityDbContext<User, Role, int, IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public EventsContext(DbContextOptions<EventsContext> options) : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Speaker> Speakers { get; set; }
        public DbSet<SpeakerEvent> SpeakerEvents { get; set; }
        public DbSet<Lot> Lots { get; set; }
        public DbSet<SocialNetWork> SocialNetWorks { get; set; }

        /// <summary>
        /// Indica relacionamento de n:n
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserRole>(userRole =>
            {
                // um papel que tem muitos usuários e o relacionamento é obrigatório
                userRole.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);
                // um usuário com muitos papéis e o relacionamento é obrigatório
                userRole.HasOne(ur => ur.User)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.UserId);
            });

            builder.Entity<SpeakerEvent>().HasKey(PE => new { PE.EventId, PE.SpeakerId });

            builder.Entity<User>(u =>
            {
                u.HasIndex(_ => _.Email).IsUnique();
                u.HasIndex(_ => _.UserName).IsUnique();
            });

        }
    }
}