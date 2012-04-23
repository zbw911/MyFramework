using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kt.Framework.State
{
    /// <summary>
    /// 包装器接口
    /// </summary>
    public interface ICacheWraper
    {

        /// <summary>
        /// 绝对过期
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="absoluteExpiration"></param>
        /// <param name="GetDataFunc"></param>
        /// <returns></returns>
        T SmartyGetPut<T>(object key, DateTime absoluteExpiration, Func<T> GetDataFunc);

        /// <summary>
        /// 绝对过期
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="absoluteExpiration"></param>
        /// <param name="GetDataFunc"></param>
        /// <returns></returns>
        T SmartyGetPut<T>(DateTime absoluteExpiration, Func<T> GetDataFunc);

        /// <summary>
        /// 不过期
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="GetDataFunc"></param>
        /// <returns></returns>
        T SmartyGetPut<T>(object key, Func<T> GetDataFunc);


        /// <summary>
        /// 不过期
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="GetDataFunc"></param>
        /// <returns></returns>
        T SmartyGetPut<T>(Func<T> GetDataFunc);


        /// <summary>
        /// 相对过期
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="slidingExpiration"></param>
        /// <param name="GetDataFunc"></param>
        /// <returns></returns>
        T SmartyGetPut<T>(object key, TimeSpan slidingExpiration, Func<T> GetDataFunc);

        /// <summary>
        /// 相对过期
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="slidingExpiration"></param>
        /// <param name="GetDataFunc"></param>
        /// <returns></returns>
        T SmartyGetPut<T>(TimeSpan slidingExpiration, Func<T> GetDataFunc);
    }
}
