using Interface;
using Microsoft.AspNetCore.Mvc;
using Model.Dto.Menu;
using Model.Other;
using WebApi.Config;
using WebApi.Controllers;

namespace Webapi.Controllers
{
    public class MenuController : BaseController
    {
        private readonly IMenuService _menuService;
        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [HttpPost]
        public async Task<ApiResult> Add(MenuAdd req)
        {
            userId = HttpContext.User.Claims.ToList()[0].Value;
            return ResultHelper.Success(await _menuService.Add(req, userId));
        }

        [HttpPost]
        public async Task<ApiResult> Edit(MenuEdit req)
        {
            userId = HttpContext.User.Claims.ToList()[0].Value;
            return ResultHelper.Success(await _menuService.Edit(req, userId));
        }

        [HttpGet]
        public async Task<ApiResult> Del(string id)
        {
            return ResultHelper.Success(await _menuService.Del(id));
        }

        [HttpGet]
        public async Task<ApiResult> BatchDel(string ids)
        {
            return ResultHelper.Success(await _menuService.BatchDel(ids));
        }

        [HttpPost]
        public async Task<ApiResult> GetMenus(MenuReq req)
        {
            userId = HttpContext.User.Claims.ToList()[0].Value;
            return ResultHelper.Success(await _menuService.GetMenus(req, userId));
        }

        [HttpGet]
        public async Task<ApiResult> SettingMenu(string rid, string mids)
        {
            return ResultHelper.Success(await _menuService.SettingMenu(rid, mids));
        }
    }
}