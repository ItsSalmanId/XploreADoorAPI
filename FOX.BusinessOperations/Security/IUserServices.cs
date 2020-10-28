using FOX.DataModels.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOX.BusinessOperations.Security
{
    public interface IUserServices
    {
        string Authenticate(string userName, string password);
        User GetUser(string _email);
        bool GenerateAndInsertCode(long _userId);
        
    }
}
