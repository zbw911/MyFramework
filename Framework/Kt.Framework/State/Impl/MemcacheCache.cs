using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kt.Framework.State.Impl
{
    /// <summary>
    /// 使用memcache,
    /// </summary>
    class MemcacheCache : ICacheState
    {
        public T Get<T>()
        {
            throw new NotImplementedException();
        }

        public T Get<T>(object key)
        {
            throw new NotImplementedException();
        }

        public void Put<T>(T instance)
        {
            throw new NotImplementedException();
        }

        public void Put<T>(object key, T instance)
        {
            throw new NotImplementedException();
        }

        public void Put<T>(T instance, DateTime absoluteExpiration)
        {
            throw new NotImplementedException();
        }

        public void Put<T>(object key, T instance, DateTime absoluteExpiration)
        {
            throw new NotImplementedException();
        }

        public void Put<T>(T instance, TimeSpan slidingExpiration)
        {
            throw new NotImplementedException();
        }

        public void Put<T>(object key, T instance, TimeSpan slidingExpiration)
        {
            throw new NotImplementedException();
        }

        public void Remove<T>()
        {
            throw new NotImplementedException();
        }

        public void Remove<T>(object key)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }
    }
}
