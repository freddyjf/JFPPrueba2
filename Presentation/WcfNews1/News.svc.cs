using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using JFP.Services.Resources;
using JFP.Services.CatalogoServices;
using JFP.Core.Domain.Catalogo;
using System.Web.Configuration;

namespace WCFNews
{
    // NOTE: Este servicio es solo para prueba de solicitudes http   
    public class News : INews
    {
        [WebInvoke(Method = "GET",
                      ResponseFormat = WebMessageFormat.Json,
                       UriTemplate = "getNews")]
        public IList<Catalogo> GetData()
        {

            FileResource fr = new FileResource();
            fr.setSource(WebConfigurationManager.AppSettings["Source:FileJSON"]);

            ICatalogoServices _catalogoServices = new CatalogoServices();
            _catalogoServices.resource(fr);
            IList<Catalogo> model = _catalogoServices.getCatalogo();

            // lookup person with the requested id 
            return model; 
        }
    }
}
