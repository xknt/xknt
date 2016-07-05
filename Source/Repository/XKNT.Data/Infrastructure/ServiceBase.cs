using System.Data.Entity;
using XKNT.Common.Infrastructure;

namespace XKNT.Data.Infrastructure
{
    public abstract class ServiceBase : RepositoryBase
    {
        public ServiceBase()
        {
            if (dbContext == null)
            {
                dbContext = new XKNT_DataContext(); //DependencyResolver.Current.GetService<C2S2B_DataContext>();
            }
        }
        public ServiceBase(DbContext _dbContent)
            : base(_dbContent)
        {

        }
    }
}
