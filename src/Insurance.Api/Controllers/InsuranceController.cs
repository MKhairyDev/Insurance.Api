using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Insurance.Api.BusinessRules.Insurance;
using Insurance.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InsuranceController : ControllerBase
    {
        private readonly IInsuranceCalculator _insuranceCalculator;
        private readonly IProductService _productService;

        public InsuranceController(IProductService productService, IInsuranceCalculator insuranceCalculator)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _insuranceCalculator = insuranceCalculator ?? throw new ArgumentNullException(nameof(insuranceCalculator));
        }

        [HttpGet("product/{productId:int}")]
        public async Task<ActionResult<float>> CalculateInsurance(int productId)
        {
            var product = await _productService.GetProductWithProductTypeAsync(productId);
            if (product == null)
                return NotFound(productId);
            var insuranceValue = _insuranceCalculator.CalculateProductInsurance(product);

            return Ok(insuranceValue);
        }

    }
}