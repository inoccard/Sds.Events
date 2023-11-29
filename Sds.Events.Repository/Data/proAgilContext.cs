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
    public class ProAgilContext : IdentityDbContext<User, Role, int, IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public ProAgilContext(DbContextOptions<ProAgilContext> options) : base(options)
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
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserRole>(userRole =>
            {
                // RelationShip Many To Many
                userRole.HasKey(ur => new { ur.RoleId, ur.UserId });
                // um papel que tem muitos usuários e o relacionamento é obrigatório
                userRole.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();
                // um usuário com muitos papéis e o relacionamento é obrigatório
                userRole.HasOne(ur => ur.User)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
            });

            modelBuilder.Entity<SpeakerEvent>().HasKey(PE => new { PE.EventId, PE.SpeakerId });
        }
    }
}