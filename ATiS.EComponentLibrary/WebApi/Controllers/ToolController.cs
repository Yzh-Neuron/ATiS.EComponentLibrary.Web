using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Model.Entitys;
using SqlSugar;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ToolController : Controller
    {
        private readonly ISqlSugarClient _sqlSugarClient;
        public ToolController(ISqlSugarClient sqlSugarClient)
        {
            _sqlSugarClient = sqlSugarClient;
        }

        [HttpGet]
        public async Task<bool> CodeFirst()
        {
            // 1. 创建数据库（如果不存在）
            _sqlSugarClient.DbMaintenance.CreateDatabase();

            // 2. 通过反射加载程序集，读取所有类型，根据实体创建表
            string nspace = "Model.Entitys";
            Type[] ass = Assembly.LoadFrom(AppContext.BaseDirectory + "Model.dll")
                .GetTypes()
                .Where(p => p.Namespace == nspace)
                .ToArray();

            _sqlSugarClient.CodeFirst.SetStringDefaultLength(200).InitTables(ass);

            // 3. 检查是否已存在超级管理员
            var existingAdmin = await _sqlSugarClient.Queryable<Users>()
                .Where(u => u.Name == "admin")
                .FirstAsync();

            if (existingAdmin != null)
            {
                Console.WriteLine("超级管理员已存在，跳过初始化");
                return true;
            }

            // 4. 初始化超级管理员
            Users user = new Users()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "admin",
                NickName = "超级管理员",
                Password = "123456", // 注意：生产环境应该加密存储
                UserType = 0,
                IsEnable = true,
                Description = "数据库初始化时默认的超级管理员",
                CreateDate = DateTime.Now,
                CreateUserId = ""
            };

            string userId = (await _sqlSugarClient.Insertable(user).ExecuteReturnEntityAsync()).Id;
            Console.WriteLine($"超级管理员创建成功，ID: {userId}");

            // 5. 清空现有菜单数据（避免重复）
            int deletedCount = await _sqlSugarClient.Deleteable<Menu>().ExecuteCommandAsync();
            Console.WriteLine($"已清理 {deletedCount} 条旧菜单记录");

            // 6. 初始化菜单
            // 一级菜单 - 菜单管理
            var m1 = new Menu()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "菜单管理",
                Index = "/menu",
                FilePath = "menu.vue",
                ParentId = "",
                Order = 1,
                IsEnable = true,
                Icon = "folder",
                Description = "数据库初始化时默认添加的默认菜单",
                CreateDate = DateTime.Now,
                CreateUserId = userId
            };
            string mid1 = (await _sqlSugarClient.Insertable(m1).ExecuteReturnEntityAsync()).Id;
            Console.WriteLine($"创建菜单管理成功，ID: {mid1}");

            // 二级菜单 - 菜单列表
            var ml1 = new Menu()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "菜单列表",
                Index = "/menu",
                FilePath = "menu.vue",
                ParentId = mid1,
                Order = 1,
                IsEnable = true,
                Icon = "list",
                Description = "数据库初始化时默认添加的默认菜单",
                CreateDate = DateTime.Now,
                CreateUserId = userId
            };
            await _sqlSugarClient.Insertable(ml1).ExecuteCommandAsync();
            Console.WriteLine("创建菜单列表成功");

            // 一级菜单 - 角色管理
            var m2 = new Menu()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "角色管理",
                Index = "/role",
                FilePath = "role.vue",
                ParentId = "",
                Order = 2,
                IsEnable = true,
                Icon = "folder",
                Description = "数据库初始化时默认添加的默认菜单",
                CreateDate = DateTime.Now,
                CreateUserId = userId
            };
            string mid2 = (await _sqlSugarClient.Insertable(m2).ExecuteReturnEntityAsync()).Id;
            Console.WriteLine($"创建角色管理成功，ID: {mid2}");

            // 二级菜单 - 角色列表
            var m22 = new Menu()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "角色列表",
                Index = "/role",
                FilePath = "role.vue",
                ParentId = mid2,
                Order = 1,
                IsEnable = true,
                Icon = "notebook",
                Description = "数据库初始化时默认添加的默认菜单",
                CreateDate = DateTime.Now,
                CreateUserId = userId
            };
            await _sqlSugarClient.Insertable(m22).ExecuteCommandAsync();
            Console.WriteLine("创建角色列表成功");

            // 一级菜单 - 用户管理
            var m3 = new Menu()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "用户管理",
                Index = "/user",
                FilePath = "user.vue",
                ParentId = "",
                Order = 3,
                IsEnable = true,
                Icon = "folder",
                Description = "数据库初始化时默认添加的默认菜单",
                CreateDate = DateTime.Now,
                CreateUserId = userId
            };
            string mid3 = (await _sqlSugarClient.Insertable(m3).ExecuteReturnEntityAsync()).Id;
            Console.WriteLine($"创建用户管理成功，ID: {mid3}");

            // 二级菜单 - 用户列表
            var m33 = new Menu()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "用户列表",
                Index = "/user",
                FilePath = "user.vue", // 修正了原始代码中的错误路径
                ParentId = mid3,
                Order = 1,
                IsEnable = true,
                Icon = "user",
                Description = "数据库初始化时默认添加的默认菜单",
                CreateDate = DateTime.Now,
                CreateUserId = userId
            };

            return await _sqlSugarClient.Insertable(m33).ExecuteCommandIdentityIntoEntityAsync();
        }
    }
}
