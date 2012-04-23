using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kt.Framework.State.Impl
{
    ///<summary>
    ///  
    ///</summary>
    public static class Utils
    {
        ///<summary>
        /// 为一个类型创建命名
        ///</summary>
        ///<param name="userKey">用户提供的类型</param>
        ///<typeparam name="T">用于创建KEY的类型</typeparam>
        ///<returns>返回KEY</returns>
        public static string BuildFullKey<T>(this object userKey)
        {
            if (userKey == null)
                return typeof(T).FullName;
            return typeof(T).FullName + userKey;
        }
    }
}
