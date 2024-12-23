using CartServiceApp.DataAccess.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace CartServiceApp.DataAccess.Entities
{
    public class CartItem
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public Image? Image { get; set; }
        [Required]
        public Money Price { get; set; }
        public int Quantity { get; set; }


    }
}
