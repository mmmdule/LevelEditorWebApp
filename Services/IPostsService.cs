using LevelEditorWebApp.Data;
using LevelEditorWebApp.Models;

namespace LevelEditorWebApp.Services {
    public interface IPostsService {

        public void AddPost(Post post);

        public void DeletePost(int id);

        public void UpdatePost(Post post);

        public ValueTask<Post> GetPost(int? id);

        public ValueTask<Post> GetPost(int id);

        public Task<List<Post>> GetAllPosts();

        public Task<Post[]> GetPostsByUser(string username);

        //check if post belongs to author
        public bool IsPostByAuthor(string username, int postId);

        //get top 5 recently updated posts
        public Post[] GetLatestFiveUpdatedPosts();
    }
}
