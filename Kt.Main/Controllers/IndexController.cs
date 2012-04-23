using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kt.Main.Core;

namespace Kt.Main.Controllers
{
    public class IndexController : BaseController
    {
        private Kt.Framework.State.ICacheWraper CacheWraper;
        private HelloWorld.Service.HServices HServices;

        public IndexController(Kt.Framework.State.ICacheWraper CacheWraper, HelloWorld.Service.HServices HServices)
        {
            this.CacheWraper = CacheWraper;
            this.HServices = HServices;
        }
        //
        // GET: /Index/

        public ActionResult Index_bak()
        {

            //if (Kt.Framework.User.User.IS_LOGIN)
            //{
            //    return RedirectToAction("Index", "Index", new { Area = "UserHome" });
            //}
            //else
            //{
            //    var GetTopicList = this._TopicServices.GetTopicList(20);
            //    return View(GetTopicList);
            //}

            return null;
        }

        /// <summary>
        /// 网站新首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {


            return View(this.HServices.GetAllData());

            return null;

        }



        /// <summary>
        /// 网站新首页
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        public ActionResult Create()
        {


            return View();

            return null;

        }


        public ActionResult Create(string name)
        {

            this.HServices.AddAModel(new HelloWorld.Model.TableH { name = name });

            return Redirect("/");

        }

    }
}
