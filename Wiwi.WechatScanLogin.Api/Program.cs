using Autofac.Core;
using NLog;
using NLog.Web;
using SqlSugar;
using Wiwi.Sample.Common.Cache;
using Wiwi.Sample.Common.Extensions;
using Wiwi.Sample.Common.Filters;
using Wiwi.Sample.Common.Helper;
using Wiwi.Sample.Common.Wp;

var builder = WebApplication.CreateBuilder(args);

#region log

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

#endregion

//配置
Appsettings.Configuration = builder.Configuration;

// Add services to the container.
builder.Services
    .AddAuthorization();
builder.Services.AddCors(options => options.AddPolicy("AllowCors",
               builder =>
               {
                   builder.AllowAnyMethod()
                       .AllowAnyHeader()
                       .SetIsOriginAllowed(_ => true)
                       .AllowCredentials();
               }));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerSetup();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
}).AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver =
        new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(); //json字符串大小写原样输出
});

builder.Services.AddRedisCacheService(() =>
{
    var config = Appsettings.GetConfig<RedisConfig>("RedisConfig");
    return new RedisConfig
    {
        ConnectionString = config.ConnectionString,
        InstanceName = config.InstanceName
    }; 
});

builder.Host.UseDefaultServiceProvider(options =>
{
    options.ValidateScopes = false;
}).UseAutofac("Wiwi.WechatScanLogin.Api.dll");

builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddDefaultWeChat();
builder.Host.UseNLog();

var app = builder.Build();

app.Services.SetServiceProvider();
app.UseHttpsRedirection();
app.UseSwaggerMiddleware();
//Cors
app.UseCors("AllowCors");

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();
app.Run();