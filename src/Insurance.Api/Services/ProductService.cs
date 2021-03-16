using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Insurance.Api.External;
using Insurance.Api.External.Models;
using Microsoft.Extensions.Logging;
using Utilities.Polly.Exceptions;

namespace Insurance.Api.Services
{
    public class ProductService : IProductService
    {
        private readonly ILogger<ProductService> _logger;
        private readonly IProductApiClient _productApiClient;

        public ProductService(IProductApiClient productApiClient, ILogger<ProductService> logger)
        {
            _productApiClient =
                productApiClient ?? throw new ArgumentNullException(paramName: nameof(productApiClient));
            _logger = logger ?? throw new ArgumentNullException(paramName: nameof(logger));
        }

        public async Task<List<ProductDto>> GetProductsWithProductTypeAsync(List<int> products)
        {
            var productsList = new List<ProductDto>();
            foreach (var productId in products)
            {
                var product = await ProductWithProductTypeAsync(productId: productId);
                if (product == null)
                    return null;
                productsList.Add(item: product);
            }

            return productsList;
        }

        public async Task<ProductDto> GetProductWithProductTypeAsync(int productId)
        {
            return await ProductWithProductTypeAsync(productId: productId);
        }

        private async Task<ProductDto> ProductWithProductTypeAsync(int productId)
        {
            try
            {
                var product = await _productApiClient.GetProductAsync(productId: productId);
                if (product == null)
                    return null;

                var productType = await _productApiClient.GetProductTypeAsync(productTypeId: product.ProductTypeId);
                if (productType == null)
                    return null;

                product.ProductTypeDto = productType;
                return product;
            }
            catch (ResourceNotFoundException ex)
            {
                _logger.LogError(message: ex.Message);
                return null;
            }
            catch (UnauthorizedApiAccessException ex)
            {
                _logger.LogError(message: ex.Message);
                return null;
            }
        }
    }
}