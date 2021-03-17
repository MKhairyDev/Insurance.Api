using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Insurance.Api.Models;
using Insurance.Data.Repositories;
using Insurance.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.Api.Controllers
{
    [ApiController]
    [Route("api/Insurance/[controller]")]
    public class SurchargeController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRepository<ProductTypeSurchargeRate> _surChargeRepository;

        public SurchargeController(IRepository<ProductTypeSurchargeRate> surChargeRepository, IMapper mapper)
        {
            _surChargeRepository = surChargeRepository ??
                                   throw new ArgumentNullException(paramName: nameof(surChargeRepository));
            _mapper = mapper ?? throw new ArgumentNullException(paramName: nameof(mapper));
        }

        [HttpPost("Import")]
        public async Task<IActionResult> Import([FromBody] List<SurchargeProductTypeDto> surchargeProductTypes)
        {
            if (!ModelState.IsValid)
                return BadRequest(modelState: ModelState);

            var surchargeRateData = surchargeProductTypes;
            if (surchargeRateData != null)
            {
                var surChargeModel = _mapper.Map<List<ProductTypeSurchargeRate>>(source: surchargeRateData);
                await _surChargeRepository.AddRangeAsync(entities: surChargeModel);
                await _surChargeRepository.SaveAsync();
            }

            return Ok();
        }
    }
}