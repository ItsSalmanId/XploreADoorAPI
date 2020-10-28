using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.GroupsModel;
using FOX.DataModels.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOX.BusinessOperations.GroupServices
{
    public interface IGroupService
    {
        ResponseModel AddUpdateGroup(GROUP group, UserProfile profile);
        List<GROUP> GetGroupsList(UserProfile userProfile);
        ResponseModel DeleteGroup(GROUP group, UserProfile userProfile);
        List<UserWithRoles> GetUsersWithTheirRole(UserProfile userProfile);
        ResponseModel AddUsersInGroup(GroupUsersCreateViewModel model, UserProfile profile);
        UsersAndGroupUsersViewModel GetGroupUsersByGroupId(long groupId, UserProfile profile);

    }
}
