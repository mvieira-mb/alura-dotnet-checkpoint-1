using DotnetCheckpoint1.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<AppUser>(options)
{
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasIndex(e => e.AppUserId);
            entity.HasOne(e => e.AppUser)
                .WithOne(e => e.Student)
                .HasForeignKey<Student>(e => e.AppUserId)
                .IsRequired();
        });

        builder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Title).IsUnique();
            entity.HasIndex(e => e.Category);
        });

        builder.Entity<Enrollment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Student)
                .WithMany(e => e.Enrollments)
                .IsRequired();
            entity.HasOne(e => e.Course)
                .WithMany(e => e.Enrollments)
                .IsRequired();
            entity.HasIndex(e => new { e.StudentId, e.CourseId })
                .IsUnique();
            entity.Property(e => e.Status).HasConversion<string>();
        });
    }

    public override int SaveChanges()
    {
        AddTimestamps();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        AddTimestamps();
        return await base.SaveChangesAsync();
    }

    private void AddTimestamps()
    {
        EntityState[] trackedStates = [EntityState.Added, EntityState.Modified, EntityState.Deleted];

        var entities = ChangeTracker.Entries()
            .Where(x => x.Entity is BaseEntity && trackedStates.Contains(x.State));

        foreach (var entity in entities)
        {
            var now = DateTime.UtcNow;

            switch (entity.State)
            {
                case EntityState.Added:
                    ((BaseEntity)entity.Entity).CreatedAt = now;
                    break;
                case EntityState.Deleted:
                    ((BaseEntity)entity.Entity).DeletedAt = now;
                    break;
                default:
                    break;
            }
            ((BaseEntity)entity.Entity).UpdatedAt = now;
        }
    }
}