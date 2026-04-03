using System.ComponentModel.DataAnnotations;

namespace Liquido.ViewModels.Account
{
    public class ProfileVM
    {
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, MinimumLength = 2)]
        [Display(Name = "First_Name")]
        public string FirstName { get; set; } = string.Empty;


        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, MinimumLength = 2)]
        [Display(Name = "Last_Name")]
        public string LastName { get; set; } = string.Empty;

        [Display(Name = "Email_Address")]
        public string Email { get; set; } = string.Empty;

        [StringLength(300)]
        [Display(Name = "Delivery_Address")]
        public string? Address { get; set; }

        [StringLength(40)]
        [Display(Name = "City")]
        public string? City { get; set; }

        [StringLength(5)]
        [Display(Name = "Postal_Code")]
        public string? PostalCode { get; set; }

        [Phone(ErrorMessage = "Please enter a valid phone number.")]
        [Display(Name = "Phone_Number")]
        public string? PhoneNumber { get; set; }

        public int LoyaltyPoints { get; set; }
        public DateTime RegisteredAt { get; set; }
        public int TotalOrders { get; set; }

    }
}
