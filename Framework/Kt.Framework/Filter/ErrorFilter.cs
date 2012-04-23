using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;

namespace Kt.Framework.Filter
{
    public class ErrorFilter : HandleErrorAttribute
    {

        public override void OnException(System.Web.Mvc.ExceptionContext filterContext)
        {
            if (filterContext.HttpContext.IsCustomErrorEnabled)
            {
                filterContext.ExceptionHandled = true;
            }

            Kt.Framework.Common.Logger.Error(filterContext.Exception);

            base.OnException(filterContext);

            //OVERRIDE THE 500 ERROR  

            //filterContext.HttpContext.Response.StatusCode = 200;

        }



        private static void RaiseErrorSignal(Exception e)
        {

            var context = HttpContext.Current;

            // using.Elmah.ErrorSignal.FromContext(context).Raise(e, context);

          

        }



    }
}
