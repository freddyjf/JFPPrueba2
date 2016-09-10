using LEGIS.PJ.Core.Data;
using LEGIS.PJ.Core.Domain.AnalisisJuridicos;
using LEGIS.PJ.Core.Domain.GraphModulacion;
using LEGIS.PJ.Core.Domain.MaestroTextos;
using LEGIS.PJ.Core.Domain.SuscriptoresAnalisisConsultados;
using LEGIS.PJ.Data;
using LEGIS.PJ.Services.AnalisisJuridicos;
using LEGIS.PJ.Services.MaestroTextos;
using NUnit.Framework;

namespace LEGIS.PJ.Tests.Services.AnalisisJuridicos
{
    [TestFixture]
    public class AnalisisJuridicoServicesTest : ServicesTests
    {
        IRepository<SuscriptorAnalisisConsultados> _repositorySuscriptor;
        IRepository<Core.Domain.AnalisisJuridicos.AnalisisJuridico> _analisisJuridico;
        IRepository<GraphModulacion> _modulacionRepository;
        IRepository<AnalisisJuridicoSmart> _analisisJuridicoSmartRepository;
        IRepository<AnalisisJuridicoModulacion> _analisisJuridicoModulacionRepository;
        IRepository<MaestroTexto> _maestroTextoRepository;

        private IMaestroTextoServices _maestroTextoServices;
        private IAnalisisJuridicoServices _analisisJuridicoServices;

        public void InitCaseTest()
        {
            _repositorySuscriptor = new EfRepository<SuscriptorAnalisisConsultados>(context);
            _analisisJuridico = new EfRepository<Core.Domain.AnalisisJuridicos.AnalisisJuridico>(context);
            _modulacionRepository = new EfRepository<GraphModulacion>(context);
            _analisisJuridicoSmartRepository = new EfRepository<AnalisisJuridicoSmart>(context);
            _analisisJuridicoModulacionRepository = new EfRepository<AnalisisJuridicoModulacion>(context);
            _maestroTextoRepository = new EfRepository<MaestroTexto>(context);

            _maestroTextoServices = new MaestroTextoServices(_maestroTextoRepository, _cacheManager);

            _analisisJuridicoServices = new AnalisisJuridicoServices(context, _cacheManager, _maestroTextoServices, _modulacionRepository, _analisisJuridicoSmartRepository, _analisisJuridicoModulacionRepository);
        }

        private const int CodAnalisis = 1669;
        private const int CodAnalisis1 = 1635;
        private const int CodLinea = 34;
        private const int CodSentencia = 10;


        [Test]
        public void Can_GetAnalisisByLinea()
        {
            InitCaseTest();

            var analisisCollection = _analisisJuridicoServices.GetAnalisisByLinea(CodLinea);
            analisisCollection.ShouldNotBeEmpty();
        }

        [Test]
        public void Can_GetAnalisisById()
        {
            InitCaseTest();

            var analisis = _analisisJuridicoServices.GetAnalisisById(CodAnalisis);
            analisis.ShouldNotBeNull();
        }

        [Test]
        public void Can_GetModulacionGraph()
        {
            InitCaseTest();
            var modulacionGraph = _analisisJuridicoServices.GetModulacionGraph(CodAnalisis).Modulaciones;
            modulacionGraph.ShouldNotBeEmpty();
        }

        [Test]
        public void Can_GetAnalisisRelacionados()
        {
            InitCaseTest();
            var analisisRelacionados = _analisisJuridicoServices.GetAnalisisRelacionados(CodAnalisis1, CodSentencia);
            analisisRelacionados.ShouldNotBeEmpty();
        }

        [Test]
        public void Can_GetAnalisisModulacion()
        {
            InitCaseTest();
            var analisisModulacion = _analisisJuridicoServices.GetAnalisisModulacion(CodAnalisis);
            analisisModulacion.ShouldNotBeEmpty();
        }

        [Test]
        public void Can_GetBubbleGraphData()
        {
            InitCaseTest();
            
            var dataGrafico = _analisisJuridicoServices.GetBubbleGraphData(CodAnalisis);
            dataGrafico.ShouldNotBeNull();
        }

    }
}
