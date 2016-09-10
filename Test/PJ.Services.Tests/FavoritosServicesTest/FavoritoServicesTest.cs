using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LEGIS.PJ.Core.Data;
using LEGIS.PJ.Core.Domain.CarpetasFavoritos;
using LEGIS.PJ.Core.Domain.Favoritos;
using LEGIS.PJ.Data;
using LEGIS.PJ.Services.CarpetasFavoritos;
using NUnit.Framework;

namespace LEGIS.PJ.Tests.Services.FavoritosServicesTest
{
    public class FavoritoServicesTest : ServicesTests
    {
        private IRepository<CarpetaFavoritos> _carpetaFavoritosRepository;
        private IRepository<Favorito> _favoritosRepository;
        private IRepository<Core.Domain.AnalisisJuridicos.AnalisisJuridico> _analisisRepository;

        IFavoritosServices _favoritosServices;

        public void InitCaseTest()
        {
            _carpetaFavoritosRepository = new EfRepository<CarpetaFavoritos>(context);
            _favoritosRepository = new EfRepository<Favorito>(context);
            _analisisRepository = new EfRepository<Core.Domain.AnalisisJuridicos.AnalisisJuridico>(context);
            _favoritosServices = new FavoritosServices(_cacheManager, _carpetaFavoritosRepository, _favoritosRepository, _analisisRepository, context);
        }

        public int CodSuscriptor = 22;

        [Test]
        public void Can_ObtenerCarpetas()
        {
            InitCaseTest();

            var carpetas = _favoritosServices.ObtenerCarpetas(CodSuscriptor);
            carpetas.ShouldNotBeEmpty();
        }

        [Test]
        public void Can_ObtenerFavoritoByAnalisis()
        {
            InitCaseTest();

            var carpetas = _favoritosServices.ObtenerFavoritoByAnalisis(CodSuscriptor, 1606);
            carpetas.ShouldNotBeNull();
        }

        [Test]
        public void Can_ObtenerFavoritos()
        {
            InitCaseTest();

            var favoritos = _favoritosServices.ObtenerFavoritos(CodSuscriptor);
            favoritos.ShouldNotBeEmpty();
        }

        [Test]
        public void Can_CrearCarpeta()
        {
            InitCaseTest();

            var carpeta = _favoritosServices.CrearCarpeta("Testing", CodSuscriptor);
            carpeta.Id.ShouldNotEqual(-1);

            _carpetaFavoritosRepository.Delete(carpeta);
        }

        [Test]
        public void Can_CrearFavorito()
        {
            InitCaseTest();

            var favorito = _favoritosServices.CrearFavorito("Un nombre", 1028, 1606, CodSuscriptor);
            favorito.ShouldNotBeNull();
        }

        [Test]
        public void Can_RenombrarCarpeta()
        {
            InitCaseTest();

            var carpeta = _carpetaFavoritosRepository.GetById(1028);
            carpeta.ShouldNotBeNull();

            var respuesta = _favoritosServices.RenombrarCarpeta("Testing", carpeta.Id, CodSuscriptor);
            respuesta.ShouldNotEqual(RespuestaCarpeta.CarpetaExiste);
            respuesta.ShouldNotEqual(RespuestaCarpeta.NoAutorizado);
        }

        [Test]
        public void Can_RenombrarFavorito()
        {
            InitCaseTest();

            var favorito = _favoritosRepository.GetById(3);
            favorito.ShouldNotBeNull();

            _favoritosServices.RenombrarFavorito("Testing", favorito.Id, CodSuscriptor);
        }

        [Test]
        public void Can_MoverFavoritos()
        {
            InitCaseTest();

            _favoritosServices.MoverFavoritos(new[] { 12 }, 1028, CodSuscriptor);
        }

        [Test]
        public void Can_EliminarCarpetas()
        {
            InitCaseTest();

            _favoritosServices.EliminarCarpetas(new []{ 2042 }, CodSuscriptor);
        }

        [Test]
        public void Can_EliminarFavoritos()
        {
            InitCaseTest();

            _favoritosServices.EliminarFavoritos(new []{ 12 }, CodSuscriptor);
        }
    }
}
