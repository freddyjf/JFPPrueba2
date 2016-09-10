using LEGIS.PJ.Core.Data;
using LEGIS.PJ.Core.Domain.SuscriptoresAnalisisConsultados;
using LEGIS.PJ.Data;
using LEGIS.PJ.Services.AnalisisConsultados;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LEGIS.PJ.Tests.Services.AnalisisConsultados
{
    [TestFixture]
    public class AnalisisConsultadosServicesTest : ServicesTests
    {
        IRepository<SuscriptorAnalisisConsultados> _repositorySuscriptor;
        IAnalisisConsultadoServices _servicesSuscriptor;

        public void initCaseTest()
        {
            _repositorySuscriptor = new EfRepository<SuscriptorAnalisisConsultados>(context);
            _servicesSuscriptor = new AnalisisConsultadoServices(_cacheManager, _repositorySuscriptor);
        }

        [Test]
        public void Can_ObtenerAnalisisConsultados()
        {
            initCaseTest();
            var data = _servicesSuscriptor.ObtenerAnalisisMasConsultados(1);
            data.ShouldNotBeNull();
            data.FirstOrDefault().CodSuscriptor.ShouldEqual(1);

        }

        [Test]
        public void Can_CrearAnalisisConsultado()
        {
            initCaseTest();


            _servicesSuscriptor.EliminarAnalisisConsultado(8, 8);
            _servicesSuscriptor.RegistrarAnalisisConsultado(8, 8);

            var data = _servicesSuscriptor.ObtenerAnalisisMasConsultados(8);            
            var val= data.Where(x => x.CodAnalisis == 8);
            val.ShouldNotBeNull();
        }

        [Test]
        public void Can_EliminarAnalisisConsultado()
        {
            initCaseTest();

            const int codAnalisisConsultado = 1;

            _servicesSuscriptor.EliminarAnalisisConsultado(codAnalisisConsultado);

            var data = _servicesSuscriptor.ObtenerAnalisisMasConsultadoById(codAnalisisConsultado);  

            data.ShouldBeNull();
        }
    }
}
