using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace JFP.Core.Domain.Text
{
    public class Texto : BaseEntity
    {

       public string NomTexto {get;set;}

       public string CodIdioma {get;set;}

       public string Text {get;set;}

       public string Descripcion {get;set;}

    
    }
}
