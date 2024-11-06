using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartServiceApp.DataAccess.Entities
{
    public class Cart
    {
        public Guid Id { get; set; }
        public List<CartItem> Items { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Cart other)
            {
                return this.Id == other.Id &&
                       this.Items == other.Items;
            }
            return false;
        }
    }
}
