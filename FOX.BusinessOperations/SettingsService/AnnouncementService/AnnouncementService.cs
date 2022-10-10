using FOX.BusinessOperations.CommonService;
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
        private readonly DbContextSettings _settingsContext = new DbContextSettings();
        private readonly GenericRepository<FoxRoles> _foxRolesRepository;
        private readonly GenericRepository<Announcements> _announcementRepository;
        private readonly GenericRepository<AnnouncementRoles> _announcementRoleRepository;
        public AnnouncementService()
        {
            _foxRolesRepository = new GenericRepository<FoxRoles>(_settingsContext);
            _announcementRepository = new GenericRepository<Announcements>(_settingsContext);
            _announcementRoleRepository = new GenericRepository<AnnouncementRoles>(_settingsContext);
        }
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
        // Description: 
        public ResponseModel InsertAnnouncement(AddEditFoxAnnouncement objAddEditAnnouncement, UserProfile profile)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                
                if (objAddEditAnnouncement != null && objAddEditAnnouncement.RoleRequest.Count != 0 && !string.IsNullOrEmpty(objAddEditAnnouncement.ANNOUNCEMENT_TITLE) && !string.IsNullOrEmpty(objAddEditAnnouncement.ANNOUNCEMENT_DETAILS) 
                    && objAddEditAnnouncement.ANNOUNCEMENT_DATE_TO != null && objAddEditAnnouncement.ANNOUNCEMENT_DATE_FROM != null)
                {
                    if (!string.IsNullOrEmpty(objAddEditAnnouncement.ANNOUNCEMENT_DATE_FROM_STR))
                    {
                        objAddEditAnnouncement.ANNOUNCEMENT_DATE_FROM = Convert.ToDateTime(objAddEditAnnouncement.ANNOUNCEMENT_DATE_FROM_STR);
                    }
                    if (!string.IsNullOrEmpty(objAddEditAnnouncement.ANNOUNCEMENT_DATE_TO_STR))
                    {
                        objAddEditAnnouncement.ANNOUNCEMENT_DATE_TO = Convert.ToDateTime(objAddEditAnnouncement.ANNOUNCEMENT_DATE_TO_STR);
                    }
                    DateTime Today = (DateTime)objAddEditAnnouncement.ANNOUNCEMENT_DATE_TO;
                    double TotalDays = Today.Subtract((DateTime)objAddEditAnnouncement.ANNOUNCEMENT_DATE_FROM).TotalDays + 1;
                    if (TotalDays <= 10)
                    {


                        if (!string.IsNullOrEmpty(objAddEditAnnouncement.ANNOUNCEMENT_DETAILS.ToString()))
                        {
                            var ExistingDetailInfo = _announcementRepository.GetFirst(r => r.ANNOUNCEMENT_ID == objAddEditAnnouncement.ANNOUNCEMENT_ID && r.DELETED == false);
                            if (ExistingDetailInfo == null)
                            {
                                var DuplicationCheckInAnnouncment = _announcementRepository.GetFirst(r => r.ANNOUNCEMENT_DATE_FROM == objAddEditAnnouncement.ANNOUNCEMENT_DATE_FROM && r.ANNOUNCEMENT_DATE_TO <= objAddEditAnnouncement.ANNOUNCEMENT_DATE_TO && r.DELETED == false);
                                if (DuplicationCheckInAnnouncment == null)
                                {
                                    for (int i = 0; i < TotalDays; i++)
                                    {
                                        DateTime AnnouncementDateFromUpdated = (DateTime)objAddEditAnnouncement.ANNOUNCEMENT_DATE_FROM;
                                        AnnouncementDateFromUpdated = AnnouncementDateFromUpdated.AddDays(i);
                                        long primaryKey = Helper.getMaximumId("ANNOUNCEMENT_ID");
                                        SqlParameter AnnouncementId = new SqlParameter("ANNOUNCEMENT_ID", primaryKey);
                                        SqlParameter AnnouncementDetails = new SqlParameter("ANNOUNCEMENT_DETAILS", objAddEditAnnouncement.ANNOUNCEMENT_DETAILS.ToString() ?? (object)DBNull.Value);
                                        SqlParameter AnnouncementTitle = new SqlParameter("ANNOUNCEMENT_TITLE", objAddEditAnnouncement.ANNOUNCEMENT_TITLE.ToString() ?? (object)DBNull.Value);
                                        SqlParameter AnnouncementDateFrom = new SqlParameter("ANNOUNCEMENT_DATE_FROM", Convert.ToDateTime(AnnouncementDateFromUpdated));
                                        SqlParameter AnnouncementDateTo = new SqlParameter("ANNOUNCEMENT_DATE_TO", AnnouncementDateFromUpdated);
                                        SqlParameter ModifiedDate = new SqlParameter("MODIFIED_DATE", Helper.GetCurrentDate());
                                        SqlParameter ModifiedBy = new SqlParameter("MODIFIED_BY", profile.UserName);
                                        SqlParameter PracticeCode = new SqlParameter("PRACTICE_CODE", profile.PracticeCode);
                                        SqlParameter Deleted = new SqlParameter("DELETED", false);
                                        SqlParameter CreatedBy = new SqlParameter("CREATED_BY", profile.UserName);
                                        SqlParameter Operaton = new SqlParameter("OPERATION", "ADD");
                                        SpRepository<Announcements>.GetListWithStoreProcedure(@"exec FOX_PROC_INSERT_ANNOUNCEMENT @ANNOUNCEMENT_ID, @ANNOUNCEMENT_DETAILS, @ANNOUNCEMENT_TITLE, @ANNOUNCEMENT_DATE_FROM, @ANNOUNCEMENT_DATE_TO, @MODIFIED_DATE, @MODIFIED_BY, @PRACTICE_CODE, @DELETED, @CREATED_BY, @OPERATION", AnnouncementId, AnnouncementDetails, AnnouncementTitle, AnnouncementDateFrom, AnnouncementDateTo, ModifiedDate, ModifiedBy, PracticeCode, Deleted, CreatedBy, Operaton);
                                        foreach (var item in objAddEditAnnouncement.RoleRequest)
                                        {
                                            long primaryKeyRol = Helper.getMaximumId("ANNOUNCEMENT_ROLE_ID");
                                            SqlParameter AnnouncementRoleId = new SqlParameter("ANNOUNCEMENT_ROLE_ID", primaryKeyRol);
                                            SqlParameter RoleId = new SqlParameter("ROLE_ID", item.ROLE_ID.ToString() ?? (object)DBNull.Value);
                                            SqlParameter RoleName = new SqlParameter("ROLE_NAME", item.ROLE_NAME.ToString() ?? (object)DBNull.Value);
                                            SqlParameter AnnouncementIdRole = new SqlParameter("ANNOUNCEMENT_ID", primaryKey);
                                            SqlParameter ROLE_MODIFIED_DATE = new SqlParameter("MODIFIED_DATE", Helper.GetCurrentDate());
                                            SqlParameter RolePracticeCode = new SqlParameter("PRACTICE_CODE", profile.PracticeCode);
                                            SpRepository<AnnouncementRoles>.GetListWithStoreProcedure(@"exec FOX_PROC_INSERT_ANNOUNCEMENT_ROLE @ANNOUNCEMENT_ROLE_ID, @ROLE_ID, @ROLE_NAME, @ANNOUNCEMENT_ID, @PRACTICE_CODE", AnnouncementRoleId, RoleId, RoleName, AnnouncementIdRole, RolePracticeCode);
                                        }
                                    }
                                    response.ErrorMessage = "Record insert successfully";
                                    response.Success = true;
                                }
                                else
                                {
                                    response.ErrorMessage = "Can not be added more than one announcment in same day";
                                    response.Success = false;
                                }
                            }
                            else
                            {
                                SqlParameter AnnouncementId = new SqlParameter("ANNOUNCEMENT_ID", objAddEditAnnouncement.ANNOUNCEMENT_ID);
                                SqlParameter AnnouncementDetails = new SqlParameter("ANNOUNCEMENT_DETAILS", objAddEditAnnouncement.ANNOUNCEMENT_DETAILS.ToString() ?? (object)DBNull.Value);
                                SqlParameter AnnouncementTitle = new SqlParameter("ANNOUNCEMENT_TITLE", objAddEditAnnouncement.ANNOUNCEMENT_TITLE.ToString() ?? (object)DBNull.Value);
                                SqlParameter AnnouncementDateFrom = new SqlParameter("ANNOUNCEMENT_DATE_FROM", Convert.ToDateTime(objAddEditAnnouncement.ANNOUNCEMENT_DATE_FROM));
                                SqlParameter AnnouncementDateTo = new SqlParameter("ANNOUNCEMENT_DATE_TO", objAddEditAnnouncement.ANNOUNCEMENT_DATE_TO);
                                SqlParameter ModifiedDate = new SqlParameter("MODIFIED_DATE", Helper.GetCurrentDate());
                                SqlParameter ModifiedBy = new SqlParameter("MODIFIED_BY", profile.UserName);
                                SqlParameter PracticeCode = new SqlParameter("PRACTICE_CODE", profile.PracticeCode);
                                SqlParameter Deleted = new SqlParameter("DELETED", false);
                                SqlParameter CreatedBy = new SqlParameter("CREATED_BY", profile.UserName);
                                SqlParameter Operaton = new SqlParameter("OPERATION", "UPDATE");
                                SpRepository<Announcements>.GetListWithStoreProcedure(@"exec FOX_PROC_INSERT_ANNOUNCEMENT @ANNOUNCEMENT_ID, @ANNOUNCEMENT_DETAILS, @ANNOUNCEMENT_TITLE, @ANNOUNCEMENT_DATE_FROM, @ANNOUNCEMENT_DATE_TO, @MODIFIED_DATE, @MODIFIED_BY, @PRACTICE_CODE, @DELETED, @CREATED_BY, @OPERATION", AnnouncementId, AnnouncementDetails, AnnouncementTitle, AnnouncementDateFrom, AnnouncementDateTo, ModifiedDate, ModifiedBy, PracticeCode, Deleted, CreatedBy, Operaton);
                                foreach (var item in objAddEditAnnouncement.RoleRequest)
                                {
                                    long primaryKeyRol = Helper.getMaximumId("ANNOUNCEMENT_ROLE_ID");
                                    SqlParameter AnnouncementRoleId = new SqlParameter("ANNOUNCEMENT_ROLE_ID", primaryKeyRol);
                                    SqlParameter RoleId = new SqlParameter("ROLE_ID", item.ROLE_ID.ToString() ?? (object)DBNull.Value);
                                    SqlParameter RoleName = new SqlParameter("ROLE_NAME", item.ROLE_NAME.ToString() ?? (object)DBNull.Value);
                                    SqlParameter AnnouncementIdRole = new SqlParameter("ANNOUNCEMENT_ID", objAddEditAnnouncement.ANNOUNCEMENT_ID);
                                    SqlParameter ROLE_MODIFIED_DATE = new SqlParameter("MODIFIED_DATE", Helper.GetCurrentDate());
                                    SqlParameter RolePracticeCode = new SqlParameter("PRACTICE_CODE", profile.PracticeCode);
                                    SpRepository<AnnouncementRoles>.GetListWithStoreProcedure(@"exec FOX_PROC_INSERT_ANNOUNCEMENT_ROLE @ANNOUNCEMENT_ROLE_ID, @ROLE_ID, @ROLE_NAME, @ANNOUNCEMENT_ID , @PRACTICE_CODE", AnnouncementRoleId, RoleId, RoleName, AnnouncementIdRole, RolePracticeCode);
                                }
                                response.ErrorMessage = "Record updated successfully";
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
                        response.ErrorMessage = "Can't be added announcement more than 10 days";
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
        // Description: 
        public List<Announcements> GetAnnouncement(Announcements objAnnouncement, UserProfile profile)
        {
            List<Announcements> announcementsList = new List<Announcements>();
            if (!string.IsNullOrEmpty(objAnnouncement.ANNOUNCEMENT_DATE_FROM_STR))
            {
                objAnnouncement.ANNOUNCEMENT_DATE_FROM = Convert.ToDateTime(objAnnouncement.ANNOUNCEMENT_DATE_FROM_STR);
            }
            else
            {
                objAnnouncement.ANNOUNCEMENT_DATE_FROM = Helper.GetCurrentDate().Date;
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
                var PracticeCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
                SqlParameter AnnouncementsDateFrom = new SqlParameter("ANNOUNCEMENT_DATE_FROM", objAnnouncement.ANNOUNCEMENT_DATE_FROM ?? (object)DBNull.Value);
                SqlParameter AnnouncementsDateTo = new SqlParameter("ANNOUNCEMENT_DATE_TO", objAnnouncement.ANNOUNCEMENT_DATE_TO ?? (object)DBNull.Value);
                SqlParameter RoleId = new SqlParameter { ParameterName = "ROLE_ID", SqlDbType = SqlDbType.VarChar, Value = objAnnouncement.ROLE_ID ?? (object)DBNull.Value };
                announcementsList = SpRepository<Announcements>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_ANNOUNCEMENT_DETAILS  @PRACTICE_CODE, @ANNOUNCEMENT_DATE_FROM, @ANNOUNCEMENT_DATE_TO, @ROLE_ID ", PracticeCode, AnnouncementsDateFrom, AnnouncementsDateTo, RoleId);
            }
            return announcementsList;
        }
        // Description: 
        public Announcements GetAnnouncementDetails(Announcements objAnnouncement, UserProfile profile)
        {
            Announcements announcementsList = new Announcements();
            if (objAnnouncement != null)
            {
                if (profile != null && profile.PracticeCode != 0)
                {
                    if (objAnnouncement.ANNOUNCEMENT_DATE_FROM != null)
                    {
                        objAnnouncement.ANNOUNCEMENT_DATE_FROM = Convert.ToDateTime(objAnnouncement.ANNOUNCEMENT_DATE_FROM);
                    }
                    if (objAnnouncement.ANNOUNCEMENT_DATE_TO != null)
                    {
                        objAnnouncement.ANNOUNCEMENT_DATE_TO = Convert.ToDateTime(objAnnouncement.ANNOUNCEMENT_DATE_TO);
                    }
                    var PracticeCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
                    var AccouncmentId = new SqlParameter { ParameterName = "ANNOUNCEMENT_ID", SqlDbType = SqlDbType.BigInt, Value = objAnnouncement.ANNOUNCEMENT_ID };
                    announcementsList = SpRepository<Announcements>.GetSingleObjectWithStoreProcedure(@"exec FOX_PROC_GET_ANNOUNCEMENT_DETAILS_FOR_UPDATE  @PRACTICE_CODE, @ANNOUNCEMENT_ID", PracticeCode, AccouncmentId);
                    if (announcementsList != null)
                    {
                        announcementsList.AnnouncementRoles = _announcementRoleRepository.GetMany(x => x.ANNOUNCEMENT_ID == announcementsList.ANNOUNCEMENT_ID && !(x.DELETED) && x.PRACTICE_CODE == profile.PracticeCode);
                    }
                }
            }
            return announcementsList;
        }
        // Description: 
        public ResponseModel DeleteAnnouncement(Announcements objAnnouncement, UserProfile profile)
        {
            ResponseModel response = new ResponseModel();
            if (objAnnouncement != null && profile.PracticeCode != 0 && !string.IsNullOrEmpty(objAnnouncement.ANNOUNCEMENT_ID.ToString()))
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
                    response.ErrorMessage = "Announcment and Announcement Roles deleted successfully";
                    response.Success = true;
                }
            }
            return response;
        }
    }
}
