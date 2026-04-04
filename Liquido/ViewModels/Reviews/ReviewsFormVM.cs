using System.ComponentModel.DataAnnotations;

namespace Liquido.ViewModels.Reviews
{
    public class ReviewsFormVM
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Please select a rating")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        [Display (Name = "Rating (1-5)")]
        public int Rating { get; set; }

        [StringLength(1000, ErrorMessage = "Comment cannot exceed 1000 characters.")]
        [Display(Name = "Comment (optional)")]
        public string Comment { get; set; } = string.Empty;
    }
}
