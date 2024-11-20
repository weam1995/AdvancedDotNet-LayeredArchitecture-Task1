using CartServiceApp.BusinessLogic.Interfaces;
using CartServiceApp.DataAccess;
using CartServiceApp.DataAccess.Entities;
using KafkaDemo.Events;
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
        public Cart GetCart(Guid id)
        {
            return _cartCollection.FindById(id) ?? new Cart();
        }
        public void AddCartItem(Guid cartId, CartItem cartItem)
        {
            var existingCart = _cartCollection.FindOne(x => x.Id == cartId);
            
            if(existingCart is not null)
            {
                var existingCartItem = existingCart.Items.FirstOrDefault(x => x.Id == cartItem.Id);
                if (existingCartItem != null)
                    existingCartItem.Quantity += cartItem.Quantity;
                else
                    existingCart.Items.Add(cartItem);
                _cartCollection.Update(existingCart);
            }
            else
            {
                var newCart = new Cart() { Id = cartId, Items = [cartItem] };
                _cartCollection.Insert(newCart);
            };
        }
        public void UpdateCartItem(Guid cartId, ProductChangedEvent productChangedEvent)
        {
            var existingCart = _cartCollection.FindOne(x => x.Id == cartId);
            if(existingCart is not null)
            {
                var existingCartItem = existingCart.Items.FirstOrDefault(x => x.Id == productChangedEvent.Id);
                if(existingCartItem != null)
                {
                    var newCartItem = GetUpdatedCartItem(existingCartItem, productChangedEvent);
                    if (existingCartItem != null)
                    {
                        existingCartItem.Name = newCartItem.Name;
                        existingCartItem.Image = newCartItem.Image;
                        existingCartItem.Price = newCartItem.Price;
                        _cartCollection.Update(existingCart);
                    }
                }
            }
        }

        public void UpdateCartsItems(ProductChangedEvent productChangedEvent)
        {
            var carts = _cartCollection.FindAll().Where(x => x.Items.Any(y => y.Id == productChangedEvent.Id));
            foreach(var cart in carts) { 
                UpdateCartItem(cart.Id, productChangedEvent);
            }
        }

        private CartItem GetUpdatedCartItem(CartItem existingCartItem, ProductChangedEvent productChangedEvent)
        {
            var newCartItem = new CartItem()
            {
                Id = productChangedEvent.Id,
                Name = productChangedEvent.Name ?? existingCartItem.Name,
                Image = new DataAccess.ValueObjects.Image() { URL = productChangedEvent.ImageURL ?? existingCartItem.Image.URL, Text = existingCartItem.Image?.Text ?? string.Empty },
                Price = new CartServiceApp.DataAccess.ValueObjects.Money(productChangedEvent.Price.Value, (CartServiceApp.DataAccess.Enums.CurrencyCode)productChangedEvent.Price.Currency),
                Quantity = existingCartItem.Quantity
            };
            return newCartItem;
        }

        public bool DeleteCartItem(Guid cartId, int cartItemId)
        {
            var cart = _cartCollection.FindById(cartId);
            if(cart != null)
            {
                var cartItem = cart.Items.Where(x=>x.Id == cartItemId).FirstOrDefault();
                if (cartItem is null) return false;
                else
                {
                 return cart.Items.Remove(cartItem);
           
                }
            }
            return false;
        }

        public List<CartItem>? GetCartItems(Guid cartId)
        {
            var cart = _cartCollection.Find(x => x.Id == cartId).FirstOrDefault();
            if (cart != null)
                return cart.Items;
            else
                return null;
        }
    }
}
