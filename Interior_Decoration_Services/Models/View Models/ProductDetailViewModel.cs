namespace Interior_Decoration_Services.Models.View_Models
{
    public class ProductDetailViewModel
    {
        public ProductDetailViewModel()
        {
            comments= new List<Comment>();
        }
        public Product product { get; set; }
        public List<Comment> comments { get; set; }
    }
}
