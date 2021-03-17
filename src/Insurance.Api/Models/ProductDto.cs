namespace Insurance.Api.Models
{
    public class ProductDto
    {
        public ProductDto()
        {
            ProductTypeDto = new ProductTypeDto();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int SalesPrice { get; set; }
        public int ProductTypeId { get; set; }
        public ProductTypeDto ProductTypeDto { get; set; }
    }
}