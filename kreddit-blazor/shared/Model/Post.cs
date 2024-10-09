namespace shared.Model;

public class Post {
    public long PostId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public int Upvote { get; set; }
    public int Downvote { get; set; }
    public string User { get; set; }
    public DateTime Date { get; set; }
    public List<Comment> Comments { get; set; } = new List<Comment>();
    public Post(string user, string title = "", string content = "", int upvote = 0, int downvote = 0, DateTime? date = null) {
        Title = title;
        Content = content;
        Upvote = upvote;
        Downvote = downvote;
        User = user;
        Date = date ?? DateTime.Now;
    }
    public Post() {
        PostId = 0;
        Title = "";
        Content = "";
        Upvote = 0;
        Downvote = 0;
        User = null;
        Date = DateTime.Now;
    }

    public override string ToString()
    {
        return $"Id: {PostId}, Title: {Title}, Content: {Content}, Upvotes: {Upvote}, Downvotes: {Downvote}, User: {User}";
    }
}