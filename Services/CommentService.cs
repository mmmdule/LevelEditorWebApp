using LevelEditorWebApp.Classes;
using LevelEditorWebApp.Data;
using Microsoft.EntityFrameworkCore;

namespace LevelEditorWebApp.Services {
    public class CommentService : ICommentService {
        private readonly ApplicationDbContext _context;
        public CommentService(ApplicationDbContext context) {
            _context = context;
        }

       public async Task CreateComment(Comment comment) {
            _context.Comment.Add(comment);
            await _context.SaveChangesAsync();
        }

        public bool CommentIsValid(Comment comment) {
            //check if all comment fields are valid
            return comment.Content is not null && comment.Content is not "" &&
                   comment.Username is not "" && comment.Username is not null &&
                   comment.UserId is not "" && comment.UserId is not null;
        }

        public async Task DeleteComment(int commentId) {
            Comment comment = _context.Comment.Find(commentId);

            if(comment != null) {
                comment.Content = "[deleted]";
                comment.Username = "[deleted]";
                comment.UserId = "-1";
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Comment> GetCommentById(int commentId) {
            return await _context.Comment.FindAsync(commentId);
        }

        public async Task<List<Comment>> GetCommentsByPostId(int postId) {
            return await _context.Comment.Where(c => c.PostId == postId).ToListAsync();
        }

        public async Task UpdateComment(Comment comment) {
            Comment dbComment = _context.Comment.Find(comment.Id);
            dbComment.Content = comment.Content;
            dbComment.CreatedAt = comment.CreatedAt;

            await _context.SaveChangesAsync();
        }
    }
}
