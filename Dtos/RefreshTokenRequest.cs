namespace DotnetCheckpoint1.Dtos;

public class RefreshTokenRequest
{
    public required string Email { get; set; }
    public required string RefreshToken { get; set; }
}
