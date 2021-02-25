using AuthorizationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AuthorizationService.Services
{
    public static class EncryptionService
    {
        public static void EncryptPassword(string password, out byte[] hashed, out byte[] salt)
        {
            using var hmac = new HMACSHA512();
            salt = hmac.Key;
            hashed = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
        public static bool CheckPassword(User user, string password)
        {
            using var hmac = new HMACSHA512(user.SaltPassword);
            return hmac.ComputeHash(Encoding.UTF8.GetBytes(password)).SequenceEqual(user.HashedPassword);
        }
    }
}
