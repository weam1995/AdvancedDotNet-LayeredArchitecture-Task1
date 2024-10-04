using CartServiceApp.DataAccess.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartServiceApp.DataAccess.Entities
{
    public class CartItem
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public Image? Image { get; set; }
        [Required]
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
