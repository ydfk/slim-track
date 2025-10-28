using Microsoft.EntityFrameworkCore;

namespace SlimTrack.Server
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<WeightEntry> WeightEntries => Set<WeightEntry>();

        protected override void OnModelCreating(ModelBuilder b)
        {
            b.Entity<WeightEntry>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.WeightKg).HasPrecision(5, 2);
                e.HasIndex(x => x.Date).IsUnique(); // 每日一条
            });
        }
    }

    public class WeightEntry
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public decimal WeightKg { get; set; }
        public string? Note { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
