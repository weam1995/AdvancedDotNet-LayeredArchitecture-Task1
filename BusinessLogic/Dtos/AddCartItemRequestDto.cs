using CartServiceApp.DataAccess.Entities;
using System.ComponentModel.DataAnnotations;

namespace CartServiceApp.BusinessLogic.Dtos
{
    public class AddCartItemRequestDto
    {
        [Required]
        public string CartId { get; set; }

        [Required]
        public CartItem CartItem { get; set; }
    }
}
