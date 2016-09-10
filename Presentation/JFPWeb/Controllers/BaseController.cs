using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using JFP.WEB.Framework.WebContext;
using JFP.Core.Authentication;
using System.IO;
using JFP.Core.Config;
using JFP.Core.Loggin;


namespace Precedente.Controllers
{
    public class BaseController : ControllerExtensions
    {
        //public readonly IWebContext CurrentContext;
        //public readonly ILogging RegistroLog;
        

        //public BaseController(IWebContext currentContext, ILogging registroLog
        //   )
        //{
        //    RegistroLog = registroLog;
        //    CurrentContext = currentContext;
          
        //}

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
           
        }

        protected override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            if (filterContext.IsChildAction) return;

            Session.Remove("MetaDescription");
            Session.Remove("MetaKeywords");
        }

     
        protected string PartialViewAsString()
        {
            return _PartialViewAsString(null, null);
        }

        protected string PartialViewAsString(string viewName)
        {
            return _PartialViewAsString(viewName, null);
        }

        protected string PartialViewAsString(string viewName, object model)
        {
            return _PartialViewAsString(viewName, model);
        }

        private string _PartialViewAsString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);

                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }


        /// <summary>
        /// Render partial view to string
        /// </summary>
        /// <param name="viewName">View name</param>
        /// <param name="model">Model</param>
        /// <returns>Result</returns>
        public virtual string RenderPartialViewToString(string viewName, object model)
        {
            //Original source code: http://craftycodeblog.com/2010/05/15/asp-net-mvc-render-partial-view-to-string/
            if (string.IsNullOrEmpty(viewName))
                viewName = this.ControllerContext.RouteData.GetRequiredString("action");

            this.ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                ViewEngineResult viewResult = System.Web.Mvc.ViewEngines.Engines.FindPartialView(this.ControllerContext, viewName);
                var viewContext = new ViewContext(this.ControllerContext, viewResult.View, this.ViewData, this.TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }




    }
}