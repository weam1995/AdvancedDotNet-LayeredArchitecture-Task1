using CartServiceApp.BusinessLogic.Services;
using LiteDB;

namespace CartServiceApp.Tests
{
    public class CartServiceTests
    {
        private LiteDatabase _db;

        private CartService _cartService; // use ICartService
        public CartServiceTests()
        {
            _db = new LiteDatabase(new MemoryStream());
            _cartService = new CartService(_db);
        }
        //[Fact]
        //public void AddItemToCart_ShouldAddSuccessfully()
        //{
        //    //Arrange
        //    Cart cart = new Cart() { Id = new Guid(), Items = [] };
        //    CartItem cartItem = new CartItem() { Id = 1, Name = "Iphone 16 ProMax", Price = new DataAccess.ValueObjects.Money(500.9m,CatalogService.Domain.Enums.CurrencyCode.PLN), Quantity = 1 };
        //    //Act
        //    _cartService.AddCartItem(cart.Id, cartItem);
        //    var cartItems = _cartService.GetCartItems(cart.Id);
        //    //Assert
        //    Assert.Single(cartItems);
        //    Assert.Equal(500.9m, cartItems[0].Price.Value);

        //}
    }
}
