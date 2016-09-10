using JFP.Core.Domain.Text;
using System.Collections.Generic;

namespace JFP.Services.Textos
{
   public interface ITextoServices
    {
       Texto GetTextoByName(string name);

       Texto GetTextoById(int id);

       string GetText(string name);

       Dictionary<string, string> GetAllResourceValues();

       IList<Texto> GetAllResource();

       void InsertTexto(Texto texto);

       void InsertTexto(IEnumerable<Texto> textos);

       void UpdateTexto(Texto texto);

       void DeleteTexto(Texto texto);

       void DeleteAllText();
    }
}
