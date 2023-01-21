using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
//using System.Data.Entity.Infrastructure;
//using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;

namespace FOX.DataModels.GenericRepository
{
    /// <summary>
    /// Generic Repository class for Entity Operations
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class GenericRepository<TEntity> where TEntity : class
    {
        #region Private member variables...
        internal DbContext Context;
        internal DbSet<TEntity> DbSet;
        #endregion

        #region Public Constructor...
        /// <summary>
        /// Public Constructor,initializes privately declared local variables.
        /// </summary>
        /// <param name="context"></param>
        public GenericRepository(DbContext context)
        {
            Context = context;
            DbSet = context.Set<TEntity>();
        }

        public GenericRepository()
        {
        }
        #endregion

        #region Public member methods...


        /// <summary>
        /// generic Execute SP
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TEntity> GetListWithStoreProcedure(string query, params object[] parameters)
        {
            SetDataBaseConfigurationString(); //Very important it will set the connection string for FOX or CCRemote
            return Context.Database.SqlQuery<TEntity>(query, parameters);
        }


        /// <summary>
        /// generic Get method for Entities
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> Get()
        {
            SetDataBaseConfigurationString(); //Very important it will set the connection string for FOX or CCRemote
            IQueryable<TEntity> query = DbSet;
            return query.ToList();
        }

        /// <summary>
        /// Generic get method on the basis of id for Entities.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual TEntity GetByID(object id)
        {
            SetDataBaseConfigurationString(); //Very important it will set the connection string for FOX or CCRemote
            return DbSet.Find(id);
        }

        /// <summary>
        /// generic Insert method for the entities
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Insert(TEntity entity)
        {
            SetDataBaseConfigurationString(); //Very important it will set the connection string for FOX or CCRemote
            DbSet.Add(entity);
        }




        /// <summary>
        /// Generic Delete method for the entities
        /// </summary>
        /// <param name="id"></param>
        public virtual void Delete(object id)
        {
            SetDataBaseConfigurationString(); //Very important it will set the connection string for FOX or CCRemote
            TEntity entityToDelete = DbSet.Find(id);
            Delete(entityToDelete);
        }

        public void SetEntityState(TEntity entity, EntityState state)
        {
            ((System.Data.Entity.Infrastructure.IObjectContextAdapter)Context).ObjectContext.Detach(entity);
            //Context.Entry(entity).State = state;
        }

        /// <summary>
        /// Generic Delete method for the entities
        /// </summary>
        /// <param name="entityToDelete"></param>
        public virtual void Delete(TEntity entityToDelete)
        {
            SetDataBaseConfigurationString(); //Very important it will set the connection string for FOX or CCRemote
            if (Context.Entry(entityToDelete).State == EntityState.Detached)
            {
                DbSet.Attach(entityToDelete);
            }
            DbSet.Remove(entityToDelete);
        }

        /// <summary>
        /// Generic update method for the entities
        /// </summary>
        /// <param name="entityToUpdate"></param>
        public virtual void Update(TEntity entityToUpdate)
        {
            SetDataBaseConfigurationString(); //Very important it will set the connection string for FOX or CCRemote
            DbSet.Attach(entityToUpdate);
            Context.Entry(entityToUpdate).State = EntityState.Modified;

        }

        /// <summary>
        /// generic method to get many record on the basis of a condition.
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual List<TEntity> GetMany(Expression<Func<TEntity, bool>> where)
        {
            SetDataBaseConfigurationString(); //Very important it will set the connection string for FOX or CCRemote
            if (EntityHelper.isTalkRehab)
            {
                Context.Database.CommandTimeout = 300;
            }
            return DbSet.Where(where).ToList();
        }

        /// <summary>
        /// generic method to get many record on the basis of a condition but query able.
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> GetManyQueryable(Expression<Func<TEntity, bool>> where)
        {
            SetDataBaseConfigurationString(); //Very important it will set the connection string for FOX or CCRemote
            return DbSet.Where(where).AsQueryable();
        }

        /// <summary>
        /// generic get method , fetches data for the entities on the basis of condition.
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public TEntity Get(Func<TEntity, Boolean> where)
        {
            SetDataBaseConfigurationString(); //Very important it will set the connection string for FOX or CCRemote
            return DbSet.Where(where).FirstOrDefault<TEntity>();
        }
        /// <summary>
        /// generic delete method , deletes data for the entities on the basis of condition.
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public void Delete(Expression<Func<TEntity, bool>> where)
        {
            SetDataBaseConfigurationString(); //Very important it will set the connection string for FOX or CCRemote
            IQueryable<TEntity> objects = DbSet.Where(where).AsQueryable();
            //foreach (TEntity obj in objects)
            //    DbSet.Remove(obj);
            DbSet.RemoveRange(objects);
        }

        /// <summary>
        /// generic method to fetch all the records from db
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> GetAll()
        {
            SetDataBaseConfigurationString(); //Very important it will set the connection string for FOX or CCRemote
            return DbSet.ToList();
        }

        /// <summary>
        /// Inclue multiple
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        public IQueryable<TEntity> GetWithInclude(
            Expression<Func<TEntity,
            bool>> predicate, params string[] include)
        {
            SetDataBaseConfigurationString(); //Very important it will set the connection string for FOX or CCRemote
            IQueryable<TEntity> query = DbSet;
            query = include.Aggregate(query, (current, inc) => current.Include(inc));
            return query.Where(predicate);
        }

        /// <summary>
        /// Generic method to check if entity exists
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        public bool Exists(object primaryKey)
        {
            SetDataBaseConfigurationString(); //Very important it will set the connection string for FOX or CCRemote
            return DbSet.Find(primaryKey) != null;
        }

        /// <summary>
        /// Gets a single record by the specified criteria (usually the unique identifier)
        /// </summary>
        /// <param name="predicate">Criteria to match on</param>
        /// <returns>A single record that matches the specified criteria</returns>
        public TEntity GetSingle(Expression<Func<TEntity, bool>> predicate)
        {
            SetDataBaseConfigurationString(); //Very important it will set the connection string for FOX or CCRemote
            return DbSet.Single(predicate);
        }
        public TEntity GetSingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            SetDataBaseConfigurationString(); //Very important it will set the connection string for FOX or CCRemote
            return DbSet.FirstOrDefault(predicate);
        }

        /// <summary>
        /// The first record matching the specified criteria
        /// </summary>
        /// <param name="predicate">Criteria to match on</param>
        /// <returns>A single record containing the first record matching the specified criteria</returns>
        public TEntity GetFirst(Expression<Func<TEntity, bool>> predicate)
        {
            SetDataBaseConfigurationString(); //Very important it will set the connection string for FOX or CCRemote
            return DbSet.FirstOrDefault(predicate);
        }

        /// <summary>
        /// Save DB Context changes
        /// </summary>
        /// <param name="predicate">Criteria to match on</param>
        /// <returns>A single record containing the first record matching the specified criteria</returns>
        public void Save()
        {
            SetDataBaseConfigurationString(); //Very important it will set the connection string for FOX or CCRemote
            try
            {
                Context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {

                var outputLines = new List<string>();
                foreach (var eve in e.EntityValidationErrors)
                {
                    outputLines.Add(string.Format(
                        "{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", DateTime.Now,
                        eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        outputLines.Add(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }
                foreach (var item in outputLines)
                {
                    HelperClasses.Helper.LogException(item);
                }
                throw e;
            }


        }

        #endregion

        public void Dispose()
        {
            Context?.Dispose();

        }
        public List<TEntity> ExecuteCommand(string query, params object[] parameters)
        {
            SetDataBaseConfigurationString(); //Very important it will set the connection string for FOX or CCRemote
            try
            {
                Context.Database.CommandTimeout = 300;
                return DbSet.SqlQuery(query, parameters).ToList<TEntity>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void SetDataBaseConfigurationString()
        {
            if (EntityHelper.isTalkRehab)
            {
                if (Context.Database.Connection.ConnectionString != ConfigurationManager.ConnectionStrings["TalkRehabConnection"].ConnectionString)
                {
                    Context.Database.Connection.ConnectionString = ConfigurationManager.ConnectionStrings["TalkRehabConnection"].ConnectionString;
                }
            }
            else
            {
                if (!EntityHelper.isTalkRehab && (Context.Database.Connection.ConnectionString != ConfigurationManager.ConnectionStrings["FOXConnection"].ConnectionString))
                {
                    Context.Database.Connection.ConnectionString = ConfigurationManager.ConnectionStrings["FOXConnection"].ConnectionString;
                }
            }            
            //if (EntityHelper.isTalkRehab && (Context.Database.Connection.ConnectionString != ConfigurationManager.ConnectionStrings["TalkRehabConnection"].ConnectionString))
            //{
            //    Context.Database.Connection.ConnectionString = ConfigurationManager.ConnectionStrings["TalkRehabConnection"].ConnectionString;
            //}
            //else if (!EntityHelper.isTalkRehab && (Context.Database.Connection.ConnectionString != ConfigurationManager.ConnectionStrings["FOXConnection"].ConnectionString))
            //{
            //    Context.Database.Connection.ConnectionString = ConfigurationManager.ConnectionStrings["FOXConnection"].ConnectionString;
            //}
        }
    }


}
