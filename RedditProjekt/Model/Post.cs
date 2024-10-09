namespace Model
{
    public class Post
    {
        public long PostId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string User {  get; set; }

        public DateTime Date { get; set; }

        public int Upvote { get; set; }

        public int Downvote { get; set; }

        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
