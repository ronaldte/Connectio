﻿namespace Connectio.Models
{
    public class Comment
    {
        public int PostId { get; set; }
        public Post Post { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser User { get; set; }
        public string Text { get; set; }
        public DateTime Created { get; set; }
    }
}
