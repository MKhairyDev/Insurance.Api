using System;
using System.Collections.Generic;
using System.Linq;
using Insurance.Api.External.Models;

namespace Insurance.Api.BusinessRules.Insurance
{
    public class SalesPriceAggregationRule : IInsuranceRule
    {
        private readonly List<IInsuranceRule> _insuranceRules;
        private IInsuranceRule _matchedRule;
        private ProductDto _product;

        public SalesPriceAggregationRule()
        {
            _insuranceRules = new List<IInsuranceRule>
            {
                //todo:those values should be configured somewhere!
                new LessThanRule(targetValue:500,insuranceValueToAdd: 0),
                new RangeRule(targetMinValue:500,targetMaxValue: 2000,insuranceValueToAdd: 1000),
                new BiggerThanRule(targetValue:2000,insuranceValueToAdd: 2000)
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

        private IInsuranceRule RetrieveMatchedRule(ProductDto product)
        {
            return _insuranceRules.FirstOrDefault(rule => rule.Match(product));
        }
    }
}