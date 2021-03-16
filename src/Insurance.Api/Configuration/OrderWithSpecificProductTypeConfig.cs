namespace Insurance.Api.Configuration
{
    public class OrderWithSpecificProductTypeConfig
    {
        public const string OrderContainsCertainProductTypeNumberRule = "OrderContainsCertainProductTypeNumberRule";
        public string ProductTypeName { get; set; }
        public int NumberOfItems { get; set; }
        public int InsuranceValueToAdd { get; set; }
    }
}