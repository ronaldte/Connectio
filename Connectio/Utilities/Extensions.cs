namespace Connectio.Utilities
{
    /// <summary>
    /// Extensions class groups extensions method created.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Creates short description of how old the entity is.
        /// </summary>
        /// <param name="created">DateTime to convert.</param>
        /// <example>
        ///     - created 10 minutes ago => 10m
        ///     - created 3 hours ago => 3h
        ///     - created few days back returns today-5 days => Day Month
        ///     - created last year => May 2022
        /// </example>
        /// <returns>Transforms DateTime to short version.</returns>
        public static string TimeSinceCreated(this DateTime created)
        {
            var timeDifference = DateTime.UtcNow - created;

            string response;

            if (timeDifference.TotalDays > 365)
            {
                response = created.ToString("MMM yyyy");
            }
            else if (timeDifference.TotalHours > 24)
            {
                response = created.ToString("d MMM");
            }
            else if (timeDifference.TotalHours > 1)
            {
                response = $"{timeDifference.Hours}h";
            }
            else if (timeDifference.TotalMinutes > 1)
            {
                response = $"{timeDifference.Minutes}m";
            }
            else
            {
                response = "Just now";
            }

            return response;
        }

    }
}
