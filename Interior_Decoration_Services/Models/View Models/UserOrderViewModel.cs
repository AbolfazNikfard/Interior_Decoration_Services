﻿

using Interior_Decoration_Services.Enum;

namespace Interior_Decoration_Services.Models.View_Models
{
    public class UserOrderViewModel
    {
        public int orderId { get; set; }
        public string orderDescription { get; set; }
        public int productId { get; set; }      
        public string productName { get; set; }
        public string productImage { get; set; }
        public int productPrice { get; set; }
        public string productColor { get; set; }
        public string productMaterial { get; set; }
        public DateTime registerDateTime { get; set; }
        public OrderStatus OrderStat { get; set; }
    }
}
