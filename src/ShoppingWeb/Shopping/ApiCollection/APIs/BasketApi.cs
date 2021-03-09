using Newtonsoft.Json;
using Shopping.API.Infrastructure;
using Shopping.ApiCollection.Interfaces;
using Shopping.ApiCollection.Settings;
using Shopping.Entities;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.ApiCollection.APIs
{
    public class BasketApi : BaseHttpClientFactory, IBasketApi
    {
        private IApiSettings _settings;

        public BasketApi(IApiSettings settings, IHttpClientFactory factory) : base(factory)
        {
            _settings = settings;
            _builder = new HttpRequestBuilder(_settings.BaseAddress);
            _builder.AddToPath(_settings.BasketPath);
        }

        public async Task<Cart> AddItem(string username, CartItem item)
        {
            using var message = _builder
            .HttpMethod(HttpMethod.Post).AddQueryString("username", username).
            Content(new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json"))
            .GetHttpMessage();
                return await GetResponseAsync<Cart>(message);
        }

        public async Task<bool> DeleteCart(string username)
        {
            if (_builder != null) _builder.Dispose();
            using (_builder = new HttpRequestBuilder(_settings.BaseAddress).AddToPath(_settings.BasketPath))
            {
                using var message = _builder.AddToPath(username)
                .HttpMethod(HttpMethod.Delete)
                .GetHttpMessage();
                var response = await GetResponseStringAsync(message);
                return response != null;
            }
        }

        public async Task<bool> DeleteItem(string username, CartItem item)
        {
            if (_builder != null) _builder.Dispose();
            using (_builder = new HttpRequestBuilder(_settings.BaseAddress).AddToPath(_settings.BasketPath))
            {
                using var message = _builder.
                    AddQueryString("username", username).
                Content(new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json"))
                .HttpMethod(HttpMethod.Delete)
                .GetHttpMessage();
                var response = await GetResponseStringAsync(message);
                return response != null;
            }
        }

        public async Task<Cart> GetCart(string username)
        {
            using var message = _builder.AddQueryString("username", username)
             .HttpMethod(HttpMethod.Get)
             .GetHttpMessage();
            return await GetResponseAsync<Cart>(message);
        }
    }
}
