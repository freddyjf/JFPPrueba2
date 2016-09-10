using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using LEGIS.PJ.Services.Notificaciones;
using LEGIS.PJ.Core.Domain.Notificacion;
using LEGIS.PJ.Core.Domain;
using LEGIS.PJ.Core.Data;
using LEGIS.PJ.Core.Loggin;
using LEGIS.PJ.Data;


namespace LEGIS.PJ.Tests.Services.NotificacionesTest
{
    [TestFixture]
    public partial class NotificacionTests : ServicesTests
    {


        private IRepository<Notificacion> _repository;
        private IRepository<DetalleNotificacion> _Detallerepository;
        private IRepository<TipoNotificacion> _TipoNotificacionrepository;
        private INotificacionServices NotificacionServices;
        private IRepository<NotificacionRecibida> _notificacionRecibidaRepository;




        [TestCase(true)]
        public void Can_Notificacion_Insert(bool delete)
        {

            initCaseTest();

            Notificacion n = new Notificacion
            {
                codSuscriptor = 8,
                Fecha = DateTime.Now,
                Activo = true,
                Frecuencia = 8
            };


            DetalleNotificacion d = new DetalleNotificacion
            {
                Notificacion = n,
                codTipoNotificacion = 1,
                UltimoEnvio = DateTime.Now,
                activo = false
            };

            n.DetallesNotificacion = new List<DetalleNotificacion>();

            n.DetallesNotificacion.Add(d);

            //this.NotificacionServices.InsertarNotificacion(n);


            n.Id.ShouldNotBeNull();
            if (delete)
            {
                
            }
            //this.NotificacionServices.BorrarNotificacion(n);

        }



        [Test]
        public void Can_Notificacion_Get()
        {
            initCaseTest();

            var n = this.NotificacionServices.GetNotificacionesByUser(8);

            n.ShouldNotBeEmpty();
        }


        private void initCaseTest()
        {

            _repository = new EfRepository<Notificacion>(context);
            _Detallerepository = new EfRepository<DetalleNotificacion>(context);
            _notificacionRecibidaRepository = new EfRepository<NotificacionRecibida>(context);
            _TipoNotificacionrepository = new EfRepository<TipoNotificacion>(context);
            NotificacionServices = new NotificacionServices(_repository, _Detallerepository, _notificacionRecibidaRepository, _TipoNotificacionrepository, _cacheManager);
        }



    }
}
