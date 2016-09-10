using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using JFP.Core;
using JFP.Core.Data;

namespace JFP.Data
{
    /// <summary>
    /// Entity Framework repository
    /// </summary>
    public class EfRepository<TElement> : IRepository<TElement> where TElement : BaseEntity
    {
        #region Fields

        private readonly IDbContext _context;
        private IDbSet<TElement> _entities;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="context">Object context</param>
        public EfRepository(IDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Detach an entity so it can be refreshed from DB.
        /// </summary>
        /// <param name="entity"></param>
        public virtual void DetachEntity(TElement entity)
        {
            _context.Detach(entity);
        }

        /// <summary>
        /// Get entity by identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        public virtual TElement GetById(object id)
        {
            //see some suggested performance optimization (not tested)
            //http://stackoverflow.com/questions/11686225/dbset-find-method-ridiculously-slow-compared-to-singleordefault-on-id/11688189#comment34876113_11688189
            return Entities.Find(id);
        }

        /// <summary>
        /// Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Insert(TElement entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                Entities.Add(entity);

                _context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;

                // Descarta los cambios hechos.
                DiscardChanges();

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
            catch
            {
                // Descarta los cambios hechos.
                DiscardChanges();
            }
        }

        /// <summary>
        /// Insert entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void Insert(IEnumerable<TElement> entities)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");

                if (!entities.Any())
                    return;

                foreach (var entity in entities)
                    Entities.Add(entity);

                _context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg +=
                            string.Format("Property: {0} Error: {1}", validationError.PropertyName,
                                validationError.ErrorMessage) + Environment.NewLine;

                // Descarta los cambios hechos.
                DiscardChanges();

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
            catch
            {
                // Descarta los cambios hechos.
                DiscardChanges();
            }
        }

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Update(TElement entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");
                _context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);

                // Descarta los cambios hechos.
                DiscardChanges();

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
            catch
            {
                // Descarta los cambios hechos.
                DiscardChanges();
            }
        }

        /// <summary>
        /// Update entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void Update(IEnumerable<TElement> entities)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");

                if (!entities.Any())
                    return;

                _context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;

                // Descarta los cambios hechos.
                DiscardChanges();

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
            catch
            {
                // Descarta los cambios hechos.
                DiscardChanges();
            }
        }

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Delete(TElement entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                Entities.Remove(entity);

                _context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);

                // Descarta los cambios hechos.
                DiscardChanges();

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
            catch
            {
                // Descarta los cambios hechos.
                DiscardChanges();
            }
        }

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void Delete(IEnumerable<TElement> entities)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");

                if (!entities.Any())
                    return;

                foreach (var entity in entities)
                    Entities.Remove(entity);

                _context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);

                // Descarta los cambios hechos.
                DiscardChanges();

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
            catch
            {
                // Descarta los cambios hechos.
                DiscardChanges();
            }
        }

        /// <summary>
        /// Creates a new instance for the entity T.
        /// </summary>
        /// <returns></returns>
        public TElement Create()
        {
            return Entities.Create<TElement>();
        }

        /// <summary>
        /// Rollback changes from the entity.
        /// </summary>
        public void DiscardChanges()
        {
            _context.DiscardChanges<TElement>();
        }

        /// <summary>
        /// Ejecuta un procedimiento almacenado sin esperar resultados.
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="parameters">Pares de (nombre, valor) para los parámetros.</param>
        public virtual void ExecuteProcedureNonQuery(string procedureName, params object[] parameters)
        {
            try
            {
                // Armar la lista de los parámetros.
                var Params = new List<IDbDataParameter>();
                for (int i = 0; i < parameters.Length; i += 2)
                {
                    if (parameters[i] is IDbDataParameter)
                    {
                        Params.Add(parameters[i] as SqlParameter);
                    }
                    else
                    {
                        var Nombre = parameters[i].ToString();
                        if (!Nombre.StartsWith("@"))
                            Nombre = "@" + Nombre;
                        Params.Add(new SqlParameter(
                            Nombre, parameters[i + 1].ToString()));
                    }
                }

                _context.ExecuteSqlCommand(procedureName, parameters: Params.ToArray());

            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);

                // Descarta los cambios hechos.
                DiscardChanges();

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

        /// <summary>
        /// Ejecuta un procedimiento almacenado y retorna una lista de resultados
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="parameters">Pares de (nombre, valor) para los parámetros.</param>
        public virtual IEnumerable<TElement> ExecuteQuery<TElement>(string procedureName, params object[] parameters)
        {
            try
            {
                // Armar la lista de los parámetros.
                var Params = new List<IDbDataParameter>();
                for (int i = 0; i < parameters.Length; i += 2)
                {
                    var Nombre = parameters[i].ToString();
                    if (!Nombre.StartsWith("@"))
                        Nombre = "@" + Nombre;

                    Params.Add(new SqlParameter(
                        Nombre, parameters[i + 1].ToString()));
                }

                return _context.SqlQuery<TElement>(procedureName, Params.ToArray());
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);

                // Descarta los cambios hechos.
                DiscardChanges();

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

        #endregion

        #region Transacciones

        DbContextTransaction _Transaction;

        /// <summary>
        /// Iniciar una transacción.
        /// </summary>
        /// <returns></returns>
        public void TransactionStart()
        {
            _Transaction = _context.GetDatabase.BeginTransaction();
        }

        /// <summary>
        /// Completar una transacción.
        /// </summary>
        /// <returns></returns>
        public void TransactionCommit()
        {
            _Transaction.Commit();
        }

        /// <summary>
        /// Completar una transacción.
        /// </summary>
        /// <returns></returns>
        public void TransactionRollback()
        {
            _Transaction.Rollback();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a table
        /// </summary>
        public virtual IQueryable<TElement> Table
        {
            get
            {
                return Entities;
            }
        }

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        public virtual IQueryable<TElement> TableNoTracking
        {
            get
            {
                return Entities.AsNoTracking();
            }
        }

        /// <summary>
        /// Entities
        /// </summary>
        protected virtual IDbSet<TElement> Entities
        {
            get
            {
                if (_entities == null)
                    _entities = _context.Set<TElement>();
                return _entities;
            }
        }


        #endregion
    }
}
