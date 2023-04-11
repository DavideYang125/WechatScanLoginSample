using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NLog;
using NLog.Web;
using SqlSugar;
using Wiwi.Sample.Common.Configs;
using Wiwi.Sample.Common.Helper;

namespace Wiwi.Sample.Common.Extensions.SqlsugarExtenstion
{
    public static class ServicesExtensions
    {
        private static Logger logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

        #region SqlSugarSetup

        /// <summary>
        /// Sqlsugar
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSqlSugarSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            // 默认添加主数据库连接
            var config = Appsettings.GetConfig<DbConfig>("DbConfig");

            if (config == null)
            {
                return services;
            }

            services.AddSingleton<ISqlSugarClient>(o =>
            {
                var dbConfig = new ConnectionConfig()
                {
                    ConnectionString = config.ConnectionString,
                    DbType = DbType.MySql,
                    IsAutoCloseConnection = true,
                    ConfigureExternalServices = new ConfigureExternalServices()
                    {
                        EntityNameService = (type, entity) =>
                        {
                            entity.IsDisabledDelete = true;//全局禁止删除列
                        }
                    }
                };
                var Db = new SqlSugarScope(dbConfig, db =>
                {
                    //#region 过滤器
                    //db.QueryFilter.AddTableFilter<IDeleted>(it => it.IsDeleted == false);
                    //#endregion
                    //调试SQL事件，可以删掉
                    db.Aop.OnLogExecuted = (sql, pars) =>
                    {
                        if (db.Ado.SqlExecutionTime.TotalSeconds > 1)
                        {
                            logger.Info("long time sql={0}\n SqlStackTrace={1}", sql, JsonConvert.SerializeObject(db.Ado.SqlStackTrace));
                        }
                    };
                    db.Aop.OnLogExecuting = (sql, pars) =>
                    {
                        var log = sql;
                        if (pars != null && pars.Length > 0)
                        {
                            log += "\n" + string.Join(",", pars.Select(it => it.ParameterName + ":" + it.Value));
                        }
                        logger.Info(log);
                    };
                });

                return Db;
            });

            return services;
        }

        #endregion
    }
}
