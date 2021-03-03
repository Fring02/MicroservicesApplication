using Shopping.ApiCollection.Interfaces;

namespace Shopping.ApiCollection.APIs
{
    public class ApiFactory : IApiFactory
    {
        private IBasketApi _basketApi;
        private IOrderApi _orderApi;
        private IUsersApi _usersApi;
        private IProductApi _productApi;
        private ICommentsApi _commentsApi;
        public ApiFactory(IBasketApi basketApi, IOrderApi orderApi, IUsersApi usersApi, IProductApi productApi, ICommentsApi commentsApi)
        {
            _basketApi = basketApi;
            _orderApi = orderApi;
            _usersApi = usersApi;
            _productApi = productApi;
            _commentsApi = commentsApi;
        }

        public IBasketApi BasketApi
        {
            get => _basketApi;
        }

        public IUsersApi UsersApi => _usersApi;

        public IOrderApi OrderApi => _orderApi;

        public IProductApi ProductApi => _productApi;
        public ICommentsApi CommentsApi => _commentsApi;
    }
}
