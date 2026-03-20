namespace DotnetCheckpoint1.Models;

public enum EnrollmentStatus
{
    Active,
    Canceled,
}

public class Enrollment : BaseEntity
{
    public required Guid Id { get; set; }

    public required EnrollmentStatus Status { get; set; }

    public required Student Student { get; set; }
    public required Guid StudentId { get; set; }

    public required Course Course { get; set; }
    public required Guid CourseId { get; set; }
}