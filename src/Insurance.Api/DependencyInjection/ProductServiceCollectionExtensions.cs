using Insurance.Api.BusinessRules.Insurance;
using Insurance.Api.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Insurance.Api.DependencyInjection
{
    public static class ProductServiceCollectionExtensions
    {
        public static IServiceCollection AddProductServices(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IInsuranceCalculator, InsuranceCalculator>();
            return services;
        }
    }
}