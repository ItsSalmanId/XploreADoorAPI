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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            List<FoxRoles> result = new List<FoxRoles>();
            if (userProfile != null && userProfile.PracticeCode != 0)
            {
                result = _foxRolesRepository.GetMany(r => r.PRACTICE_CODE == userProfile.PracticeCode && !r.DELETED);
            }
            return result;
        }
        public ResponseModel InsertAnnouncement(AddEditFoxAnnouncement objAddEditAnnouncement, UserProfile profile)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                if (!string.IsNullOrEmpty(objAddEditAnnouncement.ANNOUNCEMENT_DATE_FROM_STR))
                {
                    objAddEditAnnouncement.ANNOUNCEMENT_DATE_FROM = Convert.ToDateTime(objAddEditAnnouncement.ANNOUNCEMENT_DATE_FROM_STR);
                }
                if (!string.IsNullOrEmpty(objAddEditAnnouncement.ANNOUNCEMENT_DATE_TO_STR))
                {
                    objAddEditAnnouncement.ANNOUNCEMENT_DATE_TO = Convert.ToDateTime(objAddEditAnnouncement.ANNOUNCEMENT_DATE_TO_STR);
                }
                if (objAddEditAnnouncement != null && objAddEditAnnouncement.ANNOUNCEMENT_TITLE != null && objAddEditAnnouncement.ANNOUNCEMENT_DETAILS != null && objAddEditAnnouncement.ANNOUNCEMENT_DATE_TO != null && objAddEditAnnouncement.ANNOUNCEMENT_DATE_FROM != null)
                {

                    DateTime TODAY = (DateTime)objAddEditAnnouncement.ANNOUNCEMENT_DATE_TO;
                    double TOTAL_DATS = TODAY.Subtract((DateTime)objAddEditAnnouncement.ANNOUNCEMENT_DATE_FROM).TotalDays + 1;

                    if (!string.IsNullOrEmpty(objAddEditAnnouncement.ANNOUNCEMENT_DETAILS.ToString()))
                    {
                        var ExistingDetailInfo = _announcementRepository.GetFirst(r => r.ANNOUNCEMENT_ID == objAddEditAnnouncement.ANNOUNCEMENT_ID && r.DELETED == false);
                        if (ExistingDetailInfo == null)
                        {
                            var DuplicationCheckInAnnouncment = _announcementRepository.GetFirst(r => r.ANNOUNCEMENT_DATE_FROM <= objAddEditAnnouncement.ANNOUNCEMENT_DATE_FROM && r.ANNOUNCEMENT_DATE_TO >= objAddEditAnnouncement.ANNOUNCEMENT_DATE_TO && r.DELETED == false);
                            if (DuplicationCheckInAnnouncment == null)
                            {
                                for (int i = 0; i < TOTAL_DATS; i++)
                                {
                                    DateTime ANNOUNCEMENT_DATE_FROM_UPDATED = (DateTime)objAddEditAnnouncement.ANNOUNCEMENT_DATE_FROM;
                                    ANNOUNCEMENT_DATE_FROM_UPDATED = ANNOUNCEMENT_DATE_FROM_UPDATED.AddDays(i);
                                    long primaryKey = Helper.getMaximumId("ANNOUNCEMENT_ID");
                                    SqlParameter ANNOUNCEMENT_ID = new SqlParameter("ANNOUNCEMENT_ID", primaryKey);
                                    SqlParameter ANNOUNCEMENT_DETAILS = new SqlParameter("ANNOUNCEMENT_DETAILS", objAddEditAnnouncement.ANNOUNCEMENT_DETAILS.ToString() ?? (object)DBNull.Value);
                                    SqlParameter ANNOUNCEMENT_TITLE = new SqlParameter("ANNOUNCEMENT_TITLE", objAddEditAnnouncement.ANNOUNCEMENT_TITLE.ToString() ?? (object)DBNull.Value);
                                    SqlParameter ANNOUNCEMENT_DATE_FROM = new SqlParameter("ANNOUNCEMENT_DATE_FROM", Convert.ToDateTime(ANNOUNCEMENT_DATE_FROM_UPDATED));
                                    SqlParameter ANNOUNCEMENT_DATE_TO = new SqlParameter("ANNOUNCEMENT_DATE_TO", ANNOUNCEMENT_DATE_FROM_UPDATED);
                                    SqlParameter MODIFIED_DATE = new SqlParameter("MODIFIED_DATE", Helper.GetCurrentDate());
                                    SqlParameter PRACTICE_CODE = new SqlParameter("PRACTICE_CODE", profile.PracticeCode);
                                    SqlParameter DELETED = new SqlParameter("DELETED", false);
                                    SqlParameter CREATED_BY = new SqlParameter("CREATED_BY", profile.PracticeCode);
                                    SqlParameter OPERATION = new SqlParameter("OPERATION", "ADD");
                                    SpRepository<Announcements>.GetListWithStoreProcedure(@"exec FOX_PROC_INSERT_ANNOUNCEMENT @ANNOUNCEMENT_ID, @ANNOUNCEMENT_DETAILS, @ANNOUNCEMENT_TITLE, @ANNOUNCEMENT_DATE_FROM, @ANNOUNCEMENT_DATE_TO, @MODIFIED_DATE, @PRACTICE_CODE, @DELETED, @CREATED_BY, @OPERATION", ANNOUNCEMENT_ID, ANNOUNCEMENT_DETAILS, ANNOUNCEMENT_TITLE, ANNOUNCEMENT_DATE_FROM, ANNOUNCEMENT_DATE_TO, MODIFIED_DATE, PRACTICE_CODE, DELETED, CREATED_BY, OPERATION);


                                    foreach (var item in objAddEditAnnouncement.RoleRequest)
                                    {
                                        long primaryKeyRol = Helper.getMaximumId("ANNOUNCEMENT_ROLE_ID");
                                        SqlParameter ANNOUNCEMENT_ROLE_ID = new SqlParameter("ANNOUNCEMENT_ROLE_ID", primaryKeyRol);
                                        SqlParameter ROLE_ID = new SqlParameter("ROLE_ID", item.ROLE_ID.ToString() ?? (object)DBNull.Value);
                                        SqlParameter ROLE_NAME = new SqlParameter("ROLE_NAME", item.ROLE_NAME.ToString() ?? (object)DBNull.Value);
                                        SqlParameter ANNOUNCEMENT_ID_ROLE = new SqlParameter("ANNOUNCEMENT_ID", primaryKey);
                                        SqlParameter ROLE_MODIFIED_DATE = new SqlParameter("MODIFIED_DATE", Helper.GetCurrentDate());
                                        SqlParameter ROLE_PRACTICE_CODE = new SqlParameter("PRACTICE_CODE", profile.PracticeCode);
                                        SpRepository<AnnouncementRoles>.GetListWithStoreProcedure(@"exec FOX_PROC_INSERT_ANNOUNCEMENT_ROLE @ANNOUNCEMENT_ROLE_ID, @ROLE_ID, @ROLE_NAME, @ANNOUNCEMENT_ID, @PRACTICE_CODE", ANNOUNCEMENT_ROLE_ID,
                                         ROLE_ID, ROLE_NAME, ANNOUNCEMENT_ID_ROLE, ROLE_PRACTICE_CODE);
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
                            //long primaryKey = Helper.getMaximumId("ANNOUNCEMENT_ID");
                            SqlParameter ANNOUNCEMENT_ID = new SqlParameter("ANNOUNCEMENT_ID", objAddEditAnnouncement.ANNOUNCEMENT_ID);
                            SqlParameter ANNOUNCEMENT_DETAILS = new SqlParameter("ANNOUNCEMENT_DETAILS", objAddEditAnnouncement.ANNOUNCEMENT_DETAILS.ToString() ?? (object)DBNull.Value);
                            SqlParameter ANNOUNCEMENT_TITLE = new SqlParameter("ANNOUNCEMENT_TITLE", objAddEditAnnouncement.ANNOUNCEMENT_TITLE.ToString() ?? (object)DBNull.Value);
                            //SqlParameter ANNOUNCEMENT_DATE_FROM = new SqlParameter("ANNOUNCEMENT_DATE_FROM", Helper.GetCurrentDate());
                            //SqlParameter ANNOUNCEMENT_DATE_TO = new SqlParameter("ANNOUNCEMENT_DATE_TO", Helper.GetCurrentDate());
                            SqlParameter ANNOUNCEMENT_DATE_FROM = new SqlParameter("ANNOUNCEMENT_DATE_FROM", Convert.ToDateTime(objAddEditAnnouncement.ANNOUNCEMENT_DATE_FROM));
                            SqlParameter ANNOUNCEMENT_DATE_TO = new SqlParameter("ANNOUNCEMENT_DATE_TO", objAddEditAnnouncement.ANNOUNCEMENT_DATE_TO);
                            SqlParameter MODIFIED_DATE = new SqlParameter("MODIFIED_DATE", Helper.GetCurrentDate());
                            //SqlParameter ROLE_ID = new SqlParameter("ROLE_ID", objAnnouncement.ROLE_ID);
                            SqlParameter PRACTICE_CODE = new SqlParameter("PRACTICE_CODE", profile.PracticeCode);
                            SqlParameter DELETED = new SqlParameter("DELETED", false);
                            SqlParameter CREATED_BY = new SqlParameter("CREATED_BY", profile.PracticeCode);
                            //SqlParameter CREATED_DATE = new SqlParameter("CREATED_DATE", Helper.GetCurrentDate());
                            SqlParameter OPERATION = new SqlParameter("OPERATION", "UPDATE");

                            SpRepository<Announcements>.GetListWithStoreProcedure(@"exec FOX_PROC_INSERT_ANNOUNCEMENT @ANNOUNCEMENT_ID, @ANNOUNCEMENT_DETAILS, @ANNOUNCEMENT_TITLE 
                        ,@ANNOUNCEMENT_DATE_FROM, @ANNOUNCEMENT_DATE_TO, @MODIFIED_DATE, @PRACTICE_CODE, @DELETED, @CREATED_BY, @OPERATION", ANNOUNCEMENT_ID,
                             ANNOUNCEMENT_DETAILS, ANNOUNCEMENT_TITLE, ANNOUNCEMENT_DATE_FROM, ANNOUNCEMENT_DATE_TO, MODIFIED_DATE, PRACTICE_CODE, DELETED, CREATED_BY, OPERATION);
                            foreach (var item in objAddEditAnnouncement.RoleRequest)
                            {
                                long primaryKeyRol = Helper.getMaximumId("ANNOUNCEMENT_ROLE_ID");
                                SqlParameter ANNOUNCEMENT_ROLE_ID = new SqlParameter("ANNOUNCEMENT_ROLE_ID", primaryKeyRol);
                                SqlParameter ROLE_ID = new SqlParameter("ROLE_ID", item.ROLE_ID.ToString() ?? (object)DBNull.Value);
                                SqlParameter ROLE_NAME = new SqlParameter("ROLE_NAME", item.ROLE_NAME.ToString() ?? (object)DBNull.Value);
                                SqlParameter ANNOUNCEMENT_ID_ROLE = new SqlParameter("ANNOUNCEMENT_ID", objAddEditAnnouncement.ANNOUNCEMENT_ID);
                                SqlParameter ROLE_MODIFIED_DATE = new SqlParameter("MODIFIED_DATE", Helper.GetCurrentDate());
                                //SqlParameter ROLE_ID = new SqlParameter("ROLE_ID", objAnnouncement.ROLE_ID);
                                SqlParameter ROLE_PRACTICE_CODE = new SqlParameter("PRACTICE_CODE", profile.PracticeCode);
                                //SqlParameter ROLE_DELETED = new SqlParameter("DELETED", false);
                                //SqlParameter ROLE_CREATED_BY = new SqlParameter("CREATED_BY", profile.PracticeCode);
                                //SqlParameter ROLE_CREATED_DATE = new SqlParameter("CREATED_DATE", Helper.GetCurrentDate());

                                SpRepository<AnnouncementRoles>.GetListWithStoreProcedure(@"exec FOX_PROC_INSERT_ANNOUNCEMENT_ROLE @ANNOUNCEMENT_ROLE_ID, @ROLE_ID, @ROLE_NAME, @ANNOUNCEMENT_ID , @PRACTICE_CODE", ANNOUNCEMENT_ROLE_ID, ROLE_ID, ROLE_NAME, ANNOUNCEMENT_ID_ROLE, ROLE_PRACTICE_CODE);
                            }
                            response.ErrorMessage = "Record update successfully";
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
                    response.ErrorMessage = "Fill mendatory filed announcment";
                    response.Success = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }
        public List<Announcements> GetAnnouncement(Announcements objAnnouncement, UserProfile profile)
        {
            List<Announcements> announcementsList = new List<Announcements>();
            if (!string.IsNullOrEmpty(objAnnouncement.ANNOUNCEMENT_DATE_FROM_STR))
            {
                objAnnouncement.ANNOUNCEMENT_DATE_FROM = Convert.ToDateTime(objAnnouncement.ANNOUNCEMENT_DATE_FROM_STR);
            }
            else
            {
                objAnnouncement.ANNOUNCEMENT_DATE_FROM = null;
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
                if (!string.IsNullOrEmpty(objAnnouncement.ANNOUNCEMENT_DATE_FROM_STR) && !string.IsNullOrEmpty(objAnnouncement.ANNOUNCEMENT_DATE_TO_STR))
                {
                    objAnnouncement.ANNOUNCEMENT_DATE_FROM = Convert.ToDateTime(objAnnouncement.ANNOUNCEMENT_DATE_FROM_STR);
                    objAnnouncement.ANNOUNCEMENT_DATE_TO = Convert.ToDateTime(objAnnouncement.ANNOUNCEMENT_DATE_TO_STR);
                }
                var PracticeCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
                SqlParameter AnnouncementsDateFrom = new SqlParameter("ANNOUNCEMENT_DATE_FROM", objAnnouncement.ANNOUNCEMENT_DATE_FROM ?? (object)DBNull.Value);
                SqlParameter AnnouncementsDateTo = new SqlParameter("ANNOUNCEMENT_DATE_TO", objAnnouncement.ANNOUNCEMENT_DATE_TO ?? (object)DBNull.Value);
                SqlParameter RoleId = new SqlParameter { ParameterName = "ROLE_ID", SqlDbType = SqlDbType.VarChar, Value = objAnnouncement.ROLE_ID ?? (object)DBNull.Value };
                announcementsList = SpRepository<Announcements>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_ANNOUNCEMENT_DETAILS  @PRACTICE_CODE, @ANNOUNCEMENT_DATE_FROM, @ANNOUNCEMENT_DATE_TO, @ROLE_ID ", PracticeCode, AnnouncementsDateFrom, AnnouncementsDateTo, RoleId);
            }
            return announcementsList;
        }

        public ResponseModel AddAnnouncementRole(List<AnnouncementRoles> objAnnouncementRoles, UserProfile profile)
        {
            ResponseModel response = new ResponseModel();
            foreach (var value in objAnnouncementRoles)
            {
                AnnouncementRoles announcementRoles = new AnnouncementRoles();
                if (!string.IsNullOrEmpty(value.ANNOUNCEMENT_ROLE_ID.ToString()))
                {
                    var ExistingDetailInfo = _announcementRepository.GetFirst(r => r.ANNOUNCEMENT_ID == value.ANNOUNCEMENT_ID && r.DELETED == false);
                    if (ExistingDetailInfo == null)
                    {
                        long primaryKey = Helper.getMaximumId("ANNOUNCEMENT_ID");
                        SqlParameter ANNOUNCEMENT_ID = new SqlParameter("ANNOUNCEMENT_ID", primaryKey);
                        SqlParameter ANNOUNCEMENT_DETAILS = new SqlParameter("ANNOUNCEMENT_DETAILS", value.ANNOUNCEMENT_ROLE_ID.ToString() ?? (object)DBNull.Value);
                        SqlParameter ANNOUNCEMENT_TITLE = new SqlParameter("ANNOUNCEMENT_TITLE", value.ROLE_ID.ToString() ?? (object)DBNull.Value);
                        SqlParameter MODIFIED_DATE = new SqlParameter("MODIFIED_DATE", Helper.GetCurrentDate());
                        SqlParameter PRACTICE_CODE = new SqlParameter("PRACTICE_CODE", profile.PracticeCode);
                        SqlParameter DELETED = new SqlParameter("DELETED", false);
                        SqlParameter CREATED_BY = new SqlParameter("CREATED_BY", profile.PracticeCode);
                        SqlParameter CREATED_DATE = new SqlParameter("CREATED_DATE", Helper.GetCurrentDate());
                        SqlParameter Add = new SqlParameter("Add", "Add");
                        response.ErrorMessage = "Record insert successfully";
                        response.Success = true;
                    }
                    else
                    {
                        long primaryKey = Helper.getMaximumId("ANNOUNCEMENT_ID");
                        SqlParameter ANNOUNCEMENT_ID = new SqlParameter("ANNOUNCEMENT_ID", primaryKey);
                        SqlParameter ANNOUNCEMENT_DATE_FROM = new SqlParameter("ANNOUNCEMENT_DATE_FROM", Helper.GetCurrentDate());
                        SqlParameter ANNOUNCEMENT_DATE_TO = new SqlParameter("ANNOUNCEMENT_DATE_TO", Helper.GetCurrentDate());
                        SqlParameter MODIFIED_DATE = new SqlParameter("MODIFIED_DATE", Helper.GetCurrentDate());
                        SqlParameter PRACTICE_CODE = new SqlParameter("PRACTICE_CODE", profile.PracticeCode);
                        SqlParameter DELETED = new SqlParameter("DELETED", false);
                        SqlParameter CREATED_BY = new SqlParameter("CREATED_BY", profile.PracticeCode);
                        SqlParameter CREATED_DATE = new SqlParameter("CREATED_DATE", Helper.GetCurrentDate());
                        SqlParameter Add = new SqlParameter("Add", "Add");
                        response.ErrorMessage = "Record update successfully";
                        response.Success = true;

                    }
                }
                else
                {
                    response.ErrorMessage = "Please Entre Questions or Answers";
                    response.Success = false;
                }
            }
            return response;
        }
        public Announcements InsertAnnouncementTest(Announcements objAnnouncement, UserProfile profile)
        {
            Announcements announcements = new Announcements();
            ResponseModel response = new ResponseModel();
            try
            {

                if (!string.IsNullOrEmpty(objAnnouncement.ANNOUNCEMENT_DETAILS.ToString()))
                {
                    var ExistingDetailInfo = _announcementRepository.GetFirst(r => r.ANNOUNCEMENT_ID == objAnnouncement.ANNOUNCEMENT_ID && r.DELETED == false);
                    if (ExistingDetailInfo == null)
                    {
                        long primaryKey = Helper.getMaximumId("ANNOUNCEMENT_ID");
                        SqlParameter ANNOUNCEMENT_ID = new SqlParameter("ANNOUNCEMENT_ID", primaryKey);
                        SqlParameter ANNOUNCEMENT_DETAILS = new SqlParameter("ANNOUNCEMENT_DETAILS", objAnnouncement.ANNOUNCEMENT_DETAILS.ToString() ?? (object)DBNull.Value);
                        SqlParameter ANNOUNCEMENT_TITLE = new SqlParameter("ANNOUNCEMENT_TITLE", objAnnouncement.ANNOUNCEMENT_DETAILS.ToString() ?? (object)DBNull.Value);
                        SqlParameter ANNOUNCEMENT_DATE_FROM = new SqlParameter("ANNOUNCEMENT_DATE_FROM", Convert.ToDateTime(objAnnouncement.ANNOUNCEMENT_DATE_FROM));
                        SqlParameter ANNOUNCEMENT_DATE_TO = new SqlParameter("ANNOUNCEMENT_DATE_TO", objAnnouncement.ANNOUNCEMENT_DATE_TO);
                        SqlParameter MODIFIED_DATE = new SqlParameter("MODIFIED_DATE", Helper.GetCurrentDate());
                        //SqlParameter ROLE_ID = new SqlParameter("ROLE_ID", objAnnouncement.ROLE_ID);
                        SqlParameter PRACTICE_CODE = new SqlParameter("PRACTICE_CODE", profile.PracticeCode);
                        SqlParameter DELETED = new SqlParameter("DELETED", false);
                        SqlParameter CREATED_BY = new SqlParameter("CREATED_BY", profile.PracticeCode);
                        SqlParameter CREATED_DATE = new SqlParameter("CREATED_DATE", Helper.GetCurrentDate());
                        SqlParameter Add = new SqlParameter("Add", "Add");

                        SpRepository<Announcements>.GetListWithStoreProcedure(@"exec FOX_PROC_INSERT_ANNOUNCEMENT @ANNOUNCEMENT_ID, @ANNOUNCEMENT_DETAILS, @ANNOUNCEMENT_TITLE 
                         ,@ANNOUNCEMENT_DATE_FROM, @ANNOUNCEMENT_DATE_TO, @MODIFIED_DATE, @PRACTICE_CODE, @DELETED, @CREATED_BY, @CREATED_BY", ANNOUNCEMENT_ID,
                         ANNOUNCEMENT_DETAILS, ANNOUNCEMENT_TITLE, ANNOUNCEMENT_DATE_FROM, ANNOUNCEMENT_DATE_TO, MODIFIED_DATE, PRACTICE_CODE, DELETED, CREATED_BY, CREATED_DATE, Add);
                        //GetExportAdvancedDailyReports(profile, exportAdvancedDailyReport.CALL_USER_ID);
                        response.ErrorMessage = "Record insert successfully";
                        response.Success = true;
                    }
                    else
                    {
                        long primaryKey = Helper.getMaximumId("ANNOUNCEMENT_ID");
                        SqlParameter ANNOUNCEMENT_ID = new SqlParameter("ANNOUNCEMENT_ID", primaryKey);
                        SqlParameter ANNOUNCEMENT_DETAILS = new SqlParameter("ANNOUNCEMENT_DETAILS", objAnnouncement.ANNOUNCEMENT_DETAILS.ToString() ?? (object)DBNull.Value);
                        SqlParameter ANNOUNCEMENT_TITLE = new SqlParameter("ANNOUNCEMENT_TITLE", objAnnouncement.ANNOUNCEMENT_TITLE.ToString() ?? (object)DBNull.Value);
                        SqlParameter ANNOUNCEMENT_DATE_FROM = new SqlParameter("ANNOUNCEMENT_DATE_FROM", Helper.GetCurrentDate());
                        SqlParameter ANNOUNCEMENT_DATE_TO = new SqlParameter("ANNOUNCEMENT_DATE_TO", Helper.GetCurrentDate());
                        SqlParameter MODIFIED_DATE = new SqlParameter("MODIFIED_DATE", Helper.GetCurrentDate());
                        //SqlParameter ROLE_ID = new SqlParameter("ROLE_ID", objAnnouncement.ROLE_ID);
                        SqlParameter PRACTICE_CODE = new SqlParameter("PRACTICE_CODE", profile.PracticeCode);
                        SqlParameter DELETED = new SqlParameter("DELETED", false);
                        SqlParameter CREATED_BY = new SqlParameter("CREATED_BY", profile.PracticeCode);
                        SqlParameter CREATED_DATE = new SqlParameter("CREATED_DATE", Helper.GetCurrentDate());
                        SqlParameter Add = new SqlParameter("Add", "Add");

                        SpRepository<Announcements>.GetListWithStoreProcedure(@"exec FOX_PROC_INSERT_ANNOUNCEMENT @ANNOUNCEMENT_ID, @ANNOUNCEMENT_DETAILS, @ANNOUNCEMENT_TITLE 
                        ,@ANNOUNCEMENT_DATE_FROM, @ANNOUNCEMENT_DATE_TO, @MODIFIED_DATE, @ROLE_ID, @PRACTICE_CODE, @DELETED, @CREATED_BY, @CREATED_DATE", ANNOUNCEMENT_ID,
                         ANNOUNCEMENT_DETAILS, ANNOUNCEMENT_TITLE, ANNOUNCEMENT_DATE_FROM, ANNOUNCEMENT_DATE_TO, MODIFIED_DATE, PRACTICE_CODE, DELETED, CREATED_BY, CREATED_DATE);
                        response.ErrorMessage = "Record update successfully";
                        response.Success = true;
                    }

                }
                else
                {
                    response.ErrorMessage = "Please Entre Questions or Answers";
                    response.Success = false;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return announcements;
        }

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

        public ResponseModel DeleteAnnouncement(Announcements objAnnouncement, UserProfile profile)
        {
            ResponseModel response = new ResponseModel();
            if (objAnnouncement != null && profile.PracticeCode != 0 && !string.IsNullOrEmpty(objAnnouncement.ANNOUNCEMENT_ID.ToString()))
            {
                var existingDetailInfo = _announcementRepository.GetFirst(r => r.ANNOUNCEMENT_ID == objAnnouncement.ANNOUNCEMENT_ID && r.DELETED == false);

                if (existingDetailInfo != null)
                {
                    existingDetailInfo.ANNOUNCEMENT_ID = existingDetailInfo.ANNOUNCEMENT_ID;
                    existingDetailInfo.MODIFIED_BY = profile.UserName;
                    existingDetailInfo.MODIFIED_DATE = Helper.GetCurrentDate();
                    existingDetailInfo.DELETED = true;
                    _announcementRepository.Update(existingDetailInfo);
                    _announcementRepository.Save();
                    response.ErrorMessage = "";
                    response.ErrorMessage = "Announcment deleted successfully";
                    response.Success = true;
                }
                else
                {
                    response.ErrorMessage = "";
                   // response.Message = "Announcment deleted successfully";
                    response.Success = true;
                }
            }
            return response;
            //Announcements announcements = new Announcements();
            //ResponseModel response = new ResponseModel();
            //try
            //{

            //    if (!string.IsNullOrEmpty(objAnnouncement.ANNOUNCEMENT_ID.ToString()))
            //    {
            //        var ExistingDetailInfo = _announcementRepository.GetFirst(r => r.ANNOUNCEMENT_ID == objAnnouncement.ANNOUNCEMENT_ID && r.DELETED == false);
            //        if (ExistingDetailInfo != null)
            //        {

            //            SqlParameter ANNOUNCEMENT_ID = new SqlParameter("ANNOUNCEMENT_ID", objAnnouncement.ANNOUNCEMENT_ID);
            //            //SqlParameter ANNOUNCEMENT_DETAILS = new SqlParameter("ANNOUNCEMENT_DETAILS", objAnnouncement.ANNOUNCEMENT_DETAILS == null ? "" : objAnnouncement.ANNOUNCEMENT_DETAILS.ToString());
            //            //SqlParameter ANNOUNCEMENT_TITLE = new SqlParameter("ANNOUNCEMENT_TITLE", objAnnouncement.ANNOUNCEMENT_TITLE == null ? "" : objAnnouncement.ANNOUNCEMENT_TITLE.ToString());
            //            //SqlParameter ANNOUNCEMENT_DATE_FROM = new SqlParameter("ANNOUNCEMENT_DATE_FROM", Convert.ToDateTime(objAnnouncement.ANNOUNCEMENT_DATE_FROM));
            //            //SqlParameter ANNOUNCEMENT_DATE_TO = new SqlParameter("ANNOUNCEMENT_DATE_TO", objAnnouncement.ANNOUNCEMENT_DATE_TO);
            //            //SqlParameter MODIFIED_DATE = new SqlParameter("MODIFIED_DATE", Helper.GetCurrentDate());
            //            ////SqlParameter ROLE_ID = new SqlParameter("ROLE_ID", objAnnouncement.ROLE_ID);
            //            //SqlParameter PRACTICE_CODE = new SqlParameter("PRACTICE_CODE", profile.PracticeCode);
            //            //SqlParameter DELETED = new SqlParameter("DELETED", false);
            //            //SqlParameter CREATED_BY = new SqlParameter("CREATED_BY", profile.PracticeCode);
            //            //SqlParameter OPERATION = new SqlParameter("OPERATION", "DELETE");
            //            SpRepository<Announcements>.GetListWithStoreProcedure(@"exec FOX_PROC_DELETE_ANNOUNCEMENT @ANNOUNCEMENT_ID", ANNOUNCEMENT_ID);
            //            response.ErrorMessage = "Record delete successfully";
            //            response.Success = true;
            //        }
            //    }
            //    else
            //    {
            //        response.ErrorMessage = "Record not delete successfully";
            //        response.Success = false;
            //    }

            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //return response;
        }
    }
}
