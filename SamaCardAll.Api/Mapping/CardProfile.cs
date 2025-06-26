using AutoMapper;
using SamaCardAll.Core.Models;
using SamaCardAll.Shared.Contracts.ViewModels;

namespace SamaCardAll.Api.Mapping
{
    public class CardProfile : Profile
    {
        public CardProfile()
        {
            CreateMap<CardViewModel, Card>()
                .ReverseMap();
        }
    }
}
