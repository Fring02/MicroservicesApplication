using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using Shopping.ApiCollection.APIs;
using Shopping.ApiCollection.Interfaces;
using Shopping.Protos;
using System;

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
            services.AddScoped<ICommentsApi, CommentsApi>();
            services.AddScoped<IApiFactory, ApiFactory>();
        }

        public static void AddGrpcClient(this IServiceCollection services, string address)
        {
            
            services.AddSingleton(s => {
                AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
                var channel = GrpcChannel.ForAddress(address);
                var client = new CommentService.CommentServiceClient(channel);
                return client;
            });
        }
    }
}
