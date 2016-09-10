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

namespace LEGIS.PJ.Tests.Services
{
    
    [TestFixture]
    public class TextoServicesTest:ServicesTests
    {
    

     /// <summary>
     /// Prueba para el servicio de texto (inserta y comprueba que el valor se puede acceder)
     /// Finalmente borra el valor
     /// </summary>
    [Test]
     public void Can_Texto_Get()
     {

         var textoInsert="Texto de prueba de pruebas  unitarias";
         IRepository<Texto> _repositoryTexto = new EfRepository<Texto>(context);
         ITextoServices _servicesTexto = new TextoServices(_cacheManager, _repositoryTexto);
        

           //Insert 
            var insTexto = new Texto {
                                   CodIdioma="ESP",
                                   Descripcion="Test",
                                   NomTexto="test.texto",
                                   Text = textoInsert
            };
        //Insert
        _servicesTexto.InsertTexto(insTexto);

        //Select
        var seltexto= _servicesTexto.GetTextoByName("test.texto");
        seltexto.Text.ShouldEqual<string>(textoInsert);
         
        //Delete
        _servicesTexto.DeleteTexto(seltexto);
        seltexto = _servicesTexto.GetTextoByName("test.texto");
        seltexto.ShouldBeNull();




     }


    [Test]
    public void Can_Texto_Update()
    {

        var textoInsert = "Texto de prueba de pruebas  unitarias";
        IRepository<Texto> _repositoryTexto = new EfRepository<Texto>(context);
        ITextoServices _servicesTexto = new TextoServices(_cacheManager, _repositoryTexto);


        //Insert 
        var insTexto = new Texto
        {
            CodIdioma = "ESP",
            Descripcion = "Test",
            NomTexto = "test.texto",
            Text = textoInsert
        };
        //Insert
        _servicesTexto.InsertTexto(insTexto);

        //Select
        var seltexto = _servicesTexto.GetTextoByName("test.texto");
        seltexto.Text.ShouldEqual<string>(textoInsert);


        //Delete
        seltexto.Text = "editando";

        _servicesTexto.UpdateTexto(seltexto);
        var seltextoup = _servicesTexto.GetTextoById(seltexto.Id);
        seltextoup.Text.ShouldEqual<string>("editando");


        //Delete
        _servicesTexto.DeleteTexto(seltextoup);
        var strTexto = _servicesTexto.GetText("test.texto");
        strTexto.ShouldEqual("test.texto");




    }
    
    [Test]
    public void Can_Texto_GetAll()
    {

        IRepository<Texto> _repositoryTexto = new EfRepository<Texto>(context);
        ITextoServices _servicesTexto = new TextoServices(_cacheManager, _repositoryTexto);

        var lstTexto = _servicesTexto.GetAllResource();

        lstTexto.ShouldNotBeNull();

        (lstTexto.Count > 0).ShouldBeTrue();
    
    
    }
    
    }
}
