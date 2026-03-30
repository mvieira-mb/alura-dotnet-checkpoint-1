namespace DotnetCheckpoint1.Models;

public class Course : BaseEntity
{
    public required Guid Id { get; set; }

    public required string Title { get; set; }

    public required string Description { get; set; }

    public required string Category { get; set; }

    public required int Workload { get; set; }

    public required ICollection<Enrollment> Enrollments { get; set; }
}