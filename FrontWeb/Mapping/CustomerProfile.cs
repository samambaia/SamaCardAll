using AutoMapper;

namespace FrontWeb.Mapping
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<DTO.CustomerDTO, ViewModels.CustomerViewModel>()
                .ReverseMap();
        }
    }
}
