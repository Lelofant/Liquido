using Liquido.ViewModels.Cart;
using System.ComponentModel.DataAnnotations;

namespace Liquido.ViewModels.Orders
{
    public class CheckoutVM
    {
        [Required(ErrorMessage = "Shipping address is required.")]
        [StringLength(100,MinimumLength = 10,ErrorMessage = "Please enter a full adress")]
        [Display(Name = "Shipping Address")]
        public string ShippingAddress { get; set; } = string.Empty;
        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
        [Display(Name = "Notes (optional)")]
        public string? Notes { get; set; }

        public CartVM Cart { get; set; } = new CartVM();
    }
}
