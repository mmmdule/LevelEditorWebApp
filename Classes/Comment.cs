namespace LevelEditorWebApp.Classes {
    public class Comment {
        public int Id { get; set; }
        public string Content { get; set; }
        public int PostId { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public DateTime CreatedAt { get; set; }
        
        public Comment() {
        }
    }
}
