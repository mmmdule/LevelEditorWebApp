namespace LevelEditorWebApp.Services {
    public interface IVoteService {
        public Task AddOrUpdateVote(int postId, int vote, string userName);
        public int GetPostVotes(int postId);
        public bool UserHasVoted(int postId, string userName);
        public int GetUserVoteValue(int postId, string userName);
    }
}
