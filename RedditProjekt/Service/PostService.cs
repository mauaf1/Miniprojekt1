using Microsoft.EntityFrameworkCore;
using Model;
using Data;
using System.Reflection;

namespace Service
{
    public class PostService
    {
        private PostContext db { get; }

        public PostService(PostContext db)
        {
            this.db = db;
        }

        public void SeedData()
        {
            Post post = db.Posts.FirstOrDefault()!;
            if (post == null)
            {
                db.Add(new Post
                {
                    Title = "Testing",
                    Content = "This is a post",
                    User = "me",
                    Date = DateTime.Now,
                    Upvote = 1,
                    Downvote = 10,
                    Comments = new List<Comment>
            {
                new Comment
                {
                    User = "CommentUser1",
                    Content = "This is a comment",
                    Date = DateTime.Now,
                    Upvote = 5,
                    Downvote = 2
                },
                new Comment
                {
                    User = "CommentUser2",
                    Content = "This is another comment",
                    Date = DateTime.Now,
                    Upvote = 3,
                    Downvote = 1
                } }

                });


            }
           

            db.SaveChanges();


        }

        public List<Post> getPosts ()
        {
            return db.Posts.Include(p => p.Comments).ToList();
        }
        public Post getPost(int id)
        {
            return db.Posts.Include(p => p.Comments).FirstOrDefault(P => P.PostId == id);
        }

        public string CreatePost(string title, string content, string user, DateTime date, int upvote, int downvote)
        {
            // Post Post = db.Posts.FirstOrDefault(P => P.PostId == postId);
            db.Add(new Post { Title = title, Content = content, User = user, Date = date, Upvote = upvote, Downvote = downvote });
            db.SaveChanges();
            return "Post created";

        }
        public string CreateComment(string user, DateTime date, string content, int upvote, int downvote, long postId)
        {
            
            Post post = db.Posts.Include(p => p.Comments).FirstOrDefault(p => p.PostId == postId);

  
            Comment newComment = new Comment
            {
                User = user,
                Date = date,
                Content = content,
                Upvote = upvote,
                Downvote = downvote
            };

            post.Comments.Add(newComment);
            db.SaveChanges();

            return "Comment added";
        }

        public Post UpvotePost(int postId)
        {
            var post = getPost(postId);

            post.Upvote++;

            db.SaveChanges();

            return post;
        }
        public Post DownvotePost(int postId)
        {
            var post = getPost(postId);

            post.Downvote++;

            db.SaveChanges();

            return post;
        }
        public Post UpvoteComment(int postId, int commentId)
        {
            var post = getPost(postId);
            if (post == null)
            {
                throw new Exception($"Post with ID {postId} not found.");
            }

            var comment = post.Comments.FirstOrDefault(c => c.CommentId == commentId);

            if (comment == null)
            {
                throw new Exception($"Comment with ID {commentId} not found in Post {postId}.");
            }

            comment.Upvote++;

            db.SaveChanges();

            return post;
        }
        public Post DownvoteComment(int postId, int commentId)
        {
            var post = getPost(postId);

            var comment = post.Comments.FirstOrDefault(c => c.CommentId == commentId);

            comment.Downvote++;

            db.SaveChanges();

            return post;
        }








    }
}
