using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JFP.Core.Domain.Catalogo;
using JFP.Services.Resources;

namespace JFP.Services.CatalogoServices
{
    public interface ICatalogoServices
    {
        IList<Catalogo> getCatalogo();
        void resource(IResource resource);
    }
}
