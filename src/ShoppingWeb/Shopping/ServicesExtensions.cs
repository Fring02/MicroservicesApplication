using Microsoft.Extensions.DependencyInjection;
using Shopping.ApiCollection.APIs;
using Shopping.ApiCollection.Interfaces;

namespace Shopping
{
    public static class ServicesExtensions
    {
        public static void AddAPIs(this IServiceCollection services)
        {
            services.AddScoped<IBasketApi, BasketApi>();
            services.AddScoped<IOrderApi, OrderApi>();
            services.AddScoped<IProductApi, ProductApi>();
            services.AddScoped<IUsersApi, UsersApi>();

            services.AddScoped<IApiFactory, ApiFactory>();
        }
    }
}
