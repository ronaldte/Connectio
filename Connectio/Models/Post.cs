using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Connectio.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime Created { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public List<ApplicationUser> BookmarkedBy { get; set; } = new();
        public List<Bookmark> Bookmarks { get; set; } = new();
    }
}
