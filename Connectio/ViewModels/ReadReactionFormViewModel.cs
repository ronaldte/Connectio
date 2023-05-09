namespace Connectio.ViewModels
{
    public class ReadReactionFormViewModel
    {
        public string ReactionAction { get; set; }
        public string ReactionIcon { get; set; }
        public int ReactionCount { get; set; }
        public string ReactionType { get; set; }
        public int PostId { get; set; }
        public bool DisplayCount { get; set; }

        public ReadReactionFormViewModel(string reactionAction, string reactionIcon, int reactionCount, string reactionType, int postId, bool displayCount)
        {
            ReactionAction = reactionAction;
            ReactionIcon = reactionIcon;
            ReactionCount = reactionCount;
            ReactionType = reactionType + (reactionCount > 1 ? "s" : string.Empty);
            PostId = postId;
            DisplayCount = displayCount;
        }
    }
}
