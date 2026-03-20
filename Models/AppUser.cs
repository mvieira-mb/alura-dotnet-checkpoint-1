using Microsoft.AspNetCore.Identity;

namespace DotnetCheckpoint1.Models;

public class AppUser : IdentityUser
{
    public Student Student { get; set; }
}
