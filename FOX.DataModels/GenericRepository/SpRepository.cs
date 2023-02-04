using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Configuration;
using FOX.DataModels.Context;
using System.Data.Common;
using System.Reflection;

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
                    Context.Database.CommandTimeout = 300;
                    return Context.Database.SqlQuery<TEntity>(query, parameters).ToList<TEntity>();

                }
            }
            catch (Exception ex)
            {
                if ((retrycatch <= 2 && !string.IsNullOrEmpty(ex.Message) && (ex.Message.Contains("deadlocked on lock resources with another process") || ex.Message.Contains("Timeout")))
                    || (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message)
                    && (ex.InnerException.Message.Contains("deadlocked on lock resources with another process") || ex.InnerException.Message.Contains("Timeout"))
                    ))
                {
                    retrycatch = retrycatch + 1;
                    var clonedParameters = parameters.Select(x => x.Clone()).ToArray();
                    return GetListWithStoreProcedure(query, clonedParameters);
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
            using (DbContext Context = new DbContextSP())
            {
                try
                {
                    Context.Database.CommandTimeout = 300;
                    return Context.Database.SqlQuery<TEntity>(query, parameters).FirstOrDefault<TEntity>();

                }
                catch (Exception ex)
                {
                    if ((retrycatch <= 2 && !string.IsNullOrEmpty(ex.Message) && (ex.Message.Contains("deadlocked on lock resources with another process") || ex.Message.Contains("Timeout")))
                        || (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message)
                        && (ex.InnerException.Message.Contains("deadlocked on lock resources with another process") || ex.InnerException.Message.Contains("Timeout"))
                        ))
                    {
                        retrycatch = retrycatch + 1;
                        var clonedParameters = parameters.Select(x => x.Clone()).ToArray();
                        Context.Dispose();
                        return GetSingleObjectWithStoreProcedure(query, clonedParameters);
                    }
                    else
                    {
                        retrycatch = 0;
                        throw ex;

                    }
                }


            }
        }
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


    public static class Extension
    {
        public static SqlParameter Clone(this object @this)
        {
            var destination = new SqlParameter();

            var copyToMethod = typeof(SqlParameter).GetMethod("CopyTo", BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { typeof(DbParameter) }, null);
            copyToMethod.Invoke(@this, new[] { destination });

            return destination;
        }
    }

}
