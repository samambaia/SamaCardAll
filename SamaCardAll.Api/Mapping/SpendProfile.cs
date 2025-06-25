using AutoMapper;
using SamaCardAll.Core.Models;
using SamaCardAll.Shared.Contracts.ViewModels;

namespace SamaCardAll.Api.Mapping
{
    public class SpendProfile : Profile
    {
        public SpendProfile()
        {
            // Model → ViewModel
            CreateMap<Spend, SpendViewModel>()
                .ForMember(vm => vm.CardIdCard, m => m.MapFrom(e => e.CardIdCard))
                .ForMember(vm => vm.CustomerIdCustomer, m => m.MapFrom(e => e.CustomerIdCustomer))
                .ForMember(vm => vm.UserIdUser, m => m.MapFrom(e => e.UserIdUser))
                .ForMember(vm => vm.CardName, m => m.MapFrom(s => s.Card.Bank))
                .ForMember(vm => vm.CustomerName, m => m.MapFrom(s => s.Customer.CustomerName));

            // ViewModel → Model
            CreateMap<SpendViewModel, Spend>()
                .ForMember(e => e.CardIdCard, m => m.MapFrom(vm => vm.CardIdCard))
                .ForMember(e => e.CustomerIdCustomer, m => m.MapFrom(vm => vm.CustomerIdCustomer))
                .ForMember(e => e.UserIdUser, m => m.MapFrom(vm => vm.UserIdUser))
                .ForMember(e => e.Card, m => m.Ignore())       // ← muito importante
                .ForMember(e => e.Customer, m => m.Ignore())
                .ForMember(e => e.User, m => m.Ignore());
        }
    }
}
