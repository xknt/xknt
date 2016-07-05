using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Data.Common;

namespace XKNT.Common.Infrastructure
{
    public abstract class RepositoryBaseT<T> : RepositoryBase where T : class
    {
        #region 获取DbSet

        public DbSet<T> GetDbSet()
        {
            return dbContext.Set<T>();
        }

        #endregion

        #region 执行当前T数据的一般操作
        public void Add(T entity)
        {
            GetDbSet().Add(entity);
        }
        public void Add(IEnumerable<T> list)
        {
            foreach (T entity in list)
                GetDbSet().Add(entity);
        }
        public void Update(T entity)
        {
            GetDbSet().Attach(entity);
            dbContext.Entry(entity).State = EntityState.Modified;
        }
        public void Update(T entity, string[] arrCol)
        {
            var dbEntity = dbContext.Entry<T>(entity);
            dbEntity.State = EntityState.Unchanged;
            foreach (var col in arrCol)
            {
                dbEntity.Property(col).IsModified = true;
            }
        }
        public void Update(T entity, string sCol)
        {
            var dbEntity = dbContext.Entry<T>(entity);
            dbEntity.State = EntityState.Unchanged;
            dbEntity.Property(sCol).IsModified = true;
        }
        public void Delete(T entity)
        {
            GetDbSet().Remove(entity);
        }
        public void Delete(Expression<Func<T, bool>> where)
        {
            var list = GetMany(where).ToList();
            foreach (T entity in list) 
                Delete(entity);
        }
        //public void Delete(IEnumerable<T> list)
        //{
        //    foreach (T entity in list)
        //        Delete(entity);
        //}
        public T GetByID(object ID)
        {
            return GetDbSet().Find(ID);
        }
        public IQueryable<T> GetAll()
        {
            return GetDbSet();
        }
        public IQueryable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return GetAll().Where(where);
        }
        public T GetOne(Expression<Func<T, bool>> where)
        {
            return GetMany(where).FirstOrDefault();
        }
        #endregion  
    }
}
