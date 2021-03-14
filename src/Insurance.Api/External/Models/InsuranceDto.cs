namespace Insurance.Api.External.Models
{
    public class InsuranceDto
    {
        public int ProductId { get; set; }
        public float InsuranceValue { get; set; }
        public string ProductTypeName { get; set; }
        public bool ProductTypeHasInsurance { get; set; }
        public float SalesPrice { get; set; }
    }
}