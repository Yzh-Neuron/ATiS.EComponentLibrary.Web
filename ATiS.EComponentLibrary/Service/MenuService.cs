using AutoMapper;
using Dm.util;
using Interface;
using Microsoft.AspNetCore.Mvc.Formatters;
using Model.Dto.Menu;
using Model.Dto.User;
using Model.Entitys;
using SqlSugar;

namespace Service
{
    public class MenuService : IMenuService
    {
        private readonly IMapper _mapper;
        private ISqlSugarClient _db;
        public MenuService(IMapper mapper, ISqlSugarClient db)
        {
            _mapper = mapper;
            _db = db;
        }
        public async Task<bool> Add(MenuAdd req, string userId)
        {
            Menu info = _mapper.Map<Menu>(req);
            info.Id = Guid.NewGuid().ToString();
            info.CreateUserId = userId;
            info.CreateDate = DateTime.Now;
            info.IsDeleted = false;
            return await _db.Insertable(info).ExecuteCommandAsync() > 0;
        }
        public async Task<bool> Edit(MenuEdit req, string userId)
        {
            var info = await _db.Queryable<Menu>().FirstAsync(p => p.Id == req.Id);
            _mapper.Map(req, info);
            info.ModifyUserId = userId;
            info.ModifyDate = DateTime.Now;
            return await _db.Updateable(info).ExecuteCommandHasChangeAsync();
        }
        public async Task<bool> BatchDel(string ids)
        {
            return await _db.Deleteable<Menu>().Where(p => ids.contains(p.Id)).ExecuteCommandHasChangeAsync();
        }
        public async Task<bool> Del(string id)
        {
            return await _db.Deleteable<Menu>().Where(p => p.Id == id).ExecuteCommandHasChangeAsync();
        }
        public async Task<List<MenuRes>> GetMenus(MenuReq req, string userId)
        {
            var user = await _db.Queryable<Users>().FirstAsync(p => p.Id == userId);
            if (user.UserType == 0)
            {
                var list = await _db.Queryable<Menu>().WhereIF(!string.IsNullOrEmpty(req.Name), m => m.Name.contains(req.Name))
                                                      .WhereIF(!string.IsNullOrEmpty(req.Index), m => m.Index.contains(req.Index))
                                                      .WhereIF(!string.IsNullOrEmpty(req.FilePath), m => m.Index.contains(req.FilePath))
                                                      .WhereIF(!string.IsNullOrEmpty(req.Description), m => m.Index.contains(req.Description))
                                                      .OrderBy(m => m.Order)
                                                      .Select(m => new MenuRes(), true)
                                                      .ToTreeAsync(it => it.Children, it => it.ParentId, "");

                return list;
            }
            else
            {
                var list = await _db.Queryable<Menu>().InnerJoin<MenuRoleRelation>((m, mrr) => m.Id == mrr.MenuId)
                                                      .InnerJoin<Role>((m, mrr, r) => r.Id == mrr.RoleId)
                                                      .InnerJoin<UserRoleRelation>((m, mrr, r, urr) => r.Id == urr.RoleId)
                                                      .InnerJoin<Users>((m, mrr, r, urr, u) => u.Id == urr.UserId && u.Id == userId)
                                                      .WhereIF(!string.IsNullOrEmpty(req.Name), m => m.Name.contains(req.Name))
                                                      .WhereIF(!string.IsNullOrEmpty(req.Index), m => m.Index.contains(req.Index))
                                                      .WhereIF(!string.IsNullOrEmpty(req.FilePath), m => m.Index.contains(req.FilePath))
                                                      .WhereIF(!string.IsNullOrEmpty(req.Description), m => m.Index.contains(req.Description))
                                                      .OrderBy(m => m.Order)
                                                      .Select(m => new MenuRes(), true)
                                                      .ToTreeAsync(it => it.Children, it => it.ParentId, "");

                return list;
            }
        }
        public async Task<bool> SettingMenu(string rid, string mids)
        {
            string[] midArr = mids.Split(',', StringSplitOptions.RemoveEmptyEntries);

            //先删除关系，后批量新增
            await _db.Deleteable<MenuRoleRelation>(s => s.RoleId == rid).ExecuteCommandAsync();

            var newList = new List<MenuRoleRelation>();
            foreach (var it in midArr)
            {
                newList.Add(new MenuRoleRelation() { Id = Guid.NewGuid().ToString(), RoleId = rid, MenuId = it });
            }

            return await _db.Insertable(newList).ExecuteCommandAsync() > 0;
        }
    }
}
