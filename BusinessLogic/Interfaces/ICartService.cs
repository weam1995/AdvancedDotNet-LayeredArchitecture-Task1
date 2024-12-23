using CartServiceApp.DataAccess.Entities;
using KafkaDemo.Events;

namespace CartServiceApp.BusinessLogic.Interfaces
{
    public interface ICartService
    {
        Cart GetCart(Guid id);
        List<CartItem> GetCartItems(Guid cartId);
        void AddCartItem(Guid cartId, CartItem cart);
        bool DeleteCartItem(Guid cartId, int cartItemId);
        void UpdateCartsItems(ProductChangedEvent productChangedEvent);
    }
}
