using System;
using System.Threading.Tasks;
using Insurance.Api.BusinessRules.Insurance;
using Insurance.Api.Models;
using Insurance.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.Api.Controllers
{
    [ApiController]
    [Route("api/Insurance/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IInsuranceCalculator<ProductDto> _productCalculator;
        private readonly IProductService _productService;

        public ProductController(IProductService productService, IInsuranceCalculator<ProductDto> productCalculator)
        {
            _productService = productService ?? throw new ArgumentNullException(paramName: nameof(productService));
            _productCalculator =
                productCalculator ?? throw new ArgumentNullException(paramName: nameof(productCalculator));
        }

        [HttpGet("{productId:int}")]
        public async Task<ActionResult<float>> CalculateInsurance(int productId)
        {
            var product = await _productService.GetProductWithProductTypeAsync(productId: productId);
            if (product == null)
                return NotFound(value: productId);
            var insuranceValue = _productCalculator.Calculate(entity: product);

            return Ok(value: insuranceValue);
        }
    }
}