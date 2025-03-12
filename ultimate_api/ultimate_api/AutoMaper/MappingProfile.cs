using AutoMapper;
using Entities.Models;
using Shared.DataTransferObjects;

namespace ultimate_api.AutoMaper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Company, CompanyDTO>().ForMember(c => c.FullAddress, opt => opt.MapFrom(x => string.Join(' ', x.Address, x.Country)));
            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => DateTime.Now.Year - src.DateOfBirth.Value.Year))
                .ForMember(c => c.FullName, opt => opt.MapFrom(x => string.Join(' ', x.FirstName, x.LastName)));

            CreateMap<CompanyForCreationDTO, Company>();
            CreateMap<UserForCreationDTO, User>();
            CreateMap<UserForUpdateDTO, User>().ReverseMap();

            CreateMap<CompanyForUpdateDTO, Company>();

        }
    }
}
