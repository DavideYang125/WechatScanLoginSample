using SqlSugar;
using Wiwi.Sample.Common.Repository.UnitOfWork;

namespace Wiwi.Sample.Common.Repository.DbRepository
{
    public interface IRepository<TEntity> : ISimpleClient<TEntity> where TEntity : class, new()
    {
        public IUnitOfWork UnitOfWork { get; }
    }
}
