//using System;
//using System.Collections.Generic;
//using System.Linq.Expressions;
//using System.Data.Objects;

//namespace Kt.Framework.Repository
//{
//    /// <summary>
//    /// 公用存储接口, Repository> : IRepository<T> where T : EntityObject where Repository:ObjectContext
//    /// </summary>
//    /// <typeparam name="T"></typeparam>
//    public interface IRepository<T>
//        where T : class
//    {
//        ObjectSet<T> Table { get; }
//        /// <summary>
//        /// Returns an entity based on its primary key integer value.
//        /// </summary>
//        /// <param name="id">Integer value of the entity's primary key column.</param>
//        T Get(int id);

//        /// <summary>
//        /// 取得类型
//        /// </summary>
//        /// <typeparam name="TA"></typeparam>
//        /// <param name="id"></param>
//        /// <returns></returns>
//        T Get<TA>(TA id);
//        /// <summary>
//        /// Returns all the entities in the table.
//        /// </summary>
//        //[Obsolete("这是一个严重过时的方法，使用这个方法的同时，相当于选择了低性能.-zbw911 add 2011-5-24")]
//        IEnumerable<T> Get();

//        /// <summary>
//        /// Returns the first entity that matches a specific condition.
//        /// </summary>
//        /// <param name="where">Predicate function to test each entity.</param>
//        T First(Expression<Func<T, bool>> where);

//        /// <summary>
//        /// Returns entities that matches a specific condition.
//        /// </summary>
//        /// <param name="where">Predicate function to test each entity.</param>
//        IEnumerable<T> Where(Expression<Func<T, bool>> where);

//        /// <summary>
//        /// Adds an entity to the data context for database insertion.
//        /// </summary>
//        /// <param name="entity">Entity to add.</param>
//        T Add(T entity);

//        /// <summary>
//        /// Deletes the entity from the database.
//        /// </summary>
//        /// <param name="entity">Entity to delete.</param>
//        void Delete(T entity);

//        /// <summary>
//        /// Deletes a list of entites from the database.
//        /// </summary>
//        /// <param name="entities">List of entities to delete.</param>
//        void Delete(IEnumerable<T> entities);

//        /// <summary>
//        /// Deletes an entity from the database based on integer value of the primary key column.
//        /// </summary>
//        /// <param name="id">Integer value of the entity's primary key column.</param>
//        void Delete(int id);

//        /// <summary>
//        /// 根据ID进行删除，ID要指定类型
//        /// </summary>
//        /// <param name="id">模板类型</param>
//        void Delete<TA>(TA id);

//        /// <summary>
//        /// Updates an entity. Attaches the entity to the data context if it is not attached.
//        /// </summary>
//        /// <param name="entity">Entity to update.</param>
//        T Update(T entity);

//        /// <summary>
//        /// Saves changes made to the data context through this repository.
//        /// </summary>
//        void SaveChanges();
//    }
//}
