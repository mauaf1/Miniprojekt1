using System.ComponentModel.Design;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

using shared.Model;

namespace kreddit_app.Data;

public class ApiService
{
    private readonly HttpClient http;
    private readonly IConfiguration configuration;
    private readonly string baseAPI = "";

    public ApiService(HttpClient http, IConfiguration configuration)
    {
        this.http = http;
        this.configuration = configuration;
        this.baseAPI = configuration["base_api"];
    }

    public async Task<Post[]> getPosts()
    {
        string url = $"{baseAPI}Posts";
        return await http.GetFromJsonAsync<Post[]>(url);
    }

    public async Task<Post> GetPost(int PostId)
    {
        string url = $"{baseAPI}Post/{PostId}/";
        return await http.GetFromJsonAsync<Post>(url);
    }

    public async Task<Post> CreatePost(string title, string content, string user, DateTime date, int upvote, int downvote)
    {
        string Posturl = $"{baseAPI}CreatePost";
        var newPostData = new
        {
            user,
            date,
            title,
            content,
            upvote,
            downvote
        };
        // Post JSON to API, save the HttpResponseMessage
        HttpResponseMessage msg = await http.PostAsJsonAsync(Posturl, newPostData);

        // Get the JSON string from the response
        string json = msg.Content.ReadAsStringAsync().Result;

        // Deserialize the JSON string to a Comment object
        Post? newPost = JsonSerializer.Deserialize<Post>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true // Ignore case when matching JSON properties to C# properties 
        });

        // Return the new comment 
        return newPost;
    }



    public async Task<Comment> CreateComment(string user, DateTime date, string content, int upvote, int downvote, long postId)
    {
        string url = $"{baseAPI}CreateComment";

        var newCommentData = new
        {
            user,
            date,
            content,
            upvote,
            downvote,
            postId
        };

        // Post JSON to API, save the HttpResponseMessage
        HttpResponseMessage msg = await http.PostAsJsonAsync(url, newCommentData);

        // Get the JSON string from the response
        string json = msg.Content.ReadAsStringAsync().Result;

        // Deserialize the JSON string to a Comment object
        Comment? newComment = JsonSerializer.Deserialize<Comment>(json, new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true // Ignore case when matching JSON properties to C# properties 
        });

        // Return the new comment 
        return newComment;
    }

    public async Task<Post> UpvotePost(int id)
    {
        string url = $"{baseAPI}posts/{id}/Upvote/";

        // Post JSON to API, save the HttpResponseMessage
        HttpResponseMessage msg = await http.PutAsJsonAsync(url, "");

        // Get the JSON string from the response
        string json = msg.Content.ReadAsStringAsync().Result;

        // Deserialize the JSON string to a Post object
        Post? updatedPost = JsonSerializer.Deserialize<Post>(json, new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true // Ignore case when matching JSON properties to C# properties
        });

        // Return the updated post (vote increased)
        return updatedPost;
    }
    public async Task<Post> DownvotePost(int id)
    {
        string url = $"{baseAPI}posts/{id}/Downvote/";

        // Post JSON to API, save the HttpResponseMessage
        HttpResponseMessage msg = await http.PutAsJsonAsync(url, "");

        // Get the JSON string from the response
        string json = msg.Content.ReadAsStringAsync().Result;

        // Deserialize the JSON string to a Post object
        Post? updatedPost = JsonSerializer.Deserialize<Post>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true // Ignore case when matching JSON properties to C# properties
        });

        // Return the updated post (vote increased)
        return updatedPost;
    }
    public async Task<Post> UpvoteComment(int postid, int commentid)
    {
        string url = $"{baseAPI}posts/{postid}/comments/{commentid}/Upvote/";

        // Post JSON to API, save the HttpResponseMessage
        HttpResponseMessage msg = await http.PutAsJsonAsync(url, "");

        // Get the JSON string from the response
        string json = msg.Content.ReadAsStringAsync().Result;

        // Deserialize the JSON string to a Post object
        Post? updatedPost = JsonSerializer.Deserialize<Post>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true // Ignore case when matching JSON properties to C# properties
        });

        // Return the updated post (vote increased)
        return updatedPost;
    }
    public async Task<Post> DownvoteComment(int postid, int commentid)
    {
        string url = $"{baseAPI}posts/{postid}/comments/{commentid}/Downvote/";

        // Post JSON to API, save the HttpResponseMessage
        HttpResponseMessage msg = await http.PutAsJsonAsync(url, "");

        // Get the JSON string from the response
        string json = msg.Content.ReadAsStringAsync().Result;

        // Deserialize the JSON string to a Post object
        Post? updatedPost = JsonSerializer.Deserialize<Post>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true // Ignore case when matching JSON properties to C# properties
        });

        // Return the updated post (vote increased)
        return updatedPost;
    }
}
