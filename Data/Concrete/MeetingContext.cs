using MeetingApp.Data.Entity;
using MeetingApp.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MeetingApp.Data.Concrete
{
    public class MeetingContext : IdentityDbContext<IdentityUser>
    {
        public MeetingContext(DbContextOptions<MeetingContext> options) : base(options)
        {}

        public virtual DbSet<Meeting> Meetings => Set<Meeting>();
        public virtual DbSet<User> Users => Set<User>();
        public virtual DbSet<MeetingUser> MeetingUsers => Set<MeetingUser>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole { Name = "Admin", NormalizedName = "admin" });

            // MeetingUser ilişkileri
            modelBuilder.Entity<MeetingUser>()
                .HasKey(mu => new { mu.MeetingId, mu.UserId });

            modelBuilder.Entity<MeetingUser>()
                .HasOne(mu => mu.Meeting)
                .WithMany(m => m.MeetingUsers)
                .HasForeignKey(mu => mu.MeetingId);

            modelBuilder.Entity<MeetingUser>()
                .HasOne(mu => mu.User)
                .WithMany(u => u.MeetingUsers)
                .HasForeignKey(mu => mu.UserId);
        }
    }
}
