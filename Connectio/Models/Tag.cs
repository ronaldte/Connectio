namespace Connectio.Models
{
    /// <summary>
    /// Represents relationship between posts or grouping.
    /// </summary>
    public class Tag
    {
        /// <summary>
        /// Key of Tag.
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Tag name; text value fir tge group.
        /// </summary>
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// Posts which are part of the group.
        /// </summary>
        public List<Post> Posts { get; set; } = new();
    }
}
