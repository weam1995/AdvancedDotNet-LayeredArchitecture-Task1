using CartServiceApp.BusinessLogic.Dtos;
using CartServiceApp.BusinessLogic.Interfaces;
using CartServiceApp.Controllers;
using CartServiceApp.Controllers.V1;
using CartServiceApp.DataAccess.Entities;
using CartServiceApp.DataAccess.Enums;
using CartServiceApp.DataAccess.ValueObjects;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;
using Xunit;

namespace CartServiceApp.Tests.Controllers
{
    public class CartControllerV1Tests
    {
        private readonly Mock<ICartService> _cartService;
        private readonly CartsController _cartsController;
        public CartControllerV1Tests()
        {
            _cartService = new Mock<ICartService>();
            _cartsController = new CartsController(_cartService.Object);
        }
        public static List<CartItem> GetCartItems()
        {
            return
        [
            new CartItem
            {
                Id = 1,
                Name = "Product A",
                Price = new Money(19.99M, CurrencyCode.EUR),
                Quantity = 2
            },
            new CartItem
            {
                Id = 2,
                Name = "Product B",
                Image = new Image { URL = "http://productBImage.com", Text = "Product B image"},
                Price = new Money(29.99M, CurrencyCode.USD),
                Quantity = 1
            },
            new CartItem
            {
                Id = 3,
                Name = "Product C",
                Price = new Money(9.99M, CurrencyCode.USD),
                Quantity = 3
            }
        ];
        }
        [Fact]
        public void GetCartInfo_NonExistingId_ReturnsNotFound()
        {
            _cartService.Setup(x => x.GetCart(It.IsAny<Guid>())).Returns(new Cart());
            var result = _cartsController.GetCartInfo("74ff23a4-a23b-4922-8643-f6be34b92215");
            result.ShouldNotBeNull();
            result.ShouldBeOfType(typeof(NotFoundResult));
        }

        [Fact]
        public void GetCartInfo_SuccessfullyReturnsCartModel()
        {
            _cartService.Setup(x => x.GetCart(It.IsAny<Guid>())).Returns(new Cart() { Id = new Guid("74ff23a4-a23b-4922-8643-f6be34b92215"), Items = GetCartItems() });
            var result = _cartsController.GetCartInfo("74ff23a4-a23b-4922-8643-f6be34b92215");
            result.ShouldNotBeNull();
            result.ShouldBeOfType(typeof(OkObjectResult));

            var okResult = result as OkObjectResult;
            okResult?.Value.ShouldBeOfType(typeof(Cart));
        }
        //[Fact]
        //public void AddCartItem_IncorrectModel_ShouldReturnBadRequest()
        //{
        //    var addCartItemRequest = new AddCartItemRequestDto() { CartId = "74ff23a4-a23b-4922-8643-f6be34b92215"};
        //    var result = _cartsController.AddCartItem(addCartItemRequest);
        //    result.ShouldBeOfType(typeof(BadRequestResult));

        //}
    }
}
