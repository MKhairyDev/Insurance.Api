using System;
using Insurance.Api.Configuration;
using Insurance.Api.Models;
using Microsoft.Extensions.Options;

namespace Insurance.Api.BusinessRules.Insurance
{
    public class BiggerThanRule : IInsuranceRule<ProductDto>
    {
        private readonly SalesPriceConfig _salesPriceConfig;

        public BiggerThanRule(IOptionsMonitor<SalesPriceConfig> salesConfig)
        {
            if (salesConfig == null)
                throw new ArgumentNullException(nameof(salesConfig));
            _salesPriceConfig = salesConfig.Get(SalesPriceConfig.BiggerThanRule);
        }
        public bool Match(ProductDto product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));
            return product.ProductTypeDto.CanBeInsured && product.SalesPrice >= _salesPriceConfig.MaxValue;
        }

        public float Calculate()
        {
            return _salesPriceConfig.InsuranceValueToAdd;
        }
    }
}