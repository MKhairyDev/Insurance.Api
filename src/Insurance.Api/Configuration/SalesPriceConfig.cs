namespace Insurance.Api.Configuration
{
    public class SalesPriceConfig
    {
        public const string LessThanRule = "LessThanRule";
        public const string RangeRule = "RangeRule";
        public const string BiggerThanRule = "BiggerThanRule";
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
        public float InsuranceValueToAdd { get; set; }
    }
}