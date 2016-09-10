using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JFP.Core.Domain.Catalogo;
using JFP.Services.Resources;


namespace JFP.Services.CatalogoServices
{
    public class CatalogoServices : ICatalogoServices
    {
        #region atributos

        private  IResource _resource;

        #endregion


        public void resource(IResource resource)
        {
            _resource = resource;
        }


        public IList<Catalogo> getCatalogo()
        {
            string JSONCatalogo = _resource.getData();
            List<Catalogo> lstCatalogo=  Newtonsoft.Json.JsonConvert.DeserializeObject<List<Catalogo>>(JSONCatalogo);

            return lstCatalogo;

        }
    }
}
