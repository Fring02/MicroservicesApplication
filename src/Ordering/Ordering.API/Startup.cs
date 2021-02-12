using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventBusRabbitMQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Ordering.API.Mapping;
using Ordering.API.RabbitMQ;
using Ordering.Core.Repositories;
using Ordering.Core.Repositories.Base;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Repositories;
using RabbitMQ.Client;
using Ordering.API.Extensions;
using Microsoft.EntityFrameworkCore.Design;
namespace Ordering.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<OrderContext>(options => options.UseNpgsql(Configuration["ConnectionStrings:DefaultConnection"]));
            services.AddControllers();

            #region Project Dependencies

            //services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IOrderRepository, OrderRepository>();

            services.AddAutoMapper(typeof(OrderMapping));

            #endregion

            #region RabbitMQ Dependencies
            services.AddSingleton<IRabbitMQConnection>(s =>
            {
                var factory = new ConnectionFactory() { HostName = Configuration["EventBus:HostName"] };
                factory.UserName = Configuration["EventBus:Username"];
                factory.Password = Configuration["EventBus:Password"];
                Console.WriteLine(factory.HostName + " " + factory.UserName + " " + factory.Password);
                return new RabbitMQConnection(factory);
            });
            services.AddSingleton<EventBusRabbitMQConsumer>();
            #endregion

            #region Swagger Dependencies

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Order API", Version = "v1" });
            });

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseRabbitListener();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
