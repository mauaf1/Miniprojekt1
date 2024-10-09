using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Json;
using Data;
using Model;
using Service;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Xml.Linq;
// git
var builder = WebApplication.CreateBuilder(args);

// Set up CORS to allow the API to be used from other domains
var AllowSomeStuff = "_AllowSomeStuff";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowSomeStuff, builder => {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

// Add DbContext factory as a service.
builder.Services.AddDbContext<PostContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("ContextSQLite")));

// Add BoardService so it can be used in endpoints
builder.Services.AddScoped<PostService>();

var app = builder.Build();

// Seed data if necessary.
using (var scope = app.Services.CreateScope())
{
    var dataService = scope.ServiceProvider.GetRequiredService<PostService>();
    dataService.SeedData(); // Populate data if the database is empty.
}

app.UseHttpsRedirection();
app.UseCors(AllowSomeStuff);

// Middleware to set ContentType for all responses to "JSON".
app.Use(async (context, next) =>
{
    context.Response.ContentType = "application/json; charset=utf-8";
    await next(context);
});

// Endpoints

app.MapGet("/", (PostService service) =>
{
    return new { message = "Hello there" };
});

app.MapGet("/api/Posts", (PostService service) =>
{
    return service.getPosts().Select(P => new
    {
        postId = P.PostId,
        title = P.Title,
        content = P.Content,
        user = P.User,
        date = P.Date,
        upvote = P.Upvote,
        downvote = P.Downvote,
        comments = P.Comments.Select(c => new
        {
            commentId = c.CommentId,
            user = c.User,
            content = c.Content,
            date = c.Date,
            upvote = c.Upvote,
            downvote = c.Downvote
        }).ToList()
    });
    });

app.MapGet("/api/Post/{PostId}", (PostService service, int PostId) =>
{
    var post = service.getPost(PostId);

    return Results.Ok(new
    {
        postId = post.PostId,
        title = post.Title,
        content = post.Content,
        user = post.User,
        date = post.Date,
        upvote = post.Upvote,
        downvote = post.Downvote,
        comments = post.Comments.Select(c => new
        {
            commentId = c.CommentId,
            user = c.User,
            content = c.Content,
            date = c.Date,
            upvote = c.Upvote,
            downvote = c.Downvote
        }).ToList()
    });
});

app.MapPost("/api/CreatePost", (PostService service, NewPostData data) =>
{
    string result = service.CreatePost(data.title, data.content, data.user, data.Date, data.upvote, data.downvote);
    return new { message = result };
});

app.MapPost("/api/CreateComment", (PostService service, NewCommentData data) =>
{
    string result = service.CreateComment(data.user, data.Date, data.content, data.upvote, data.downvote, data.postId);
    return new { message = result };
});

app.MapPut("/api/posts/{id}/Upvote", (PostService service, int id) =>
{

    return Results.Ok(service.UpvotePost(id));
});
app.MapPut("/api/posts/{id}/Downvote", (PostService service, int id) =>
{

    return Results.Ok(service.DownvotePost(id));
});
app.MapPut("/api/posts/{postid}/comments/{commentid}/Upvote", (PostService service, int postid, int commentid) =>
{

    return Results.Ok(service.UpvoteComment(postid, commentid));
});
app.MapPut("/api/posts/{postid}/comments/{commentid}/Downvote", (PostService service, int postid, int commentid) =>
{

    return Results.Ok(service.DownvoteComment(postid, commentid));
});



app.Run();

record NewPostData ( string title, string content, string user, DateTime Date, int upvote, int downvote);

record NewCommentData(string user, DateTime Date, string content, int upvote, int downvote, long postId);

