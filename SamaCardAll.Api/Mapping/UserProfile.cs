using AutoMapper;
using SamaCardAll.Core.Models;
using SamaCardAll.Shared.Contracts.ViewModels;

namespace SamaCardAll.Api.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserViewModel, User>()
                .ReverseMap();
        }
    }
}
