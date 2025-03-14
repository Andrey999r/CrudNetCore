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
        modelBuilder.Entity<Answer>()
            .HasOne(a => a.Participant)
            .WithMany(p => p.Answers)
            .HasForeignKey(a => a.ParticipantId)
            .OnDelete(DeleteBehavior.Cascade); // Включаем каскадное удаление
    }

}
