using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Configuration;
using FOX.DataModels.Context;

namespace FOX.DataModels.GenericRepository
{
    /// <summary>
    /// Generic Repository class for Entity Operations
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public static class SpRepository<TEntity> where TEntity : class
    {
        static long retrycatch = 0;
        #region Private member variables...
        // internal static DbContext Context =new DbContextSP();
        // internal static DbSet<TEntity> DbSet= Context.Set<TEntity>();
        //private object 
        #endregion

        #region Public Constructor...

        static SpRepository()
        {
        }
        #endregion

        #region Public member methods...


        /// <summary>
        /// generic Execute SP
        /// </summary>
        /// <returns></returns>
        public static List<TEntity> GetListWithStoreProcedure(string query, params object[] parameters)
        {
            try
            {
                using (DbContext Context = new DbContextSP())
                {
                    retrycatch = 0;
                    Context.Database.CommandTimeout = 300;
                    return Context.Database.SqlQuery<TEntity>(query, parameters).ToList<TEntity>();

                }
            }
            catch (Exception ex)
            {
                if (retrycatch <= 2 && !string.IsNullOrEmpty(ex.Message) && ex.Message.Contains("deadlocked on lock resources with another process")
                    || (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message) && ex.InnerException.Message.Contains("deadlocked on lock resources with another process")))
                {
                    retrycatch = retrycatch + 1;
                    return GetListWithStoreProcedure(query, parameters);
                }
                else
                {
                    retrycatch = 0;
                    throw ex;

                }
            }
        }

        public static TEntity GetSingleObjectWithStoreProcedure(string query, params object[] parameters)
        {
            try
            {

                using (DbContext Context = new DbContextSP())
                {
                    retrycatch = 0;
                    Context.Database.CommandTimeout = 300;
                    return Context.Database.SqlQuery<TEntity>(query, parameters).FirstOrDefault<TEntity>();
                }
            }
            catch (Exception ex)
            {
                if (retrycatch <= 2 && !string.IsNullOrEmpty(ex.Message) && ex.Message.Contains("deadlocked on lock resources with another process")
                   || (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message) && ex.InnerException.Message.Contains("deadlocked on lock resources with another process")))
                {
                    retrycatch = retrycatch + 1;
                    return GetSingleObjectWithStoreProcedure(query, parameters);
                }
                else
                {
                    retrycatch = 0;
                    throw ex;

                }
            }

        }
        //sql dataAdapter for dataset
        public static SqlDataAdapter getSpSqlDataAdapter(string query)
        {
            using (DbContext Context = new DbContextSP())
            {
                Context.Database.CommandTimeout = 300;
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, Context.Database.Connection.ConnectionString);
                return dataAdapter;
            }
        }
        #endregion
    }

}
