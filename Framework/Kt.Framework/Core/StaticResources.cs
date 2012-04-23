using System.Collections.Generic;
using System.Web.Routing;
using System.Web.Mvc;

namespace Kt.Framework.Core
{
    public static class StaticResources
    {
        private static RouteCollection _routes;
        private static string _DefaultRouteUrl;
        public static void Initialize(RouteCollection routes,string defaultRouteUrl)
        {
            _routes = routes;
            _DefaultRouteUrl = defaultRouteUrl;
        }

        public static void OverrideDefaultRoute(RouteValueDictionary routeValues)
        {
            _routes.RemoveAt(2);

            Route route = new Route(_DefaultRouteUrl, new MvcRouteHandler())
            {
                Defaults = routeValues
            };

            _routes.Add(_DefaultRouteUrl, route);
        }

        public static void Insert(IEnumerable<RouteBase> routes)
        {
            foreach (var route in routes)
                _routes.Insert(2, route);
        }
    }
}