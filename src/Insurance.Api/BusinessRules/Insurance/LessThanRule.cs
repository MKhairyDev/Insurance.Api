using System;
using Insurance.Api.Configuration;
using Insurance.Api.External.Models;
using Microsoft.Extensions.Options;

namespace Insurance.Api.BusinessRules.Insurance
{
    public class LessThanRule : IInsuranceRule<ProductDto>
    {
        private readonly SalesPriceConfig _salesPriceConfig;

        public LessThanRule(IOptionsMonitor<SalesPriceConfig> salesConfig)
        {
            if (salesConfig == null)
                throw new ArgumentNullException(nameof(salesConfig));
            _salesPriceConfig = salesConfig.Get(SalesPriceConfig.LessThanRule);
        }

        public bool Match(ProductDto product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));
            return product.SalesPrice < _salesPriceConfig.MinValue;
        }

        public float Calculate()
        {
            return _salesPriceConfig.InsuranceValueToAdd;
        }
    }
}