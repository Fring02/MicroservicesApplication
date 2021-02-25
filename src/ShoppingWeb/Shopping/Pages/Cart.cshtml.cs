using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shopping.ApiCollection.Interfaces;
using Shopping.Entities;

namespace Shopping.Pages
{
    public class CartModel : PageModel
    {
        private readonly IBasketApi _api;
        private string username;
        public CartModel(IApiFactory factory)
        {
            _api = factory.BasketApi ?? throw new ArgumentNullException(nameof(_api));
        }

        public Cart Cart { get; set; } = new Cart();        
        
        public async Task<IActionResult> OnGetAsync()
        {
            username = HttpContext.Session.GetString("username");
            Cart = await _api.GetCart(username);           
            return Page();
        }

        public async Task<IActionResult> OnPostRemoveToCartAsync(string cartItemId)
        {
            username = HttpContext.Session.GetString("username");
            Cart = await _api.GetCart(username);
            await _api.DeleteItem(Cart.Username, Cart.Items.Find(i => i.ProductId == cartItemId));
            return RedirectToPage();
        }
    }
}