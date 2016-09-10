using NUnit.Framework;
using LEGIS.PJ.Core.Domain.Suscriptores;
using LEGIS.PJ.Core.Domain.AreasDerecho;
using LEGIS.PJ.Core.Domain.LineasJurisprudenciales;
using LEGIS.PJ.Services.Authentication;
using LEGIS.PJ.Services.Suscriptores;
using LEGIS.PJ.Services.AreasDerecho;
using LEGIS.PJ.Services.LineasJurisprudencial;
using LEGIS.PJ.Data;
using LEGIS.PJ.Core.Data;
using LEGIS.PJ.Core.Domain.SuscriptoresAreaDerecho;
using LEGIS.PJ.Core.Domain.Text;
using LEGIS.PJ.Services.Textos;

namespace LEGIS.PJ.Tests.Services.Authentication
{
   [TestFixture]
   public class AuthenticationServicesTest:ServicesTests
    {

       private IAuthenticationServices _authenticacionServices;
       private ISuscriptorServices _suscriptorServices;
       private IAreaDerechoServices _areaDerechoServices;
       //private ILineasJurisprudencialServices _lineaJService = null;
       private ITextoServices _textosServices;

       private void InitCaseTest()
       {
           IRepository<Suscriptor> repositorysuscriptor = new EfRepository<Suscriptor>(context);
           IRepository<SuscriptorAreaDerecho> repositoryAreaSuscriptor = new EfRepository<SuscriptorAreaDerecho>(context);
           IRepository<AreaDerecho> repositoryArea = new EfRepository<AreaDerecho>(context);
           IRepository<AreaDerechoPPV> repositoryAreaPpv = new EfRepository<AreaDerechoPPV>(context);
           IRepository<LineaJurisprudencial> repositoryLinea = new EfRepository<LineaJurisprudencial>(context);
           IRepository<LineaJurisprudencialSuscriptor> repositoryLineaJSus = new EfRepository<LineaJurisprudencialSuscriptor>(context);
           IRepository<Texto> repositoryTextos = new EfRepository<Texto>(context);


           _suscriptorServices = new SuscriptorServices(_cacheManager, repositorysuscriptor, repositoryAreaSuscriptor, repositoryAreaPpv, repositoryLinea);
           _areaDerechoServices = new AreaDerechoServices(_cacheManager, repositoryArea, repositoryLinea);
           ILineasJurisprudencialServices lineaJService = new LineasJurisprudencialServices(_cacheManager, repositoryLinea, repositoryLineaJSus, null, null);
           _textosServices = new TextoServices(_cacheManager, repositoryTextos);

           _authenticacionServices = new AuthenticationScalaServices(_loggin, _suscriptorServices, _areaDerechoServices, lineaJService, _textosServices, _cacheManager);
       }
   
       [Test]
       public void Can_Scala_Init()
       {
           InitCaseTest();
           //var tituloGrafico = _textosServices.GetText("PJ.Grafico.Titulo");
           var url = _authenticacionServices.authenticate("freddy.parra@legis.com.co", "carolina80130223", "", "");
           url.ShouldContains("Ticket");
       }


       [Test]
       public void Can_Scala_AccesByIP()
       {
           InitCaseTest();
           _authenticacionServices.isAccesByIP("127.0.0.1").ShouldBeTrue();
           _authenticacionServices.isAccesByIP("10.0.0.1").ShouldEqual(false);
       }


       [Test]
       public void Can_Scala_AccesByURL()
       {
           InitCaseTest();
           _authenticacionServices.isAccesByURL("http://localhost:61472/precedente.html").ShouldBeTrue();
           _authenticacionServices.isAccesByURL("http://localhost:61472/error.html").ShouldEqual(false);
       }

       [TestCase("4B84560C-AD09-4658-9575-222395454435")]
       public void Can_Scala_GetDataUser_By_Ticket(string ticket)
       {
           InitCaseTest();
           var user = _authenticacionServices.getDataUserByGUIDSesion(ticket);
           user.ShouldNotBeNull();
       }
   }
}
