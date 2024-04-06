using ecom.Data;
using ecom.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ecom.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecom.Models
{
    public class ShoppingCart
    {
        private readonly ApplicationDbContext _context;

        public ShoppingCart(ApplicationDbContext context, IServiceProvider services)
        {
            _context = context;
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();
            session.SetString("CartId", cartId);
            Id = cartId;
        }
        
        public IEnumerable<ShoppingCartItem> ShoppingCartItems { get; set; }
        public string Id { get; set; }


        public bool AddToCart(Product product, int amount)
        {
            if (product.inStock == 0 || amount <= 0)
            {
                return false;
            }

            var shoppingCartItem = _context.ShoppingCartItems.SingleOrDefault(
                s => s.Product.Id == product.Id && s.ShoppingCartId == Id);

            if (shoppingCartItem == null)
            {
                // Create a new cart item if it doesn't exist, with amount not exceeding the in-stock quantity
                shoppingCartItem = new ShoppingCartItem
                {
                    ShoppingCartId = Id,
                    Product = product,
                    Amount = Math.Min(product.inStock, amount) // Ensure the amount does not exceed in-stock quantity
                };
                _context.ShoppingCartItems.Add(shoppingCartItem);
            }
            else
            {
                // Replace the existing amount with the new amount, ensuring it does not exceed the in-stock quantity
                shoppingCartItem.Amount = Math.Min(product.inStock, amount); // Set amount directly instead of adding
            }

            _context.SaveChanges();
            return true; // Assuming you want to return true as long as the method doesn't hit the initial checks
        }


        public int RemoveFromCart(Product product)
        {
            var shoppingCartItem = _context.ShoppingCartItems.SingleOrDefault(
                s => s.Product.Id == product.Id && s.ShoppingCartId == Id);
            int localAmount = 0;
            if (shoppingCartItem != null)
            {
                if (shoppingCartItem.Amount > 1)
                {
                    shoppingCartItem.Amount--;
                    localAmount = shoppingCartItem.Amount;
                }
                else
                {
                    _context.ShoppingCartItems.Remove(shoppingCartItem);
                }
            }

            _context.SaveChanges();
            return localAmount;
        }

        public IEnumerable<ShoppingCartItem> GetShoppingCartItems()
        {
            return ShoppingCartItems ??
                   (ShoppingCartItems = _context.ShoppingCartItems.Where(c => c.ShoppingCartId == Id)
                       .Include(s => s.Product));
        }

        public async Task<ShoppingCartItem?> GetSingleItem(Product product)
        {
           return  await _context.ShoppingCartItems.SingleOrDefaultAsync(
                s => s.Product.Id == product.Id && s.ShoppingCartId == Id);
     
        }

        public void ClearCart()
        {
            var cartItems = _context
                .ShoppingCartItems
                .Where(cart => cart.ShoppingCartId == Id);

            _context.ShoppingCartItems.RemoveRange(cartItems);
            _context.SaveChanges();
        }

        public decimal GetShoppingCartTotal()
        {
            return _context.ShoppingCartItems.Where(c => c.ShoppingCartId == Id)
                .Select(c => c.Product.Price * c.Amount).Sum();
        }
    }
}
