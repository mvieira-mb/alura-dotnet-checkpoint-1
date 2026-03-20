namespace DotnetCheckpoint1.Models;

public class Student : BaseEntity
{
    public required Guid Id { get; set; }

    public required string FullName { get; set; }

    public required string Email { get; set; }

    public required string AppUserId { get; set; }
    public required AppUser AppUser { get; set; }

    public required ICollection<Enrollment> Enrollments { get; set; }
}