using System;
using System.Linq;
using Insurance.Api.Configuration;
using Insurance.Api.Models;
using Microsoft.Extensions.Options;

namespace Insurance.Api.BusinessRules.Insurance
{
    public class ProductTypeNameRule : IInsuranceRule<ProductDto>
    {
        private readonly ProductTypeConfig _productConfig;
        private readonly IInsuranceRule<ProductDto> _salesPriceCalculator;
        private bool _isSalesPriceCalculator;
        private ProductDto _product;

        public ProductTypeNameRule(IOptionsMonitor<ProductTypeConfig> productConfig,
            IInsuranceRule<ProductDto> salesPriceCalculator)
        {
            if (productConfig == null)
                throw new ArgumentNullException(paramName: nameof(productConfig));
            _productConfig = productConfig.Get(name: ProductTypeConfig.ProductTypeRule);
            _salesPriceCalculator = salesPriceCalculator;
        }

        public bool Match(ProductDto product)
        {
            _product = product ?? throw new ArgumentNullException(paramName: nameof(product));
            var isMatch = _product.ProductTypeDto.CanBeInsured &&
                          _productConfig.ProductTypesLookup.Contains(value: _product.ProductTypeDto.Name,
                              comparer: StringComparer.CurrentCultureIgnoreCase);
            if (isMatch) _isSalesPriceCalculator = _salesPriceCalculator.Match(entity: product);

            return isMatch;
        }

        public float Calculate()
        {
            return _isSalesPriceCalculator
                ? _productConfig.InsuranceValueToAdd + _salesPriceCalculator.Calculate()
                : _productConfig.InsuranceValueToAdd;
        }
    }
}