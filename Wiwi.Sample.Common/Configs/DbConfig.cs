namespace Wiwi.Sample.Common.Configs
{
    public class DbConfig
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 是否迁移表结构
        /// </summary>
        public bool MigrateTable { get; set; }
    }
}
