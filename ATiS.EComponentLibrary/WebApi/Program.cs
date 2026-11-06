using Autofac;
using Autofac.Extensions.DependencyInjection;
using SqlSugar;
using WebApi.Config;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<AutoMapperConfigs>();
});
builder.Services.AddControllers();

//替换内置IOC
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(container =>
{
    #region 通过模块化的方法注册接口层和实现层
    container.RegisterModule(new AutofacModuleRegister());
    #endregion

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

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();
app.Run();
