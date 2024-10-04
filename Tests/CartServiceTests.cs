using CartServiceApp.BusinessLogic.Services;
using CartServiceApp.DataAccess.Entities;
using LiteDB;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Xunit;

namespace CartServiceApp.Tests
{
    public class CartServiceTests
    {
        private LiteDatabase _db;

        private CartService _cartService;
        public CartServiceTests()
        {
            _db = new LiteDatabase(new MemoryStream());
            _cartService = new CartService(_db);
        }
        [Fact]
        public void AddItemToCart_ShouldAddSuccessfully()
        {
            //Arrange
            Cart cart = new Cart() { Id = 1, Items = [] };
            CartItem cartItem = new CartItem() { Id = 1, Name = "Iphone 16 ProMax", Price = 5000, Quantity = 1 };
            //Act
            _cartService.AddCartItem(cart, cartItem);
            var cartItems = _cartService.GetCartItems(cart.Id);
            //Assert
            Assert.Single(cartItems);
            Assert.Equal(5000, cartItems[0].Price);

        }
    }
}
