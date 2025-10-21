using Microsoft.EntityFrameworkCore;
using System.Text.Json;

using Data;
using shared.Model;

namespace Service;

public class DataService
{
    private PostContext db { get; }

    public DataService(PostContext db)
    {
        this.db = db;
    }

    /// <summary>
    /// Seeder noget nyt data i databasen hvis det er nødvendigt.
    /// </summary>
    public void SeedData()
    {

        if (!db.Posts.Any())
        {
            var posts = Enumerable.Range(1, 51).Select(i => new Post
            {
                Title = $"Titel {i}",
                Content = $"Indhold {i}",
                Username = "Anonymous",
                Upvotes = 0,
                Downvotes = 0,
                DateCreated = DateTime.Now // .AddDays(i)?
            }).ToList();

            db.Posts.AddRange(posts);
            db.SaveChanges();

            db.Comments.AddRange(
                new Comment { Content = "Kommentar", PostId = 1, Upvotes = 0, Downvotes = 0, DateCreated = DateTime.Now}
            );

            db.SaveChanges();
        }
    }


    // GET
    public List<Post> GetPosts()
    {
        return db.Posts.ToList();
    }

    public Post? GetPost(int id)
    {
        return db.Posts
            .Include(p => p.Comments) // <- så comments hentes med
            .FirstOrDefault(p => p.Id == id);
    }

    public List<Comment> GetCommentsByPostId(int id)
    {
        var comments = db.Comments.Where(c => c.PostId == id);
        return comments.ToList();
    }
    
    /*public List<Post> GetAllCommentsByPost(int id)
    {
        var post = db.Posts.Where(p => p.Id == id);
        var comments = db.Comments.Where(c => c.PostId == id);
        Return db.Comments.ToList(post, comments);
    }
    
    public List<Comment> GetCommentById(int id)
    {
        return db.Comments.Where(c => c.Id == id).ToList();
    }
*/
    // PUT

    public Post? UpvotePost(int id)
    {
        var post = db.Posts.FirstOrDefault(p => p.Id == id);
        if (post == null) return null;
        post.Upvotes++;
        db.SaveChanges();
        return post;
    }
    
    public Post? DownvotePost(int id)
    {
        var post = db.Posts.FirstOrDefault(p => p.Id == id);
        if (post == null) return null;
        post.Downvotes++;
        db.SaveChanges();
        return post;
    }
    public Comment? UpvoteComment(int postid, int commentid)
    {
        var comment = db.Comments.FirstOrDefault(c => c.Id == commentid && c.PostId == postid);
        if (comment != null)
        {
            comment.Upvotes++;
            db.SaveChanges();
        }
        return comment;
    }

    public Comment? DownvoteComment(int postid, int commentid)
    {
        var comment = db.Comments.FirstOrDefault(c => c.PostId == postid && c.Id == commentid);
        if (comment == null) return null;
        comment.Downvotes++;
        db.SaveChanges();
        return comment;
    }
    /*public List<Comment> CreateComment(int postId, int commentId, string username)
    {
        var comment = db.Comments.FirstOrDefault(c => c.PostId == postId && c.Id == commentId && c.Username == username);
        if (comment == null) return null;
        return null; // TODO
    }*/

    public Post CreatePost(Post post)
    {
        db.Posts.Add(post);
        db.SaveChanges();
        return post;
    }

    public Comment CreateComment(Comment comment)
    {
        db.Comments.Add(comment);
        db.SaveChanges();
        return comment;
    }
}

/*public string CreateUser(string Content, int UserId) {
    User user = db.Users.FirstOrDefault(a => a.UserId == UserId);
    db.Comment.Add(new Comments { Content = Content, User = user });
    db.SaveChanges();
    return "Comment created";
}
*/
    /*public string DeleteAuthor(int UserId)
    {
        UserId = 1;
        Users users = db.Users.FirstOrDefault(a => a.UserId == UserId);
        db.Users.Remove(users);
        db.SaveChanges();
        return "Author deleted";
    }*/