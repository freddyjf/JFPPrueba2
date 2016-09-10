using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JFP.Core.Common
{
    /// <summary>
    /// Clase para retornar información relacionada con una prueba unitaria.
    /// </summary>
    public class UnitTestData
    {
        public UnitTestData(bool sucess, string title =null, string message=null, Exception exceptionData=null)
        {
            Sucess = sucess;
            Title = title;
            Message = message;
            ExceptionData = exceptionData;
        }

        /// <summary>
        /// True: La prueba fué exitosa.
        /// </summary>
        public bool Sucess { get; set; }

        /// <summary>
        /// Título del test.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Mensaje a reportar.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Excepción detectada.
        /// </summary>
        public Exception ExceptionData { get; set; }
    }
}
