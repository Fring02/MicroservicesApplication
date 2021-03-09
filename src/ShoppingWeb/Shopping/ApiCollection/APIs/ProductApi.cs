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

namespace Shopping.ApiCollection.APIs
{
    public class ProductApi : BaseHttpClientFactory, IProductApi
    {
        private readonly IApiSettings _settings;
        public ProductApi(IHttpClientFactory factory, IApiSettings settings) : base(factory)
        {
            _settings = settings;
            _builder = new HttpRequestBuilder(_settings.BaseAddress);
            _builder.AddToPath(_settings.CatalogPath);
        }
        public async Task CreateProduct(Product product)
        {
            using var message = _builder
           .HttpMethod(HttpMethod.Post).
           Content(new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json"))
           .GetHttpMessage();
           await GetResponseAsync<Product>(message);
        }

        public async Task<bool> DeleteProduct(string id)
        {
            using var message = _builder.AddToPath(id)
            .HttpMethod(HttpMethod.Delete)
            .GetHttpMessage();
           var response = await GetResponseStringAsync(message);
            return response != null;
        }

        public async Task<IEnumerable<Product>> GetFilteredProducts(string productName)
        {
            using var message = _builder.AddQueryString("productName", productName)
              .HttpMethod(HttpMethod.Get)
              .GetHttpMessage();
            return await GetResponseAsync<IEnumerable<Product>>(message);
        }

        public async Task<Product> GetProduct(string id)
        {
            using var message = _builder.AddToPath(id)
             .HttpMethod(HttpMethod.Get)
             .GetHttpMessage();
            return await GetResponseAsync<Product>(message);
        }

        public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
        {
            using var message = _builder.AddToPath("GetProductsByCategory/" + categoryName)
              .HttpMethod(HttpMethod.Get)
              .GetHttpMessage();
            return await GetResponseAsync<IEnumerable<Product>>(message);
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            using var message = _builder.AddToPath(name)
              .HttpMethod(HttpMethod.Get)
              .GetHttpMessage();
            return await GetResponseAsync<IEnumerable<Product>>(message);
        }

        public async Task<IEnumerable<Product>> GetProductByPage(int page)
        {
            if (_builder != null) _builder.Dispose();
            using (_builder = new HttpRequestBuilder(_settings.BaseAddress).AddToPath(_settings.CatalogPath))
            {
              var message = _builder.AddQueryString("page", page.ToString())
              .HttpMethod(HttpMethod.Get)
              .GetHttpMessage();
                return await GetResponseAsync<IEnumerable<Product>>(message);
            }
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            using var message = _builder
              .HttpMethod(HttpMethod.Get)
              .GetHttpMessage();
            return await GetResponseAsync<IEnumerable<Product>>(message);
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            if (_builder != null) _builder.Dispose();
            using (_builder = new HttpRequestBuilder(_settings.BaseAddress).AddToPath(_settings.CatalogPath)) {
             var message = _builder.HttpMethod(HttpMethod.Put).Content(new StringContent(JsonConvert.SerializeObject(product),
                Encoding.UTF8, "application/json"))
              .GetHttpMessage();
                var response = await GetResponseAsync<Product>(message);
                return response != null;
            }
        }
    }
}
