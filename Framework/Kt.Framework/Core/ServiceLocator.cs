using Ninject.Parameters;
using Ninject;

//namespace Kt.Framework.Core
//{
//    /// <summary>
//    /// 反转器
//    /// </summary>
//    public static class ServiceLocator
//    {
//        public static IKernel Kernel { get; private set; }

//        public static void Initialize(IKernel kernel)
//        {
//            Kernel = kernel;
//        }

//        public static T Get<T>(params IParameter[] parameters)
//        {
//            return Kernel.Get<T>(parameters);
//        }
//    }
//}