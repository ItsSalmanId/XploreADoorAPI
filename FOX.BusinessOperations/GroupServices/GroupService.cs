using FOX.DataModels.Models.GroupsModel;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Context;
using FOX.DataModels.Models.Security;
using FOX.BusinessOperations.CommonService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace FOX.BusinessOperations.GroupServices
{
    public class GroupService : IGroupService
    {
        private readonly GenericRepository<GROUP> _groupRepository;
        private readonly GenericRepository<USERS_GROUP> _userGroupRepository;
        private DbContextSettings _settingsDbContext = new DbContextSettings();

        public GroupService()
        {
            _groupRepository = new GenericRepository<GROUP>(_settingsDbContext);
            _userGroupRepository = new GenericRepository<USERS_GROUP>(_settingsDbContext);
        }

        public ResponseModel AddUpdateGroup(GROUP model, UserProfile profile)
        {
            GROUP group = new GROUP();
            try
            {
                group = _groupRepository.GetFirst(m => m.GROUP_ID == model.GROUP_ID);
                if (group != null)
                {
                    group.MODIFIED_DATE = Helper.GetCurrentDate();
                    group.MODIFIED_BY = profile.UserName;
                    group.GROUP_NAME = model.GROUP_NAME;
                    group.CREATED_BY = model.CREATED_BY;
                    group.CREATED_DATE = model.CREATED_DATE;
                    _groupRepository.Update(group);
                }
                else
                {
                    group = model;
                    group.GROUP_ID = Helper.getMaximumId("GROUP_ID");
                    group.CREATED_BY = profile.UserName;
                    group.CREATED_DATE = Helper.GetCurrentDate();
                    group.MODIFIED_BY = profile.UserName;
                    group.MODIFIED_DATE = Helper.GetCurrentDate();
                    group.PRACTICE_CODE = profile.PracticeCode;
                    _groupRepository.Insert(group);
                }
                _groupRepository.Save();
                return new ResponseModel()
                {
                    ID = group.GROUP_ID.ToString(),
                    ErrorMessage = "",
                    Message = "Success",
                    Success = true
                };
            }
            catch (System.Exception ex)
            {
                return new ResponseModel()
                {
                    ErrorMessage = ex.ToString(),
                    Message = ex.ToString(),
                    Success = false
                };
            }
        }

        public List<GROUP> GetGroupsList(UserProfile userProfile)
        {
            try
            {
                return _groupRepository.GetMany(t => t.PRACTICE_CODE == userProfile.PracticeCode && !t.DELETED).OrderByDescending(g=>g.CREATED_DATE).ToList();
            }
            catch (Exception)
            {
                return new List<GROUP>();
            }
        }

        public ResponseModel DeleteGroup(GROUP group, UserProfile userProfile)
        {
            try
            {
                var groupToDelete = _groupRepository.GetFirst(t => t.GROUP_ID == group.GROUP_ID && t.PRACTICE_CODE == userProfile.PracticeCode && !t.DELETED);
                if (groupToDelete != null)
                {
                    groupToDelete.MODIFIED_BY = userProfile.UserName;
                    groupToDelete.MODIFIED_DATE = Helper.GetCurrentDate();
                    groupToDelete.DELETED = true;
                    try
                    {
                        _groupRepository.Update(groupToDelete);
                        _groupRepository.Save();
                        var groupMembers = _userGroupRepository.GetMany(t => t.GROUP_ID == group.GROUP_ID && t.PRACTICE_CODE == group.PRACTICE_CODE && !t.DELETED);
                        foreach (var member in groupMembers)
                        {
                            member.MODIFIED_BY = userProfile.UserName;
                            member.MODIFIED_DATE = Helper.GetCurrentDate();
                            member.GROUP_ID = 0;
                            member.USER_NAME = "";
                            member.DELETED = true;
                            _userGroupRepository.Update(member);
                        }
                        _userGroupRepository.Save();
                    }
                    catch (Exception ex)
                    {
                        return new ResponseModel()
                        {
                            ErrorMessage = ex.ToString(),
                            ID = group.GROUP_ID + "",
                            Message = ex.ToString(),
                            Success = false
                        };

                    }
                    return new ResponseModel()
                    {
                        ErrorMessage = "",
                        ID = groupToDelete.GROUP_ID + "",
                        Message = "Group deleted successfully.",
                        Success = true

                    };
                }
                else
                {
                    return new ResponseModel()
                    {
                        ErrorMessage = "",
                        ID = groupToDelete.GROUP_ID + "",
                        Message = "Group deleted successfully.",
                        Success = true

                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseModel()
                {
                    ErrorMessage = ex.ToString(),
                    ID = group.GROUP_ID + "",
                    Message = ex.ToString(),
                    Success = false
                };
            }
        }

        public List<UserWithRoles> GetUsersWithTheirRole(UserProfile userProfile)
        {
            try
            {
                var paramPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt)
                {
                    Value = userProfile.PracticeCode
                };
                return SpRepository<UserWithRoles>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_USERS_WITH_THEIR_ROLE @PRACTICE_CODE", paramPracticeCode);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ResponseModel AddUsersInGroup(GroupUsersCreateViewModel groupUsersCreateViewModel, UserProfile userProfile)
        {

            try
            {
                if (groupUsersCreateViewModel.USERS.Length > 0)
                {
                    var groupId = groupUsersCreateViewModel.USERS[0].GROUP_ID;
                    foreach (var user in _userGroupRepository.GetMany(g => g.GROUP_ID == groupId && g.PRACTICE_CODE == userProfile.PracticeCode && !g.DELETED))
                    {
                        user.DELETED = true;
                        user.MODIFIED_BY = userProfile.UserName;
                        user.MODIFIED_DATE = Helper.GetCurrentDate();
                        _userGroupRepository.Update(user);
                    }
                    _userGroupRepository.Save();
                }
                else
                {
                    return new ResponseModel()
                    {
                        ErrorMessage = "Add at least one user in this group.",
                        ID = "",
                        Message = "Add at least one user in this group.",
                        Success = false
                    };
                }
                foreach (var user in groupUsersCreateViewModel.USERS)
                {
                    var userToCreateOrUpdate = _userGroupRepository.GetFirst(u => u.USER_NAME == user.USER_NAME && u.GROUP_ID == user.GROUP_ID);
                    if (userToCreateOrUpdate != null)
                    {
                        if (userToCreateOrUpdate.DELETED)
                        {
                            userToCreateOrUpdate.DELETED = false;
                            userToCreateOrUpdate.MODIFIED_BY = userProfile.UserName;
                            userToCreateOrUpdate.MODIFIED_DATE = Helper.GetCurrentDate();
                            _userGroupRepository.Update(userToCreateOrUpdate);
                        }
                    }
                    else
                    {
                        _userGroupRepository.Insert(new USERS_GROUP()
                        {
                            GROUP_USER_ID = Helper.getMaximumId("GROUP_USER_ID"),
                            GROUP_ID = (long)user.GROUP_ID,
                            USER_NAME = user.USER_NAME,
                            PRACTICE_CODE = userProfile.PracticeCode,
                            CREATED_BY = userProfile.UserName,
                            CREATED_DATE = Helper.GetCurrentDate(),
                            MODIFIED_BY = userProfile.UserName,
                            MODIFIED_DATE = Helper.GetCurrentDate(),
                            DELETED = false
                        });
                    }
                }
                _userGroupRepository.Save();
                return new ResponseModel()
                {
                    ErrorMessage = "",
                    ID = "",
                    Message = "Success",
                    Success = true
                };
            }
            catch (Exception ex)
            {

                return new ResponseModel()
                {
                    ErrorMessage = ex.ToString(),
                    ID = "",
                    Message = ex.ToString(),
                    Success = false
                };
            }
        }

        public UsersAndGroupUsersViewModel GetGroupUsersByGroupId(long groupId, UserProfile profile)
        {
            try
            {
                var paramPracticeCode = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt)
                {
                    Value = profile.PracticeCode
                };
                var paramPracticeCode1 = new SqlParameter("PRACTICE_CODE", SqlDbType.BigInt)
                {
                    Value = profile.PracticeCode
                };
                var paramGroupId = new SqlParameter("GROUP_ID", SqlDbType.BigInt)
                {
                    Value = groupId
                };
                var results = new UsersAndGroupUsersViewModel()
                {
                    AllUsers = SpRepository<UserWithRoles>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_USERS_WITH_THEIR_ROLE @PRACTICE_CODE", paramPracticeCode),
                    GroupUsers = SpRepository<UserWithRoles>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_USERS_BY_GROUP_ID @PRACTICE_CODE, @GROUP_ID", paramPracticeCode1, paramGroupId)
                };
                foreach (var a in results.GroupUsers)
                {
                    results.AllUsers.RemoveAt(results.AllUsers.FindIndex(u => u.USER_NAME == a.USER_NAME));
                }
                return results;
            }
            catch (Exception ex)
            {
                return new UsersAndGroupUsersViewModel();
            }
        }
    }
}
