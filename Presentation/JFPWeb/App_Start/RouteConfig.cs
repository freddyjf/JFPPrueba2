using System.Web.Mvc;
using System.Web.Routing;

namespace Precedente
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                name: "AnalisisConsultadosGet",
                url: "AnalisisConsultados/{start}",
                defaults: new { controller = "AnalisisConsultados", action = "Index", start = 1 }
            );

            routes.MapRoute(
               name: "AnalisisJuridicoObtenerTesis",
               url: "AnalisisJuridico/ObtenerTesis",
               defaults: new { controller = "AnalisisJuridico", action = "ObtenerTesis" }
            );

            routes.MapRoute(
               name: "AnalisisJuridicoCargarTab",
               url: "AnalisisJuridico/CargarTab",
               defaults: new { controller = "AnalisisJuridico", action = "CargarTab" }
            );

            routes.MapRoute(
              name: "AnalisisJuridicoEliminarTabSesion",
              url: "AnalisisJuridico/EliminarTabSesion",
              defaults: new { controller = "AnalisisJuridico", action = "EliminarTabSesion" }
           );

            routes.MapRoute(
               name: "AnalisisJuridicoDocumento",
               url: "AnalisisJuridico/Documento/{codAnalisis}",
               defaults: new { controller = "AnalisisJuridico", action = "Documento", codAnalisis = "codAnalisis" }
            );

            routes.MapRoute(
               name: "AnalisisJuridicoGraficoBurbuja",
               url: "AnalisisJuridico/GraficoBurbuja/{codAnalisis}/{filtros}",
               defaults: new { controller = "AnalisisJuridico", action = "GraficoBurbuja", 
                   codAnalisis = "codAnalisis", filtros = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "AnalisisJuridicoGraficoBurbujaModulacion",
               url: "AnalisisJuridico/GraficoBurbujaModulacion",
               defaults: new { controller = "AnalisisJuridico", action = "GraficoBurbujaModulacion" }
            );

            routes.MapRoute(
               name: "AnalisisJuridicoCacheRemove",
               url: "AnalisisJuridico/RemoveCacheAnalisis/{codAnalisis}",
               defaults: new { controller = "AnalisisJuridico", action = "RemoveCacheAnalisis", codAnalisis = "codAnalisis" }
            );

            routes.MapRoute(
               name: "AnalisisJuridicoGet",
               url: "AnalisisJuridico/{NomArea}/{NomAnalisis}/{codAnalisis}/{outputType}",
               defaults: new { controller = "AnalisisJuridico", action = "Index", NomArea = UrlParameter.Optional, NomAnalisis = UrlParameter.Optional, outputType = UrlParameter.Optional }
            );

            routes.MapRoute(
              name: "AnalisisJuridicoByLinea",
              url: "Linea-Jurisprudencial/{NomArea}/{NomLinea}/{codLinea}",
              defaults: new { controller = "LineaJurisprudencial", action = "Index",nomArea=UrlParameter.Optional, NomLinea=UrlParameter.Optional, codLinea = UrlParameter.Optional }
            );

            routes.MapRoute(
              name: "AnalisisJuridicoArticuloTexto",
              url: "MaestroTexto/{codMaestroTexto}",
              defaults: new { controller = "AnalisisJuridico", action = "ObtenerTexto" }
            );

            routes.MapRoute(
               name: "Perfil",
               url: "Perfil/{action}/{id}",
               defaults: new { controller = "Account", action = "Index", id = UrlParameter.Optional },
               namespaces: new[] { "Precedente.Controllers" }
            );

            routes.MapRoute(
             name: "TerminosYCondiciones",
             url: "TerminosYCondiciones",
             defaults: new { controller = "Home", action = "TerminosYCondiciones" },
             namespaces: new[] { "Precedente.Controllers" }
           );

            routes.MapRoute(
             name: "Legal",
             url: "Legal",
             defaults: new { controller = "Home", action = "Legal" },
             namespaces: new[] { "Precedente.Controllers" }
            );


            #region Default

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Precedente.Controllers" }
            );

            #endregion
        }
    }
}
