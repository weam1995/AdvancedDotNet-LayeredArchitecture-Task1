
using Asp.Versioning;
using CartServiceApp.BusinessLogic.Dtos;
using CartServiceApp.BusinessLogic.Interfaces;
using CartServiceApp.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CartServiceApp.Controllers.V2
{
    [ApiVersion("2.0")]
    [ApiController]
    [Route("/api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]

    public class CartsController(ICartService cartService) : ControllerBase
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
            bool result = cartService.DeleteCartItem(new Guid(cartId), cartItemId);
            if (!result) return NotFound();
            else return Ok();

        }
    }
}
