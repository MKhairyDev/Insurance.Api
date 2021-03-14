using System;
using Insurance.Api.External.Models;

namespace Insurance.Api.BusinessRules.Insurance
{
    public class RangeRule : IInsuranceRule
    {
        private readonly int _targetMinValue;
        private readonly int _targetMaxValue;
        private readonly int _insuranceValueToAdd;
        public RangeRule(int targetMinValue,int targetMaxValue,int insuranceValueToAdd)
        {
            _targetMinValue = targetMinValue;
            _targetMaxValue = targetMaxValue;
            _insuranceValueToAdd = insuranceValueToAdd;
        }

        public bool Match(ProductDto product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));
            return product.ProductTypeDto.CanBeInsured && product.SalesPrice >_targetMinValue&& product.SalesPrice<_targetMaxValue;
        }

        public float Calculate()
        {
            return _insuranceValueToAdd;
        }
    }
}