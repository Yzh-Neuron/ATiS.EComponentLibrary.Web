using AutoMapper;
using Interface;
using Model.Dto.Login;
using Model.Dto.User;
using Model.Entitys;
using SqlSugar;

namespace Service
{
    public class UserService : IUserService
    {
        private readonly ISqlSugarClient _db;

        public UserService(IMapper mapper, ISqlSugarClient db)
        {
            _db = db;
        }

        public async Task<UserRes> GetUser(LoginReq req)
        {
            var user = await _db.Queryable<Users>()
                                .Where(t => t.Name == req.UserName && t.Password == req.PassWord)
                                .Select(p => new UserRes() { }, true)
                                .FirstAsync();

            //DTO转实体
            //1.属性手动赋值
            //2.AutoMapper
            //3.Sqlsugar的Select的自动映射
            return user;
        }
    }
}
