using AutoMapper;
using FrontWeb.DTO;
using FrontWeb.ViewModels;

namespace FrontWeb.Mapping
{
    public class SpendProfile : Profile
    {
        public SpendProfile()
        {
            CreateMap<SpendDTO, SpendViewModel>()
                .ReverseMap();

            CreateMap<CustomerDTO, CustomerViewModel>()
                .ReverseMap();
            CreateMap<CardDTO, CardViewModel>()
                .ReverseMap();
            CreateMap<UserDTO, UserViewModel>()
                .ReverseMap();

        }
    }
}
