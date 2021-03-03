using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Comments.API.Models
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public DateTime LeavedAt { get; set; }
        public string Description { get; set; }
        public string ProductId { get; set; }
    }
}
