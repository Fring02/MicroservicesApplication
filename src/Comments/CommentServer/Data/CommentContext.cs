using CommentServer.Models;
using Microsoft.EntityFrameworkCore;

namespace CommentServer.Data
{
    public class CommentContext : DbContext
    {
        public CommentContext(DbContextOptions<CommentContext> options) : base(options)
        {

        }
        public DbSet<Comment> Comments { get; set; }
    }
}
