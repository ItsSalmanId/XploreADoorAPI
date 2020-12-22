using FOX.BusinessOperations.CommonService;
using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.HrAutoEmail;
using FOX.DataModels.Models.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FOX.BusinessOperations.HrAutoEmail
{
    public class HrAutoEmailService : IHrAutoEmailService
    {
        private readonly DBContextHrAutoEmail _dbContextHrAutoEmail = new DBContextHrAutoEmail();
        private readonly GenericRepository<HrAutoEmailConfigure> _hrAutoEmailRepository;
        private readonly GenericRepository<HrEmailDocumentFileAll> _hrEmailDocumentFileAll;
        ResponseModel response = new ResponseModel();

        public HrAutoEmailService()
        {
            _hrAutoEmailRepository = new GenericRepository<HrAutoEmailConfigure>(_dbContextHrAutoEmail);
            _hrEmailDocumentFileAll = new GenericRepository<HrEmailDocumentFileAll>(_dbContextHrAutoEmail);
        }
        /// <summary>
        /// This Function is used to Save Record against 
        /// the AutoEmailConfigure
        /// </summary>
        /// <param name="hrAutoEmail"></param>
        /// <param name="userProfile"></param>
        /// <returns></returns>
        public ResponseModel AddHrAutoEmailConfigure(HrAutoEmailConfigure hrAutoEmail, UserProfile userProfile)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                if (hrAutoEmail != null && hrAutoEmail.NAME != null && !string.IsNullOrWhiteSpace(hrAutoEmail.NAME))
                {
                    var result = _hrAutoEmailRepository.GetAll().Where(h => h.DELETED == false && h.PRACTICE_CODE == userProfile.PracticeCode).Select(s => s.NAME.Trim().ToLower()).Contains(hrAutoEmail.NAME.Trim().ToLower());

                    if(!result)
                    {
                        hrAutoEmail.HR_CONFIGURE_ID = Helper.getMaximumId("HR_CONFIGURE_ID");
                        hrAutoEmail.PRACTICE_CODE = userProfile.PracticeCode;
                        hrAutoEmail.CREATED_BY = userProfile.UserName;
                        hrAutoEmail.MODIFIED_BY = userProfile.UserName;
                        hrAutoEmail.CREATED_DATE = hrAutoEmail.MODIFIED_DATE = Helper.GetCurrentDate();
                        hrAutoEmail.DELETED = false;
                        hrAutoEmail.IS_ENABLED = false;
                        _hrAutoEmailRepository.Insert(hrAutoEmail);
                        _hrAutoEmailRepository.Save();

                        response.Success = true;
                        response.ErrorMessage = "";
                        response.Message = "HR Auto Email Configure Added successfully";
                    }
                    else
                    {
                        response.Success = false;
                        response.ErrorMessage = "";
                        response.Message = "Failed to Add Hr Auto Email Records";
                    }
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = "";
                response.Message = "Not Added";
            }
            return response;
        }
        /// <summary>
        /// This Function is used to Delete Record
        /// </summary>
        /// <param name="hrAutoEmail"></param>
        /// <param name="userProfile"></param>
        /// <returns></returns>
        public ResponseModel DeleteHrAutoEmailConfigure(HrAutoEmailConfigure hrAutoEmail, UserProfile userProfile)
        {
            try
            {
                var ExistingDetailInfo = _hrAutoEmailRepository.GetFirst(r => r.HR_CONFIGURE_ID == hrAutoEmail.HR_CONFIGURE_ID && r.DELETED == false);

                if (ExistingDetailInfo != null)
                {
                    ExistingDetailInfo.MODIFIED_BY = userProfile.UserName;
                    ExistingDetailInfo.MODIFIED_DATE = Helper.GetCurrentDate();
                    ExistingDetailInfo.DELETED = true;
                    ExistingDetailInfo.IS_ENABLED = false;
                    _hrAutoEmailRepository.Update(ExistingDetailInfo);
                    _hrAutoEmailRepository.Save();

                    response.Success = true;
                    response.ErrorMessage = "";
                    response.Message = "HR Auto Email Configure Added successfully";
                }
            }
            catch (Exception)
            {
                response.Success = false;
                response.ErrorMessage = "";
                response.Message = "Not Deleted";
            }
            return response;
        }
        /// <summary>
        /// This Function is trigger to Enable Certificate
        /// </summary>
        /// <param name="hrAutoEmailConfigure"></param>
        /// <param name="userProfile"></param>
        /// <returns></returns>
        public ResponseModel EnableHrAutoEmailCertificate(HrAutoEmailConfigure hrAutoEmailConfigure, UserProfile userProfile)
        {
            try
            {
                var ExistingDetailInfo = _hrAutoEmailRepository.GetFirst(r => r.HR_CONFIGURE_ID == hrAutoEmailConfigure.HR_CONFIGURE_ID && !r.DELETED && hrAutoEmailConfigure.PRACTICE_CODE == userProfile.PracticeCode);

                if (ExistingDetailInfo != null)
                {
                    ExistingDetailInfo.MODIFIED_BY = userProfile.UserName;
                    ExistingDetailInfo.MODIFIED_DATE = Helper.GetCurrentDate();
                    ExistingDetailInfo.IS_ENABLED = hrAutoEmailConfigure.IS_ENABLED;
                    _hrAutoEmailRepository.Update(ExistingDetailInfo);
                    _hrAutoEmailRepository.Save();

                    response.Success = true;
                    response.ErrorMessage = "";
                    response.Message = "HR Auto Email Configure Updated successfully";
                }
            }
            catch (Exception)
            {
                response.Success = false;
                response.ErrorMessage = "";
                response.Message = "Not Updated";
                throw;
            }
            return response;
        }

        /// <summary>
        /// Get Hr Auto Email Distinct Names
        /// </summary>
        /// <param name="userProfile"></param>
        /// <returns></returns>
        public IEnumerable<HrAutoEmailConfigure> GetHrAutoEmailConfigureRecords(UserProfile userProfile)
        {
            IEnumerable<HrAutoEmailConfigure> distinctNames = new List<HrAutoEmailConfigure>();
            try
            {
                if (_hrAutoEmailRepository != null)
                {
                    distinctNames = _hrAutoEmailRepository.GetAll().Where(s => s.PRACTICE_CODE == userProfile.PracticeCode && !s.DELETED).Select(s => new HrAutoEmailConfigure
                    {
                        NAME = s.NAME,
                        IS_ENABLED = s.IS_ENABLED,
                        HR_CONFIGURE_ID = s.HR_CONFIGURE_ID,
                        PRACTICE_CODE = s.PRACTICE_CODE,
                        CREATED_DATE = s.CREATED_DATE
                    }).Distinct().ToList().OrderByDescending(s =>s.CREATED_DATE);
                }
                return distinctNames;
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// This is used to trigger to get Hr Auto Email By Id
        /// </summary>
        /// <param name="hrAutoEmail"></param>
        /// <param name="userProfile"></param>
        /// <returns></returns>
        public IEnumerable<HrEmailDocumentFileAll> GetHrAutoEmalById(int hrAutoEmail, UserProfile userProfile)
        {
            try
            {
                IEnumerable<HrEmailDocumentFileAll> distinctNames = new List<HrEmailDocumentFileAll>();
                if (_hrAutoEmailRepository != null)
                {
                    distinctNames = _hrEmailDocumentFileAll.GetAll().Where(s => s.PRACTICE_CODE == userProfile.PracticeCode && !s.DELETED && s.HR_CONFIGURE_ID == hrAutoEmail).Select(s => new HrEmailDocumentFileAll
                    {
                        DOCUMENT_PATH = s.DOCUMENT_PATH,
                        ORIGINAL_FILE_NAME = s.ORIGINAL_FILE_NAME,
                        HR_CONFIGURE_ID = s.HR_CONFIGURE_ID,
                        PRACTICE_CODE = s.PRACTICE_CODE,
                    }).Distinct().ToList();
                }
                return distinctNames;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// This Function is used to Get Full File Path 
        /// </summary>
        /// <param name="userProfile"></param>
        /// <returns></returns>
        public IEnumerable<GetDocumentFileDetails> GetMTBCDocumentFileDetails(UserProfile userProfile)
        {
            try
            {
                SqlParameter praticeCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = userProfile.PracticeCode };
                List<GetDocumentFileDetails> result = SpRepository<GetDocumentFileDetails>.GetListWithStoreProcedure(@"EXEC [FOX_PROC_GET_HR_EMAIL_DOCUMENT_FILE_DETAILS] @PRACTICE_CODE", praticeCode);
                if (result == null)
                {
                    return result = new List<GetDocumentFileDetails>();
                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// This function is used to Get Un Mapped Category
        /// </summary>
        /// <param name="userProfile"></param>
        /// <returns></returns>
        public List<string> GetMTBCUnMappedCategory(UserProfile userProfile)
        {
            try
            {
                SqlParameter praticeCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = userProfile.PracticeCode };
                List<string> result = SpRepository<string>.GetListWithStoreProcedure(@"EXEC [FOX_PROC_GET_DISTINCT_MTBC_UNMAPPED_CATEGORY] @PRACTICE_CODE", praticeCode);
                if (result == null)
                {
                    return result = new List<string>();
                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// This Function is used to Save Record of HRAUTOEMAIL
        /// </summary>
        /// <param name="hrAutoEmailConfigure"></param>
        /// <param name="Profile"></param>
        /// <returns></returns>
        public ResponseModel SaveHrMTBCEMailDocumentFiles(List<HrEmailDocumentFileAll> hrEmailDocumentFileAll, UserProfile user)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                if (hrEmailDocumentFileAll != null)
                {
                    var basePath = hrEmailDocumentFileAll[0].DOCUMENT_PATH.Substring(0, hrEmailDocumentFileAll[0].DOCUMENT_PATH.IndexOf("HRAutoEmailAttachmentFiles"));
                    if (!string.IsNullOrWhiteSpace(basePath))
                    {
                        basePath = basePath + @"\HRAutoEmailAttachmentFiles";

                        var configureId = hrEmailDocumentFileAll[0].HR_CONFIGURE_ID;

                        if(configureId != null)
                        {
                            var result = _hrAutoEmailRepository.GetFirst(f => f.HR_CONFIGURE_ID == configureId && !f.DELETED).NAME;
                            string uploadFilesPath = basePath + "\\" + result;

                            if (Directory.Exists(uploadFilesPath))
                            {
                                var _hrEmailDocumentFileAlls = _hrEmailDocumentFileAll.GetMany(m => m.HR_CONFIGURE_ID == configureId && !m.DELETED);
                                foreach (var item in _hrEmailDocumentFileAlls)
                                {
                                    item.DELETED = true;
                                    item.MODIFIED_DATE = Helper.GetCurrentDate();
                                    item.MODIFIED_BY = user.UserName;

                                    _hrEmailDocumentFileAll.Update(item);
                                    _hrEmailDocumentFileAll.Save();

                                    File.Delete(item.DOCUMENT_PATH);
                                }
                                //Directory.Delete(uploadFilesPath, true);
                            }

                        }
                    }                   
                    foreach (var item in hrEmailDocumentFileAll)
                    {
                        item.HR_MTBC_EMAIL_DOCUMENT_FILE_ID = Helper.getMaximumId("HR_MTBC_EMAIL_DOCUMENT_FILE_ID");
                        item.PRACTICE_CODE = user.PracticeCode;
                        item.CREATED_BY = user.UserName;
                        item.MODIFIED_BY = user.UserName;
                        item.CREATED_DATE = item.MODIFIED_DATE = Helper.GetCurrentDate();
                        item.DOCUMENT_PATH = item.DOCUMENT_PATH;
                        item.ORIGINAL_FILE_NAME = item.ORIGINAL_FILE_NAME;
                        _hrEmailDocumentFileAll.Insert(item);
                        _hrEmailDocumentFileAll.Save();
                    }
                    response.Success = true;
                    response.ErrorMessage = "";
                    response.Message = "HR Auto Email Document Files Added successfully";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = "";
                response.Message = "Not Added";
            }
            return response;
        }
        /// <summary>
        /// This Function is used to Update Records
        /// </summary>
        /// <param name="hrAutoEmail"></param>
        /// <param name="userProfile"></param>
        /// <returns></returns>
        public ResponseModel UpdateHrAutoEmailConfigure(HrAutoEmailConfigure hrAutoEmail, UserProfile userProfile)
        {
            try
            {
                var ExistingHrAutoEmailDetail = _hrAutoEmailRepository.GetFirst(r => r.HR_CONFIGURE_ID == hrAutoEmail.HR_CONFIGURE_ID && r.DELETED == false);

                var newStr = Regex.Replace(hrAutoEmail.NAME, " {2,}", " ");
                var result = _hrAutoEmailRepository.GetAll().Where(h => h.PRACTICE_CODE == userProfile.PracticeCode).Select(s => s.NAME.Trim().ToLower()).Contains(newStr.Trim().ToLower());

                if (ExistingHrAutoEmailDetail.NAME.Trim().ToLower() == newStr.Trim().ToLower() && result == true)
                {
                    response.Success = false;
                    response.ErrorMessage = "";
                    response.Message = "HR Auto Email Configure Note Updated.";
                }
                else if (ExistingHrAutoEmailDetail.NAME.Trim().ToLower() != newStr.Trim().ToLower() &&  result == true )
                {
                    response.Success = false;
                    response.ErrorMessage = "";
                    response.Message = "HR Auto Email Configure Note Updated.";
                }
                else if(ExistingHrAutoEmailDetail != null && ExistingHrAutoEmailDetail.NAME != null && !string.IsNullOrWhiteSpace(ExistingHrAutoEmailDetail.NAME))
                {
                    ExistingHrAutoEmailDetail.NAME = hrAutoEmail.NAME;
                    ExistingHrAutoEmailDetail.MODIFIED_DATE = Helper.GetCurrentDate();
                    ExistingHrAutoEmailDetail.MODIFIED_BY = userProfile.UserName;
                    _hrAutoEmailRepository.Update(ExistingHrAutoEmailDetail);
                    _hrAutoEmailRepository.Save();

                    response.Success = true;
                    response.ErrorMessage = "";
                    response.Message = "HR Auto Email Configure Updated successfully";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = "";
                response.Message = "Not Updated";
            }
            return response;
        }
    }
}
