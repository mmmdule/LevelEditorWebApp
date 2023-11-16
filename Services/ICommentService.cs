using LevelEditorWebApp.Classes;

namespace LevelEditorWebApp.Services {
    public interface ICommentService {
        Task<List<Comment>> GetCommentsByPostId(int postId);
        Task<Comment> GetCommentById(int commentId);
        Task CreateComment(Comment comment);
        Task UpdateComment(Comment comment);
        Task DeleteComment(int commentId);
        bool CommentIsValid(Comment comment);
    }
}
