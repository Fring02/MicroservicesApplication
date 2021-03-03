using Comments.API.Data;
using Comments.API.Interfaces;
using Comments.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Comments.API.Repositories
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
