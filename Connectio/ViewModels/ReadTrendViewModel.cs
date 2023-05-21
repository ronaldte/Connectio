namespace Connectio.ViewModels
{
    public class ReadTrendViewModel
    {
        public IEnumerable<ReadUserViewModel> PeopleToFollow { get; set; }
        public IEnumerable<ReadTagViewModel> TrendingTags { get; set; }

        public ReadTrendViewModel(IEnumerable<ReadUserViewModel> peopleToFollow, IEnumerable<ReadTagViewModel> trendingTags)
        {
            PeopleToFollow = peopleToFollow;
            TrendingTags = trendingTags;
        }
    }
}
