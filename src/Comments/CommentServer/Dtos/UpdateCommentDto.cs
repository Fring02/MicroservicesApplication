using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommentServer.Dtos
{
    public class UpdateCommentDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
    }
}
