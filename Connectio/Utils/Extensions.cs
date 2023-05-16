namespace Connectio.Utils
{
    public static class Extensions
    {
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
