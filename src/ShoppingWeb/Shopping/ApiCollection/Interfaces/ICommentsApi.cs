using Shopping.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopping.ApiCollection.Interfaces
{
    public interface ICommentsApi
    {
        Task<bool> AddComment(Comment comment);
        Task<IEnumerable<Comment>> GetComments(string productId);
    }
}
