﻿using AutoMapper;
using Entities.Models;
using Shared.DataTransferObjects;

namespace ultimate_api.AutoMaper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Company, CompanyDTO>().ForCtorParam("FullAddress", opt => opt.MapFrom(x => string.Join(' ', x.Address, x.Country)));
            CreateMap<User, UserDTO>().ForCtorParam("Fullname", opt => opt.MapFrom(x => string.Join(' ', x.FirstName, x.LastName)));
        }
    }
}
