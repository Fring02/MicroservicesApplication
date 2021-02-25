using AuthorizationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationService.Repositories.Interfaces
{
    public interface IUsersRepository : IModelRepository<User>
    {
        Task<User> Register(User model, string password);
        Task<User> Login(User model, string password);
        Task<bool> UserExists(User model);
    }
}
