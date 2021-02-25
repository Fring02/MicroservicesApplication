using AuthorizationService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationService
{
    public static class HostExtensions
    {
        public static IHost ApplyMigrations(this IHost host)
        {
            using(var scope = host.Services.CreateScope())
            {
                using var context = scope.ServiceProvider.GetRequiredService<ModelContext>();
                context.Database.Migrate();
            }
            return host;
        }
    }
}
