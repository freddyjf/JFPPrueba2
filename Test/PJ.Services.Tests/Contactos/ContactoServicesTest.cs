using System;
using NUnit.Framework;
using LEGIS.PJ.Core.Domain.Contacto;
using LEGIS.PJ.Services.Contactos;
using LEGIS.PJ.Data;
using LEGIS.PJ.Core.Data;

namespace LEGIS.PJ.Tests.Services.Contactos
{
    [TestFixture]
    public class ContactoServicesTest:ServicesTests
    {
        private IContactoServices _contactoServices = null;

        public void InitCaseTest()
        {
            IRepository<Contacto> _repositoryContacto = new EfRepository<Contacto>(context);
            _contactoServices = new ContactoServices(_repositoryContacto, _cacheManager);            
        }
    
        [Test]
        public void Can_Contacto_Insert()
        {

            InitCaseTest();

            var ct= new Contacto{
                Nombre="freddy parra",
                Telefono = "3142278277",
                Mensaje="test case",
                Email="freddy.parra@legis.com.co",
                Fecha=DateTime.Now,
                TipoIdentificacion = "CC",
                CodSilenioTipoIdentificacion = "8693",
                NumIdentificacion = "1014847594",
                Departamento = "C MARCA" ,
                Ciudad = "Bogotá",
                CodSilenioCiudad = "33",
                Direccion = "Calle 26 # 10 - 58",
                Ocupacion = "Abogado",
                CodSilenioOcupacion = "48547",
                HabeasData = true
            };            

            _contactoServices.insertContacto(ct);
            ct.Id.ShouldNotBeNull();
            var lstContacto= _contactoServices.getAllContactos();
            lstContacto.Contains(ct).ShouldBeTrue();
        }

        [Test]
        public void Can_GetContactos()
        {
            InitCaseTest();

            var contactos = _contactoServices.getAllContactos();
            contactos.ShouldNotBeEmpty();
        }
    }
}
