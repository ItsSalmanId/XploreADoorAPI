using FOX.DataModels.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOX.BusinessOperations.Security
{
    public interface ITokenService
    {
        #region Interface member methods.
        /// <summary>
        ///  Function to generate unique token with expiry against the provided userId.
        ///  Also add a record in database for generated token.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        ProfileToken GenerateToken(string userName, string onlyAuthenticate);
        /// <summary>
        /// Function to Update Profile Againts Logged-In User By Token.
        /// </summary>
        /// <param name="tokenId"></param>
        /// <returns></returns>
        ProfileToken UpdateToken(string userName, string token);
        /// <summary>
        /// Function to validate token againt expiry and existance in database.
        /// </summary>
        /// <param name="tokenId"></param>
        /// <returns></returns>
        bool ValidateToken(string tokenId, string serverId);

        /// <summary>
        /// Method to kill the provided token id.
        /// </summary>
        /// <param name="tokenId"></param>
        bool Kill(string tokenId);

        /// <summary>
        /// Delete tokens for the specific deleted user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        bool DeleteByUserId(string token);
        #endregion

        /// <summary>
        /// Get User Profile by token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        ProfileToken GetProfile(string token);

        ProfileToken SaveTokenWithProfile(UserProfile UserDetails, string token);
    }
}
