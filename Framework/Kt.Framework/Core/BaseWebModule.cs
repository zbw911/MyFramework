//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using Ninject.Parameters;
//using Ninject.Modules;
//using System.Data.Objects;
//using Kt.Framework.Common;
//using System.Data.Objects.DataClasses;
//using Kt.Framework.Repository.Data;

//namespace Kt.Framework.Core
//{
//    public class BaseWebModule<OContext> : NinjectModule where OContext:ObjectContext
//    {
//        private string EntityConnectionString;

//        public BaseWebModule(string EntityConnectionString)
//        {
//            // TODO: Complete member initialization
//            this.EntityConnectionString = EntityConnectionString;
//        }
        
//        public override void Load()
//        {
//            ConstructorArgument parameter = new ConstructorArgument("connectionString", this.EntityConnectionString);
//            //装系统的方法注入构造函数
//            Bind<ObjectContext>().To<OContext>().InRequestScope().WithParameter(parameter);
//            //注入到存储过程方法
//            Bind<IStoredProcedures<OContext>>().To<StoredProcedures<OContext>>().InRequestScope();
//            //批量进行绑定
//            Bind(typeof(IRepository<>)).To(typeof(EntityRepository<>)).InRequestScope();
//        }
//    }
//}