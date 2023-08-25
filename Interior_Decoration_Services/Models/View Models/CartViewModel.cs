

using Interior_Decoration_Services.Enum;

namespace Interior_Decoration_Services.Models.View_Models
{
    public class CartViewModel
    {
        public int productId { get; set; }      
        public string productName { get; set; }
        public string productImage { get; set; }
        public int productPrice { get; set; }
        public string productColor { get; set; }
        public string productMaterial { get; set; }

        public ProductStatus ProductStat { get; set; }
        //public bool CompletePurchaseAndPayment { get; set; }
        // public UnitOFMassMeasurement productWeightUnit { get; set; }
        // public int productWeight { get; set; }
    }
}
