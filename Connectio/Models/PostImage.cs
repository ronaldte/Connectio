namespace Connectio.Models
{
    public class PostImage
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; } = null!;
        public int Order { get; set; } = 1;
        public Post Post { get; set; } = null!;
    }
}
