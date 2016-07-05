using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Common;
using System.Collections.Specialized;
using System.Data.Entity.Infrastructure;
using XKNT.Common.Component;
using XKNT.Common.Helper;

namespace XKNT.Common.Infrastructure
{
    public abstract class RepositoryBase
    {
        protected DbContext dbContext;

        #region 构造方法
        public RepositoryBase()
        {

        }
        public RepositoryBase(DbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        #endregion

        #region 获取DbSet

        public DbSet<TEntity> GetDbSet<TEntity>() where TEntity : class
        {
            return dbContext.Set<TEntity>();
        }

        #endregion

        #region 执行数据的一般操作

        public void Add<TEntity>(TEntity entity) where TEntity : class
        {
            GetDbSet<TEntity>().Add(entity);
        }
        public void Add<TEntity>(IEnumerable<TEntity> list) where TEntity : class
        {
            foreach (TEntity entity in list)
                GetDbSet<TEntity>().Add(entity);
        }
        public void Update<TEntity>(TEntity entity) where TEntity : class
        {
            GetDbSet<TEntity>().Attach(entity);
            dbContext.Entry(entity).State = EntityState.Modified;
        }
        public void Update<TEntity>(TEntity entity, IEnumerable<string> listCol) where TEntity : class
        {
            var dbEntity = dbContext.Entry<TEntity>(entity);
            dbEntity.State = EntityState.Unchanged;
            foreach (var col in listCol)
            {
                dbEntity.Property(col).IsModified = true;
            }
        }
        public void Update<TEntity>(TEntity entity, string sCol) where TEntity : class
        {
            var dbEntity = dbContext.Entry<TEntity>(entity);
            dbEntity.State = EntityState.Unchanged;
            dbEntity.Property(sCol).IsModified = true;
        }
        public void Delete<TEntity>(TEntity entity) where TEntity : class
        {
            GetDbSet<TEntity>().Attach(entity);
            dbContext.Entry(entity).State = EntityState.Deleted;
        }
        public void Delete<TEntity>(Expression<Func<TEntity, bool>> where) where TEntity : class
        {
            var list = GetMany<TEntity>(where).ToList();
            foreach (TEntity entity in list)
                Delete<TEntity>(entity);
        }
        public void Delete<TEntity>(IEnumerable<TEntity> list) where TEntity : class
        {
            foreach (TEntity entity in list)
                Delete<TEntity>(entity);
        }
        public TEntity GetByID<TEntity>(object ID) where TEntity : class
        {
            return GetDbSet<TEntity>().Find(ID);
        }
        public IQueryable<TEntity> GetAll<TEntity>() where TEntity : class
        {
            return GetDbSet<TEntity>();
        }
        public IQueryable<TEntity> GetMany<TEntity>(Expression<Func<TEntity, bool>> where) where TEntity : class
        {
            return GetAll<TEntity>().Where(where);
        }
        public TEntity GetOne<TEntity>(Expression<Func<TEntity, bool>> where) where TEntity : class
        {
            return GetMany<TEntity>(where).FirstOrDefault();
        }
        #endregion

        #region 处理显式事务
        private DbConnection conn = null;
        protected DbTransaction trans = null;

        protected void BeginTrans()
        {
            conn = dbContext.Database.Connection;
            conn.Open();
            conn.BeginTransaction();
        }
        protected void EndTrans()
        {
            if (trans != null) trans.Dispose();
            conn.Close();
        }
        #endregion

        public int Commit()
        {
            try
            {
                return dbContext.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                throw e;
            }
            catch (DbException e)
            {
                throw e;
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #region 查询数据

        private readonly string Row_NumberSql = @"select * from (
select *,Row_Number() over(order by {0}) as RowIndex  from ({1} ) as _rn
) as _row
where _row.RowIndex> {2} and _row.RowIndex<{3}";

        public int Excute(string sql, params object[] parameters)
        {
            return dbContext.Database.ExecuteSqlCommand(sql, parameters);
        }
        public TEntity ExcuteToEntity<TEntity>(string sql, params object[] parameters)
        {
            return dbContext.Database.SqlQuery<TEntity>(sql, parameters).FirstOrDefault();
        }
        public IEnumerable<TEntity> ExcuteToEnumerable<TEntity>(string sql, params object[] parameters)
        {
            return dbContext.Database.SqlQuery<TEntity>(sql, parameters);
        }
        public List<TEntity> ExcuteToEnumerable<TEntity>(IPaginationSql pSql) where TEntity : class,new()
        {
            if (!pSql.IsAll)
            {
                if (string.IsNullOrEmpty(pSql.sqlCount))
                {
                    pSql.sqlCount = string.Format("select count(*) from ({0}) as a", pSql.sql);
                }
                pSql.RowCount = this.ExcuteToEntity<Int32>(pSql.sqlCount);
            }
            string sql = this.GetPaginationSql(pSql);

            var result = this.ExcuteToEnumerable<TEntity>(sql).ToList();
            return result;
        }
        /// <summary>
        /// 获取分页sql
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="BeginRowIndex"></param>
        /// <param name="EndRowIndex"></param>
        /// <returns></returns>
        protected string GetPaginationSql(IPaginationSql pSql)
        {
            if (pSql.IsAll)
            {
                return pSql.sql;
            }
            //id desc
            string orderby = "id desc";
            if (!String.IsNullOrEmpty(pSql.OrderBy))
            {
                orderby = pSql.OrderBy;
            }
            return string.Format(Row_NumberSql, orderby, pSql.sql, pSql.BeginRowIndex - 1, pSql.EndRowIndex + 1);
        }

        public List<TEntity> ExcuteToEnumerable<TEntity>(JQueryDataTableParamModel pSql, out int rowCount) where TEntity : class,new()
        {
            string sort = string.Empty;

            int isortCol = pSql.iSortCol_0;
            string sortName = pSql.sSortDir_0;
            string[] arrCol = pSql.sColumns.Split(',');

            if (arrCol != null && arrCol.Length > 0)
            {
                sort = arrCol[isortCol] + " " + sortName;
            }

            int startIndex = pSql.iDisplayStart <= 0 ? 1 : pSql.iDisplayStart;
            string sql = string.Format(Row_NumberSql, sort, pSql.sql, startIndex - 1, startIndex + pSql.iDisplayLength);
            var result = this.ExcuteToEnumerable<TEntity>(sql).ToList();

            sql = string.Format("select count(*) from ({0}) as a", pSql.sql);
            rowCount = this.ExcuteToEntity<Int32>(sql);

            return result;
        }

        public List<TEntity> ExcuteToEnumerable<TEntity>(NameValueCollection form, out int rowCount) where TEntity : class,new()
        {
            int isortCol = TypeHelper.ToInt(form["iSortCol_0"]);
            int iDisplayStart = TypeHelper.ToInt(form["iDisplayStart"]);
            int iDisplayLength = TypeHelper.ToInt(form["iDisplayLength"]);
            string sCol = StringHelper.Filter(form["sColumns"]);
            string sortName = StringHelper.Filter(form["sSortDir_0"]);

            string tableSql = form["table"];
            string sort = form["sort"];

            if (!string.IsNullOrEmpty(sortName) && !string.IsNullOrEmpty(sCol))
            {
                string[] arrCol = sCol.Split(',');
                if (arrCol.Length > 0)
                {
                    sort = arrCol[isortCol] + " " + sortName;
                }
            }
            string sql = string.Empty;
            if (iDisplayLength == -1)
            {
                sql = tableSql;
            }
            else
            {
                sql = string.Format(Row_NumberSql, sort, tableSql, iDisplayStart, iDisplayStart + iDisplayLength + 1);
            }
            var result = this.ExcuteToEnumerable<TEntity>(sql).ToList();

            sql = string.Format("select count(*) from ({0}) as a", tableSql);
            rowCount = this.ExcuteToEntity<Int32>(sql);
            return result;
        }


        /// <summary>
        /// 进货底层分页
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="form"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public List<TEntity> ExcuteToEnumerablePurchase<TEntity>(NameValueCollection form, out int rowCount) where TEntity : class,new()
        {
            int isortCol = TypeHelper.ToInt(form["iSort"]);//排序方式 0 正序 1倒序
            int iDisplayLength = TypeHelper.ToInt(form["PageSize"]);//一页显示条数
            int iDisplayStart = iDisplayLength * TypeHelper.ToInt(form["PageIndex"]);

            string tableSql = form["table"];
            string sort = form["sort"];


            string sql = string.Empty;
            if (iDisplayLength == -1)
            {
                sql = tableSql;
            }
            else
            {
                sql = string.Format(Row_NumberSql, sort, tableSql, iDisplayStart, iDisplayStart + iDisplayLength + 1);
            }
            var result = this.ExcuteToEnumerable<TEntity>(sql).ToList();

            sql = string.Format("select count(*) from ({0}) as a", tableSql);
            rowCount = this.ExcuteToEntity<Int32>(sql);
            return result;
        }

        #endregion

    }
}
