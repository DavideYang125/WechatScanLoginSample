using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Wiwi.Sample.Common.Repository.DbRepository;
using Wiwi.Sample.Common.Repository.UnitOfWork;

namespace Wiwi.Sample.Common.Extensions
{
    public static class ApplicationExtensions
    {
        #region Autofac
        public static IHostBuilder UseAutofac(this IHostBuilder hostBuilder, params string[] dllFiles)
        {
            hostBuilder.UseServiceProviderFactory(x => new AutofacServiceProviderFactory());
            hostBuilder.ConfigureContainer<ContainerBuilder>(builder =>
            {
                builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerDependency().PropertiesAutowired();//注册仓储
                builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();

                if (dllFiles != null && dllFiles.Length > 0)
                {
                    var basePath = AppContext.BaseDirectory;
                    foreach (var dllFile in dllFiles)
                    {
                        builder.RegisterAssemble(Path.Combine(basePath, dllFile));
                    }
                }
            });

            return hostBuilder;
        }

        private static void RegisterAssemble(this ContainerBuilder builder, string path)
        {
            var assemblysServices = Assembly.LoadFrom(path);
            builder.RegisterAssemblyTypes(assemblysServices)
                      .AsImplementedInterfaces()
                      .InstancePerDependency()
                      .PropertiesAutowired();
        }
        #endregion
    }
}
