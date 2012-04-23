using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace Kt.Framework.Models
{
    public class Pagination
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public string Controller { get; private set; }
        public string Action { get; private set; }
        public bool Shorten { get; private set; }
        public int Start { get; private set; }
        public int Stop { get; private set; }
        public RouteValueDictionary RouteValues { get; private set; }
        public string PageQueryString { get; private set; }
        public int PageSize { get; set; }
        /// <summary>
        /// Javascript 方法体 ， 例如 ： @"ajaxpage({0})" 
        /// 
        /// <![CDATA[
        ///  function ajaxpage(pageNo){
        ///      pageSize, 条件 等从页面中取得或写成固定值。。。
        ///      //  调用 Ajax 的方法 从服务器读取数据
        ///  }
        /// ]]>
        /// </summary>
        public string JavascriptFun { get; set; }

        private void Init(int currentPage, int totalPages, RouteValueDictionary routeValues, string pageQueryString)
        {
            this.CurrentPage = currentPage;
            this.TotalPages = totalPages;
            this.Controller = (string)routeValues["controller"];
            this.Action = (string)routeValues["action"];
            this.PageQueryString = pageQueryString;
            this.Shorten = this.TotalPages >= 10;

            this.Start = 2;
            this.Stop = this.TotalPages - 1;

            if (this.Shorten)
            {
                if (this.CurrentPage - 3 > 0) this.Start = this.CurrentPage - 2;
                if (this.Start == 3 && this.CurrentPage == 5) this.Start--;

                if (this.TotalPages - this.CurrentPage > 4) this.Stop = this.CurrentPage + 2;


                if (this.Start == 1) this.Start++;
                if (this.Stop == this.TotalPages || this.Stop > this.TotalPages) this.Stop = this.TotalPages - 1;

            }

            routeValues.Add(pageQueryString, 0);
            routeValues.Add("pagesize", PageSize);
            this.RouteValues = routeValues;

        }

        //public Pagination(int currentPage, int totalPages, string action, string controller, object routeValues = null, string pageQueryString = "page")
        //{
        //    RouteValueDictionary routeDictionary = new RouteValueDictionary(routeValues);
        //    routeDictionary.Add("controller", controller);
        //    routeDictionary.Add("action", action);
        //    this.Init(currentPage, totalPages, routeDictionary, pageQueryString);
        //}

        public Pagination(int currentPage, int count, int pageSize, string action, string controller, object routeValues = null, string pageQueryString = "page")
            : this(currentPage, count, pageSize, action, controller, new RouteValueDictionary(routeValues), pageQueryString)
        {

        }

        public Pagination(int currentPage, int count, int pageSize, string action, string controller, RouteValueDictionary routeDictionary = null, string pageQueryString = "page")
        {
            routeDictionary = routeDictionary ?? new RouteValueDictionary();

            routeDictionary.Add("controller", controller);
            routeDictionary.Add("action", action);
            int totalPages = (int)Math.Ceiling((decimal)count / pageSize);
            this.PageSize = pageSize;
            this.Init(currentPage, totalPages, routeDictionary, pageQueryString);
        }

        public Pagination(int currentPage, int count, int pageSize, object routeValues, string pageQueryString = "page")
        {
            RouteValueDictionary routeDictionary = new RouteValueDictionary(routeValues);
            this.PageSize = pageSize;
            int totalPages = (int)Math.Ceiling((decimal)count / pageSize);
            this.Init(currentPage, totalPages, routeDictionary, pageQueryString);
        }


        /// <summary>
        /// AJAX方法调用的构造函数
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="count"></param>
        /// <param name="pageSize"></param>
        /// <param name="JavascriptFun"></param>
        public Pagination(int currentPage, int count, int pageSize, string JavascriptFun)
            : this(currentPage, count, pageSize, "", "", null, "page")
        {
            this.JavascriptFun = JavascriptFun;
        }

        public RouteValueDictionary GetDictionary(int pageNumber)
        {
            this.RouteValues[this.PageQueryString] = pageNumber;
            return RouteValues;
        }

    }
}
