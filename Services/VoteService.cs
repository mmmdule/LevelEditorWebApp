using LevelEditorWebApp.Classes;
using LevelEditorWebApp.Data;
using LevelEditorWebApp.Models;

namespace LevelEditorWebApp.Services {
    public class VoteService : IVoteService {
        public ApplicationDbContext applicationDbContext;

        public VoteService(ApplicationDbContext applicationDbContext) {
            this.applicationDbContext = applicationDbContext;
        }

        //has to be async Task because we are using database and mustn't dispose it before we are done
        public async Task AddOrUpdateVote(int postId, int vote, string userName) {
            //check if user has already voted on this post
            //if so, update the vote. Remove the vote if the vote is same as before
            //if not, add a new vote
            if(UserHasVoted(postId, userName)) {
                if(GetUserVoteValue(postId, userName) == vote) {
                    applicationDbContext.UserVoteInfo.Remove(applicationDbContext.UserVoteInfo.Where(u => u.PostId == postId && u.UserName == userName).FirstOrDefault());
                } else {
                    applicationDbContext.UserVoteInfo.Where(u => u.PostId == postId && u.UserName == userName).FirstOrDefault().Vote = vote;
                }
            } else {
                applicationDbContext.UserVoteInfo.Add(new Classes.UserVoteInfo() { PostId = postId, Vote = vote, UserName = userName });
            }

            await applicationDbContext.SaveChangesAsync();
        }

        public int GetPostVotes(int postId) {
            int result = 0;
            //get post by id and sum up all votes
            applicationDbContext.UserVoteInfo.Where(u => u.PostId == postId).ToList().ForEach(u => result += u.Vote);
            return result;
        }

        public bool UserHasVoted(int postId, string userName) {
            //get post by id and check if user has voted on it
            return applicationDbContext.UserVoteInfo.Where(u => u.PostId == postId && u.UserName == userName).FirstOrDefault() != null;
        }

        public int GetUserVoteValue(int postId, string userName) {
            //get post by id and check if user has voted on it
            //return 0 if user has not voted on it
            return UserHasVoted(postId, userName) ? 
                applicationDbContext.UserVoteInfo.Where(u => u.PostId == postId && u.UserName == userName).FirstOrDefault().Vote : 0;
        }

        public Post[] GetBestRatedPostsAllTime(int count) {
            //get all posts and sort them by votes

            //get all posts
            var posts = applicationDbContext.Post.ToList();

            //sort them by votes
            posts.Sort((x, y) => GetPostVotes(y.PostId).CompareTo(GetPostVotes(x.PostId)));

            //remove posts with no votes or negative votes
            posts.RemoveAll(p => GetPostVotes(p.PostId) <= 0);

            //return the first count posts
            return posts.Take(count).ToArray();
        }
    }
}
