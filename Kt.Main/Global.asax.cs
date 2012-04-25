#define USE_INJECT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject.Web.Mvc;
//using Kt.Framework.Core;
using Ninject;
using System.Reflection;
using Kt.Framework.Repository;
using Kt.Framework.Repository.ContainerAdapter.NInject;
using Kt.Framework.Repository.Data.EntityFramework;
using Ninject.Parameters;
using System.Data.Objects;
using Kt.Framework.Repository.Configuration;
using Microsoft.Practices.ServiceLocation;
using Kt.Framework.Core;

namespace Kt.Main
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

#if    USE_INJECT

    public class MvcApplication : NinjectHttpApplication//: System.Web.HttpApplication
    {
        public static string DefaultRouteUrl = "{controller}/{action}/{id}";
        public static string DefaultRouteName = "Index";
        //Kt.Framework.Tool.TrackEffect track;

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("api/uc");

            routes.MapRoute("ShortUrl", "U/{user}", new { controller = "U", action = "Index" }/*, new { user = @".*" }*/);

            routes.MapRoute(
               DefaultRouteName, // 路由名称
               DefaultRouteUrl,// "{controller}/{action}/{id}", // 带有参数的 URL
                new { controller = "Index", action = "Index", id = UrlParameter.Optional } // 参数默认值

                , new string[] { // register the controller
                 "Kt.Main.Controllers"
                }
                );


            StaticResources.Initialize(routes, DefaultRouteUrl);
        }

        public void Application_BeginRequest(Object sender, EventArgs e)
        {
            //track = new Framework.Tool.TrackEffect();
            //track.Begin("页面时间");
        }

        protected void Application_EndRequest(Object sender, EventArgs e)
        {
            //track.End();
            //track.Flush();

            //HttpContext.Current.Response.Write( "/*" +   Framework.Tool.TrackEffect.TrackinfosString + "*/" );
        }

        protected override void OnApplicationStarted()
        {
            base.OnApplicationStarted();



            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            //使用自定义视图引擎及控制器
            //System.Web.Mvc.ViewEngines.Engines.Clear();
            //System.Web.Mvc.ViewEngines.Engines.Add(new ViewEngine());
            //ControllerBuilder.Current.SetControllerFactory(new ControllerFactory(this.Kernel));

            //ServiceLocator.Initialize(this.Kernel);
        }

        protected override Ninject.IKernel CreateKernel()
        {

            //var modules = new INinjectModule[]
            //    {
            //        new WebModel()
            //    };


            var kernel = new StandardKernel();

            kernel.Load(Assembly.GetExecutingAssembly());

            init(kernel);
            return kernel;

        }

        public static void init(IKernel _kernel)
        {
            //var _kernel = new StandardKernel();

            Microsoft.Practices.ServiceLocation.ServiceLocator.SetLocatorProvider(() =>
            {
                return new CommonServiceLocator.NinjectAdapter.NinjectServiceLocator(_kernel);
            });

            Configure
                .Using(new NInjectContainerAdapter(_kernel))
                .ConfigureData<EFConfiguration>(EfConfig =>
                {
                    EfConfig.WithObjectContext(() =>
                    {
                        ConstructorArgument parameter2 = new ConstructorArgument("connectionString", System.Configuration.ConfigurationManager.ConnectionStrings["HelloWordEntities"].ConnectionString);
                        //装系统的方法注入构造函数
                        _kernel.Bind<ObjectContext>().To<HelloWorld.Model.HelloWordEntities>().InSingletonScope().WithParameter(parameter2);
                        var Entities2 = _kernel.Get<HelloWorld.Model.HelloWordEntities>();
                        return Entities2;
                    });
                    //EfConfig.WithObjectContext(() =>
                    //{
                    //    ConstructorArgument parameter = new ConstructorArgument("connectionString", System.Configuration.ConfigurationManager.ConnectionStrings["GameWeiBoEntities"].ConnectionString);
                    //    //装系统的方法注入构造函数
                    //    _kernel.Bind<ObjectContext>().To<Kt.GameWeiBo.Data.GameWeiBoEntities>().InRequestScope().WithParameter(parameter);
                    //    var Entities = _kernel.Get<Kt.GameWeiBo.Data.GameWeiBoEntities>();
                    //    return Entities;
                    //});

                    //EfConfig.WithObjectContext(() =>
                    //{
                    //    ConstructorArgument parameter = new ConstructorArgument("connectionString", System.Configuration.ConfigurationManager.ConnectionStrings["UserHomeEntities"].ConnectionString);
                    //    //装系统的方法注入构造函数
                    //    _kernel.Bind<ObjectContext>().To<Kt.UserHome.Data.UserHomeEntities>().InRequestScope().WithParameter(parameter);
                    //    var Entities = _kernel.Get<Kt.UserHome.Data.UserHomeEntities>();
                    //    return Entities;
                    //});

                    //EfConfig.WithObjectContext(() =>
                    //{
                    //    ConstructorArgument parameter = new ConstructorArgument("connectionString", System.Configuration.ConfigurationManager.ConnectionStrings["GameGroupEntities"].ConnectionString);
                    //    //装系统的方法注入构造函数
                    //    _kernel.Bind<ObjectContext>().To<Kt.GameGroup.Data.GameGroupEntities>().InRequestScope().WithParameter(parameter);
                    //    var Entities = _kernel.Get<Kt.GameGroup.Data.GameGroupEntities>();
                    //    return Entities;
                    //});
                })
                //.ConfigureState<DefaultStateConfiguration>(config => config.Configure(new NInjectContainerAdapter(_kernel)))
                .ConfigureState<DefaultStateConfiguration>()

                //.ConfigureUnitOfWork<DefaultUnitOfWorkConfiguration>(x => x.AutoCompleteScope())
                ;
        }


    }


#else
    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // 路由名称
                "{controller}/{action}/{id}", // 带有参数的 URL
                new { controller = "Index", action = "Index", id = UrlParameter.Optional } // 参数默认值

                ,

                new string[] { // register the c
                  //"Kt.Main.Areas.GameNav.Controllers"
                  "Kt.Main.Controllers"
                  //,"Kt.Main.Areas.GameWeiBo.Controllers"
                  //,"Kt.Main.Areas.UserHome.Controllers"
                }
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
#endif
}