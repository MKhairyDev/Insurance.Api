using System;
using Insurance.Api.External.Models;

namespace Insurance.Api.BusinessRules.Insurance
{
    public class LessThanRule : IInsuranceRule
    {
        private readonly int _targetValue;
        private readonly int _insuranceValueToAdd;
        public LessThanRule(int targetValue, int insuranceValueToAdd)
        {
            _targetValue = targetValue;
            _insuranceValueToAdd = insuranceValueToAdd;
        }

        public bool Match(ProductDto product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));
            return product.SalesPrice < _targetValue;
        }

        public float Calculate()
        {
            return _insuranceValueToAdd;
        }
    }
}