using Shopping.ApiCollection.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopping.ApiCollection.Interfaces
{
    public interface IApiFactory
    {
        IBasketApi BasketApi { get; }
        IUsersApi UsersApi { get; }
        IOrderApi OrderApi { get; }
        IProductApi ProductApi { get; }
        ICommentsApi CommentsApi { get; }
    }
}
