using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Basket.API.Entities;
using Basket.API.Repositories.Interfaces;
using EventBusRabbitMQ.Common;
using EventBusRabbitMQ.Events;
using EventBusRabbitMQ.Producers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Basket.API.Controllers
{
    [Route("api/v1/[controller]/")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _repos;
        private readonly ILogger<BasketController> _logger;
        private readonly EventBusRabbitMQProducer _producer;
        private readonly IMapper _mapper;
        public BasketController(IBasketRepository repos, ILogger<BasketController> logger, EventBusRabbitMQProducer producer,
            IMapper mapper)
        {
            _repos = repos;
            _logger = logger;
            _producer = producer;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetBasket(string username)
        {
            var basket = await _repos.GetCart(username);
            return Ok(basket ?? new BasketCart() { Username = username });
        }

        [HttpPost]
        public async Task<IActionResult> AddItemToBasket(BasketCartItem item, string username)
        {
            if (string.IsNullOrEmpty(username)) return BadRequest("Couldn't find basket");
            var basket = await _repos.AddItem(username, item);
            if (basket == null) return StatusCode(500, "Failed to add item to basket");
            return Ok(basket);
        }

        [HttpDelete]
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


        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout checkout)
        {
            var basket = await _repos.GetCart(checkout.Username);
            if(basket == null)
            {
                _logger.LogError("Basket is not found for username: " + checkout.Username);
                return BadRequest();
            }
            var removed = await _repos.DeleteCart(checkout.Username);
            if (!removed)
            {
                _logger.LogError("Basket can't be removed for username: " + checkout.Username);
                return BadRequest();
            }
            var eventMessage = _mapper.Map<BasketCheckoutEvent>(checkout);
            eventMessage.RequestId = Guid.NewGuid();
            try
            {
                _producer.PublishBasketCheckout(EventBusConstants.BasketCheckoutQueue, eventMessage);
            }
            catch (Exception e)
            {
                _logger.LogError("Message can't be published for user " + checkout.Username + ": " + e.Message);
                throw;
            }
            return Accepted();
        }
    }
}
