using Insurance.Api.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Insurance.Api.DependencyInjection
{
    public static class ConfigurationCollectionExtensions
    {
        public static IServiceCollection AddConfigurationServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            //Using .Net core Option pattern for dealing wih configuration 

            services.Configure<ExternalServicesConfig>(name: ExternalServicesConfig.ProductApi,
                config: configuration.GetSection(key: $"ExternalServices:{ExternalServicesConfig.ProductApi}"));

            //Business Rules

            services.Configure<SalesPriceConfig>(name: SalesPriceConfig.LessThanRule,
                config: configuration.GetSection(key: $"BusinessRules:SalesRule:{SalesPriceConfig.LessThanRule}"));

            services.Configure<SalesPriceConfig>(name: SalesPriceConfig.RangeRule,
                config: configuration.GetSection(key: $"BusinessRules:SalesRule:{SalesPriceConfig.RangeRule}"));

            services.Configure<SalesPriceConfig>(name: SalesPriceConfig.BiggerThanRule,
                config: configuration.GetSection(key: $"BusinessRules:SalesRule:{SalesPriceConfig.BiggerThanRule}"));

            services.Configure<ProductTypeConfig>(name: ProductTypeConfig.ProductTypeRule,
                config: configuration.GetSection(key: $"BusinessRules:{ProductTypeConfig.ProductTypeRule}"));

            services.Configure<OrderWithSpecificProductTypeConfig>(
                name: OrderWithSpecificProductTypeConfig.OrderContainsCertainProductTypeNumberRule,
                config: configuration.GetSection(
                    key:
                    $"BusinessRules:{OrderWithSpecificProductTypeConfig.OrderContainsCertainProductTypeNumberRule}"));
            return services;
        }
    }
}