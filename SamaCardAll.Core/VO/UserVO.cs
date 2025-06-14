namespace SamaCardAll.Core.VO
{
    public record UserVO(
        int IdUser,
        string Name,
        string Password,
        short Active
    );
}
