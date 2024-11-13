using Asp.Versioning;
using CartServiceApp.BusinessLogic.Dtos;
using CartServiceApp.BusinessLogic.Interfaces;
using CartServiceApp.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
namespace CartServiceApp.Controllers.V1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("/api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]

    public class CartsController(ICartService cartService) : ControllerBase
    {
        /// <summary>
        /// Returns Cart ID along with the related list of cart items
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A Cart</returns>
        /// <response code = "200">A cart</response>
        /// <response code = "404">A cart was not found for the given Id</response>
        
        [HttpGet("{id}")]
        //[MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(Cart), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCartInfo(string id)
        {
            var cart = cartService.GetCart(new Guid(id));
            if (cart.Equals(new Cart())) return NotFound();
            return Ok(cart);
        }
        /// <summary>
        /// Adds a cart item to the cart if it exists or creates a new cart otherwise
        /// </summary>
        /// <param name="request"></param>
        /// <response code = "200"> if the cart and item are added successfully </response>
        /// <response code = "400"> if invalid request object </response>
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //[MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddCartItem([FromBody] AddCartItemRequestDto request)
        {    if (!ModelState.IsValid) return BadRequest();
            cartService.AddCartItem(new Guid(request.CartId), request.CartItem);
            return Ok();
        }

        [HttpDelete("{cartId}/{cartItemId}")]
        //[MapToApiVersion("1.0")]
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
