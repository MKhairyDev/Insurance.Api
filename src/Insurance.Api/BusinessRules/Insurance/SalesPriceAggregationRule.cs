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
        private readonly List<IInsuranceRule<ProductDto>> _insuranceRules;
        private IInsuranceRule<ProductDto> _matchedRule;
        private ProductDto _product;

        public SalesPriceAggregationRule(IOptionsMonitor<SalesPriceConfig> salesPriceConfig)
        {
            if (salesPriceConfig == null)
                throw new ArgumentNullException(paramName: nameof(salesPriceConfig));
            _insuranceRules = new List<IInsuranceRule<ProductDto>>
            {
                //todo:those values should be configured somewhere!
                new LessThanRule(salesConfig: salesPriceConfig),
                new RangeRule(salesConfig: salesPriceConfig),
                new BiggerThanRule(salesConfig: salesPriceConfig)
            };
        }

        public bool Match(ProductDto product)
        {
            _product = product ?? throw new ArgumentNullException(paramName: nameof(product));
            _matchedRule = RetrieveMatchedRule(product: product);
            return _matchedRule != null;
        }

        public float Calculate()
        {
            if (_product == null)
                throw new ArgumentNullException(paramName: nameof(_product));
            if (_insuranceRules == null)
                _matchedRule = RetrieveMatchedRule(product: _product);

            return _matchedRule?.Calculate() ?? 0f;
        }

        private IInsuranceRule<ProductDto> RetrieveMatchedRule(ProductDto product)
        {
            return _insuranceRules.FirstOrDefault(rule => rule.Match(entity: product));
        }
    }
}