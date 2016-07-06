using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using XKNT.Common.Helper;

namespace XKNT.UI.PC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult index()
        {
            ArrayList al = new ArrayList();

            al.Add("127.0.0.1:11211");
            //
            bool result = MemcacheHelper.AddCache(al, "mc", "test", "hello word!", 1);

            return Content("test的缓存内容为：" + MemcacheHelper.GetCache(al, "mc", "test").ToString());
        }
    }
}
