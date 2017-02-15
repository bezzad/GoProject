using System.Web.Mvc;
using System.Web.Routing;

namespace GoProject.Sample
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{lang}/{controller}/{action}/{id}",
                defaults: new { lang="en", controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
