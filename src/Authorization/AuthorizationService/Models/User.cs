using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationService.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public byte[] HashedPassword { get; set; }
        public byte[] SaltPassword { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
    }
}
