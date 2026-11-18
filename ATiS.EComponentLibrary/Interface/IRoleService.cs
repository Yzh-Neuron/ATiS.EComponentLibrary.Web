using Model.Dto.Role;
using Model.Other;

namespace Interface
{
    public interface IRoleService
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="req"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> Add(RoleAdd req, string userId);

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="req"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> Edit(RoleEdit req, string userId);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> Del(string id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> BatchDel(string ids);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="req"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<PageInfo<RoleRes>> GetRoles(RoleReq req, string userId);
    }
}
