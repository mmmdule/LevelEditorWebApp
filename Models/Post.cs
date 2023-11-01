namespace LevelEditorWebApp.Models {
    public class Post {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime? CreatedAt { get; set; }
        public byte[]? ZipFile { get; set; }
        public string? Author { get; set; }
        public string? ZipFileName { get; set; }
        public Post() { 
        }
    }
}
