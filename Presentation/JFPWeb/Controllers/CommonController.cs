using System.Web.Mvc;
using JFP.Core.Loggin;

using JFP.WEB.Framework.WebContext;

namespace Precedente.Controllers
{
    public class CommonController : BaseController
    {
        public CommonController()   
        {

        }

        //page not found
        public ActionResult PageNotFound()
        {
            Response.StatusCode = 404;
            Response.TrySkipIisCustomErrors = true;

            return View();
        }

        //page error
        public ActionResult PageError()
        {
            Response.StatusCode = 500;
            Response.TrySkipIisCustomErrors = true;

            return View();
        }
    }
}