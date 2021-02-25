using AuthorizationService.Data;
using AuthorizationService.Models;
using AuthorizationService.Repositories.Interfaces;
using AuthorizationService.Services;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AuthorizationService.Repositories
{
    public class UsersRepository : ModelRepository<User>, IUsersRepository
    {
        public UsersRepository(ModelContext context) : base(context)
        {
        }

        public async Task<User> Login(User model, string password)
        {
            var user = await Context
                .Users.FirstOrDefaultAsync(m => m.Username == model.Username);
            if(user != null)
            {
                if (EncryptionService.CheckPassword(user, password)) return user;
            }
            return null;
        }

        public async Task<User> Register(User model, string password)
        {
            EncryptionService.EncryptPassword(password, out byte[] hashed, out byte[] salt);
            model.HashedPassword = hashed;
            model.SaltPassword = salt;
            if (await base.Create(model))
            {
                return model;
            }
            return null;
        }

        public async Task<bool> UserExists(User model)
        {
            return await Context.Users.AnyAsync(u => u.Username == model.Username);
        }
    }
}
