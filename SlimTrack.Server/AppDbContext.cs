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
                e.Property(x => x.WeightJin).HasPrecision(6, 2); // 斤,保留2位小数
                e.Property(x => x.WeightGongJin).HasPrecision(5, 2); // 公斤,保留2位小数
                e.HasIndex(x => x.Date).IsUnique(); // 每日一条
            });
        }
    }

    public class WeightEntry
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public decimal WeightJin { get; set; } // 斤
        public decimal WeightGongJin { get; set; } // 公斤
        public string? Note { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
