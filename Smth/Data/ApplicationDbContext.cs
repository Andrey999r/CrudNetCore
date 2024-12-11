namespace Smth.Data;

using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<ApplicationUser> Users { get; set; }
    public DbSet<Survey> Surveys { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Participant> Participants { get; set; }
    public DbSet<Answer> Answers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ApplicationUser>()
            .HasMany(u => u.Surveys)
            .WithOne(s => s.Owner)
            .HasForeignKey(s => s.ApplicationUserId);

        modelBuilder.Entity<Survey>()
            .HasMany(s => s.Questions)
            .WithOne(q => q.Survey)
            .HasForeignKey(q => q.SurveyId);

        modelBuilder.Entity<Survey>()
            .HasMany(s => s.Participants)
            .WithOne(p => p.Survey)
            .HasForeignKey(p => p.SurveyId);

        modelBuilder.Entity<Participant>()
            .HasMany(p => p.Answers)
            .WithOne(a => a.Participant)
            .HasForeignKey(a => a.ParticipantId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}
