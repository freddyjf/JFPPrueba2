using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using LEGIS.PJ.Data;
using LEGIS.PJ.Core.Data;
using LEGIS.PJ.Core.Domain;
using LEGIS.PJ.Core.Domain.Text;
using LEGIS.PJ.Services.Textos;
using LEGIS.PJ.Core.Domain.Suscriptores;
using LEGIS.PJ.Services.Suscriptores;
using LEGIS.PJ.Core.Domain.AreasDerecho;
using LEGIS.PJ.Services.AreasDerecho;
using LEGIS.PJ.Core.Domain.SuscriptoresAreaDerecho;
using LEGIS.PJ.Core.Domain.LineasJurisprudenciales;

namespace LEGIS.PJ.Tests.Services
{

    [TestFixture]
    public class SuscriptorServicesTest : ServicesTests
    {
        [Test(Description = "Inserción de un nuevo suscriptor solo en la tabla 'Suscriptor'")]
        public void Can_Suscriptor_Insert01()
        {
            IRepository<Suscriptor> _repositorySuscriptor = new EfRepository<Suscriptor>(context);
            IRepository<SuscriptorAreaDerecho> _repositoryAreaSuscriptor = new EfRepository<SuscriptorAreaDerecho>(context);
            IRepository<AreaDerechoPPV> _repositoryAreaPPV = new EfRepository<AreaDerechoPPV>(context);
            IRepository<LineaJurisprudencial> _repositoryLinea = new EfRepository<LineaJurisprudencial>(context);

            ISuscriptorServices _servicesSuscriptor = new SuscriptorServices(
                _cacheManager, _repositorySuscriptor,
                _repositoryAreaSuscriptor,
                _repositoryAreaPPV,
                _repositoryLinea);


            var correo = "rafael.eduardo@legis.com.co";
            var s= _servicesSuscriptor.GetSuscriptorByCorreo(correo);
            
            if(s!=null)
            {
                correo = correo + (s.Id + 1).ToString();

            }
            var NS = new Suscriptor
            {
                Nombres = "Rafael Eduardo",
                Apellidos = "Fernández Ricaurte",
                Correo = correo,
                EsSubAtencion = false,
                Clave = "piquin",
                CodScala = "CS02"
            };

            _servicesSuscriptor.InsertSuscriptor(NS);
            NS.Id.ShouldBeNotBeTheSameAs(0);
        }

        [Test(Description = "Inserción de un nuevo suscriptor con áreas del derecho")]
        public void Can_Suscriptor_Insert02()
        {
            IRepository<Suscriptor> _repositorySuscriptor = new EfRepository<Suscriptor>(context);
            IRepository<SuscriptorAreaDerecho> _repositoryAreaSuscriptor = new EfRepository<SuscriptorAreaDerecho>(context);
            IRepository<AreaDerechoPPV> _repositoryAreaPPV = new EfRepository<AreaDerechoPPV>(context);
            IRepository<LineaJurisprudencial> _repositoryLinea = new EfRepository<LineaJurisprudencial>(context);

            ISuscriptorServices _servicesSuscriptor = new SuscriptorServices(
                _cacheManager, _repositorySuscriptor,
                _repositoryAreaSuscriptor, 
                _repositoryAreaPPV,
                _repositoryLinea);

            IAreaDerechoServices _AreaServices=new AreaDerechoServices(
                _cacheManager,
                new EfRepository<AreaDerecho>(context),
                new EfRepository<LineaJurisprudencial>(context));

            var correo = "rafael.eduardoñ@legis.com.co";
            var s = _servicesSuscriptor.GetSuscriptorByCorreo(correo);

            if (s != null)
            {
                correo = correo + (s.Id + 1).ToString();

            }

            var NS = new Suscriptor
            {
                Nombres = "Rafael Eduardo",
                Apellidos = "Fernández Ricaurte",
                Correo = correo,
                EsSubAtencion = false,
                Clave = "piquin",
                CodScala = "CS02"
            };

            var AD = _AreaServices.GetAreaById(1076);

            var SAD = new SuscriptorAreaDerecho
            {
                Suscriptor = NS,
                AreaDerecho = AD,
                FechaCreacion = DateTime.Now,
                FechaVencimiento = DateTime.Now
            };

            NS.SuscriptorAreasDerecho.Add(SAD);

            _servicesSuscriptor.InsertSuscriptor(NS);

            // TEST
            NS.Id.ShouldBeNotBeTheSameAs(0);
            SAD.Id.ShouldBeNotBeTheSameAs(0);

            // CLEAN.
           // _servicesSuscriptor.(NS);

        }

        /// <summary>
        /// Prueba para el servicio de texto (inserta y comprueba que el valor se puede acceder)
        /// Finalmente borra el valor
        /// </summary>
        [Test]
        public void Can_Suscriptor_GetById()
        {
            IRepository<Suscriptor> _repositorySuscriptor = new EfRepository<Suscriptor>(context);
            IRepository<SuscriptorAreaDerecho> _repositoryAreaSuscriptor = new EfRepository<SuscriptorAreaDerecho>(context);
            IRepository<AreaDerechoPPV> _repositoryAreaPPV = new EfRepository<AreaDerechoPPV>(context);
            IRepository<LineaJurisprudencial> _repositoryLinea = new EfRepository<LineaJurisprudencial>(context);


            ISuscriptorServices _servicesSuscriptor = new SuscriptorServices(_cacheManager, _repositorySuscriptor, _repositoryAreaSuscriptor, _repositoryAreaPPV, _repositoryLinea);

            //Select
            var SuscriptorData = _servicesSuscriptor.GetSuscriptorById(1);
            SuscriptorData.Correo.ShouldEqual<string>("nestor.fernandez@legis.com.co");
            SuscriptorData.Nombres.ShouldEqual<string>("Néstor Arturo Fernández Ricaurte");
        }

        [Test]
        public void Can_Suscriptor_GetByCorreo()
        {
            IRepository<Suscriptor> _repositorySuscriptor = new EfRepository<Suscriptor>(context);
            IRepository<SuscriptorAreaDerecho> _repositoryAreaSuscriptor = new EfRepository<SuscriptorAreaDerecho>(context);
            IRepository<AreaDerechoPPV> _repositoryAreaPPV = new EfRepository<AreaDerechoPPV>(context);
                        IRepository<LineaJurisprudencial> _repositoryLinea = new EfRepository<LineaJurisprudencial>(context);

            ISuscriptorServices _servicesSuscriptor = new SuscriptorServices(_cacheManager, _repositorySuscriptor, _repositoryAreaSuscriptor, _repositoryAreaPPV, _repositoryLinea);

            //Select
            var SuscriptorData = _servicesSuscriptor.GetSuscriptorByCorreo("nestor.fernandez@legis.com.co");
            SuscriptorData.Id.ShouldEqual<int>(1);
            SuscriptorData.Nombres.ShouldEqual<string>("Néstor Arturo Fernández Ricaurte");

        }

    }
}
