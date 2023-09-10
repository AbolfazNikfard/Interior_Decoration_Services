

using Interior_Decoration_Services.Enum;

namespace Interior_Decoration_Services.Models
{
    public class Order
    {

        public int Id { get; set; }
        public string? Description { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public DateTime createdAt { get; set; }
        //Navigation Property
        public int productId { get; set; }
        public Product product { get; set; }
        public int buyerId { get; set; }
        public Buyer buyer { get; set; }
    }
}
