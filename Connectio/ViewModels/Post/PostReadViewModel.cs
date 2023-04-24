namespace Connectio.ViewModels.Post
{
    public class PostReadViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;

        public PostReadViewModel(int id, string text)
        {
            Id = id;
            Text = text;
        }
    }
}
