using CommentServer.Data;
using CommentServer.Interfaces;
using CommentServer.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommentServer.Repositories
{
    public class CommentsRepository : BaseRepository<Comment>, ICommentsRepository
    {
        public CommentsRepository(CommentContext context) : base(context)
        {

        }
        public async Task<IEnumerable<Comment>> GetCommentsByProductId(string productId)
        {
            return await _context.Comments.Where(c => c.ProductId == productId).ToListAsync();
        }
    }
}
