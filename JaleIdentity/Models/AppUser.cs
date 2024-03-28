using Microsoft.AspNetCore.Identity;

namespace JaleIdentity.Models
{
    public class AppUser:IdentityUser
    {
        public string Fullname { get; set; } = null!;

    }
}
