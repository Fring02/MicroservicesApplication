using CommentServer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CommentServer
{
    public static class HostExtensions
    {
        public static IHost ApplyMigrations(this IHost host)
        {
            Thread.Sleep(3000);
            using (var scope = host.Services.CreateScope())
            {
                using var context = scope.ServiceProvider.GetRequiredService<CommentContext>();
                context.Database.Migrate();
            }
            return host;
        }
    }
}
