using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JFP.Core.Data
{
    /// <summary>
    /// Repository
    /// </summary>
    public partial interface IRepository<T> where T : BaseEntity
    {

        /// <summary>
        /// Desacopla una entidad de su contexto.
        /// Utilizarlo para que se recargue nuevamente desde base de datos omitiendo el caché del EF.
        /// </summary>
        /// <param name="entity"></param>
        void DetachEntity(T entity);

        /// <summary>
        /// Get entity by identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        T GetById(object id);

        /// <summary>
        /// Creates a new instance for the entity T.
        /// </summary>
        /// <returns></returns>
        T Create();

        /// <summary>
        /// Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Insert(T entity);

        /// <summary>
        /// Insert entities
        /// </summary>
        /// <param name="entities">Entities</param>
        void Insert(IEnumerable<T> entities);

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Update(T entity);

        /// <summary>
        /// Update entities
        /// </summary>
        /// <param name="entities">Entities</param>
        void Update(IEnumerable<T> entities);

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Delete(T entity);

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        void Delete(IEnumerable<T> entities);

        /// <summary>
        /// Ejecuta un procedimiento almacenado sin esperar resultados.
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="parameters">Pares de (nombre, valor) para los parámetros.</param>
        void ExecuteProcedureNonQuery(string procedureName, params object[] parameters);

        /// <summary>
        /// Ejecuta un procedimiento almacenado y retorna una lista de resultados
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="parameters">Pares de (nombre, valor) para los parámetros.</param>
        IEnumerable<TElement> ExecuteQuery<TElement>(string procedureName, params object[] parameters);

        /// <summary>
        /// Gets a table
        /// </summary>
        IQueryable<T> Table { get; }

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        IQueryable<T> TableNoTracking { get; }

        /// <summary>
        /// Iniciar una transacción.
        /// </summary>
        /// <returns></returns>
        void TransactionStart();

        /// <summary>
        /// Completar una transacción.
        /// </summary>
        /// <returns></returns>
        void TransactionCommit();

        /// <summary>
        /// Completar una transacción.
        /// </summary>
        /// <returns></returns>
        void TransactionRollback();

        /// <summary>
        /// Rollback changes from the entity.
        /// </summary>
        void DiscardChanges();
    }
}
