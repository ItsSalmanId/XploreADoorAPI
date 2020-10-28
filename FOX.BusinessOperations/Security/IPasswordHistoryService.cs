using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.User;

namespace FOX.BusinessOperations.Security
{
    public interface IPasswordHistoryService
    {
        bool CheckIfPasswordIsPreviouslyUser(PreviousPasswordCheck password, UserProfile profile);
    }
}
