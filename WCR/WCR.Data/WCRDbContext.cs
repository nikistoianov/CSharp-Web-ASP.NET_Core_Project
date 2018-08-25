namespace WCR.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using WCR.Models;

    public class WCRDbContext : IdentityDbContext<User>
    {
        public DbSet<Team> Teams { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<BetMatch> BetsForMatch { get; set; }
        public DbSet<BetPosition> BetsForPosition { get; set; }

        public WCRDbContext(DbContextOptions<WCRDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<BetPosition>()
                .HasKey(x => new { x.UserId, x.TeamId });

            builder.Entity<Match>(ent =>
            {
                ent.HasOne(x => x.FirstTeam)
                   .WithMany(x => x.HomeMatches)
                   .HasForeignKey(x => x.FirstTeamId)
                   .OnDelete(DeleteBehavior.Restrict);

                ent.HasOne(x => x.SecondTeam)
                   .WithMany(x => x.GuestMatches)
                   .HasForeignKey(x => x.SecondTeamId)
                   .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<User>()
                .HasIndex(x => x.ShortName)
                .IsUnique();

            base.OnModelCreating(builder);
        }
    }
}
