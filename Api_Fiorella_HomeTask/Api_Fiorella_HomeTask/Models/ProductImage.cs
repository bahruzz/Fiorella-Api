namespace Api_Fiorella_HomeTask.Models
{
    public class ProductImage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public List<string> Images { get; set; }
    }
}
