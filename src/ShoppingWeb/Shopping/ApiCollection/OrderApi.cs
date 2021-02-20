using Newtonsoft.Json;
using Shopping.API.Infrastructure;
using Shopping.ApiCollection.Interfaces;
using Shopping.ApiCollection.Settings;
using Shopping.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.ApiCollection
{
    public class OrderApi : BaseHttpClientFactory, IOrderApi
    {
        private IApiSettings _settings;
        public OrderApi(IHttpClientFactory factory, IApiSettings settings) : base(factory)
        {
            _settings = settings;
            _builder = new HttpRequestBuilder(_settings.BaseAddress);
            _builder.SetPath(_settings.OrderingPath);
        }

        public async Task Checkout(Order order)
        {
            _builder.SetPath(_settings.BasketPath).AddToPath("/Checkout");
            using var message = _builder.Content(new StringContent(JsonConvert.SerializeObject(order), Encoding.UTF8, "application/json"))
            .HttpMethod(HttpMethod.Post)
            .GetHttpMessage();
            await GetResponseStringAsync(message);
        }

        public async Task<IEnumerable<Order>> GetOrdersByUsername(string username)
        {
            using var message = _builder.AddQueryString("username", username)
            .HttpMethod(HttpMethod.Get)
            .GetHttpMessage();
            return await GetResponseAsync<IEnumerable<Order>>(message);
        }
    }
}
