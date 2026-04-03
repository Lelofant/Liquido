using System.ComponentModel.DataAnnotations;

namespace Liquido.ViewModels.Account
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        [Display(Name = "First_Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        [Display(Name = "Last_Name")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [Display(Name = "Email_Address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Password must be alteast 6 characters")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm password is required.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [Display(Name = "Confirm_Password")]    
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
