using AutoMapper;

namespace FrontWeb.Mapping
{
    public class CardProfile : Profile
    {
        public CardProfile()
        {
            CreateMap<DTO.CardDTO, ViewModels.CardViewModel>()
                .ReverseMap();
        }
    }
}
