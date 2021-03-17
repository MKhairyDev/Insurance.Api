using System.Collections.Generic;
using Insurance.Api.BusinessRules.Insurance;
using Insurance.Api.Models;
using Insurance.Api.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Insurance.Api.DependencyInjection
{
    public static class ProductServiceCollectionExtensions
    {
        public static IServiceCollection AddProductServices(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped(typeof(IInsuranceCalculator<ProductDto>), typeof(ProductInsuranceCalculator));
            services.AddScoped(typeof(IInsuranceCalculator<List<ProductDto>>), typeof(OrderInsuranceCalculator));
            return services;
        }
    }
}