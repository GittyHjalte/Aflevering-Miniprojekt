using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Json;

using Data;
using Microsoft.AspNetCore.Components.Web;
using shared.Model;
using Service;

var builder = WebApplication.CreateBuilder(args);

// Sætter CORS så API'en kan bruges fra andre domæner
var AllowSomeStuff = "_AllowSomeStuff";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowSomeStuff, builder => {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});
//var postWithComments = db.Posts.Include(p => p.Comments)
//    .FirstOrDefault(p => p.Id == 1);

// Tilføj DbContext factory som service.
builder.Services.AddDbContext<PostContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("ContextSQLite")));

// Tilføj DataService så den kan bruges i endpoints
builder.Services.AddScoped<DataService>();

// Dette kode kan bruges til at fjerne "cykler" i JSON objekterne.
/*
builder.Services.Configure<JsonOptions>(options =>
{
    // Her kan man fjerne fejl der opstår, når man returnerer JSON med objekter,
    // der refererer til hinanden i en cykel.
    // (altså dobbelrettede associeringer)
    options.SerializerOptions.ReferenceHandler = 
        System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});
*/

var app = builder.Build();

// Seed data hvis nødvendigt.
using (var scope = app.Services.CreateScope())
{
    var dataService = scope.ServiceProvider.GetRequiredService<DataService>();
    dataService.SeedData(); // Fylder data på, hvis databasen er tom. Ellers ikke.
}

app.UseHttpsRedirection();
app.UseCors(AllowSomeStuff);

// Middlware der kører før hver request. Sætter ContentType for alle responses til "JSON".
app.Use(async (context, next) =>
{
    context.Response.ContentType = "application/json; charset=utf-8";
    await next(context);
});


// DataService fås via "Dependency Injection" (DI)








// GET:
app.MapGet("/api/posts", (DataService service) =>
{
    return service.GetPosts();
});


app.MapGet("/api/posts/{id}", (DataService service, int id) =>
{
    return service.GetPost(id);
});

app.MapGet("/api/posts/{id}/comments", (DataService service, int id) =>
{
   return service.GetCommentsByPostId(id);
});


/*
app.MapGet("api/posts/{id}", (DataService service) =>
 {
     return service.GetAllCommentsByPost(postid);
 }
 );



app.MapPut("/api/posts/{id}/upvote", (DataService service, int id, NewPostData data) =>
 {
     var upvoted = data.UpvoteButton;
     if (upvoted == false) return null;
   service.PutPostUpvote(id);
 }
);

*/
// PUT:

app.MapPut("/api/posts/{id}/upvote", (DataService service, int id) =>
    {
        var post = service.UpvotePost(id);
        return post;
    }
);

app.MapPut("/api/posts/{id}/downvote", (DataService service, int id) =>
    {
        var post = service.DownvotePost(id);
        return post;
    });

app.MapPut("/api/posts/{postid}/comments/{commentid}/upvote", (DataService service, int postid, int commentid) =>
{
    var post = service.UpvoteComment(postid, commentid);
    return post;
});


app.MapPut("/api/posts/{postid}/comments/{commentid}/downvote", (DataService service, int postid, int commentid) =>
{
    var post = service.DownvoteComment(postid, commentid);
    return post;
});
/*
app.MapPut("/api/posts/{id}/comments",(DataService service, int id) =>
    {
        return service.PutComment()
    });
    */
// POST:

app.MapPost("/api/posts", (DataService service, NewPostData data) =>
{
    var post = new Post 
    { 
        Title = data.Title, 
        Content = data.Content, 
        Username = data.Username, 
        DateCreated = data.DateCreated
    };

    service.CreatePost(post);


    return Results.Json(post);
});



app.MapPost("/api/posts/{id}/comments", (DataService service, int id, NewCommentData data) =>
{
    var comment = new Comment { Content = data.Content, Username = data.Username, PostId = id, DateCreated = data.DateCreated};
    service.CreateComment(comment);
});

    







/*app.MapDelete("/api/comment/{userId}", (DataService service, int authorId) =>
{
    string result = service.DeleteUser(userId);
    return new { message = result };
});
*/
app.Run();

record NewPostData(string Title, string Content, string Username, DateTime DateCreated);
record NewCommentData(string Content, string Username, DateTime DateCreated);







