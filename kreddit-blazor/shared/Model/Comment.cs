namespace shared.Model;

public class Comment
{
    public long CommentId { get; set; }
    public string Content { get; set; }
    public int Upvote { get; set; }
    public int Downvote { get; set; }
    public string User { get; set; }

    public DateTime Date { get; set; }
    
}
