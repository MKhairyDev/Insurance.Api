using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Insurance.Api.BusinessRules.Insurance;
using Insurance.Api.External.Models;
using Insurance.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.Api.Controllers
{
    [ApiController]
    [Route("api/Insurance/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IInsuranceCalculator<ProductDto> _productCalculator;
        private readonly IInsuranceCalculator<List<ProductDto>> _orderCalculator;
        private readonly IProductService _productService;

        public ProductController(IProductService productService, IInsuranceCalculator<ProductDto> productCalculator)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _productCalculator = productCalculator ?? throw new ArgumentNullException(nameof(productCalculator));
        }

        [HttpGet("{productId:int}")]
        public async Task<ActionResult<float>> CalculateInsurance(int productId)
        {
            var product = await _productService.GetProductWithProductTypeAsync(productId);
            if (product == null)
                return NotFound(productId);
            var insuranceValue = _productCalculator.Calculate(product);

            return Ok(insuranceValue);
        }
    }
}