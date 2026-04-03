using System.ComponentModel.DataAnnotations;

namespace Liquido.ViewModels.Account
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Display(Name = "Email_Address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Remember_Me")]
        public bool RememberMe { get; set; }
    }
}
