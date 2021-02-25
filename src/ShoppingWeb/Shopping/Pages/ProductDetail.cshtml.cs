﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shopping.ApiCollection.Interfaces;
using Shopping.Entities;

namespace Shopping.Pages
{
    public class ProductDetailModel : PageModel
    {
        private readonly IProductApi _productApi;
        private readonly IBasketApi _basketApi;

        public ProductDetailModel(IApiFactory factory)
        {
            _productApi = factory.ProductApi ?? throw new ArgumentNullException(nameof(_productApi));
            _basketApi = factory.BasketApi ?? throw new ArgumentNullException(nameof(_basketApi));
        }

        public Product Product { get; set; }

        [BindProperty]
        public string Color { get; set; }

        [BindProperty]
        public int Quantity { get; set; }

        public async Task<IActionResult> OnGetAsync(string productId)
        {
            if (string.IsNullOrEmpty(productId))
            {
                return NotFound();
            }
            Product = await _productApi.GetProduct(productId);
            if (Product == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(string productId)
        {
            string username = HttpContext.Session.GetString("username");
            if (string.IsNullOrEmpty(username)) return RedirectToPage("Login", new { loginError = "Please sign in" });
            Product = await _productApi.GetProduct(productId);
            var item = new CartItem
            {
                ProductId = productId,
                Quantity = Quantity,
                Color = Color,
                ProductName = Product.Name,
                Price = Product.Price
            };
            await _basketApi.AddItem(username, item);
            return RedirectToPage("Cart");
        }
    }
}