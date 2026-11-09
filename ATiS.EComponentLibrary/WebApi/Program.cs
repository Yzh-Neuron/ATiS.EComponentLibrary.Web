using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Model.Other;
using Newtonsoft.Json.Serialization;
using SqlSugar;
using WebApi.Config;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//替换内置IOC
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(container =>
{
    //通过模块化的方法注册接口层和实现层
    container.RegisterModule(new AutofacModuleRegister());

    //注册数据库访问客户端
    container.Register<ISqlSugarClient>(context =>
    {
        return new SqlSugarClient(new ConnectionConfig
        {
            DbType = DbType.MySql,
            IsAutoCloseConnection = true,
            ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")
        });
    });

});
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<AutoMapperConfigs>();
});

#region Jwt鉴权和配置的添加
var jwtSection = builder.Configuration.GetSection("Jwt");
builder.Services.Configure<JWTTokenOptions>(jwtSection);

JWTTokenOptions tokenOptions = new JWTTokenOptions();
builder.Configuration.Bind("Jwt", tokenOptions);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,//是否验证Issure
        ValidateAudience = true,//是否验证Audience
        ValidateLifetime = true,//是否验证失效时间
        ValidateIssuerSigningKey = true,//是否验证安全密钥

        ValidIssuer = tokenOptions.Issuer,
        ValidAudience = tokenOptions.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.SecurityKey)),

        ClockSkew = TimeSpan.FromSeconds(0), //设置Token过期多久失效
    };
});
#endregion

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    //忽略循环引用
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    //设置统一的日期格式
    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
    //设置Json返回格式同Model一致（默认Json首字符为小写，改为同后端Model一致）
    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();
app.Run();
