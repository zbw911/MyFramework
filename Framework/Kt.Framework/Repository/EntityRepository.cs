//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Objects;
//using System.Data.Objects.DataClasses;
//using System.Linq;
//using System.Linq.Expressions;


//namespace Kt.Framework.Repository
//{
//    /// <summary>
//    /// 库公用方法
//    /// </summary>
//    /// <typeparam name="T"></typeparam>
//    public class EntityRepository<T> : IRepository<T>
//        where T : EntityObject
//    {
//        /// <summary>
//        /// 返回ObjectSet,直接使用,用以提升多表查询性能
//        /// </summary>
//        public ObjectSet<T> Table
//        {
//            get { return _table; }
//        }

//        protected ObjectContext _context;
//        protected ObjectSet<T> _table;
//        public EntityRepository(ObjectContext objectContext)
//        {
//            _context = objectContext;
//            _table = _context.CreateObjectSet<T>();
//        }

//        public void Detach(T entity)
//        {
//            _table.Detach(entity);
//        }

//        public T Get(int id)
//        {
//            return this.Get<int>(id);
//        }

//        /// <summary>
//        /// 根据ID取得，added by zbw911
//        /// </summary>
//        /// <typeparam name="TA"></typeparam>
//        /// <param name="id"></param>
//        /// <returns></returns>
//        public T Get<TA>(TA id)
//        {
//            string entitySetName = _context.DefaultContainerName + "." + _table.EntitySet.Name;
//            string keyName = _table.EntitySet.ElementType.KeyMembers[0].ToString();
//            EntityKey key = new EntityKey(entitySetName, new[] { new EntityKeyMember(keyName, id) });

//            object found;
//            if (_context.TryGetObjectByKey(key, out found))
//                return (T)found;
//            else
//                return null;
//        }

//        public IEnumerable<T> Get()
//        {
//            return _table.AsEnumerable();
//        }

//        public T Add(T entity)
//        {
//            _table.AddObject(entity);
//            SaveChanges();
//            return entity;
//        }

//        public void Delete(T entity)
//        {
//            _table.DeleteObject(entity);
//            SaveChanges();
//        }

//        public T Update(T entity)
//        {
//            if (entity.EntityState == EntityState.Detached)
//            {
//                _table.Attach(entity);
//            }

//            _table.Context.ObjectStateManager.ChangeObjectState(entity, System.Data.EntityState.Modified);

//            SaveChanges();

//            return entity;
//        }

//        public T First(Expression<Func<T, bool>> where)
//        {
//            return _table.FirstOrDefault(where);
//        }

//        public IEnumerable<T> Where(Expression<Func<T, bool>> where)
//        {
//            return _table.Where<T>(where);
//        }

//        public void Delete(int id)
//        {
//            this.Delete<int>(id);
//        }

//        public void Delete<TA>(TA id)
//        {
//            var entity = Get<TA>(id);
//            _table.DeleteObject(entity);
//            SaveChanges();
//        }

//        public void Delete(IEnumerable<T> entities)
//        {
//            foreach (T entity in entities)
//            {
//                _table.DeleteObject(entity);
//            }

//            SaveChanges();
//        }

//        public void SaveChanges()
//        {
//            _table.Context.SaveChanges();
//        }
//    }

//}
