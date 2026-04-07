using Microsoft.AspNetCore.Identity;

namespace Liquido.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }

        public int LoyaltyPoints { get; set; } = 0;


        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;

        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    }
}
