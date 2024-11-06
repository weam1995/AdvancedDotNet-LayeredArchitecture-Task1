using Asp.Versioning;
using CartServiceApp.BusinessLogic.Dtos;
using CartServiceApp.BusinessLogic.Interfaces;
using CartServiceApp.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace CartServiceApp.Controllers
{
    [ApiController]
    [Route("/api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiVersion("2.0")]
    public class CartsControllerV2(ICartService cartService) : ControllerBase
    {
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(List<CartItem>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCartInfo(string id)
        {
            var cartItems = cartService.GetCartItems(new Guid(id));
            if (cartItems is null) return NotFound();
            return Ok(cartItems);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult AddCartItem([FromBody] AddCartItemRequestDto request)
        {
            if (ModelState.IsValid)
            {
                cartService.AddCartItem(new Guid(request.CartId), request.CartItem);
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("{cartId}/{cartItemId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteCartItem(string cartId, int cartItemId)
        {
            bool result =  cartService.DeleteCartItem(new Guid(cartId), cartItemId);
            if (!result) return NotFound();
            else return Ok();
                
        }
    }
}
