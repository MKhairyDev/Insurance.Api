using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Insurance.Api.Configuration
{
    public class BusinessRulesConfig
    {
        public const string BusinessRules = "BusinessRules";
        public SalesPriceConfig SalesRule { get; set; }
        public ProductTypeConfig ProductTypeRule { get; set; }
    }
}
