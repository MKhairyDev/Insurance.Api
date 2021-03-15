using System.Collections.Generic;

namespace Insurance.Api.Configuration
{
    public class ProductTypeConfig
    {
        public const string ProductTypeRule = "ProductTypeRule";
        public List<string> ProductTypesLookup { get; set; }
        public float InsuranceValueToAdd { get; set; }
    }
}