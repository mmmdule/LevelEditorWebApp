namespace LevelEditorWebApp.Classes {
    public class UserVoteInfo {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int Vote { get; set; } // 1 for upvote, -1 for downvote
        public string UserName { get; set; }
        public UserVoteInfo() { }
    }
}
