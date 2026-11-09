using Interface;
using Microsoft.AspNetCore.Mvc;
using Model.Dto.Login;
using Model.Dto.User;
using Model.Other;
using WebApi.Config;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IUserService _userService;
        private readonly ICustomJWTService _jwtService;
        public LoginController(ILogger<LoginController> logger, IUserService userService, ICustomJWTService jwtService)
        {
            _logger = logger;
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpPost]
        public async Task<ApiResult> GetToken([FromBody] LoginReq req)
        {
            //模型验证
            if (ModelState.IsValid)
            {
                UserRes user = await _userService.GetUser(req);
                if (user == null)
                {
                    return ResultHelper.Error("账号不存在，用户名或密码错误");
                }

                _logger.LogInformation("登录");
                return ResultHelper.Success(await _jwtService.GetToken(user)); //返沪JWT
            }
            else
            {
                return ResultHelper.Error("参数错误");
            }
        }
    }
}
