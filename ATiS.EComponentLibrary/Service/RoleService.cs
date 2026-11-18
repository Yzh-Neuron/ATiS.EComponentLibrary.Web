using AutoMapper;
using Interface;
using Model.Dto.Role;
using Model.Entitys;
using Model.Other;
using SqlSugar;

namespace Service
{
    public class RoleService : IRoleService
    {
        private readonly IMapper _mapper;
        private ISqlSugarClient _db { get; set; }
        public RoleService(IMapper mapper, ISqlSugarClient db)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task<bool> Add(RoleAdd req, string userId)
        {
            Role info = _mapper.Map<Role>(req);
            info.Id = Guid.NewGuid().ToString();
            info.CreateUserId = userId;
            info.CreateDate = DateTime.Now;
            info.IsDeleted = false;
            return await _db.Insertable(info).ExecuteCommandAsync() > 0;
        }

        public async Task<bool> BatchDel(string ids)
        {
            var list = _db.Queryable<Role>().Where(p => ids.Contains(p.Id));
            return await _db.Deleteable<Role>(list).ExecuteCommandAsync() > 0;
        }

        public async Task<bool> Del(string id)
        {
            var info = _db.Queryable<Role>().First(p => p.Id == id);
            return await _db.Deleteable<Role>(info).ExecuteCommandAsync() > 0;
        }

        public async Task<bool> Edit(RoleEdit req, string userId)
        {
            var info = _db.Queryable<Role>().First(p => p.Id == req.Id);
            _mapper.Map(req, info);
            info.ModifyUserId = userId;
            info.ModifyDate = DateTime.Now;
            return await _db.Updateable(info).ExecuteCommandAsync() > 0;
        }

        public async Task<PageInfo<RoleRes>> GetRoles(RoleReq req, string userId)
        {
            PageInfo<RoleRes> res = new PageInfo<RoleRes>();
            int total = 0;
            var list = await _db.Queryable<Role>()
                .LeftJoin<Users>((r, ul) => r.CreateUserId == ul.Id)
                .LeftJoin<Users>((r, ul, u2) => r.ModifyUserId == u2.Id)
                .WhereIF(!string.IsNullOrEmpty(req.Name), u => u.Name.Contains(req.Name))
                .WhereIF(!string.IsNullOrEmpty(req.Description), u => u.Description.Contains(req.Description))
                .OrderBy((r) => r.Order)
                .Select((r, ul, u2) => new RoleRes()
                {
                    Name = r.Name,
                    CreateUserName = ul.Name,
                    ModifyUserName = u2.Name,
                }, true)
                .ToOffsetPageAsync(req.PageIndex, req.PageSize, total);
            res.Data = list;
            res.Total = total;
            return res;
        }
    }
}
