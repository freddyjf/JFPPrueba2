using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using LEGIS.PJ.Core.Domain;
using LEGIS.PJ.Core.Domain.Text;
using LEGIS.PJ.Data;

namespace LEGIS.PJ.Test.Data
{
 
    public class TextoTest:PersistenceTest
    {



        public void Can_Texto_Insert()
        {
            
            //Insert 
            var oTexto = new Texto {
                                   CodIdioma="ESP",
                                   Descripcion="Test",
                                   NomTexto="test.texto",
                                   Text="Texto de prueba de pruebas  unitarias"
            };


            var fromDb = SaveAndLoadEntity(oTexto);
            fromDb.ShouldNotBeNull();
            fromDb.Descripcion.ShouldEqual("Test");
            fromDb.NomTexto.ShouldEqual("test.texto");
            
          


            //Delete
            var deleteDb = DeleteEntity(oTexto);
            deleteDb.ShouldNotNull();
            

        }
    
    }
}
