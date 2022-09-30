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
        public Announcements InsertAnnouncement(Announcements objAnnouncement, UserProfile profile)
        {
            ResponseModel response = new ResponseModel();
            Announcements announcements = new Announcements();
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
                        foreach (var item in objAnnouncement.DIAGNOSIS)
                        {
                            long primaryKeyRol = Helper.getMaximumId("ANNOUNCEMENT_ROLE_ID");
                            SqlParameter ANNOUNCEMENT_ROLE_ID = new SqlParameter("ANNOUNCEMENT_ROLE_ID", primaryKeyRol);
                            SqlParameter ROLE_ID = new SqlParameter("ROLE_ID", item.ROLE_ID.ToString() ?? (object)DBNull.Value);
                            SqlParameter ROLE_NAME = new SqlParameter("ROLE_NAME", item.ROLE_NAME.ToString() ?? (object)DBNull.Value);
                            SqlParameter ANNOUNCEMENT_ID_ROLE = new SqlParameter("ANNOUNCEMENT_ID_ROLE", primaryKey);
                            SqlParameter ROLE_MODIFIED_DATE = new SqlParameter("MODIFIED_DATE", Helper.GetCurrentDate());
                            //SqlParameter ROLE_ID = new SqlParameter("ROLE_ID", objAnnouncement.ROLE_ID);
                            SqlParameter ROLE_PRACTICE_CODE = new SqlParameter("PRACTICE_CODE", profile.PracticeCode);
                            SqlParameter ROLE_DELETED = new SqlParameter("DELETED", false);
                            SqlParameter ROLE_CREATED_BY = new SqlParameter("CREATED_BY", profile.PracticeCode);
                            SqlParameter ROLE_CREATED_DATE = new SqlParameter("CREATED_DATE", Helper.GetCurrentDate());

                            SpRepository<AnnouncementRoles>.GetListWithStoreProcedure(@"exec FOX_PROC_INSERT_ANNOUNCEMENT_ROLE @ANNOUNCEMENT_ROLE_ID, @ROLE_ID, @ROLE_NAME, @ANNOUNCEMENT_ID_ROLE 
                         , @MODIFIED_DATE, @PRACTICE_CODE, @DELETED, @CREATED_BY, @CREATED_BY", ANNOUNCEMENT_ROLE_ID,
                             ROLE_ID, ROLE_NAME, ANNOUNCEMENT_ID_ROLE, ROLE_MODIFIED_DATE, ROLE_PRACTICE_CODE, ROLE_DELETED, ROLE_CREATED_BY, ROLE_CREATED_DATE);
                        }

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
                        //if (!string.IsNullOrEmpty(ObjPHDFAQsDetail.QUESTIONS.ToString()) && !string.IsNullOrEmpty(ObjPHDFAQsDetail.ANSWERS.ToString()))
                        //{
                        //ExistingDetailInfo.FAQS_ID = objPhdFaqsDetail.FAQS_ID;
                        //ExistingDetailInfo.QUESTIONS = objPhdFaqsDetail.QUESTIONS.ToString();
                        //ExistingDetailInfo.ANSWERS = objPhdFaqsDetail.ANSWERS.ToString();
                        //ExistingDetailInfo.PRACTICE_CODE = profile.PracticeCode;
                        //ExistingDetailInfo.DELETED = false;
                        //ExistingDetailInfo.MODIFIED_BY = objPhdFaqsDetail.MODIFIED_BY = profile.UserName;
                        //ExistingDetailInfo.MODIFIED_DATE = objPhdFaqsDetail.MODIFIED_DATE = Helper.GetCurrentDate();
                        //_phdFaqsDetailRepository.Update(ExistingDetailInfo);
                        //_phdFaqsDetailRepository.Save();
                        response.ErrorMessage = "Record update successfully";
                        response.Success = true;
                        //}
                        //else
                        //{
                        //    response.ErrorMessage = "Please Entre Questions or Answers";
                        //    response.Success = false;
                        //}
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
        //public List<Announcements> GetAnnouncement(UserProfile profile)
        //{
        //    List<Announcements> announcementsList = new List<Announcements>();
        //    if (profile != null && profile.PracticeCode != 0)
        //    {
        //        var PracticeCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
        //        announcementsList = SpRepository<Announcements>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_PHD_FAQS_LIST  @PRACTICE_CODE ", PracticeCode);
        //    }
        //    return announcementsList;
        //}

        public List<Announcements> GetAnnouncement(Announcements objAnnouncement, UserProfile profile)
        {

            List<Announcements> announcementsList = new List<Announcements>();
            foreach (var item in objAnnouncement.DIAGNOSIS)
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
                    //var AnnouncementsDateFrom = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
                    //SqlParameter code = new SqlParameter { ParameterName = "DESCRIPTION", Value = taskTpyeSearch.Code };
                    //SqlParameter code = new SqlParameter { ParameterName = "DESCRIPTION", Value = taskTpyeSearch.Code };
                    //SqlParameter code = new SqlParameter { ParameterName = "DESCRIPTION", Value = taskTpyeSearch.Code };
                    SqlParameter AnnouncementsDateFrom = new SqlParameter("ANNOUNCEMENT_DATE_FROM", objAnnouncement.ANNOUNCEMENT_DATE_FROM);
                    SqlParameter AnnouncementsDateTo = new SqlParameter("ANNOUNCEMENT_DATE_TO", objAnnouncement.ANNOUNCEMENT_DATE_TO);
                    SqlParameter RoleId = new SqlParameter { ParameterName = "ROLE_ID", SqlDbType = SqlDbType.BigInt, Value = item.ROLE_ID };
                    //announcementsList = SpRepository<Announcement>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_ANNOUNCEMENT_LIST  @PRACTICE_CODE, @ANNOUNCEMENT_DATE_FROM, @ANNOUNCEMENT_DATE_FROM, @ROLE_ID ",
                    //    PracticeCode, AnnouncementsDateFrom, AnnouncementsDateTo, RoleId);
                    announcementsList = SpRepository<Announcements>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_ANNOUNCEMENT_LIST  @PRACTICE_CODE, @ANNOUNCEMENT_DATE_FROM, @ANNOUNCEMENT_DATE_TO, @ROLE_ID ",
                        PracticeCode, AnnouncementsDateFrom, AnnouncementsDateTo, RoleId);

                    //foreach (var item in objAnnouncement.DIAGNOSIS)
                    //{

                    //    if (profile != null && profile.PracticeCode != 0)
                    //    {
                    //        if (objAnnouncement.ANNOUNCEMENT_DATE_FROM != null)
                    //        {
                    //            objAnnouncement.ANNOUNCEMENT_DATE_FROM = Convert.ToDateTime(objAnnouncement.ANNOUNCEMENT_DATE_FROM);
                    //        }
                    //        if (objAnnouncement.ANNOUNCEMENT_DATE_TO != null)
                    //        {
                    //            objAnnouncement.ANNOUNCEMENT_DATE_TO = Convert.ToDateTime(objAnnouncement.ANNOUNCEMENT_DATE_TO);
                    //        }
                    //        var PracticeCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
                    //        SqlParameter AnnouncementsDateFrom = new SqlParameter("ANNOUNCEMENT_DATE_FROM", objAnnouncement.ANNOUNCEMENT_DATE_FROM);
                    //        SqlParameter AnnouncementsDateTo = new SqlParameter("ANNOUNCEMENT_DATE_TO", objAnnouncement.ANNOUNCEMENT_DATE_TO);
                    //        var RoleId = new SqlParameter { ParameterName = "ROLE_ID", SqlDbType = SqlDbType.BigInt, Value = item.ROLE_ID };
                    //        //announcementsList = SpRepository<Announcement>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_ANNOUNCEMENT_LIST  @PRACTICE_CODE, @ANNOUNCEMENT_DATE_FROM, @ANNOUNCEMENT_DATE_FROM, @ROLE_ID ",
                    //        //    PracticeCode, AnnouncementsDateFrom, AnnouncementsDateTo, RoleId);
                    //        var announcementsListtemp = SpRepository<Announcement>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_ANNOUNCEMENT_LIST  @PRACTICE_CODE, @ANNOUNCEMENT_DATE_FROM, @ANNOUNCEMENT_DATE_FROM, @ROLE_ID ",
                    //            PracticeCode, AnnouncementsDateFrom, AnnouncementsDateTo, RoleId);
                    //        return announcementsListtemp;
                    //    }

                }
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

                        //SpRepository<Announcements>.GetListWithStoreProcedure(@"exec FOX_PROC_INSERT_ANNOUNCEMENT @ANNOUNCEMENT_ID, @ANNOUNCEMENT_DETAILS, @ANNOUNCEMENT_TITLE 
                        // ,@ANNOUNCEMENT_DATE_FROM, @ANNOUNCEMENT_DATE_TO, @MODIFIED_DATE, @PRACTICE_CODE, @DELETED, @CREATED_BY, @CREATED_BY", ANNOUNCEMENT_ID,
                        // ANNOUNCEMENT_DETAILS, ANNOUNCEMENT_TITLE, ANNOUNCEMENT_DATE_FROM, ANNOUNCEMENT_DATE_TO, MODIFIED_DATE, PRACTICE_CODE, DELETED, CREATED_BY, CREATED_DATE, Add);
                        //GetExportAdvancedDailyReports(profile, exportAdvancedDailyReport.CALL_USER_ID);
                        response.ErrorMessage = "Record insert successfully";
                        response.Success = true;
                    }
                    else
                    {
                        long primaryKey = Helper.getMaximumId("ANNOUNCEMENT_ID");
                        SqlParameter ANNOUNCEMENT_ID = new SqlParameter("ANNOUNCEMENT_ID", primaryKey);
                        //SqlParameter ANNOUNCEMENT_DETAILS = new SqlParameter("ANNOUNCEMENT_DETAILS", objAnnouncement.ANNOUNCEMENT_DETAILS.ToString() ?? (object)DBNull.Value);
                        //SqlParameter ANNOUNCEMENT_TITLE = new SqlParameter("ANNOUNCEMENT_TITLE", objAnnouncement.ANNOUNCEMENT_TITLE.ToString() ?? (object)DBNull.Value);
                        SqlParameter ANNOUNCEMENT_DATE_FROM = new SqlParameter("ANNOUNCEMENT_DATE_FROM", Helper.GetCurrentDate());
                        SqlParameter ANNOUNCEMENT_DATE_TO = new SqlParameter("ANNOUNCEMENT_DATE_TO", Helper.GetCurrentDate());
                        SqlParameter MODIFIED_DATE = new SqlParameter("MODIFIED_DATE", Helper.GetCurrentDate());
                        //SqlParameter ROLE_ID = new SqlParameter("ROLE_ID", objAnnouncement.ROLE_ID);
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
                        //}
                        //else
                        //{
                        //    response.ErrorMessage = "Please Entre Questions or Answers";
                        //    response.Success = false;
                        //}
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
                        //}
                        //else
                        //{
                        //    response.ErrorMessage = "Please Entre Questions or Answers";
                        //    response.Success = false;
                        //}
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

        //public ResponseModel InsertAnnouncement(Announcements objAnnouncement, UserProfile profile)
        //{
        //    try
        //    {

        //        var ExistingDetailInfo = _announcementRepository.GetFirst(r => r.ANNOUNCEMENT_ID == objAnnouncement.ANNOUNCEMENT_ID && r.DELETED == false);
        //        if (ExistingDetailInfo == null)
        //        {
        //            var newAccouncmentId = Helper.getMaximumId("ANNOUNCEMENT_ID");
        //            var newAccouncment = new Announcements()
        //            {
        //                ANNOUNCEMENT_ID = newAccouncmentId,
        //                ANNOUNCEMENT_TITLE = "tiutle",
        //                ANNOUNCEMENT_DETAILS = objAnnouncement.ANNOUNCEMENT_DETAILS,
        //                CREATED_BY = profile.UserName,
        //                CREATED_DATE = Helper.GetCurrentDate(),
        //                DELETED = false,
        //                MODIFIED_BY = profile.UserName,
        //                MODIFIED_DATE = Helper.GetCurrentDate(),


        //                ANNOUNCEMENT_DATE_FROM = Helper.GetCurrentDate(),
        //                ANNOUNCEMENT_DATE_TO = Helper.GetCurrentDate(),
        //                ROLE_ID = 6565,
        //                PRACTICE_CODE = profile.PracticeCode,
        //            };
        //            //ExistingDetailInfo.ANNOUNCEMENT_ID = Helper.getMaximumId("ANNOUNCEMENT_ID");
        //            //ExistingDetailInfo.ANNOUNCEMENT_TITLE = objAnnouncement.ANNOUNCEMENT_TITLE;
        //            //ExistingDetailInfo.ANNOUNCEMENT_DATE_FROM = Helper.GetCurrentDate();
        //            //ExistingDetailInfo.ANNOUNCEMENT_DATE_TO = Helper.GetCurrentDate();
        //            //ExistingDetailInfo.ANNOUNCEMENT_DETAILS = objAnnouncement.ANNOUNCEMENT_DETAILS;
        //            //ExistingDetailInfo.PRACTICE_CODE = profile.PracticeCode;
        //            //ExistingDetailInfo.CREATED_BY = profile.UserName;
        //            //ExistingDetailInfo.CREATED_DATE = Helper.GetCurrentDate();
        //            //ExistingDetailInfo.DELETED = true;
        //            _announcementRepository.Insert(newAccouncment);
        //            _announcementRepository.Save();
        //        }
        //        else
        //        {
        //            ExistingDetailInfo.ANNOUNCEMENT_TITLE = objAnnouncement.ANNOUNCEMENT_TITLE;
        //            ExistingDetailInfo.ANNOUNCEMENT_DATE_FROM = Helper.GetCurrentDate();
        //            ExistingDetailInfo.ANNOUNCEMENT_DATE_TO = Helper.GetCurrentDate();
        //            ExistingDetailInfo.ANNOUNCEMENT_DETAILS = objAnnouncement.ANNOUNCEMENT_DETAILS;
        //            ExistingDetailInfo.PRACTICE_CODE = profile.PracticeCode;
        //            ExistingDetailInfo.CREATED_BY = profile.UserName;
        //            ExistingDetailInfo.CREATED_DATE = Helper.GetCurrentDate();
        //            ExistingDetailInfo.DELETED = true;
        //            _announcementRepository.Update(ExistingDetailInfo);
        //            _announcementRepository.Save();

        //        }

        //        ResponseModel response = new ResponseModel()
        //        {
        //            ErrorMessage = "",
        //            Message = "Inserted",
        //            Success = true
        //        };
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //        //ResponseModel response = new ResponseModel()
        //        //{
        //        //    ErrorMessage = "",
        //        //    Message = "Not Inserted",
        //        //    Success = false
        //        //};
        //        //return response;
        //    }
        //}


    }
}
