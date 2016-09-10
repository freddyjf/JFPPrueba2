using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using NUnit.Framework;
using LEGIS.PJ.Core.Caching;
using LEGIS.PJ.Core.Loggin;

using LEGIS.PJ.Data;


namespace LEGIS.PJ.Tests.Services
{
    public abstract class ServicesTests
    {
        #region Fields
         protected ICacheManager _cacheManager;
         protected PJObjectContext context;
         protected LoggingManager _loggin;
         
        #endregion

        [SetUp]
        public void SetUp()
        {
            
            Init();
        }

        public void Init()
        {
            #pragma warning disable 0618
            Database.DefaultConnectionFactory = new SqlConnectionFactory(); //new SqlCeConnectionFactory("System.Data.SqlServerCe.4.0");
            context = new PJObjectContext(GetTestDbName());
            //context.Configuration.LazyLoadingEnabled = false;
            

         


            _cacheManager = new MemoryCacheManager();
            _loggin = new LoggingManager();
         
        }


        protected string GetTestDbName()
        {
            //TODO-Dejar esta cadena de conexion en un archivo "xdp.config"
            //"metadata=res://*/;provider=System.Data.SqlClient;provider connection string='data source=Delico;initial catalog=Precedente;user id=SQL_PrecedenteJ;password=123456;MultipleActiveResultSets=True;App=PrecedenteJ'";
            string testDbName = @"data source=GLAUCO\LATIN1_GEN;initial catalog=Precedente;user id=SQL_PrecedenteJ;password=123456;MultipleActiveResultSets=True;App=PrecedenteJ";
            return testDbName;
        }
    
    }
}
