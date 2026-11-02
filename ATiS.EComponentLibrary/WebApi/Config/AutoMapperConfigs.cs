using AutoMapper;
using Model.Dto.Menu;
using Model.Dto.Role;
using Model.Dto.User;
using Model.Entitys;

namespace WebApi.Config
{
    public class AutoMapperConfigs : Profile
    {
        public AutoMapperConfigs()
        {
            // 角色
            CreateMap<RoleAdd, Role>();
            CreateMap<RoleEdit, Role>();

            // 用户
            CreateMap<UserAdd, Users>();
            CreateMap<UserEdit, Users>();

            // 菜单
            CreateMap<MenuAdd, Menu>();
            CreateMap<MenuEdit, Menu>();
        }
    }
}
