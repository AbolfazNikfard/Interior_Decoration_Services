namespace Interior_Decoration_Services.Models.View_Models
{
    public class FactorViewModel
    {
        public FactorViewModel()
        {
            cartItems = new List<UserOrderViewModel>();
        }
        public string Telphone { get; set; }
        public string Address { get; set; }
        public List<UserOrderViewModel> cartItems { get; set; }
    }
}
