using System;
using System.Collections.Generic;
using System.Linq;
using Insurance.Api.External.Models;

namespace Insurance.Api.BusinessRules.Insurance
{
    public class ProductTypeNameRule : IInsuranceRule
    {
        private readonly int _insuranceValueToAdd;
        private readonly IEnumerable<string> _lookupCollection;
        private readonly IInsuranceRule _salesPriceCalculator;
        private bool _isSalesPriceCalculator;
        private ProductDto _product;

        public ProductTypeNameRule(int insuranceValueToAdd, IEnumerable<string> lookupCollection,
            IInsuranceRule salesPriceCalculator)
        {
            _insuranceValueToAdd = insuranceValueToAdd;
            _lookupCollection = lookupCollection;
            _salesPriceCalculator = salesPriceCalculator;
        }

        public bool Match(ProductDto product)
        {
            _product = product ?? throw new ArgumentNullException(nameof(product));
            var isMatch = _product.ProductTypeDto.CanBeInsured &&
                          _lookupCollection.Contains(_product.ProductTypeDto.Name);
            if (isMatch) _isSalesPriceCalculator = _salesPriceCalculator.Match(product);

            return isMatch;
        }

        public float Calculate()
        {
            return _isSalesPriceCalculator
                ? _insuranceValueToAdd + _salesPriceCalculator.Calculate()
                : _insuranceValueToAdd;
        }
    }
}