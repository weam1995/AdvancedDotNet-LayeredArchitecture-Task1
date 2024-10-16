using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartServiceApp.DataAccess.Entities
{
    public class Cart
    {
        public Guid Id { get; set; } // Guid instead of int
        public List<CartItem> Items { get; set; } = [];
    }
}
