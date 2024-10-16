using CartServiceApp.BusinessLogic.Interfaces;
using CartServiceApp.DataAccess;
using CartServiceApp.DataAccess.Entities;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartServiceApp.BusinessLogic.Services
{
    public class CartService : ICartService
    {
        private readonly LiteDatabase _liteDb;
        private readonly ILiteCollection<Cart> _cartCollection;
        private const string CartDataCollectionName = "Cart";
        public CartService(LiteDatabase liteDb)
        {
            _liteDb = liteDb;
            _cartCollection = _liteDb.GetCollection<Cart>(CartDataCollectionName);
        }
        public CartService(ILiteDbContext liteDbContext)
        {
            _liteDb = liteDbContext.Database;
            _cartCollection = _liteDb.GetCollection<Cart>(CartDataCollectionName);
        }
        public void AddCartItem(Cart cart, CartItem cartItem)
        {
            var existingCart = _cartCollection.FindOne(x => x.Id == cart.Id);
            
            if(existingCart != null)
            {
                var existingCartItem = existingCart.Items.FirstOrDefault(x => x.Id == cartItem.Id);
                if (existingCartItem != null)
                    existingCartItem.Quantity = existingCartItem.Quantity + cartItem.Quantity;
                else
                    existingCart.Items.Add(cartItem);
                _cartCollection.Update(existingCart);
            }
            else
            {
                Guid cartId = _cartCollection.Insert(cart);
                var addedCart = _cartCollection.FindById(cartId);
                
                addedCart.Items.Add(cartItem);
                _cartCollection.Update(addedCart);
            };
        }

        public bool DeleteCartItem(Guid cartId, CartItem cartItem)
        {
            var cart = _cartCollection.FindById(cartId);
            return cart == null ? throw new ArgumentNullException($"No Cart exists with id {cartId}") : cart.Items.Remove(cartItem);
        }

        public List<CartItem> GetCartItems(Guid cartId)
        {
            return _cartCollection.FindById(cartId).Items.ToList();
        }
    }
}
