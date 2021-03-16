using System;
using System.Collections.Generic;
using System.Linq;
using Insurance.Api.Configuration;
using Insurance.Api.External.Models;
using Microsoft.Extensions.Options;

namespace Insurance.Api.BusinessRules.Insurance
{
    public class OrderContainsCertainProductTypeNumberRule : IInsuranceRule<List<ProductDto>>
    {
        private readonly OrderWithSpecificProductTypeConfig _orderWithProductTypeConfig;

        public OrderContainsCertainProductTypeNumberRule(
            IOptionsMonitor<OrderWithSpecificProductTypeConfig> orderConfig)
        {
            if (orderConfig == null)
                throw new ArgumentNullException(paramName: nameof(orderConfig));
            _orderWithProductTypeConfig =
                orderConfig.Get(name: OrderWithSpecificProductTypeConfig.OrderContainsCertainProductTypeNumberRule);
        }

        public bool Match(List<ProductDto> productList)
        {
            if (productList == null)
                throw new ArgumentNullException(paramName: nameof(productList));
            return productList.Count(x => x.ProductTypeDto.Name.Equals(
                       value: _orderWithProductTypeConfig.ProductTypeName,
                       comparisonType: StringComparison.CurrentCultureIgnoreCase)) >=
                   _orderWithProductTypeConfig.NumberOfItems;
        }

        public float Calculate()
        {
            return _orderWithProductTypeConfig.InsuranceValueToAdd;
        }
    }
}