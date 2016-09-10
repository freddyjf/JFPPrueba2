using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using FluentValidation.Mvc;
using JFP.Core.Infrastructure;
using JFP.WEB.Framework;
using JFP.Core.Loggin;
using JFP.Core.Config;

namespace Precedente
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {


            //initialize engine context Container Inversion of Control
            EngineContext.Initialize(false);

            //fluent validation
            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;
            ModelValidatorProviders.Providers.Add(new FluentValidationModelValidatorProvider(new ValidatorFactory()));



            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }




        protected void Application_Error(Object sender, EventArgs e)
        {
            var exception = Server.GetLastError();

            var loggin = new LoggingManager();

            var redirect501 = Config.LlaveBoolean("REDIRECT_ERR_501");

            //process 404 HTTP errors
            var httpException = exception as HttpException;
            if (httpException != null && httpException.GetHttpCode() == 404)
            {
                if (!IsStaticResource(this.Request))
                {
                    Response.Clear();
                    Server.ClearError();
                    Response.TrySkipIisCustomErrors = true;

                    // Call target Controller and pass the routeData.
                    IController errorController = EngineContext.Current.Resolve<Precedente.Controllers.CommonController>();

                    var routeData = new RouteData();
                    routeData.Values.Add("controller", "Common");
                    routeData.Values.Add("action", "PageNotFound");


                    var ErrorData = string.Format("Error 404 registrado recurso '{0}': {1}", this.Request.Path, httpException.Message);
                    loggin.writeLog(eTypeLog.Error, ErrorData, exception);

                    //errorController.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
                }
            }
            else if (httpException != null && redirect501 && (httpException.GetHttpCode() >= 500))
            {
                Response.Clear();
                Server.ClearError();
                Response.TrySkipIisCustomErrors = true;

                // Call target Controller and pass the routeData.
                IController errorController = EngineContext.Current.Resolve<Precedente.Controllers.CommonController>();

                var routeData = new RouteData();
                routeData.Values.Add("controller", "Common");
                routeData.Values.Add("action", "PageError");

                var errMsg = httpException.Message;

                if (httpException.InnerException != null)
                    errMsg += httpException.InnerException.Message + " " + httpException.InnerException.StackTrace;

                var errorData = string.Format("Error 500 registrado  '{0}': {1}", this.Request.Path, errMsg);

                loggin.writeLog(eTypeLog.Error, errorData, exception);


                errorController.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
            }
            else
            {
                loggin.writeLog(eTypeLog.Error, "Excepción generica", exception);
            }

        }



        /// <summary>
        /// Returns true if the requested resource is one of the typical resources that needn't be processed by the cms engine.
        /// </summary>
        /// <param name="request">HTTP Request</param>
        /// <returns>True if the request targets a static resource file.</returns>
        /// <remarks>
        /// These are the file extensions considered to be static resources:
        /// .css
        ///	.gif
        /// .png 
        /// .jpg
        /// .jpeg
        /// .js
        /// .axd
        /// .ashx
        /// </remarks>
        private bool IsStaticResource(HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            string path = request.Path;
            string extension = VirtualPathUtility.GetExtension(path);

            if (extension == null) return false;

            switch (extension.ToLower())
            {
                case ".axd":
                case ".ashx":
                case ".bmp":
                case ".css":
                case ".gif":
                case ".htm":
                case ".html":
                case ".ico":
                case ".jpeg":
                case ".jpg":
                case ".js":
                case ".png":
                case ".rar":
                case ".zip":
                    return true;
            }

            return false;
        }

    }
}
