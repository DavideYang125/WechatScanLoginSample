using SqlSugar;

namespace Wiwi.Sample.Common.Configs
{
    public interface IBaseDomain
    {
    }

    public abstract class BaseDomain : IBaseDomain
    {
        /// <summary>
        /// 是否删除
        /// </summary>
        [SugarColumn(ColumnName = "is_deleted", ColumnDescription = "是否删除", DefaultValue = "0")]
        public bool IsDeleted { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(ColumnName = "create_time", ColumnDescription = "创建时间")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [SugarColumn(ColumnName = "update_time", IsNullable = true, ColumnDescription = "更新时间")]
        public DateTime UpdateTime { get; set; }
    }
}
