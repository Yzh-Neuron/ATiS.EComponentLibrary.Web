using Model.Dto.Login;
using Model.Dto.User;

namespace Interface
{
    public interface IUserService
    {
        /// <summary>
        /// 登录时获取用户信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<UserRes> GetUser(LoginReq req);
    }
}
