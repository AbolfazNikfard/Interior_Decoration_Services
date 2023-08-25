

namespace Interior_Decoration_Services.Models.View_Models
{
    public class OrderViewModel
    {
        public int orderId { get; set; }
        public string userName { get; set; }
        public int productId { get; set; }
        public string productImage { get; set; }
        public string productName { get; set; }
        //public int Weight { get; set; }
        //public UnitOFMassMeasurement WeightMassUnit { get; set; }
       // public int Price { get; set; }
        //public int Number { get; set; }
        public DateTime orderDate { get; set; }
        
    }
}
