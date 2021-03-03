using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CommentServer.Dtos
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        [Required]
        public string Username { get; set; }
        public DateTime LeavedAt { get; set; } = DateTime.Now;
        [Required]
        public string Description { get; set; }
        [Required]
        public string ProductId { get; set; }
    }
}
