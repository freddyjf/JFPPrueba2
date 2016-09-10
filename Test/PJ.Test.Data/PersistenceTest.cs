using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using LEGIS.PJ.Core;
using LEGIS.PJ.Data;
using LEGIS.PJ.Core.Config;
using NUnit.Framework;

namespace LEGIS.PJ.Test.Data
{
    [TestFixture]
    public class PersistenceTest
    {

        protected PJObjectContext context;


        [SetUp]
        public void SetUp()
        {
            //TODO fix compilation warning (below)
            #pragma warning disable 0618
            Database.DefaultConnectionFactory = new SqlConnectionFactory(); //new SqlCeConnectionFactory("System.Data.SqlServerCe.4.0");
            context = new PJObjectContext(GetTestDbName());
            //context.Database.Delete();
            //context.Database.Create();
        }

        protected string GetTestDbName()
        {
            //TODO-Dejar esta cadena de conexion en un archivo "xdp.config"
            //"metadata=res://*/;provider=System.Data.SqlClient;provider connection string='data source=Delico;initial catalog=Precedente;user id=SQL_PrecedenteJ;password=123456;MultipleActiveResultSets=True;App=PrecedenteJ'";
            string testDbName = Config.LlaveString("DBPrecedente"); 
            return testDbName;
        }

        /// <summary>
        /// Persistance test helper
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="disposeContext">A value indicating whether to dispose context</param>
        protected T SaveAndLoadEntity<T>(T entity, bool disposeContext = true) where T : BaseEntity
        {

            context.Set<T>().Add(entity);
            context.SaveChanges();

            object id = entity.Id;

            if (disposeContext)
            {
                context.Dispose();
                context = new PJObjectContext(GetTestDbName());
            }

            var fromDb = context.Set<T>().Find(id);
            return fromDb;
        }

        /// <summary>
        /// Persistance test helper
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="disposeContext">A value indicating whether to dispose context</param>
        protected T DeleteEntity<T>(T entity) where T : BaseEntity
        {

            object id = entity.Id;
            context = new PJObjectContext(GetTestDbName());
            context.Set<T>().Attach(entity);                
            
            EfRepository<T> oEfr = new EfRepository<T>(context);
            oEfr.Delete(entity);
            
            var fromDb = context.Set<T>().Find(id);
            return fromDb;
        }

    }
}
