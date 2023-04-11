using System.Runtime.InteropServices;
using IGeekFan.AspNetCore.Knife4jUI;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using Swashbuckle.AspNetCore.Filters;

namespace Wiwi.Sample.Common.Extensions
{
    public static class SwaggerExtension
    {
        private static Logger logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

        /// <summary>
        /// 添加Swagger
        /// </summary>
        /// <param name="services"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddSwaggerSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            //var config = Appsettings.GetConfig<ApiConfig>("ApiConfig");

            var basePath = AppContext.BaseDirectory;
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = $"接口文档——{RuntimeInformation.FrameworkDescription}",
                    //Description = $"{config.ApiName} HTTP API " + config.ApiVersion,
                    //Contact = new OpenApiContact { Name = config.ApiName, Email = config.Email, Url = new Uri(config.Domain) },
                    //License = new OpenApiLicense { Name = config.ApiName + " 官方网站", Url = new Uri(config.Domain) }
                });
                c.OrderActionsBy(o => o.RelativePath);

                var xmlPath = Path.Combine(AppContext.BaseDirectory, "");
                var xmlFiles = Directory.GetFiles(xmlPath);
                if (xmlFiles.Length > 0)
                {
                    foreach (var file in xmlFiles)
                    {
                        if (file.EndsWith("xml"))
                        {
                            c.IncludeXmlComments(file, true); //添加控制器层注释（true表示显示控制器注释）
                        }
                    }
                }

                // 开启加权小锁
                c.OperationFilter<AddResponseHeadersFilter>();
                c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();

                // 在header中添加token，传递到后台
                c.OperationFilter<SecurityRequirementsOperationFilter>();

            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerMiddleware(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseKnife4UI(c =>
            {
                c.RoutePrefix = "";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "web api");
            });
            return app;
        }
    }
}
