using AutoMapper;
using SamaCardAll.Core.Models;
using SamaCardAll.Shared.Contracts.ViewModels;

namespace FrontWeb.Mapping
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<CustomerViewModel, Customer>()
                .ReverseMap();
        }
    }
}
