using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Insurance.Api.Models;
using Insurance.Domain;

namespace Insurance.Api.MapperProfiles
{
    public class SurchargeProductTypeProfile:Profile
    {
        public SurchargeProductTypeProfile()
        {
            CreateMap<ProductTypeSurchargeRate, SurchargeProductTypeDto>().ReverseMap();
        }
    }
}
