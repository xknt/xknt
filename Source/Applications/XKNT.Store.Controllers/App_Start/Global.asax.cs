using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using XKNT.Store.Controllers;

namespace XKNT.Store.UI.PC
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
