using System;
using System.Collections.Generic;
using System.Linq;
using Insurance.Api.Configuration;
using Insurance.Api.External.Models;
using Microsoft.Extensions.Options;

namespace Insurance.Api.BusinessRules.Insurance
{
    public class SalesPriceAggregationRule : IInsuranceRule<ProductDto>
    {
        private readonly IOptionsMonitor<SalesPriceConfig> _salesPriceConfig;
        private readonly List<IInsuranceRule<ProductDto>> _insuranceRules;
        private IInsuranceRule<ProductDto> _matchedRule;
        private ProductDto _product;

        public SalesPriceAggregationRule(IOptionsMonitor<SalesPriceConfig> salesPriceConfig)
        {
            _salesPriceConfig = salesPriceConfig;
            _insuranceRules = new List<IInsuranceRule<ProductDto>>
            {
                //todo:those values should be configured somewhere!
                new LessThanRule(_salesPriceConfig),
                new RangeRule(_salesPriceConfig),
                new BiggerThanRule(_salesPriceConfig)
            };
        }

        public bool Match(ProductDto product)
        {
            _product = product ?? throw new ArgumentNullException(nameof(product));
            _matchedRule = RetrieveMatchedRule(product);
            return _matchedRule != null;
        }

        public float Calculate()
        {
            if (_product == null)
                throw new ArgumentNullException(nameof(_product));
            if (_insuranceRules == null)
                _matchedRule = RetrieveMatchedRule(_product);

            return _matchedRule?.Calculate() ?? 0f;
        }

        private IInsuranceRule<ProductDto> RetrieveMatchedRule(ProductDto product)
        {
            return _insuranceRules.FirstOrDefault(rule => rule.Match(product));
        }
    }
}