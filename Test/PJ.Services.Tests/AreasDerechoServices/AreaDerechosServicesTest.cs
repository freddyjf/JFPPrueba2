using NUnit.Framework;
using LEGIS.PJ.Core.Data;
using LEGIS.PJ.Core.Domain.AreasDerecho;
using LEGIS.PJ.Services.AreasDerecho;
using LEGIS.PJ.Data;
using LEGIS.PJ.Core.Domain.LineasJurisprudenciales;

namespace LEGIS.PJ.Tests.Services.AreasDerechoServices
{
    [TestFixture]
    public partial class AreaDerechosServicesTest : ServicesTests
    {

        private IRepository<AreaDerecho> _repository;
        private IAreaDerechoServices AreaServices;

        private int CodSuscriptor = 8;

        private void InitCaseTest()
        {
            _repository = new EfRepository<AreaDerecho>(context);
            var _lineaRespository = new EfRepository<LineaJurisprudencial>(context);
            AreaServices = new AreaDerechoServices(_cacheManager, _repository, _lineaRespository);
        }

        [Test]
        public void Can_AreaDerecho_Insert()
        {
            InitCaseTest();

            var areaLaboral = new AreaDerecho
            {
                NomAreaDerecho = "Test.Laboral",
                Visible = true,
                codProductoSil = 1000,
                Alias = "Some alias"
            };

            //insert
            //Laboral
            AreaServices.InsertAreaDerecho(areaLaboral);

            var area1 = AreaServices.GetAreaById(areaLaboral.Id);
            //AreaDerecho area1 = AreaServices.GetAreaById(19);
            area1.NomAreaDerecho.ShouldEqual("Test.Laboral");

            Can_AreaDerecho_Delete(area1);
        }

        [TestCase(1076)]
        public void Can_AreaDerecho_get(int codArea)
        {
            InitCaseTest();

            AreaDerecho area1 = AreaServices.GetAreaById(codArea);
            area1.ShouldNotBeNull();
        }


        [Test]
        public void Can_AreaDerecho_Hidden_Insert()
        {
            InitCaseTest();

            var areaAdministrativo = new AreaDerecho
            {
                NomAreaDerecho = "Test.Administrativo",
                Visible = false,
                codProductoSil = 1001,
                Alias = "Some alias"
            };

            //administrativa
            AreaServices.InsertAreaDerecho(areaAdministrativo);

            var area2 = AreaServices.GetAreaById(areaAdministrativo.Id);
            area2.ShouldBeNull();

            Can_AreaDerecho_Delete(areaAdministrativo);

        }

        public void Can_AreaDerecho_Delete(AreaDerecho areaDelete)
        {
            areaDelete.ShouldNotBeNull();
            var id = areaDelete.Id;
            AreaServices.DeleteAreaDerecho(areaDelete);
            var area1 = AreaServices.GetAreaById(id);
            area1.ShouldBeNull();
        }

        [Test]
        public void Can_GetAllAreaDerecho()
        {
            InitCaseTest();

            var collection = AreaServices.GetAllAreaDerecho();
            collection.ShouldNotBeEmpty();

            collection = AreaServices.GetAllAreaDerecho(true);
            collection.ShouldNotBeEmpty();

            collection = AreaServices.GetAllAreaDerecho(true, true);
            collection.ShouldNotBeEmpty();
        }

        [Test]
        public void Can_GetAreaDerechoLineas()
        {
            InitCaseTest();

            var collection = AreaServices.GetAreaDerechoLineas(CodSuscriptor);
            collection.ShouldNotBeEmpty();
        }

        [Test]
        public void Can_GetAreaByAlias()
        {
            InitCaseTest();

            const string alias = "PJSOCIETARIA";
            var collection = AreaServices.GetAreaByAlias(alias);
            collection.ShouldNotBeNull();
        }

        [Test]
        public void Can_GetAreaByCodSilProd()
        {
            InitCaseTest();

            const int idCodSilenio = 1806;
            var collection = AreaServices.GetAreaByCodSilProd(idCodSilenio);
            collection.ShouldNotBeNull();
        }

        [Test]
        public void Can_UpdateAreaDerecho()
        {
            InitCaseTest();

            const int codArea = 1076;
            var area = AreaServices.GetAreaById(codArea);
            area.NomAreaDerecho += "+";

            AreaServices.UpdateAreaDerecho(area);
        }
    }
}
