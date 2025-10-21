namespace shared.Model
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string? Username { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public int Upvotes { get; set; } = 0;
        public int Downvotes { get; set; } = 0;
        public List<Comment> Comments { get; set; }
    }
}