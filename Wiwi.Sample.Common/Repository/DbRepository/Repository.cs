using SqlSugar;
using Wiwi.Sample.Common.Repository.UnitOfWork;

namespace Wiwi.Sample.Common.Repository.DbRepository
{
    public class Repository<TEntity> : SimpleClient<TEntity>, IRepository<TEntity> where TEntity : class, new()
    {
        public IUnitOfWork UnitOfWork { get; private set; }

        public Repository(IUnitOfWork unitOfWork) : base(unitOfWork.DbClient)
        {
            UnitOfWork = unitOfWork;
        }
    }
}