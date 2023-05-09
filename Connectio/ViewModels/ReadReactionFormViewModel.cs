namespace Connectio.ViewModels
{
    public class ReadReactionFormViewModel
    {
        public string ReactionAction { get; set; }
        public string ReactionIcon { get; set; }
        public int ReactionCount { get; set; }
        public int PostId { get; set; }
        public bool DisplayCount { get; set; }

        public ReadReactionFormViewModel(string reactionAction, string reactionIcon, int reactionCount, int postId, bool displayCount)
        {
            ReactionAction = reactionAction;
            ReactionIcon = reactionIcon;
            ReactionCount = reactionCount;
            PostId = postId;
            DisplayCount = displayCount;
        }
    }
}
