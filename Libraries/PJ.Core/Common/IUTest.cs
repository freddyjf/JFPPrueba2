using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JFP.Core.Common
{
    public interface IUTest
    {
        /// <summary>
        /// Información generada por la prueba unitaria. Null si la prueba fué exitosa.
        /// </summary>
        /// <returns></returns>
        UnitTestData UTest_Data { get; set; }

        /// <summary>
        /// True: Debe generarse información para pruebas unitarias.
        /// </summary>
        bool UTest_Enable { get; set; }
    }
}
