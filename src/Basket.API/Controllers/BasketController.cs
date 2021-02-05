using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Basket.API.Entities;
using Basket.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
    [Route("api/v1/[controller]/")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _repos;

        public BasketController(IBasketRepository repos)
        {
            _repos = repos;
        }

        [HttpGet]
        public async Task<IActionResult> GetBasket(string username)
        {
            if (string.IsNullOrEmpty(username)) return NotFound();
            var basket = await _repos.GetCart(username);
            return Ok(basket ?? new BasketCart() { Username = username});
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddItemToBasket(BasketCartItem item, string username)
        {
            if (string.IsNullOrEmpty(username)) return BadRequest("Couldn't find basket");
            var basket = await _repos.AddItem(username, item);
            if (basket == null) return StatusCode(500, "Failed to add item to basket");
            return Ok(basket);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteItemFromBasket(BasketCartItem item, string username)
        {
            if (string.IsNullOrEmpty(username)) return BadRequest("Couldn't find basket");
            if (await _repos.DeleteItem(username, item))
                return Ok("Deleted item from basket");
            
            return StatusCode(500, "Failed to delete item from basket");
        }


        [HttpDelete("{username}")]
        public async Task<IActionResult> DeleteBasket(string username)
        {
            if (string.IsNullOrEmpty(username)) return BadRequest("Couldn't find basket");
            if (await _repos.DeleteCart(username)) return Ok("Deleted item from basket");
            return StatusCode(500, "Failed to delete item from basket");
        }
    }
}
