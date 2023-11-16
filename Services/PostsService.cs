using LevelEditorWebApp.Data;
using LevelEditorWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LevelEditorWebApp.Services {
    public class PostsService : IPostsService {
        public ApplicationDbContext applicationDbContext;


        public PostsService(ApplicationDbContext applicationDbContext) {
            this.applicationDbContext = applicationDbContext;
        }

        public async void AddPost(Post post) {
            applicationDbContext.Post.AddAsync(post);
            await applicationDbContext.SaveChangesAsync();
        }

        public async void DeletePost(int id) {
            var post = applicationDbContext.Post.Find(id);
            applicationDbContext.Post.Remove(post);
            await applicationDbContext.SaveChangesAsync();
        }

        public async void UpdatePost(Post post) {
            applicationDbContext.Post.Update(post);
            await applicationDbContext.SaveChangesAsync();
        }

        public ValueTask<Post> GetPost(int? id) {
            return applicationDbContext.Post.FindAsync(id);
        }

        public ValueTask<Post> GetPost(int id) {
            return applicationDbContext.Post.FindAsync(id);
        }

        public Task<List<Post>> GetAllPosts() {
            return applicationDbContext.Post.ToListAsync();
        }

        public Task<Post[]> GetPostsByUser(string username) {
            return applicationDbContext.Post.Where(post => post.Author == username).ToArrayAsync();
        }

        //check if post belongs to author
        public bool IsPostByAuthor(string username, int postId) {
            var post = applicationDbContext.Post.Find(postId);
            return post.Author == username;
        }

        //get top 5 recently updated posts
        public Post[] GetLatestFiveUpdatedPosts() {
            return applicationDbContext.Post.OrderByDescending(post => post.CreatedAt).Take(5).ToArray();
        }
    }
}
