using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shopping.ApiCollection.Interfaces;
using Shopping.Entities;

namespace Shopping.Pages
{
    public class CheckOutModel : PageModel
    {
        private readonly IBasketApi _basketApi;
        private readonly IOrderApi _orderApi;

        public CheckOutModel(IBasketApi basketApi, IOrderApi orderApi)
        {
            _basketApi = basketApi ?? throw new ArgumentNullException(nameof(basketApi));
            _orderApi = orderApi ?? throw new ArgumentNullException(nameof(orderApi));
        }

        [BindProperty]
        public Order Order { get; set; }

        public Cart Cart { get; set; } = new Cart();

        public async Task<IActionResult> OnGetAsync()
        {
            Cart = await _basketApi.GetCart("test");
            return Page();
        }

        public async Task<IActionResult> OnPostCheckOutAsync()
        {
            Cart = await _basketApi.GetCart("test");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            Order.UserName = "test";
            Order.TotalPrice = Cart.TotalPrice;

            await _orderApi.Checkout(Order);
            await _basketApi.DeleteCart("test");
            
            return RedirectToPage("Confirmation", "OrderSubmitted");
        }       
    }
}