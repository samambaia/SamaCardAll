namespace SamaCardAll.Core.VO
{
    public record SpendVO(
        int IdSpend,
        string Expenses,
        decimal Amount,
        DateTime Date,
        int InstallmentPlan,
        decimal InstallmentValue,
        short Deleted,
        DateTime CreatedDate,
        int CustomerIdCustomer,
        CustomerVO Customer,
        int CardIdCard,
        CardVO Card,
        int UserIdUser,
        UserVO User
    )
    {         
        public SpendVO() : this(0, string.Empty, 0, DateTime.MinValue, 0, 0, 0, DateTime.MinValue, 0, null!, 0, null!, 0, null!) 
        { 

        } 
    }
}
