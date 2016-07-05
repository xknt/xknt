using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace XKNT.UI.PC.Controllers
{
    public class RouteConfigOfUI
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            string biz = "Shared|System";

            routes.MapRoute(
                "BizM", // Route name
                "{Biz}/{controller}/{action}/{id}/{pid}", // URL with parameters
                new { Biz = "Default", controller = "Home", action = "Index" }, // Parameter defaults
                new { Biz = biz }
            );

            routes.MapRoute(
                "Biz", // Route name
                "{Biz}/{controller}/{action}/{id}", // URL with parameters
                new { Biz = "Default", controller = "Home", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
                new { Biz = biz }
            );
            routes.MapRoute(
             "BizQ", // 路由名称//////
             "{Biz}/{controller}/{action}/{queryname}/{*queryvalues}", // 带有参数的 URL
             new { Biz = "Default", controller = "Home", action = "Index", queryname = UrlParameter.Optional, queryvalues = UrlParameter.Optional }, // 参数默认值
             new { Biz = biz }
            );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
             "queryvalues", // 路由名称//////
             "{controller}/{action}/{queryname}/{*queryvalues}", // 带有参数的 URL
             new { controller = "Home", action = "Index", queryname = UrlParameter.Optional, queryvalues = UrlParameter.Optional } // 参数默认值
             );
        }
    }
}
