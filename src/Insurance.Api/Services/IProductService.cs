using System.Collections.Generic;
using System.Threading.Tasks;
using Insurance.Api.External.Models;

namespace Insurance.Api.Services
{
    public interface IProductService
    {
       Task<ProductDto> GetProductWithProductTypeAsync(int productId);
       Task<List<ProductDto>> GetProductsWithProductTypeAsync(List<int> products);
    }
}
