using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject.Parameters;
using Ninject.Modules;
 

namespace Kt.Main.Core
{

    //public class WebModel : BaseWebModule<Kt.GameWeiBo.Data.GameWeiBoEntities>
    //{

    //    public WebModel()
    //        : base(Settings.EntityConnectionString)
    //    {
    //        //a
    //    }
    //}

    //GameNavEntities
    //public class GameNavModel : NinjectModule
    //{

    //    //public GameNavModel()
    //    //    : base(System.Configuration.ConfigurationManager.ConnectionStrings["GameNavEntities"].ConnectionString)
    //    //{
    //    //    //a
    //    //}



    //    public override void Load()
    //    {
    //        ConstructorArgument parameter = new ConstructorArgument("connectionString", System.Configuration.ConfigurationManager.ConnectionStrings["GameNavEntities"].ConnectionString);
    //        //装系统的方法注入构造函数
    //        Bind<Kt.GameNav.Data.GameNavEntities>().ToSelf().InRequestScope().WithParameter(parameter);
    //        //注入到存储过程方法

    //        //批量进行绑定
    //        Bind(typeof(IRepository<>)).To(typeof(GameNavEntityRepository<>)).InRequestScope();
    //    }
    //}

    ////GameNavEntities
    //public class GameGroupModel : NinjectModule
    //{

    //    //public GameNavModel()
    //    //    : base(System.Configuration.ConfigurationManager.ConnectionStrings["GameNavEntities"].ConnectionString)
    //    //{
    //    //    //a
    //    //}



    //    public override void Load()
    //    {
    //        ConstructorArgument parameter = new ConstructorArgument("connectionString", System.Configuration.ConfigurationManager.ConnectionStrings["GameGroupEntities"].ConnectionString);
    //        //装系统的方法注入构造函数
    //        Bind<Kt.GameGroup.Data.GameGroupEntities>().ToSelf().InRequestScope().WithParameter(parameter);
    //        //注入到存储过程方法

    //        //批量进行绑定
    //        Bind(typeof(IRepository<>)).To(typeof(GameGroupEntityRepository<>)).InRequestScope();
    //    }
    //}
    ////GameNavEntities
    //public class UserHomeModel : NinjectModule
    //{

    //    public override void Load()
    //    {
    //        ConstructorArgument parameter = new ConstructorArgument("connectionString", System.Configuration.ConfigurationManager.ConnectionStrings["UserHomeEntities"].ConnectionString);
    //        //装系统的方法注入构造函数
    //        Bind<Kt.UserHome.Data.UserHomeEntities>().ToSelf().InRequestScope().WithParameter(parameter);
    //        //注入到存储过程方法

    //        //批量进行绑定
    //        Bind(typeof(IRepository<>)).To(typeof(UserHomeEntityRepository<>)).InRequestScope();
    //    }
    //}


    /// <summary>
    /// 配置缓存方法
    /// </summary>
    public class CacheModel : NinjectModule
    {
        public override void Load()
        {
            Bind<Kt.Framework.State.ICacheState>().To<Kt.Framework.State.Impl.HttpRuntimeCache>();
            Bind<Kt.Framework.State.ICacheWraper>().To<Kt.Framework.State.Impl.CacheWraper>();
        }
    }


    


}