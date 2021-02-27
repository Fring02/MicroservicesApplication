using AuthorizationService.Data;
using AuthorizationService.Models;
using AuthorizationService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;

namespace AuthorizationService
{
    public static class HostExtensions
    {
        public static IHost ApplyMigrations(this IHost host)
        {
            Thread.Sleep(3000);
            using(var scope = host.Services.CreateScope())
            {
                using var context = scope.ServiceProvider.GetRequiredService<ModelContext>();
                context.Database.Migrate();
                if (!context.Users.Any(u => u.Username == "Fring01"))
                {
                    EncryptionService.EncryptPassword("qwerty123", out byte[] hashed, out byte[] salt);
                    var user = new User
                    {
                        Role = "admin",
                        HashedPassword = hashed,
                        SaltPassword = salt,
                        Lastname = "Khassenov",
                        Firstname = "Sultanbek",
                        Email = "hasenovsultanbek@gmail.com",
                        Id = Guid.NewGuid(),
                        Username = "Fring01"
                    };
                    context.Users.Add(user);
                    context.SaveChanges();
                }
            }
            return host;
        }
    }
}
