using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;


namespace XKNT.Data
{
    public class XKNT_DataContext : DbContext
    {

        /// <summary>
        /// 将实体表转化实体对象
        /// </summary>
        /// <param name="modelBuilder"></param>
        void LoadEntityToTable(DbModelBuilder modelBuilder)
        {
           // modelBuilder.Entity<System_Store_Info>().ToTable("System_Store_Info");
            

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();//移除复数表名的契约
            LoadEntityToTable(modelBuilder);
        }
    }

    public class ForeignAffairsDataContextInitializer : DropCreateDatabaseIfModelChanges<XKNT_DataContext>
    {
        protected override void Seed(XKNT_DataContext context)
        {

        }
    }
}