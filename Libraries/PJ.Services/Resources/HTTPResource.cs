using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JFP.Services.Resources
{
    public class HTTPResource : IResource
    {

        private string source;
        public string getData()
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(source) as HttpWebRequest;
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).",
                        response.StatusCode,
                        response.StatusDescription));

                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                        return  reader.ReadToEnd();
                    }

                   
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.Read();
                return "error " + e.Message;
            }
        }

        public void setSource(string Path)
        {
            source = Path;
        }
    }
}
