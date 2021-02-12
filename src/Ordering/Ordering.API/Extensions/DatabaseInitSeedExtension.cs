using Microsoft.Extensions.Hosting;
using Ordering.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Ordering.API.Extensions
{
    public static class DatabaseInitSeedExtension
    {
        public static IHost ApplyMigrations(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<OrderContext>();
                context.Database.Migrate();
            }
            return host;
        }
    }
}
