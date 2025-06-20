using AutoMapper;
using SamaCardAll.Core.Models;
using SamaCardAll.Shared.Contracts.ViewModels;

namespace FrontWeb.Mapping
{
    public class SpendProfile : Profile
    {
        public SpendProfile()
        {
            CreateMap<Spend, SpendViewModel>()
                .ReverseMap();

            CreateMap<Customer, CustomerViewModel>()
                .ReverseMap();
            CreateMap<Card, CardViewModel>()
                .ReverseMap();
            CreateMap<User, UserViewModel>()
                .ReverseMap();
        }
    }
}
