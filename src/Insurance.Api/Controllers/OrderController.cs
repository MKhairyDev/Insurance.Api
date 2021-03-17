using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Insurance.Api.BusinessRules.Insurance;
using Insurance.Api.Models;
using Insurance.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.Api.Controllers
{
    [ApiController]
    [Route("api/Insurance/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IInsuranceCalculator<List<ProductDto>> _orderCalculator;
        private readonly IProductService _productService;

        public OrderController(IProductService productService,
            IInsuranceCalculator<List<ProductDto>> insuranceCalculator)
        {
            _productService = productService ?? throw new ArgumentNullException(paramName: nameof(productService));
            _orderCalculator = insuranceCalculator ??
                               throw new ArgumentNullException(paramName: nameof(insuranceCalculator));
        }


        [HttpGet]
        public async Task<ActionResult<float>> CalculateInsurance([FromQuery(Name = "id")] List<int> productsId)
        {
            var productsList = await _productService.GetProductsWithProductTypeAsync(products: productsId);
            if (productsList == null)
                return NotFound();
            var insuranceValue = _orderCalculator.Calculate(entity: productsList);

            return Ok(value: insuranceValue);
        }
    }
}