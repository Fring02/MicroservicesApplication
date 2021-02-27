using AuthorizationService.Models;
using AuthorizationService.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;

namespace AuthorizationService.Data
{
    public class ModelContext : DbContext
    {
        public ModelContext(DbContextOptions<ModelContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
    }
}
