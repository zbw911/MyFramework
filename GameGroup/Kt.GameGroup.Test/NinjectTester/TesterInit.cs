using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using System.Reflection;

namespace Kt.GameGroup.Tests.NinjectTester
{
    /// <summary>
    /// 
    /// </summary>
    public class TesterInit
    {
        private static IKernel kernel;

        public static IKernel Kernel
        {
            get { return TesterInit.kernel; }
        }

        static TesterInit()
        {
            kernel = new StandardKernel();

            kernel.Load(Assembly.GetExecutingAssembly());

        }

        public T Get<T>()
        {
            return (T)TesterInit.Kernel.Get(typeof(T));
        }

    }
}
