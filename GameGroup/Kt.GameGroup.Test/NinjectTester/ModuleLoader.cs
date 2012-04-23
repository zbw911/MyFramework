using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kt.Framework.Core;
using Ninject.Modules;
using Ninject.Parameters;
using Kt.GameGroup.Data;


namespace Kt.GameGroup.Tests.NinjectTester
{


    public class GameGroupModel : NinjectModule
    {

        //public GameNavModel()
        //    : base(System.Configuration.ConfigurationManager.ConnectionStrings["GameNavEntities"].ConnectionString)
        //{
        //    //a
        //}



        //public override void Load()
        //{
        //    ConstructorArgument parameter = new ConstructorArgument("connectionString", System.Configuration.ConfigurationManager.ConnectionStrings["GameGroupEntities"].ConnectionString);
        //    //装系统的方法注入构造函数
        //    Bind<Kt.GameGroup.Data.GameGroupEntities>().ToSelf().InRequestScope().WithParameter(parameter);
        //    //注入到存储过程方法

        //    //批量进行绑定
        //    Bind(typeof(IRepository<>)).To(typeof(GameGroupEntityRepository<>)).InRequestScope();
        //}
        public override void Load()
        {
            throw new NotImplementedException();
        }
    }


}
