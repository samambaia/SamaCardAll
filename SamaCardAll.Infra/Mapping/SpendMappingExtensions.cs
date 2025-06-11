using SamaCardAll.Core.VO;
using SamaCardAll.Infra.Models;

namespace SamaCardAll.Infra.Mapping
{
    public static class SpendMappingExtensions
    {
        public static SpendVO ToVO(this Models.Spend spend)
        {
            if (spend == null) return null;
            return new SpendVO
            (
                IdSpend: spend.IdSpend,
                Expenses: spend.Expenses,
                Amount: spend.Amount,
                Date: spend.Date,
                InstallmentPlan: spend.InstallmentPlan,
                InstallmentValue: spend.InstallmentValue,
                Deleted: spend.Deleted,
                CreatedDate: spend.CreatedDate,
                CustomerIdCustomer: spend.CustomerIdCustomer,
                Customer: spend.Customer?.ToVO(),
                CardIdCard: spend.CardIdCard,
                Card: spend.Card?.ToVO(),
                UserIdUser: spend.UserIdUser,
                User: spend.User?.ToVO()
            );
        }

        public static Spend ToModel(this Core.VO.SpendVO spendVO)
        {
            if (spendVO == null) return null;
            return new Spend
            {
                IdSpend = spendVO.IdSpend,
                Expenses = spendVO.Expenses,
                Amount = spendVO.Amount,
                Date = spendVO.Date,
                InstallmentPlan = spendVO.InstallmentPlan,
                InstallmentValue = spendVO.InstallmentValue,
                Deleted = spendVO.Deleted,
                CreatedDate = spendVO.CreatedDate,
                CustomerIdCustomer = spendVO.CustomerIdCustomer,
                Customer = spendVO.Customer?.ToModel(),
                CardIdCard = spendVO.CardIdCard,
                Card = spendVO.Card?.ToModel(),
                UserIdUser = spendVO.UserIdUser,
                User = spendVO.User?.ToModel()
            };
        }
    }
}
