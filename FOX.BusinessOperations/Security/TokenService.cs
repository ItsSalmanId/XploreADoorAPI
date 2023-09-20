using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;
using FOX.BusinessOperations.CommonService;
using System.Configuration;

namespace FOX.BusinessOperations.Security
{
    public class TokenService : ITokenService
    {
        #region Private member variables.
        private readonly DbContextSecurity _SecurityContext = new DbContextSecurity();
        private readonly GenericRepository<ProfileToken> _TokenRepository;
        private readonly GenericRepository<User> _UserRepository;
        #endregion

        public TokenService()
        {
            _TokenRepository = new GenericRepository<ProfileToken>(_SecurityContext);
            _UserRepository = new GenericRepository<User>(_SecurityContext);
        }

        public bool DeleteByUserId(string token)
        {
            _TokenRepository.Delete(x => x.AuthToken == token);
            _TokenRepository.Save();
            var isNotDeleted = _TokenRepository.GetMany(x => x.AuthToken == token).Any();
            return !isNotDeleted;
        }

        public ProfileToken GenerateToken(string userName, string onlyAuthenticate)
        {
            if (!string.IsNullOrEmpty(onlyAuthenticate))
            {
                var usrParmAuth = new SqlParameter("UserName", SqlDbType.VarChar) { Value = userName };
                var UserDetailsAuth = SpRepository<UserProfile>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_USER_PROFILING_DATA @UserName", usrParmAuth).FirstOrDefault();
                var obj = new ProfileToken();
                obj.Profile = JsonConvert.SerializeObject(UserDetailsAuth).ToString();
                return obj;
            }
            try
            {
                var usrParm = new SqlParameter("UserName", SqlDbType.VarChar) { Value = userName };
                var usrName = new SqlParameter("USERNAME", SqlDbType.VarChar) { Value = userName };
                var UserDetails = SpRepository<UserProfile>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_USER_PROFILING_DATA @UserName", usrParm).FirstOrDefault();
                string token = Guid.NewGuid().ToString();
                UserDetails.Token = token;

                userName = UserDetails.UserName;
                ProfileToken tokenModel = SaveTokenWithProfile(UserDetails, token);

                //var userNameTokenp = new SqlParameter("USERNAME", SqlDbType.BigInt) { Value = UserDetails.userID };
                //var userTokenp = new SqlParameter("TOKEN", SqlDbType.VarChar) { Value = token };
                //var userProfilep = new SqlParameter("USER_PROFILE", SqlDbType.VarChar) { Value = JsonConvert.SerializeObject(UserDetails).ToString() };
                //var tokenModelp = SpRepository<ProfileToken>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GENERATE_INSERT_TOKEN @USERNAME, @TOKEN , @USER_PROFILE", userNameTokenp, userTokenp, userProfilep);

                var userEntity = SpRepository<User>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_USER @USERNAME", usrName);
                userEntity.FAILED_PASSWORD_ATTEMPT_COUNT = 0;
                userEntity.MODIFIED_BY = userEntity.USER_NAME;
                userEntity.MODIFIED_DATE = Helper.GetCurrentDate();
                _UserRepository.Update(userEntity);
                _UserRepository.Save();



                return tokenModel;
            }
            catch (Exception) {
                return new ProfileToken();
            }
        }

        public ProfileToken SaveTokenWithProfile(UserProfile profile, string token)
        {
            var userNameToken = new SqlParameter("USERNAME", SqlDbType.BigInt) { Value = profile.userID };
            var userToken = new SqlParameter("TOKEN", SqlDbType.VarChar) { Value = token };
            var userProfile = new SqlParameter("USER_PROFILE", SqlDbType.VarChar) { Value = JsonConvert.SerializeObject(profile).ToString() };

            int isLogoutValue = 0;
            int isMFAVerified = 0;
            int isValidate = 0;
            if (profile.MFA == true && profile.showMfaEanbleScreen == 1)
            {
                isLogoutValue = 1;
                isValidate = 0;
            }
            if (profile.MFA == false && profile.showMfaEanbleScreen == 0)
            {
                isValidate = 1;
            }
            var isMFAVerify = new SqlParameter("ISMFAVERIFIED", SqlDbType.BigInt) { Value = isMFAVerified };
            var islogout = new SqlParameter("ISLOGOUT", SqlDbType.BigInt) { Value = isLogoutValue };
            var validate = new SqlParameter("ISVALIDATE", SqlDbType.BigInt) { Value = isValidate };
            var tokenModel = SpRepository<ProfileToken>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GENERATE_INSERT_TOKEN @USERNAME, @TOKEN , @USER_PROFILE, @ISMFAVERIFIED, @ISLOGOUT, @ISVALIDATE", userNameToken, userToken, userProfile, isMFAVerify, islogout, validate);
            tokenModel.isLogOut = false;
            return tokenModel;

        }

        public ProfileToken UpdateToken(string userName, string token, bool isSecondCall)
        {
            try
            { 
                var usrParmAuth = new SqlParameter("UserName", SqlDbType.VarChar) { Value = userName };
                var UserDetailsAuth = SpRepository<UserProfile>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_USER_PROFILING_DATA @UserName", usrParmAuth).FirstOrDefault();
                UserDetailsAuth.Token = token;
                int islogoutValue = 1; 
                int isMFAVerified = 0;
                int isValidate = 0;
                var islogout = new SqlParameter("ISLOGOUT", SqlDbType.BigInt) { Value = islogoutValue };
                if (isSecondCall == true)
                {
                    islogoutValue = 1;
                    isMFAVerified = 1;
                    isValidate = 1;
                    islogout = new SqlParameter("ISLOGOUT", SqlDbType.BigInt) { Value = islogoutValue };
                }
                var isUserValidate = new SqlParameter("@ISVALIDATE", SqlDbType.BigInt) { Value = isValidate };
                var isMFAVerify = new SqlParameter("@ISMFAVERIFIED", SqlDbType.BigInt) { Value = isMFAVerified };
                var userNameToken = new SqlParameter("@USERNAME", SqlDbType.BigInt) { Value = UserDetailsAuth.userID };
                var userToken = new SqlParameter("@TOKEN", SqlDbType.VarChar) { Value = token };
                var userProfile = new SqlParameter("@USER_PROFILE", SqlDbType.VarChar) { Value = JsonConvert.SerializeObject(UserDetailsAuth).ToString() };
                var tokenModel = SpRepository<ProfileToken>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_UPDATE_TOKEN @ISLOGOUT, @ISVALIDATE, @ISMFAVERIFIED, @USERNAME, @TOKEN , @USER_PROFILE", islogout, isUserValidate, isMFAVerify, userNameToken, userToken, userProfile);

                return tokenModel;
            }
            catch (Exception)
            {
                return new ProfileToken();
            }
        }

        public ProfileToken GetProfile(string token)
        {
           return _TokenRepository.Get(t => t.AuthToken == token);
        }

        public bool Kill(string tokenId)
        {
            _TokenRepository.Delete(x => x.AuthToken == tokenId);
            _TokenRepository.Save();
            var isNotDeleted = _TokenRepository.GetMany(x => x.AuthToken == tokenId).Any();
            if (isNotDeleted) { return false; }
            return true;
        }

        public bool ValidateToken(string tokenId, string serverId)
        {
            string connectionString = Helper.getConnectionString();
            var conn = ConfigurationManager.ConnectionStrings[connectionString].ConnectionString;
            var token = "";
            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "FOX_PROC_VALIDATE_UPDATE_TOKEN";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TOKEN_VALUE", tokenId);
                cmd.Connection = connection;
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    token = reader["TOKEN_VALID"].ToString();
                }

            }

            if (token == "1")
            {
                return true;
            }
            else
            {
                var tokenExpire = _TokenRepository.Get(t => t.AuthToken == tokenId);
                if (tokenExpire != null)
                {
                    var UserName = new SqlParameter("USERNAME", SqlDbType.VarChar) { Value = tokenExpire.userProfile.UserName };
                    var userEntity = SpRepository<User>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_USER @USERNAME", UserName);
                    userEntity.MODIFIED_BY = tokenExpire.userProfile.UserName;
                    userEntity.MODIFIED_DATE = Helper.GetCurrentDate();

                    _UserRepository.Update(userEntity);
                    _UserRepository.Save();

                    //for update on authentication server
                    _UserRepository.Update(userEntity);
                    _UserRepository.Save();
                }


            }
            return false;
        }
    }
}