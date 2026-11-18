using Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Dto.User;
using Model.Entitys;
using Model.Other;
using WebApi.Config;
using WebApi.Controllers;

namespace Webapi.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<ApiResult> Add(UserAdd req)
        {
            userId = HttpContext.User.Claims.ToList()[0].Value;
            return ResultHelper.Success(await _userService.Add(req, userId));
        }

        [HttpPost]
        public async Task<ApiResult> Edit(UserEdit req)
        {
            userId = HttpContext.User.Claims.ToList()[0].Value;
            return ResultHelper.Success(await _userService.Edit(req, userId));
        }
        [HttpGet]
        public async Task<ApiResult> Del(string id)
        {
            return ResultHelper.Success(await _userService.Del(id));
        }
        [HttpGet]
        public async Task<ApiResult> BatchDel(string ids)
        {
            return ResultHelper.Success(await _userService.BatchDel(ids));
        }
        [HttpPost]
        public async Task<ApiResult> GetUsers(UserReq req)
        {
            userId = HttpContext.User.Claims.ToList()[0].Value;
            return ResultHelper.Success(await _userService.GetUsers(req, userId));
        }
        [HttpGet]
        public async Task<ApiResult> SettingRole(string uid, string rids)
        {
            return ResultHelper.Success(await _userService.SettingRole(uid, rids));
        }
        [HttpPost]
        public async Task<ApiResult> EditNickNameOrPassword([FromBody] PersonEdit req)
        {
            userId = HttpContext.User.Claims.ToList()[0].Value;
            return ResultHelper.Success(await _userService.EditNickNameOrPassword(userId, req));
        }
    }
}