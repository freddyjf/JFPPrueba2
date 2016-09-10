using LEGIS.PJ.Core.Common;
using LEGIS.PJ.Core.Statistics;
using LEGIS.PJ.Services.Statistics;
using NUnit.Framework;

namespace LEGIS.PJ.Tests.Services
{

    [TestFixture]
    public class StatisticsTest : ServicesTests
    {
        /// <summary>
        /// Prueba el registro de estadísticas en archivo local de texto.
        /// </summary>
        [Test]
        public void Can_Statistics_LocalFileWrite()
        {
            var _statisticsServices = new StatisticsServices(_loggin);
            var tester = (IUTest) _statisticsServices;

            tester.UTest_Enable = true;

            // Prueba de registro en archivo local.
            _statisticsServices.WriteLocal(
                EstadisticasUsos.AperturaSentencia,
                EstadisticasCampos.AliasObra, "jurcol");

            // Todos los registros deben ser exitosos.
            _statisticsServices.UTest_Data.Sucess.ShouldEqual(true);
        }

        /// <summary>
        /// Prueba el registro de estadísticas en servicio Logos.
        /// </summary>
        [Test]
        public void Can_Statistics_LogosWrite()
        {
            StatisticsServices _statisticsServices = new StatisticsServices(_loggin);
            var Tester = _statisticsServices as IUTest;

            Tester.UTest_Enable = true;

            // Prueba de registro en logos.
            _statisticsServices.WriteLogos(
                EstadisticasUsos.AperturaSentencia,
                EstadisticasCampos.AliasObra, "jurcol",
                EstadisticasCampos.Dispositivo,"APP"
                );

            // Todos los registros deben ser exitosos.
            _statisticsServices.UTest_Data.Sucess.ShouldEqual(true);
        }

        /// <summary>
        /// Prueba el registro de estadísticas en archivo local de texto y logos.
        /// </summary>
        [Test]
        public void Can_Statistics_FullWrite()
        {
            StatisticsServices _statisticsServices = new StatisticsServices(_loggin);
            var Tester = _statisticsServices as IUTest;

            Tester.UTest_Enable = true;

            // Prueba de registro en archivo y en logos.
            _statisticsServices.Write(
                EstadisticasUsos.AperturaSentencia,
                EstadisticasCampos.AliasObra, "jurcol");

            // Todos los registros deben ser exitosos.
            _statisticsServices.UTest_Data.Sucess.ShouldEqual(true);
        }


    }
}
