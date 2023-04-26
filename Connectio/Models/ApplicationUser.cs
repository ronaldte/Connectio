using Microsoft.AspNetCore.Identity;

namespace Connectio.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public string? Location { get; set; }
        public string? Url { get; set; }
        public string? Description { get; set; }
        public bool Protected { get; set; } = false;
        public bool Verified { get; set; } = false;
        public DateTime Created { get; set; }
        public ICollection<Post>? Posts { get; set; }
    }
}
