using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JFP.Services.Resources
{
    public interface IResource
    {


        void setSource(string Path);
        string getData();



    }
}
