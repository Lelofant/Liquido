using Liquido.Models.Enums;

namespace Liquido.Models
{
    public class Subscription
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;

        public SubscriptionFrequency Frequency { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime NextDeliveryDate { get; set; }
        public string ShippingAddress { get; set; } = string.Empty;
        public ICollection<SubscriptionItem> Items { get; set; } = new List<SubscriptionItem>();
    }
}
