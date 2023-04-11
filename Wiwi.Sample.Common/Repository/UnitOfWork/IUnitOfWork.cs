using SqlSugar;

namespace Wiwi.Sample.Common.Repository.UnitOfWork
{
    public interface IUnitOfWork
    {
        SqlSugarScope DbClient { get; }

        void BeginTran();

        void CommitTran();
        void RollbackTran();
    }
}
