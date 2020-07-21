using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

/// <summary>
/// Entities
/// </summary>
using ProAgil.Domain.Identity;
using ProAgil.Domain.Entities;

/// <summary>
/// Conext DB Class
/// </summary>
namespace ProAgil.Repository.Data
{
    public class ProAgilContext : IdentityDbContext<User, Role, int, 
                                IdentityUserClaim<int>, UserRole, 
                                IdentityUserLogin<int>, IdentityRoleClaim<int>, 
                                IdentityUserToken<int>>
    {
        /*public DataContext(DbContextOptions<DbContext> options) : base(options)
        {
            
        }*/

        public ProAgilContext (DbContextOptions<ProAgilContext> options) : base (options) { }

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
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.RoleId, ur.UserId });

                // RelationShip Many To Many
                userRole.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

                userRole.HasOne(ur => ur.User)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
            });
            
            modelBuilder.Entity<SpeakerEvent> ().HasKey (PE => new { PE.EventId, PE.SpeakerId });
        }
    }
}