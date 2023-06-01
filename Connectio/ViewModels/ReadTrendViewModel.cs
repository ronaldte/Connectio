namespace Connectio.ViewModels
{
    /// <summary>
    /// ReadTrend model represents trends on the social media.
    /// </summary>
    public class ReadTrendViewModel
    {
        /// <summary>
        /// List of people to follow.
        /// </summary>
        public IEnumerable<ReadUserViewModel> PeopleToFollow { get; set; }
        
        /// <summary>
        /// Popular tag used on the social media.
        /// </summary>
        public IEnumerable<ReadTagViewModel> TrendingTags { get; set; }

        public ReadTrendViewModel(IEnumerable<ReadUserViewModel> peopleToFollow, IEnumerable<ReadTagViewModel> trendingTags)
        {
            PeopleToFollow = peopleToFollow;
            TrendingTags = trendingTags;
        }
    }
}
