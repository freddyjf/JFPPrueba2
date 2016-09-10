using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace JFP.Services.Resources
{
    public class FileResource : IResource
    {

        #region atributos 

        private string source;

        #endregion


        public string getData()
        {
            return loadData();
        }

        public void setSource(string Path)
        {
            source = Path;
        }




        private string loadData()
        {
            if (File.Exists(source))
            {
              string[] JSONFile = File.ReadAllLines(source);
              return string.Join("", JSONFile);


            }
            else
            {
                throw new System.FieldAccessException ("El archivo no Existe ["+source+"]");

            }
        }
    }
}
