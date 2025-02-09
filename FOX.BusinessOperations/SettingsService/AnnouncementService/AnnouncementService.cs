﻿using FOX.BusinessOperations.CommonService;
using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.Announcement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace FOX.BusinessOperations.SettingsService.AnnouncementService
{
    public class AnnouncementService : IAnnouncementService
    {
        #region PROPERTIES
        private readonly DbContextSettings _settingsContext = new DbContextSettings();
        private readonly GenericRepository<FoxRoles> _foxRolesRepository;
        private readonly GenericRepository<Announcements> _announcementRepository;
        private readonly GenericRepository<AnnouncementRoles> _announcementRoleRepository;
        #endregion

        #region CONSTRUCTOR
        public AnnouncementService()
        {
            _foxRolesRepository = new GenericRepository<FoxRoles>(_settingsContext);
            _announcementRepository = new GenericRepository<Announcements>(_settingsContext);
            _announcementRoleRepository = new GenericRepository<AnnouncementRoles>(_settingsContext);
        }
        #endregion

        #region FUNCTIONS
        // Description: This function is trigger to get role names
        public List<FoxRoles> GetFoxRoles(UserProfile userProfile)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            List<FoxRoles> result = new List<FoxRoles>();
            if (userProfile != null && userProfile.PracticeCode != 0)
            {
                result = _foxRolesRepository.GetMany(r => (r.PRACTICE_CODE == null || r.PRACTICE_CODE == userProfile.PracticeCode) && !r.DELETED);
                foreach (var item in result)
                {
                    item.ROLE_NAME = textInfo.ToTitleCase(item.ROLE_NAME.ToLower());
                }
            }
            return result;
        }
        // Description: This function is trigger to add and update announcement and roles to db
        public ResponseModel InsertAnnouncement(AddEditFoxAnnouncement objAddEditAnnouncement, UserProfile profile)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                if (objAddEditAnnouncement != null && objAddEditAnnouncement.RoleRequest.Count != 0 && !string.IsNullOrEmpty(objAddEditAnnouncement.ANNOUNCEMENT_TITLE) && !string.IsNullOrEmpty(objAddEditAnnouncement.ANNOUNCEMENT_DETAILS) && objAddEditAnnouncement.ANNOUNCEMENT_DATE_TO != null && objAddEditAnnouncement.ANNOUNCEMENT_DATE_FROM != null && profile.PracticeCode != 0 && profile != null && profile.UserName != null)
                {
                    if (!string.IsNullOrEmpty(objAddEditAnnouncement.ANNOUNCEMENT_DATE_FROM_STR))
                    {
                        objAddEditAnnouncement.ANNOUNCEMENT_DATE_FROM = Convert.ToDateTime(objAddEditAnnouncement.ANNOUNCEMENT_DATE_FROM_STR);
                    }
                    if (!string.IsNullOrEmpty(objAddEditAnnouncement.ANNOUNCEMENT_DATE_TO_STR))
                    {
                        objAddEditAnnouncement.ANNOUNCEMENT_DATE_TO = Convert.ToDateTime(objAddEditAnnouncement.ANNOUNCEMENT_DATE_TO_STR);
                    }
                    DateTime today = (DateTime)objAddEditAnnouncement.ANNOUNCEMENT_DATE_TO;
                    double totalDays = today.Subtract((DateTime)objAddEditAnnouncement.ANNOUNCEMENT_DATE_FROM).TotalDays + 1;
                    if (totalDays <= 10)
                    {
                        if (!string.IsNullOrEmpty(objAddEditAnnouncement.ANNOUNCEMENT_DETAILS.ToString()))
                        {
                            var existingDetailInfo = _announcementRepository.GetFirst(r => r.ANNOUNCEMENT_ID == objAddEditAnnouncement.ANNOUNCEMENT_ID && r.DELETED == false);
                            if (existingDetailInfo == null)
                            {
                                SqlParameter anouncementDateFrom = new SqlParameter("ANNOUNCEMENT_DATE_FROM", Convert.ToDateTime(objAddEditAnnouncement.ANNOUNCEMENT_DATE_FROM));
                                SqlParameter anouncementDateTo = new SqlParameter("ANNOUNCEMENT_DATE_TO", objAddEditAnnouncement.ANNOUNCEMENT_DATE_TO);
                                SqlParameter rolesIds = new SqlParameter("ROLES_IDs", objAddEditAnnouncement.EditSelectedRolesID.Substring(1));
                                var duplicationCheckInAnnouncment = SpRepository<Announcements>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_CHECK_ANNOUNCEMENT_DUPLICATION @ANNOUNCEMENT_DATE_FROM, @ANNOUNCEMENT_DATE_TO, @ROLES_IDs", anouncementDateFrom, anouncementDateTo, rolesIds);
                                if (duplicationCheckInAnnouncment == null)
                                {
                                    for (int i = 0; i < totalDays; i++)
                                    {
                                        DateTime announcementDateFromUpdated = (DateTime)objAddEditAnnouncement.ANNOUNCEMENT_DATE_FROM;
                                        announcementDateFromUpdated = announcementDateFromUpdated.AddDays(i);
                                        long primaryKey = Helper.getMaximumId("ANNOUNCEMENT_ID");
                                        SqlParameter announcementId = new SqlParameter("ANNOUNCEMENT_ID", primaryKey);
                                        SqlParameter announcementDetails = new SqlParameter("ANNOUNCEMENT_DETAILS", objAddEditAnnouncement.ANNOUNCEMENT_DETAILS.ToString() ?? (object)DBNull.Value);
                                        SqlParameter announcementTitle = new SqlParameter("ANNOUNCEMENT_TITLE", objAddEditAnnouncement.ANNOUNCEMENT_TITLE.ToString() ?? (object)DBNull.Value);
                                        SqlParameter announcementDateFrom = new SqlParameter("ANNOUNCEMENT_DATE_FROM", Convert.ToDateTime(announcementDateFromUpdated));
                                        SqlParameter announcementDateTo = new SqlParameter("ANNOUNCEMENT_DATE_TO", announcementDateFromUpdated);
                                        SqlParameter modifiedDate = new SqlParameter("MODIFIED_DATE", Helper.GetCurrentDate());
                                        SqlParameter modifiedBy = new SqlParameter("MODIFIED_BY", profile.UserName);
                                        SqlParameter practiceCode = new SqlParameter("PRACTICE_CODE", profile.PracticeCode);
                                        SqlParameter deleted = new SqlParameter("DELETED", false);
                                        SqlParameter createdBy = new SqlParameter("CREATED_BY", profile.UserName);
                                        SqlParameter operaton = new SqlParameter("OPERATION", "ADD");
                                        SpRepository<Announcements>.GetListWithStoreProcedure(@"exec FOX_PROC_INSERT_ANNOUNCEMENT @ANNOUNCEMENT_ID, @ANNOUNCEMENT_DETAILS, @ANNOUNCEMENT_TITLE, @ANNOUNCEMENT_DATE_FROM, @ANNOUNCEMENT_DATE_TO, @MODIFIED_DATE, @MODIFIED_BY, @PRACTICE_CODE, @DELETED, @CREATED_BY, @OPERATION", announcementId, announcementDetails, announcementTitle, announcementDateFrom, announcementDateTo, modifiedDate, modifiedBy, practiceCode, deleted, createdBy, operaton);
                                        foreach (var item in objAddEditAnnouncement.RoleRequest)
                                        {
                                            long primaryKeyRole = Helper.getMaximumId("ANNOUNCEMENT_ROLE_ID");
                                            SqlParameter announcementRoleId = new SqlParameter("ANNOUNCEMENT_ROLE_ID", primaryKeyRole);
                                            SqlParameter roleId = new SqlParameter("ROLE_ID", item.ROLE_ID.ToString() ?? (object)DBNull.Value);
                                            SqlParameter roleName = new SqlParameter("ROLE_NAME", item.ROLE_NAME.ToString() ?? (object)DBNull.Value);
                                            SqlParameter announcementIdRole = new SqlParameter("ANNOUNCEMENT_ID", primaryKey);
                                            SqlParameter roleModifiedDate = new SqlParameter("MODIFIED_DATE", Helper.GetCurrentDate());
                                            SqlParameter rolePracticeCode = new SqlParameter("PRACTICE_CODE", profile.PracticeCode);
                                            SpRepository<AnnouncementRoles>.GetListWithStoreProcedure(@"exec FOX_PROC_INSERT_ANNOUNCEMENT_ROLE @ANNOUNCEMENT_ROLE_ID, @ROLE_ID, @ROLE_NAME, @ANNOUNCEMENT_ID, @PRACTICE_CODE", announcementRoleId, roleId, roleName, announcementIdRole, rolePracticeCode);
                                        }
                                    }
                                    response.ErrorMessage = "Announcement added successfully.";
                                    response.Success = true;
                                }
                                else
                                {
                                    response.ErrorMessage = "Can't add more than one announcement for a role in a same day.";
                                    response.Success = false;
                                }
                            }
                            else
                            {
                                SqlParameter announcementId = new SqlParameter("ANNOUNCEMENT_ID", objAddEditAnnouncement.ANNOUNCEMENT_ID);
                                SqlParameter announcementDetails = new SqlParameter("ANNOUNCEMENT_DETAILS", objAddEditAnnouncement.ANNOUNCEMENT_DETAILS.ToString() ?? (object)DBNull.Value);
                                SqlParameter announcementTitle = new SqlParameter("ANNOUNCEMENT_TITLE", objAddEditAnnouncement.ANNOUNCEMENT_TITLE.ToString() ?? (object)DBNull.Value);
                                SqlParameter announcementDateFrom = new SqlParameter("ANNOUNCEMENT_DATE_FROM", Convert.ToDateTime(objAddEditAnnouncement.ANNOUNCEMENT_DATE_FROM));
                                SqlParameter announcementDateTo = new SqlParameter("ANNOUNCEMENT_DATE_TO", objAddEditAnnouncement.ANNOUNCEMENT_DATE_TO);
                                SqlParameter modifiedDate = new SqlParameter("MODIFIED_DATE", Helper.GetCurrentDate());
                                SqlParameter modifiedBy = new SqlParameter("MODIFIED_BY", profile.UserName);
                                SqlParameter practiceCode = new SqlParameter("PRACTICE_CODE", profile.PracticeCode);
                                SqlParameter deleted = new SqlParameter("DELETED", false);
                                SqlParameter createdBy = new SqlParameter("CREATED_BY", profile.UserName);
                                SqlParameter operaton = new SqlParameter("OPERATION", "UPDATE");
                                SpRepository<Announcements>.GetListWithStoreProcedure(@"exec FOX_PROC_INSERT_ANNOUNCEMENT @ANNOUNCEMENT_ID, @ANNOUNCEMENT_DETAILS, @ANNOUNCEMENT_TITLE, @ANNOUNCEMENT_DATE_FROM, @ANNOUNCEMENT_DATE_TO, @MODIFIED_DATE, @MODIFIED_BY, @PRACTICE_CODE, @DELETED, @CREATED_BY, @OPERATION", announcementId, announcementDetails, announcementTitle, announcementDateFrom, announcementDateTo, modifiedDate, modifiedBy, practiceCode, deleted, createdBy, operaton);
                                foreach (var item in objAddEditAnnouncement.RoleRequest)
                                {
                                    long primaryKeyRole = Helper.getMaximumId("ANNOUNCEMENT_ROLE_ID");
                                    SqlParameter announcementRoleId = new SqlParameter("ANNOUNCEMENT_ROLE_ID", primaryKeyRole);
                                    SqlParameter roleId = new SqlParameter("ROLE_ID", item.ROLE_ID.ToString() ?? (object)DBNull.Value);
                                    SqlParameter roleName = new SqlParameter("ROLE_NAME", item.ROLE_NAME.ToString() ?? (object)DBNull.Value);
                                    SqlParameter announcementIdRole = new SqlParameter("ANNOUNCEMENT_ID", objAddEditAnnouncement.ANNOUNCEMENT_ID);
                                    SqlParameter roleModifiedDate = new SqlParameter("MODIFIED_DATE", Helper.GetCurrentDate());
                                    SqlParameter rolePracticeCode = new SqlParameter("PRACTICE_CODE", profile.PracticeCode);
                                    SpRepository<AnnouncementRoles>.GetListWithStoreProcedure(@"exec FOX_PROC_INSERT_ANNOUNCEMENT_ROLE @ANNOUNCEMENT_ROLE_ID, @ROLE_ID, @ROLE_NAME, @ANNOUNCEMENT_ID , @PRACTICE_CODE", announcementRoleId, roleId, roleName, announcementIdRole, rolePracticeCode);
                                }
                                response.ErrorMessage = "Announcement updated  successfully.";
                                response.Success = true;
                            }
                        }
                        else
                        {
                            response.ErrorMessage = "Record not inserted";
                            response.Success = false;
                        }
                    }
                    else
                    {
                        response.ErrorMessage = "Can't add announcement for more than 10 days.";
                        response.Success = false;
                    }
                }
                else
                {
                    response.ErrorMessage = "Fill mandatory announcment field";
                    response.Success = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }
        // Description: This function is trigger to get announcement and roles details to db
        public List<Announcements> GetAnnouncement(Announcements objAnnouncement, UserProfile profile)
        {
            List<Announcements> announcementsList = new List<Announcements>();
            if (objAnnouncement != null && profile.PracticeCode != 0 && profile != null)
            {
                if (!string.IsNullOrEmpty(objAnnouncement.ANNOUNCEMENT_DATE_FROM_STR))
                {
                    objAnnouncement.ANNOUNCEMENT_DATE_FROM = Convert.ToDateTime(objAnnouncement.ANNOUNCEMENT_DATE_FROM_STR);
                }
                if (!string.IsNullOrEmpty(objAnnouncement.ANNOUNCEMENT_DATE_TO_STR))
                {
                    objAnnouncement.ANNOUNCEMENT_DATE_TO = Convert.ToDateTime(objAnnouncement.ANNOUNCEMENT_DATE_TO_STR);
                }
                else
                {
                    objAnnouncement.ANNOUNCEMENT_DATE_TO = null;
                }
                if (objAnnouncement != null && profile.PracticeCode != 0)
                {
                    var practiceCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
                    SqlParameter announcementsDateFrom = new SqlParameter("ANNOUNCEMENT_DATE_FROM", objAnnouncement.ANNOUNCEMENT_DATE_FROM ?? (object)DBNull.Value);
                    SqlParameter announcementsDateTo = new SqlParameter("ANNOUNCEMENT_DATE_TO", objAnnouncement.ANNOUNCEMENT_DATE_TO ?? (object)DBNull.Value);
                    SqlParameter roleId = new SqlParameter { ParameterName = "ROLE_ID", SqlDbType = SqlDbType.VarChar, Value = objAnnouncement.ROLE_ID ?? (object)DBNull.Value };
                    SqlParameter announcementTitle = new SqlParameter { ParameterName = "ANNOUNCEMENT_TITLE", SqlDbType = SqlDbType.VarChar, Value = objAnnouncement.ANNOUNCEMENT_TITLE == null ? null : objAnnouncement.ANNOUNCEMENT_TITLE };
                    announcementsList = SpRepository<Announcements>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_ANNOUNCEMENT_DETAILS @PRACTICE_CODE, @ANNOUNCEMENT_DATE_FROM, @ANNOUNCEMENT_DATE_TO, @ROLE_ID, @ANNOUNCEMENT_TITLE", practiceCode, announcementsDateFrom, announcementsDateTo, roleId, announcementTitle);
                }
            }
            return announcementsList;
        }
        // Description: This function is trigger to get announcement details to db
        public Announcements GetAnnouncementDetails(Announcements objAnnouncement, UserProfile profile)
        {
            Announcements announcementsList = new Announcements();
            if (objAnnouncement != null && profile.PracticeCode != 0 && profile != null && !string.IsNullOrEmpty(objAnnouncement.ANNOUNCEMENT_ID.ToString()))
            {
                if (objAnnouncement.ANNOUNCEMENT_DATE_FROM != null)
                {
                    objAnnouncement.ANNOUNCEMENT_DATE_FROM = Convert.ToDateTime(objAnnouncement.ANNOUNCEMENT_DATE_FROM);
                }
                if (objAnnouncement.ANNOUNCEMENT_DATE_TO != null)
                {
                    objAnnouncement.ANNOUNCEMENT_DATE_TO = Convert.ToDateTime(objAnnouncement.ANNOUNCEMENT_DATE_TO);
                }
                var practiceCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
                var accouncmentId = new SqlParameter { ParameterName = "ANNOUNCEMENT_ID", SqlDbType = SqlDbType.BigInt, Value = objAnnouncement.ANNOUNCEMENT_ID };
                announcementsList = SpRepository<Announcements>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_ANNOUNCEMENT_DETAILS_FOR_UPDATE  @PRACTICE_CODE, @ANNOUNCEMENT_ID", practiceCode, accouncmentId);
                if (announcementsList != null)
                {
                    announcementsList.AnnouncementRoles = _announcementRoleRepository.GetMany(x => x.ANNOUNCEMENT_ID == announcementsList.ANNOUNCEMENT_ID && !(x.DELETED) && x.PRACTICE_CODE == profile.PracticeCode);
                }
            }
            return announcementsList;
        }
        // Description: This function is trigger to delete announcement and announcementroles to db
        public ResponseModel DeleteAnnouncement(Announcements objAnnouncement, UserProfile profile)
        {
            ResponseModel response = new ResponseModel();
            if (objAnnouncement != null && profile != null && profile.PracticeCode != 0 && !string.IsNullOrEmpty(objAnnouncement.ANNOUNCEMENT_ID.ToString()))
            {
                var existingAnnouncmentDetailInfo = _announcementRepository.GetFirst(r => r.ANNOUNCEMENT_ID == objAnnouncement.ANNOUNCEMENT_ID && r.DELETED == false);
                var existingRolesDetailInfo = _announcementRoleRepository.GetMany(r => r.ANNOUNCEMENT_ID == objAnnouncement.ANNOUNCEMENT_ID && r.DELETED == false);

                if (existingAnnouncmentDetailInfo != null && existingRolesDetailInfo != null)
                {
                    existingAnnouncmentDetailInfo.MODIFIED_BY = profile.UserName;
                    existingAnnouncmentDetailInfo.MODIFIED_DATE = Helper.GetCurrentDate();
                    existingAnnouncmentDetailInfo.DELETED = true;
                    _announcementRepository.Update(existingAnnouncmentDetailInfo);
                    _announcementRepository.Save();
                    foreach (var item in existingRolesDetailInfo)
                    {
                        item.MODIFIED_BY = profile.UserName;
                        item.MODIFIED_DATE = Helper.GetCurrentDate();
                        item.DELETED = true;
                        _announcementRoleRepository.Update(item);
                        _announcementRoleRepository.Save();
                    }
                    response.ErrorMessage = "Announcement deleted successfully.";
                    response.Success = true;
                }
            }
            return response;
        }
        #endregion
    }
}
