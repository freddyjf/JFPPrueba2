using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LEGIS.PJ.Core.Data;
using LEGIS.PJ.Core.Domain.AnalisisJuridicos;
using LEGIS.PJ.Core.Domain.GraphModulacion;
using LEGIS.PJ.Core.Domain.MaestroTextos;
using LEGIS.PJ.Core.Domain.SuscriptoresAnalisisConsultados;
using LEGIS.PJ.Core.Loggin;
using LEGIS.PJ.Data;
using LEGIS.PJ.Services.AnalisisJuridicos;
using LEGIS.PJ.Services.MaestroTextos;
using NUnit.Framework;

namespace LEGIS.PJ.Tests.Services.AnalisisJuridico
{
    [TestFixture]
    public class AnalisisJuridicoSearchServicesTest : ServicesTests
    {
        private ILogging _logging;
        private IAnalisisJuridicoSearchServices _analisisJuridicoServices;

        public void InitCaseTest()
        {
            _logging = new LoggingManager();

            _analisisJuridicoServices = new AnalisisJuridicoSearchServices(_logging, _cacheManager);
        }

        [Test]
        public void Can_Buscar()
        {
            InitCaseTest();

            var result = _analisisJuridicoServices.Buscar(new AnalisisJuridico_Search
            {
                Termino = "Familia"
            });

            result.ShouldNotBeNull();
        }
    }
}
