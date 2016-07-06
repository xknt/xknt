using System.Web.Mvc;
using System.Web.Routing;
using XKNT.UI.PC.Controllers;

namespace XKNT.Platform.UI.PC
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
