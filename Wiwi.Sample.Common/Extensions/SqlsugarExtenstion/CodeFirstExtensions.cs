using System.Reflection;
using Microsoft.AspNetCore.Builder;
using SqlSugar;
using Wiwi.Sample.Common.Configs;
using Wiwi.Sample.Common.Helper;

namespace Wiwi.Sample.Common.Extensions.SqlsugarExtenstion
{
    public static class CodeFirstExtensions
    {
        public static IApplicationBuilder UseCodeFirst(this IApplicationBuilder app, Assembly assembly)
        {
            var config = Appsettings.GetConfig<DbConfig>("DbConfig");
            if (config != null && config.MigrateTable)
            {
                var db = ServiceProviderHelper.GetService<ISqlSugarClient>();
                //code first
                var types = assembly.GetTypes()
                            .Where(it => !it.IsAbstract && it.IsPublic && !it.IsEnum)
                            .Where(it => typeof(IBaseDomain).IsAssignableFrom(it)).ToArray();
                db.CodeFirst.SetStringDefaultLength(200).InitTables(types);
            }
            return app;
        }
    }
}
