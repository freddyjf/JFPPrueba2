using System;
using NUnit.Framework;
using LEGIS.PJ.Services.LineasJurisprudencial;
using LEGIS.PJ.Core.Domain.LineasJurisprudenciales;
using LEGIS.PJ.Core.Data;
using LEGIS.PJ.Core.Domain.AreasDerecho;
using LEGIS.PJ.Data;

namespace LEGIS.PJ.Tests.Services.LineasJurisprudenciales
{
    [TestFixture]
    public class LineasJurisprudencialTests : ServicesTests
    {
        private IRepository<LineaJurisprudencial> _repository;
        private IRepository<LineaJurisprudencialSuscriptor> _LineaSuscriptorrepository;
        private ILineasJurisprudencialServices LineaServices;

        private void InitCaseTest()
        {
            _repository = new EfRepository<LineaJurisprudencial>(context);
            _LineaSuscriptorrepository = new EfRepository<LineaJurisprudencialSuscriptor>(context);
            LineaServices = new LineasJurisprudencialServices(_cacheManager, _repository, _LineaSuscriptorrepository, null, null);
        }

        [Test]
        public void Can_Linea_Insert()
        {
            InitCaseTest();

            var ln = new LineaJurisprudencial
            {
                NomLineaJurisprudencial = "El fuero de Maternidad (test)",
                Visible = true,
                codProductoSil = 1001,
                FechaActualizacion = DateTime.Now,
                codAreaDerecho = 2079,
                Alias = "Some alias",
                Precio = 15000,
                Url = "google.com.co"
            };

            LineaServices.InsertLineaJurisprudencial(ln);

            var l = LineaServices.GetLineaById(ln.Id);
            l.ShouldNotBeNull();
            
            l.NomLineaJurisprudencial.ShouldEqual("El fuero de Maternidad (test)");

            LineaServices.DeleteLineaJurisprudencial(ln);
        }

        public int CodSuscriptor = 8;

        [Test]
        public void Can_GetAllLineaJurisprudencial()
        {
            InitCaseTest();

            var lineas = LineaServices.GetAllLineaJurisprudencial();
            lineas.ShouldNotBeEmpty();
        }

        [Test]
        public void Can_GetLineaById()
        {
            InitCaseTest();

            var linea = LineaServices.GetLineaById(34);
            linea.ShouldNotBeNull();
        }

        [Test]
        public void Can_GetLineaJurisprudencialByUser()
        {
            InitCaseTest();

            var lineaUser = LineaServices.GetLineaJurisprudencialByUser(CodSuscriptor);
            lineaUser.ShouldNotBeEmpty();
        }

        [Test]
        public void Can_GetLineaJurisprudencialByArea()
        {
            InitCaseTest();

            var lineaArea = LineaServices.GetLineaJurisprudencialByArea(new AreaDerecho { Id = 1076 });
            lineaArea.ShouldNotBeEmpty();
        }

        [Test]
        public void Can_DeleteLineaJurisprudencial()
        {
            InitCaseTest();

            var linea = LineaServices.GetLineaById(2060);
            linea.ShouldNotBeNull();

            LineaServices.DeleteLineaJurisprudencial(linea);
        }

        [Test]
        public void Can_UpdateLineaJurisprudencial()
        {
            InitCaseTest();

            var linea = LineaServices.GetLineaById(2060);
            linea.ShouldNotBeNull();

            linea.NomLineaJurisprudencial += "+";

            LineaServices.UpdateLineaJurisprudencial(linea);
        }

    }
}
