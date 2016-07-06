using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using XKNT.Mobile.Controllers;

namespace XKNT.Mobile.UI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfigOfUI.RegisterRoutes(RouteTable.Routes);
        }
    }
}
