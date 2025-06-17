using AutoMapper;

namespace FrontWeb.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<DTO.UserDTO, ViewModels.UserViewModel>()
                .ReverseMap();
        }
    }
}
