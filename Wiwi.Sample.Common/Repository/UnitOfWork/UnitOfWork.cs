using Microsoft.Extensions.Logging;
using SqlSugar;

namespace Wiwi.Sample.Common.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ISqlSugarClient _sqlSugarClient;
        private readonly ILogger<UnitOfWork> _logger;

        public UnitOfWork(ISqlSugarClient sqlSugarClient, ILogger<UnitOfWork> logger)
        {
            _sqlSugarClient = sqlSugarClient;
            _logger = logger;
        }
        public SqlSugarScope DbClient
        {
            get { return _sqlSugarClient as SqlSugarScope; }
        }

        public void BeginTran()
        {
            DbClient.BeginTran();
        }

        public void CommitTran()
        {
            try
            {
                DbClient.CommitTran();
            }
            catch (Exception ex)
            {
                DbClient.RollbackTran();
                _logger.LogError($"{ex.Message}\r\n{ex.InnerException}");
            }
        }

        public void RollbackTran()
        {
            DbClient.RollbackTran();
        }

    }
}
