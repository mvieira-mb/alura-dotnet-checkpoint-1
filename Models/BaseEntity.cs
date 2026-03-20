namespace DotnetCheckpoint1.Models;

public abstract class BaseEntity
{
    public required DateTime CreatedAt { get; set; }
    public required DateTime UpdatedAt { get; set; }
    public required DateTime? DeletedAt { get; set; } = null;
}