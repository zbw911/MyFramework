using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kt.Framework.State.Impl
{
    /// <summary>
    /// 包装器
    /// </summary>
    public class CacheWraper : ICacheWraper
    {
        public ICacheState CacheState;

        public CacheWraper(ICacheState CacheState)
        {
            this.CacheState = CacheState;
        }

        /// <summary>
        /// 绝对过期的智能方式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="absoluteExpiration"></param>
        /// <param name="GetDataFunc"></param>
        /// <returns></returns>
        public T SmartyGetPut<T>(object key, DateTime absoluteExpiration, Func<T> GetDataFunc)
        {
            T instance = this.CacheState.Get<T>(key.BuildFullKey<T>());
            if (instance != null) return instance;

            instance = GetDataFunc();
            if (instance == null) return instance;
            //放入缓存
            this.CacheState.Put<T>(key.BuildFullKey<T>(), instance, absoluteExpiration);
            return instance;
        }

        /// <summary>
        /// 绝对过期的智能方式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="absoluteExpiration"></param>
        /// <param name="GetDataFunc"></param>
        /// <returns></returns>
        public T SmartyGetPut<T>(DateTime absoluteExpiration, Func<T> GetDataFunc)
        {
            return this.SmartyGetPut<T>(null, absoluteExpiration, GetDataFunc);
        }

        /// <summary>
        /// 相对过期
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="slidingExpiration"></param>
        /// <param name="GetDataFunc"></param>
        /// <returns></returns>
        public T SmartyGetPut<T>(object key, TimeSpan slidingExpiration, Func<T> GetDataFunc)
        {
            T instance = this.CacheState.Get<T>(key.BuildFullKey<T>());
            if (instance != null) return instance;

            instance = GetDataFunc();
            if (instance == null) return instance;
            //放入缓存
            this.CacheState.Put<T>(key.BuildFullKey<T>(), instance, slidingExpiration);
            return instance;
        }

        /// <summary>
        /// 相对过期
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="slidingExpiration"></param>
        /// <param name="GetDataFunc"></param>
        /// <returns></returns>
        public T SmartyGetPut<T>(TimeSpan slidingExpiration, Func<T> GetDataFunc)
        {
            return SmartyGetPut<T>(null, slidingExpiration, GetDataFunc);
        }

        /// <summary>
        /// 永远不过期
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="GetDataFunc"></param>
        /// <returns></returns>
        public T SmartyGetPut<T>(object key, Func<T> GetDataFunc)
        {
            T instance = this.CacheState.Get<T>(key.BuildFullKey<T>());
            if (instance != null) return instance;

            instance = GetDataFunc();

            if (instance == null) return instance;

            //放入缓存
            this.CacheState.Put<T>(key.BuildFullKey<T>(), instance);
            return instance;
        }

        /// <summary>
        /// 永远不过期
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="GetDataFunc"></param>
        /// <returns></returns>
        public T SmartyGetPut<T>(Func<T> GetDataFunc)
        {
            return SmartyGetPut<T>(null, GetDataFunc);
        }
    }
}
