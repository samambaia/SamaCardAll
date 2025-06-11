using SamaCardAll.Core.VO;
using SamaCardAll.Infra.Models;

namespace SamaCardAll.Infra.Mapping
{
    public static class UserMappingExtensions
    {
        public static UserVO ToVO(this User user)
        {
            if (user == null) return null;
            return new UserVO(
                IdUser: user.IdUser,
                Name: user.Name,
                Password: user.Password,
                Active: user.Active
            );
        }
        public static User ToModel(this UserVO userVO)
        {
            if (userVO == null) return null;
            return new User
            {
                IdUser = userVO.IdUser,
                Name = userVO.Name,
                Password = userVO.Password,
                Active = userVO.Active
            };
        }
    }
}
