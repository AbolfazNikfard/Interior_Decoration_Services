

using Interior_Decoration_Services.Enum;

namespace Interior_Decoration_Services.Models
{
    public class Cart
    {

        public int Id { get; set; }
        public ProductStatus Status { get; set; } = ProductStatus.Pending;
        //Navigation Property
        public int productId { get; set; }
        public Product product { get; set; }
        public int buyerId { get; set; }
        public Buyer buyer { get; set; }
    }
}
