using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LevelEditorWebApp.Models;

namespace LevelEditorWebApp.Data {
    public class ApplicationDbContext : IdentityDbContext {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) {
        }
        public DbSet<LevelEditorWebApp.Models.Post>? Post { get; set; }
        public DbSet<LevelEditorWebApp.Classes.Comment>? Comment { get; set; }
        public DbSet<LevelEditorWebApp.Classes.UserVoteInfo>? UserVoteInfo { get; set; }
        public DbSet<LevelEditorWebApp.Classes.PostDownloadInfo>? PostDownloadInfo { get; set; }
    }
}