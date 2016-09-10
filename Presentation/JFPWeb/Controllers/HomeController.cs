using System;
using System.Web;
using System.Web.Mvc;
using JFP.Core.Authentication;
using JFP.Core.Loggin;
using JFP.WEB.Framework.WebContext;
using JFP.Services.CatalogoServices;
using JFP.Services.Resources;
using JFP.Core.Infrastructure;
using System.Web.Configuration;

namespace Precedente.Controllers
{
    public class HomeController: BaseController
    {

        #region attr
        private readonly ICatalogoServices _catalogoServices; 
        #endregion

        public HomeController(ICatalogoServices catalogo)            
        {
            _catalogoServices = catalogo;
        }

        public ActionResult Index()
        {
            //IResource resource;
            //if (WebConfigurationManager.AppSettings["Source:Type"] == "1")
            //{
            //    resource = new FileResource();
            //    resource.setSource(WebConfigurationManager.AppSettings["Source:FileJson"]);
            //}
            //else
            //{
            //    resource = new HTTPResource();
            //    resource.setSource(WebConfigurationManager.AppSettings["Source:HTTPJson"]);
            //}
            //_catalogoServices.resource(resource);
            //var model = _catalogoServices.getCatalogo();
            return View();
        }


        [HttpGet]
        public ActionResult getData()
        {

            IResource resource;
            if (WebConfigurationManager.AppSettings["Source:Type"] == "1")
            {
                resource = new FileResource();
                resource.setSource(WebConfigurationManager.AppSettings["Source:FileJson"]);
            }
            else
            {
                resource = new HTTPResource();
                resource.setSource(WebConfigurationManager.AppSettings["Source:HTTPJson"]);
            }
            _catalogoServices.resource(resource);
            var model = _catalogoServices.getCatalogo();

            string html = RenderPartialViewToString("_ListNews", model);

            return Content(html);


        }
        public ActionResult Header()
        {
            return PartialView("_Header");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        

    }
}