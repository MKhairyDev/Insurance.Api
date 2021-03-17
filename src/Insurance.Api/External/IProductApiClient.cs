using System.Threading.Tasks;
using Insurance.Api.Models;

namespace Insurance.Api.External
{
    public interface IProductApiClient
    {
        Task<ProductDto> GetProductAsync(int productId);
        Task<ProductTypeDto> GetProductTypeAsync(int productTypeId);
    }
}