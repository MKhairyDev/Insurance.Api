using Insurance.Api.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;

namespace Insurance.Api.UnitTests.BusinessRules.Tests
{
    public class InsuranceBase
    {
        private SalesPriceConfig _biggerThanRule;
        private IConfiguration _configuration;
        private SalesPriceConfig _lessThanRule;
        private ProductTypeConfig _productTypeConfig;
        private OrderWithSpecificProductTypeConfig _orderWithSpecificProductType;
        private SalesPriceConfig _rangeRule;
        protected Mock<IOptionsMonitor<ProductTypeConfig>> ProductConfigMock;
        protected Mock<IOptionsMonitor<SalesPriceConfig>> SalesConfigMock;
        protected Mock<IOptionsMonitor<OrderWithSpecificProductTypeConfig>> OrderWithSpecificProductTypeConfigMock;
        [OneTimeSetUp]
        public void Init()
        {
            ReadBusinessRulesConfigurations();
            SetUpBusinessRulesIOptionMonitorMocks();
        }

        private void ReadBusinessRulesConfigurations()
        {
            _lessThanRule = new SalesPriceConfig();
            _biggerThanRule = new SalesPriceConfig();
            _rangeRule = new SalesPriceConfig();
            _productTypeConfig = new ProductTypeConfig();
            _orderWithSpecificProductType = new OrderWithSpecificProductTypeConfig();

            _configuration = TestHelper.BuildConfiguration(testDirectory: TestContext.CurrentContext.TestDirectory);
            _configuration.GetSection(key: $"BusinessRules:{ProductTypeConfig.ProductTypeRule}")
                .Bind(instance: _productTypeConfig);

            _configuration.GetSection(key: $"BusinessRules:SalesRule:{SalesPriceConfig.LessThanRule}")
                .Bind(instance: _lessThanRule);

            _configuration.GetSection(key: $"BusinessRules:SalesRule:{SalesPriceConfig.BiggerThanRule}")
                .Bind(instance: _biggerThanRule);

            _configuration.GetSection(key: $"BusinessRules:SalesRule:{SalesPriceConfig.RangeRule}")
                .Bind(instance: _rangeRule);
            _configuration.GetSection(
                key:
                $"BusinessRules:{OrderWithSpecificProductTypeConfig.OrderContainsCertainProductTypeNumberRule}").Bind(_orderWithSpecificProductType);
        }

        private void SetUpBusinessRulesIOptionMonitorMocks()
        {
            SalesConfigMock = new Mock<IOptionsMonitor<SalesPriceConfig>>();
            ProductConfigMock = new Mock<IOptionsMonitor<ProductTypeConfig>>();
            OrderWithSpecificProductTypeConfigMock = new Mock<IOptionsMonitor<OrderWithSpecificProductTypeConfig>>();
            ProductConfigMock.Setup(x => x.Get(ProductTypeConfig.ProductTypeRule))
                .Returns(value: _productTypeConfig);

            SalesConfigMock.Setup(x => x.Get(SalesPriceConfig.LessThanRule))
                .Returns(value: _lessThanRule);
            SalesConfigMock.Setup(x => x.Get(SalesPriceConfig.BiggerThanRule))
                .Returns(value: _biggerThanRule);
            SalesConfigMock.Setup(x => x.Get(SalesPriceConfig.RangeRule))
                .Returns(value: _rangeRule);
            OrderWithSpecificProductTypeConfigMock.Setup(x => x.Get(OrderWithSpecificProductTypeConfig.OrderContainsCertainProductTypeNumberRule))
                .Returns(value: _orderWithSpecificProductType);
        }
    }
}