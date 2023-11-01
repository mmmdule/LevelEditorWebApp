using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LevelEditorWebApp.Models;

namespace LevelEditorWebApp.Data {
    public class ApplicationDbContext : IdentityDbContext {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) {
        }
        public DbSet<LevelEditorWebApp.Models.Post>? Post { get; set; }
    }
}