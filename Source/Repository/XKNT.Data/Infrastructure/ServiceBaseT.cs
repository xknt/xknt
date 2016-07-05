using System.Data.Entity;
using XKNT.Common.Infrastructure;

namespace XKNT.Data.Infrastructure
{
    public abstract class ServiceBaseT<T> : RepositoryBaseT<T> where T : class
    {
        #region 构造方法
        public ServiceBaseT()
        {
            if (dbContext == null)
            {
                dbContext = new XKNT_DataContext(); //DependencyResolver.Current.GetService<C2S2B_DataContext>();
            }
        }
        public ServiceBaseT(DbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        #endregion
    }
}
