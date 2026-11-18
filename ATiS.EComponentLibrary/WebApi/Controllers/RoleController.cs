using Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Dto.Role;
using Model.Other;
using WebApi.Config;
using WebApi.Controllers;

namespace Webapi.Controllers
{
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost]
        public async Task<ApiResult> Add(RoleAdd req)
        {
            userId = HttpContext.User.Claims.ToList()[0].Value;
            return ResultHelper.Success(await _roleService.Add(req, userId));
        }

        [HttpPost]
        public async Task<ApiResult> Edit(RoleEdit req)
        {
            userId = HttpContext.User.Claims.ToList()[0].Value;
            return ResultHelper.Success(await _roleService.Edit(req, userId));
        }
        [HttpGet]
        public async Task<ApiResult> Del(string id)
        {
            return ResultHelper.Success(await _roleService.Del(id));
        }
        [HttpGet]
        public async Task<ApiResult> BatchDel(string ids)
        {
            return ResultHelper.Success(await _roleService.BatchDel(ids));
        }
        [HttpPost]
        public async Task<ApiResult> GetRoles(RoleReq req)
        {
            userId = HttpContext.User.Claims.ToList()[0].Value;
            return ResultHelper.Success(await _roleService.GetRoles(req, userId));
        }
    }
}