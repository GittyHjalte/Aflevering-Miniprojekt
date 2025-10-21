using System.Text.Json.Serialization;

namespace shared.Model
{
    public class Comment
    {
        public int Id { get; set; } // PK
        
        public int PostId { get; set; } // FK
        public string Content { get; set; }
        public string? Username { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public int Upvotes { get; set; } = 0;
        public int Downvotes { get; set; } = 0;
        
        [JsonIgnore] // Virker ikke uden? Tilføjer for sikkerheds skyld.
        public Post Post { get; set; } 
    }
}