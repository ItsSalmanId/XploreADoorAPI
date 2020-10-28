using FOX.BusinessOperations.CommonService;
using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FOX.BusinessOperations.Security
{
    public class UserService : IUserServices
    {
        private readonly DbContextSecurity _UserContext = new DbContextSecurity();
        private readonly GenericRepository<User> _UserRepository;
        public UserService()
        {
            _UserRepository = new GenericRepository<User>(_UserContext);
        }
        /// <summary>
        /// Public method to authenticate user by user name and password.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        //  private readonly DbContextSecurity _UserContext=new DbContextSecurity();
        //  private readonly GenericRepository<User> _UserRepository;
        public string Authenticate(string user_name, string password)
        {
            password = System.Net.WebUtility.HtmlDecode(password);
            string EncPass = Encrypt.getEncryptedCode(password);
            // var user = _UserRepository.Get(u => u.user_name.Equals(user_name,StringComparison.OrdinalIgnoreCase) && u.password.Equals(EncPass));
            var userName = new SqlParameter("USERNAME", SqlDbType.VarChar) { Value = user_name };
            var Pass = new SqlParameter("PASSWORD", SqlDbType.VarChar) { Value = EncPass };
            var user = new User();
            try
            {
                user = SpRepository<User>.GetSingleObjectWithStoreProcedure(@"exec [FOX_PROC_AUTHENTICATE_USER] @USERNAME,@PASSWORD", userName, Pass);

                if (user.STATUS == 200) // VALID USER
                {
                    var UserName = new SqlParameter("USERNAME", SqlDbType.VarChar) { Value = user_name };
                    var userEntity = SpRepository<User>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_USER @USERNAME", UserName);
                    //var userEntity = _UserRepository.Get(x => x.USER_NAME.Equals(user_name));

                    if (userEntity != null)
                    {
                        userEntity.FAILED_PASSWORD_ATTEMPT_COUNT = 0;
                        userEntity.MODIFIED_BY = userEntity.USER_NAME;
                        userEntity.MODIFIED_DATE = Helper.GetCurrentDate();
                        _UserRepository.Update(userEntity);
                        try
                        {
                            _UserRepository.Save();
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                    return user.USER_NAME;
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return "";
        }

        public User GetUser(string _email)
        {
            var user = _UserRepository.GetSingle(u => u.EMAIL.Equals(_email, StringComparison.OrdinalIgnoreCase));

            if (user != null) // User Exists
            {
                return user;
            }

            return null;
        }

        public bool GenerateAndInsertCode(long _userId)
        {
            //var user = _UserRepository.GetSingle(u => u.EMAIL.Equals(_email, StringComparison.OrdinalIgnoreCase));

            //if (user != null) // User Exists
            //{
            //    return user;
            //}

            return false;
        }
    }
}