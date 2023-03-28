using System;
using System.Collections.Generic;
using System.Linq;
using FOX.DataModels.Models.Security;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Context;
using FOX.BusinessOperations.CommonService;
using System.Data.SqlClient;
using FOX.DataModels.Models.GeneralNotesModel;
using System.Data;
using FOX.DataModels.Models.TasksModel;
using FOX.DataModels.Models.CasesModel;
using FOX.DataModels.Models.Patient;
using System.Web;
using FOX.BusinessOperations.CommonServices;
using System.IO;

namespace FOX.BusinessOperations.GeneralNotesService
{
    public class GeneralNotesServices : IGeneralNotesServices
    {
        private readonly DbContextCases _CaseContext = new DbContextCases();
        private readonly GenericRepository<FOX_TBL_GENERAL_NOTE> _generalNotesRepository;
        private readonly DbContextPatient _patientContext = new DbContextPatient();
        private readonly DbContextCases _caseContext = new DbContextCases();
        private readonly DbContextSecurity _securityContext = new DbContextSecurity();
        private readonly GenericRepository<User> _userRepository;
        private readonly GenericRepository<AlertType> _alertTypeRepository;
        private GenericRepository<FOX_TBL_TASK> _taskRepository;
        private readonly GenericRepository<FOX_VW_CASE> _vwCaseRepository;
        private readonly GenericRepository<NoteAlert> _noteAlertRepository;
        private readonly GenericRepository<InterfaceSynchModel> __InterfaceSynchModelRepository;
        private readonly GenericRepository<GeneralNotesInterfaceLog> __InterfaceLogModelRepository;
        private readonly GenericRepository<Patient> _PatientRepository;

        public GeneralNotesServices()
        {
            _generalNotesRepository = new GenericRepository<FOX_TBL_GENERAL_NOTE>(_patientContext);
            _taskRepository = new GenericRepository<FOX_TBL_TASK>(_caseContext);
            _userRepository = new GenericRepository<User>(_securityContext);
            _alertTypeRepository = new GenericRepository<AlertType>(_patientContext);
            _noteAlertRepository = new GenericRepository<NoteAlert>(_patientContext);
            _vwCaseRepository = new GenericRepository<FOX_VW_CASE>(_caseContext);
            __InterfaceSynchModelRepository = new GenericRepository<InterfaceSynchModel>(_CaseContext);
            __InterfaceLogModelRepository = new GenericRepository<GeneralNotesInterfaceLog>(_patientContext);
            _PatientRepository = new GenericRepository<Patient>(_patientContext);
        }
        public List<FOX_TBL_GENERAL_NOTE> GetGeneralNotes(UserProfile profile, GeneralNotesSearchRequest request)
        {
            try
            {
                var practiceCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = System.Data.SqlDbType.BigInt, Value = profile.PracticeCode };
                var currentPage = new SqlParameter { ParameterName = "CURRENT_PAGE", SqlDbType = System.Data.SqlDbType.Int, Value = request.CurrentPage };
                var recordsPerPage = new SqlParameter { ParameterName = "RECORDS_PER_PAGE", SqlDbType = System.Data.SqlDbType.Int, Value = request.RecordPerPage };
                var searchText = new SqlParameter { ParameterName = "SEARCH_TEXT", Value = request.SearchText };
                var patientAccount = new SqlParameter { ParameterName = "PATIENT_ACCOUNT", SqlDbType = System.Data.SqlDbType.BigInt, Value = request.PATIENT_ACCOUNT };
                var sortBy = new SqlParameter { ParameterName = "SORT_BY", Value = request.Sort_By };
                var sortOrder = new SqlParameter { ParameterName = "SORT_ORDER", Value = request.Sort_Order };
                var result = SpRepository<FOX_TBL_GENERAL_NOTE>.GetListWithStoreProcedure(@" exec [FOX_PROC_GET_ALL_GENERAL_NOTES] @PRACTICE_CODE, @CURRENT_PAGE, @RECORDS_PER_PAGE, @SEARCH_TEXT, @PATIENT_ACCOUNT,@SORT_BY,@SORT_ORDER", practiceCode, currentPage, recordsPerPage, searchText, patientAccount, sortBy, sortOrder);
                if (result != null && result.Count > 0)
                {
                    foreach (var note in result)
                    {
                        var task = _taskRepository.GetFirst(e => e.PRACTICE_CODE == profile.PracticeCode && !e.DELETED &&
                            e.GENERAL_NOTE_ID.HasValue && e.GENERAL_NOTE_ID.Value == note.PARENT_GENERAL_NOTE_ID);
                        if (task != null)
                        {
                            //if (task.IS_COMPLETED_INT == 2)
                            if (task.IS_FINALROUTE_MARK_COMPLETE && task.IS_SENDTO_MARK_COMPLETE)
                                note.IS_GREEN = true;
                            else
                                note.IS_YELLOW = true;
                        }
                        else
                        {
                            note.IS_GREEN = false;
                            note.IS_YELLOW = false;
                        }
                        var vrHistory = _generalNotesRepository.GetMany(h => h.PARENT_GENERAL_NOTE_ID == note.GENERAL_NOTE_ID && !h.DELETED && h.PRACTICE_CODE == note.PRACTICE_CODE);
                        if (vrHistory != null && vrHistory.Count > 0)
                        {
                            var vrNoteHistory = (from n in vrHistory
                                                 join u in _securityContext.Users on n.CREATED_BY equals u.USER_NAME into un
                                                 from u in un.DefaultIfEmpty()
                                                 select new FOX_TBL_GENERAL_NOTE
                                                 {
                                                     CREAETED_BY_FIRST_NAME = u != null ? u.FIRST_NAME : "",
                                                     CREATED_BY_LAST_NAME = u != null ? u.LAST_NAME : "",
                                                     CREATED_BY_FULL_NAME = u != null ? u.LAST_NAME != null && u.LAST_NAME != "" ? u.LAST_NAME + ", " + u.FIRST_NAME : u.FIRST_NAME : "",
                                                     CREATED_BY = n.CREATED_BY,
                                                     PRACTICE_CODE = n.PRACTICE_CODE,
                                                     DELETED = n.DELETED,
                                                     CASE_ID = n.CASE_ID != null ? n.CASE_ID : 0,
                                                     CREATED_DATE = n.CREATED_DATE,
                                                     GENERAL_NOTE_ID = n.GENERAL_NOTE_ID,
                                                     NOTE_DESCRIPTION = n.NOTE_DESCRIPTION,
                                                     PATIENT_ACCOUNT = n.PATIENT_ACCOUNT,
                                                     PARENT_GENERAL_NOTE_ID = n.PARENT_GENERAL_NOTE_ID,
                                                     MODIFIED_BY = n.MODIFIED_BY,
                                                     MODIFIED_DATE = n.MODIFIED_DATE
                                                 }
                           ).ToList();
                            if (vrNoteHistory != null && vrNoteHistory.Count > 0)
                            {
                                note.LAST_REPLY_BY = vrNoteHistory.Last().CREATED_BY_FULL_NAME;
                                note.LAST_REPLY_ON = vrNoteHistory.Last().CREATED_DATE;
                                note.NOTE_REPLIES = vrNoteHistory;
                                note.NOTE_REPLIES.RemoveAt(0);
                            }
                        }
                    }
                }
                return result;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public List<NoteAlert> GetAlertGeneralNotes(UserProfile profile, string patientAccount)
        {
            long longPatientAccount;
            if (long.TryParse(patientAccount, out longPatientAccount))
            {
                var _paramsPracticeCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = System.Data.SqlDbType.BigInt, Value = profile.PracticeCode };
                var _paramsPatientAccount = new SqlParameter { ParameterName = "PATIENT_ACCOUNT", SqlDbType = System.Data.SqlDbType.BigInt, Value = longPatientAccount };
                return SpRepository<NoteAlert>.GetListWithStoreProcedure(@"exec [FOX_PROC_GET_ALL_ALERT_GENERAL_NOTES] @PRACTICE_CODE,@PATIENT_ACCOUNT", _paramsPracticeCode, _paramsPatientAccount);
            }
            else
            {
                return new List<NoteAlert>();
            }
        }
        public FOX_TBL_GENERAL_NOTE GetSingleNoteForUpdate(UserProfile profile, GeneralNoteRequestModel request)
        {
            FOX_TBL_GENERAL_NOTE res = new FOX_TBL_GENERAL_NOTE();
            res = _generalNotesRepository.GetSingle(h => h.GENERAL_NOTE_ID == request.GENERAL_NOTE_ID && !h.DELETED);
            if (res != null) {
                var usr = _userRepository.GetFirst(e => e.USER_NAME == res.CREATED_BY && !e.DELETED && e.PRACTICE_CODE == profile.PracticeCode && e.IS_ACTIVE);
                if (usr != null) {
                    res.CREAETED_BY_FIRST_NAME = usr.FIRST_NAME;
                    res.CREATED_BY_LAST_NAME= usr.LAST_NAME;
                    if (res.CREATED_BY_LAST_NAME != null && res.CREATED_BY_LAST_NAME != "")
                    {
                        res.CREATED_BY_FULL_NAME = usr.LAST_NAME + ", " + usr.FIRST_NAME;
                    }
                    else
                    {
                        res.CREATED_BY_FULL_NAME = usr.FIRST_NAME;
                    }
                }
            }
            var casNo = _vwCaseRepository.GetFirst(e => e.CASE_ID == res.CASE_ID && !e.DELETED && e.PRACTICE_CODE == profile.PracticeCode)?.CASE_NO ?? "";
            if (casNo != null)
            {
                res.CASE_NO = casNo;
            }
            return res;
        }
        public GeneralNoteResponseModel GetSingleGeneralNote(UserProfile profile, GeneralNoteRequestModel request)
        {
            GeneralNoteResponseModel toReturn = new GeneralNoteResponseModel();
            var PatientAccount = Convert.ToInt64(request.PATIENT_ACCOUNT_AS_STRING);
            try
            {
                var vrNote = (from n in _patientContext.GeneralNotes
                              join c in _patientContext.Case on n.CASE_ID equals c.CASE_ID into nc
                              from c in nc.DefaultIfEmpty()
                              join h in _patientContext.GeneralNotes on n.GENERAL_NOTE_ID equals h.PARENT_GENERAL_NOTE_ID into nh
                              from h in nh.DefaultIfEmpty()
                              where n.PRACTICE_CODE == profile.PracticeCode && n.PATIENT_ACCOUNT == PatientAccount && n.GENERAL_NOTE_ID == request.GENERAL_NOTE_ID && n.DELETED == false
                              select new GeneralNoteResponseModel()
                              {
                                  Case = c.CASE_NO,
                                  Success = true,
                                  ErrorMessage = "Note and it's history has been retrieved successfully.",
                                  Message = "Note and it's history has been retrieved successfully.",
                                  Note = n,
                                  NoteHistory = nh
                              }).FirstOrDefault();
                var noteHistory = (from n in vrNote.NoteHistory
                                   join u in _securityContext.Users on n.CREATED_BY equals u.USER_NAME into un
                                   from u in un.DefaultIfEmpty()
                                   select new FOX_TBL_GENERAL_NOTE()
                                   {
                                       CASE_ID = n.CASE_ID,
                                       CREAETED_BY_FIRST_NAME = u.FIRST_NAME,
                                       CREATED_BY_LAST_NAME = u.LAST_NAME,
                                       CREATED_BY_FULL_NAME = u.LAST_NAME != null && u.LAST_NAME != "" ? u.LAST_NAME + ", " + u.FIRST_NAME : u.FIRST_NAME,
                                       CREATED_BY = n.CREATED_BY,
                                       CREATED_DATE = n.CREATED_DATE,
                                       DELETED = n.DELETED,
                                       GENERAL_NOTE_ID = n.GENERAL_NOTE_ID,
                                       MODIFIED_BY = n.MODIFIED_BY,
                                       MODIFIED_DATE = n.MODIFIED_DATE,
                                       NOTE_DESCRIPTION = n.NOTE_DESCRIPTION,
                                       PATIENT_ACCOUNT = n.PATIENT_ACCOUNT,
                                       PARENT_GENERAL_NOTE_ID = n.PARENT_GENERAL_NOTE_ID,
                                       PRACTICE_CODE = n.PRACTICE_CODE
                                   }).ToList();
                if (vrNote != null)
                {
                    vrNote.NoteHistory = noteHistory;
                    vrNote.Note.CREAETED_BY_FIRST_NAME = noteHistory[0].CREAETED_BY_FIRST_NAME;
                    vrNote.Note.CREATED_BY_LAST_NAME = noteHistory[0].CREATED_BY_LAST_NAME;
                    vrNote.Note.CREATED_BY_FULL_NAME = noteHistory[0].CREATED_BY_FULL_NAME;
                    toReturn = vrNote;
                    toReturn.NoteHistory = toReturn.NoteHistory;
                }
                else
                {
                    toReturn.Message = "No record found";
                    toReturn.ErrorMessage = "No record found";
                    toReturn.Success = true;
                }
            }
            catch (Exception ex)
            {
                toReturn.Message = ex.Message;
                toReturn.ErrorMessage = ex.Message;
                toReturn.Success = false;
            }
            return toReturn;
        }
        public ResponseModel DeleteGeneralNote(UserProfile profile, GeneralNoteDeleteRequestModel request)
        {
            ResponseModel toReturn = null;
            var noteId = Convert.ToInt64(request.GENERAL_NOTE_ID_AS_STRING);
            try
            {

                var note = _generalNotesRepository.GetFirst(n => n.GENERAL_NOTE_ID == noteId && !n.DELETED && n.PRACTICE_CODE == profile.PracticeCode);
                if (note != null)
                {
                    var historyList = _generalNotesRepository.GetMany(h => h.PARENT_GENERAL_NOTE_ID == noteId && !h.DELETED && h.PRACTICE_CODE == profile.PracticeCode);
                    if (historyList.Count > 0)
                    {
                        foreach (var hist in historyList)
                        {
                            hist.DELETED = true;
                        }
                        _generalNotesRepository.Save();
                    }
                    toReturn = new ResponseModel()
                    {
                        ErrorMessage = "",
                        ID = noteId.ToString(),
                        Message = "Delete Success",
                        Success = true
                    };
                }
                if (request.IS_TASK_DELETE)
                {
                    var task = _taskRepository.GetFirst(t => t.GENERAL_NOTE_ID == noteId && !t.DELETED && t.PRACTICE_CODE == profile.PracticeCode);
                    if (task != null)
                    {
                        task.DELETED = true;
                        _taskRepository.Save();
                        toReturn = new ResponseModel()
                        {
                            ErrorMessage = "",
                            ID = noteId.ToString(),
                            Message = "Note and it's related task has been deleted.",
                            Success = true
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                toReturn = new ResponseModel()
                {
                    ErrorMessage = ex.Message.ToString(),
                    ID = noteId.ToString(),
                    Message = "Failed to delete.",
                    Success = false
                };
            }
            return toReturn;
        }
        public ResponseModel CreateUpdateNote(GeneralNoteCreateUpdateRequestModel request, UserProfile profile)
        {
            try
            {
                InterfaceSynchModel interfaceSynch = new InterfaceSynchModel();
                string ID = string.Empty;
                var patientAccount = Convert.ToInt64(request.GENERAL_NOTE.PATIENT_ACCOUNT_AS_STRING);
                interfaceSynch.PATIENT_ACCOUNT = patientAccount;
                var note = _generalNotesRepository.GetFirst(n => n.GENERAL_NOTE_ID == request.GENERAL_NOTE.GENERAL_NOTE_ID && !n.DELETED && n.PRACTICE_CODE == profile.PracticeCode && n.PATIENT_ACCOUNT == patientAccount);
                if (note != null)
                {
                    var CASE_ID = request.GENERAL_NOTE.CASE_ID != null ? request.GENERAL_NOTE.CASE_ID : null;
                    if (CASE_ID != null && note.CASE_ID == null)
                    {
                        var existingHistory = _generalNotesRepository.GetMany(n => n.PARENT_GENERAL_NOTE_ID == note.GENERAL_NOTE_ID && n.PRACTICE_CODE == profile.PracticeCode && !n.DELETED);
                        foreach (var item in existingHistory)
                        {
                            item.MODIFIED_BY = profile.UserName;
                            item.MODIFIED_DATE = Helper.GetCurrentDate();
                            item.CASE_ID = CASE_ID;
                            _generalNotesRepository.Update(item);
                            //Task 149402:Dev Task: FOX-RT 105. Disabling editing of patient info. from RFO, Usman Nasir
                            //InsertInterfaceTeamData(interfaceSynch, profile);
                        }
                    }
                    if (!string.IsNullOrEmpty(request.GENERAL_NOTE_HISTORY.NOTE_DESCRIPTION.Trim()))
                    {
                        var noteHistory = new FOX_TBL_GENERAL_NOTE()
                        {
                            GENERAL_NOTE_ID = Helper.getMaximumId("GENERAL_NOTE_ID"),
                            CREATED_BY = profile.UserName,
                            CREATED_DATE = Helper.GetCurrentDate(),
                            DELETED = false,
                            MODIFIED_BY = profile.UserName,
                            MODIFIED_DATE = Helper.GetCurrentDate(),
                            NOTE_DESCRIPTION = request.GENERAL_NOTE_HISTORY.NOTE_DESCRIPTION,
                            PARENT_GENERAL_NOTE_ID = note.GENERAL_NOTE_ID,
                            PRACTICE_CODE = profile.PracticeCode,
                            PATIENT_ACCOUNT = patientAccount,
                            CASE_ID = CASE_ID,
                        };
                        interfaceSynch.TASK_ID = noteHistory.TASK_ID;
                        interfaceSynch.CASE_ID = noteHistory.CASE_ID;
                        //Task 149402:Dev Task: FOX-RT 105. Disabling editing of patient info. from RFO, Usman Nasir
                        //InsertInterfaceTeamData(interfaceSynch, profile);
                        _generalNotesRepository.Insert(noteHistory);
                    }
                    _generalNotesRepository.Save();

                }
                else
                {
                    var newNoteId = Helper.getMaximumId("GENERAL_NOTE_ID");
                    var newNote = new FOX_TBL_GENERAL_NOTE()
                    {

                        GENERAL_NOTE_ID = newNoteId,
                        CREATED_BY = profile.UserName,
                        CREATED_DATE = Helper.GetCurrentDate(),
                        DELETED = false,
                        MODIFIED_BY = profile.UserName,
                        MODIFIED_DATE = Helper.GetCurrentDate(),
                        NOTE_DESCRIPTION = request.GENERAL_NOTE.NOTE_DESCRIPTION,
                        PRACTICE_CODE = profile.PracticeCode,
                        PATIENT_ACCOUNT = patientAccount,
                        CASE_ID = request.GENERAL_NOTE.CASE_ID,
                        PARENT_GENERAL_NOTE_ID = newNoteId,
                    };
                    interfaceSynch.TASK_ID = newNote.TASK_ID;
                    interfaceSynch.CASE_ID = newNote.CASE_ID;
                    _generalNotesRepository.Insert(newNote);
                    _generalNotesRepository.Save();
                    //Task 149402:Dev Task: FOX-RT 105. Disabling editing of patient info. from RFO, Usman Nasir
                    //InsertInterfaceTeamData(interfaceSynch, profile);
                    ID = newNote.GENERAL_NOTE_ID.ToString();
                }
                return new ResponseModel()
                {
                    ErrorMessage = "",
                    Message = "Note Successfully created/updated",
                    Success = true,
                    ID = ID
                };
            }
            catch (Exception e)
            {
                return new ResponseModel()
                {
                    ErrorMessage = e.Message.ToString(),
                    Message = "Exception occured, returned from catch block.",
                    Success = false
                };
            }
        }

        public ResponseModel UpdateNote(GeneralNoteCreateUpdateRequestModel request, UserProfile profile)
        {
            
            InterfaceSynchModel interfaceSynch = new InterfaceSynchModel();
            string ID = string.Empty;
            var patientAccount = Convert.ToInt64(request.GENERAL_NOTE.PATIENT_ACCOUNT_AS_STRING);
            interfaceSynch.PATIENT_ACCOUNT = patientAccount;
            var note = _generalNotesRepository.GetFirst(n => n.GENERAL_NOTE_ID == request.GENERAL_NOTE.GENERAL_NOTE_ID && !n.DELETED && n.PRACTICE_CODE == profile.PracticeCode && n.PATIENT_ACCOUNT == patientAccount);
            var ChildNote = _generalNotesRepository.GetMany(x => x.PARENT_GENERAL_NOTE_ID == request.GENERAL_NOTE.GENERAL_NOTE_ID).ToList();        
                        
            if (note == null)
            {

            }
            else
            {
                note.GENERAL_NOTE_ID = request.GENERAL_NOTE.GENERAL_NOTE_ID;
                note.NOTE_DESCRIPTION = request.GENERAL_NOTE.NOTE_DESCRIPTION;
                note.PRACTICE_CODE = profile.PracticeCode;
                note.MODIFIED_BY = profile.UserName;
                if (ChildNote.Count > 1)
                {
                    for (int i = 0; i < ChildNote.Count(); i++)
                    {
                        ChildNote[i].CASE_ID = request.GENERAL_NOTE.CASE_ID;
                        _generalNotesRepository.Update(ChildNote[i]);
                    }
                }
                else
                {
                    note.CASE_ID = request.GENERAL_NOTE.CASE_ID;
                }
                note.TASK_ID = request.GENERAL_NOTE.TASK_ID;
                note.MODIFIED_DATE = Helper.GetCurrentDate();
                note.DELETED = false;

                interfaceSynch.TASK_ID = request.GENERAL_NOTE.TASK_ID;
                interfaceSynch.CASE_ID = request.GENERAL_NOTE.CASE_ID;
                //Task 149402:Dev Task: FOX-RT 105. Disabling editing of patient info. from RFO, Usman Nasir
                //InsertInterfaceTeamData(interfaceSynch, profile);
                _generalNotesRepository.Update(note);
            }
            try
            {
                _generalNotesRepository.Save();
                return new ResponseModel()
                {
                    Success = true,
                    ErrorMessage = "note has been updated successfully.",
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel()
                {
                    Success = false,
                    ErrorMessage = ex.ToString(),

                };
            }

        }
        public GeneralNoteHistoryResponseModel GetGeneralNoteHistory(GeneralNoteHistoryRequestModel request, UserProfile profile)
        {
            GeneralNoteHistoryResponseModel toReturn = new GeneralNoteHistoryResponseModel();
            var patientAccount = Convert.ToInt64(request.PATIENT_ACCOUNT_AS_STRING);
            try
            {
                var vrHistory = _generalNotesRepository.GetMany(h => h.PARENT_GENERAL_NOTE_ID == request.PARIENT_GENERAL_NOTE_ID && !h.DELETED && h.PRACTICE_CODE == profile.PracticeCode && h.PATIENT_ACCOUNT == patientAccount);
                var vrNoteHistory = (from n in vrHistory
                                     join u in _securityContext.Users on n.CREATED_BY equals u.USER_NAME into un
                                     from u in un.DefaultIfEmpty()
                                     select new FOX_TBL_GENERAL_NOTE
                                     {
                                         CREAETED_BY_FIRST_NAME = u.FIRST_NAME,
                                         CREATED_BY_LAST_NAME = u.LAST_NAME,
                                         CREATED_BY_FULL_NAME = u.LAST_NAME != null && u.LAST_NAME != "" ? u.LAST_NAME + ", " + u.FIRST_NAME : u.FIRST_NAME,
                                         CREATED_BY = n.CREATED_BY,
                                         PRACTICE_CODE = n.PRACTICE_CODE,
                                         DELETED = n.DELETED,
                                         CASE_ID = n.CASE_ID,
                                         CREATED_DATE = n.CREATED_DATE,
                                         GENERAL_NOTE_ID = n.GENERAL_NOTE_ID,
                                         NOTE_DESCRIPTION = n.NOTE_DESCRIPTION,
                                         PATIENT_ACCOUNT = n.PATIENT_ACCOUNT,
                                         PARENT_GENERAL_NOTE_ID = n.PARENT_GENERAL_NOTE_ID,
                                         MODIFIED_BY = n.MODIFIED_BY,
                                         MODIFIED_DATE = n.MODIFIED_DATE
                                     }
                        ).ToList();
                if (vrNoteHistory.Count() <= 0)
                {
                    toReturn.Message = "No record found";
                    toReturn.ErrorMessage = "No record found";
                    toReturn.Success = true;
                }
                else
                {
                    toReturn.History = vrNoteHistory;
                    toReturn.History.RemoveAt(0);
                    toReturn.Message = "History retrieved successfully";
                    toReturn.ErrorMessage = "History retrieved successfully";
                    toReturn.Success = true;
                }

            }
            catch (Exception ex)
            {
                toReturn.Message = ex.Message;
                toReturn.ErrorMessage = ex.Message;
                toReturn.Success = false;
            }
            return toReturn;
        }
        public List<AlertType> GetAlertTypes(long practiceCode)
        {
            try
            {
                return _alertTypeRepository.GetMany(t => t.PRACTICE_CODE == practiceCode && (t.IS_ACTIVE ?? true) && t.DELETED == false).OrderBy(t => t.CODE).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<AlertType> GetAlertTypeswithInactive(long practiceCode)
        {
            try
            {
                return _alertTypeRepository.GetMany(t => t.PRACTICE_CODE == practiceCode && t.DELETED == false).OrderBy(t => t.CODE).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<PatientCasesForDD> GetPatientCasesList(long patientAccount, UserProfile Profile)
        {
            try
            {
                List<PatientCasesForDD> caseList = _vwCaseRepository.GetMany(e => !e.DELETED && e.PRACTICE_CODE == Profile.PracticeCode && e.PATIENT_ACCOUNT == patientAccount)
                    .Select(w => new PatientCasesForDD()
                    {
                        CASE_ID = w.CASE_ID,
                        CASE_NO = w.CASE_NO,
                        RT_CASE_NO = w.RT_CASE_NO,
                        CASE_STATUS_NAME = w.CASE_STATUS_NAME,
                        DISCIPLINE_NAME = w.DISCIPLINE_NAME,
                        CREATED_DATE = w.CREATED_DATE
                    }).ToList();
                return caseList;
            }
            catch (Exception ex)
            {
                //log exception
                throw ex;
            }

        }
        public void InsertInterfaceTeamData2(InterfaceSynchModel obj, UserProfile Profile)
        {
            InterfaceSynchModel interfaceSynch = new InterfaceSynchModel();
            if (obj.PATIENT_ACCOUNT == null && obj.CASE_ID == null && obj.TASK_ID == null)
            {

            }
            else
            {
                interfaceSynch = __InterfaceSynchModelRepository.GetFirst(x => !x.DELETED && x.FOX_INTERFACE_SYNCH_ID == obj.FOX_INTERFACE_SYNCH_ID );
                interfaceSynch.MODIFIED_BY = Profile.UserName;
                interfaceSynch.MODIFIED_DATE = DateTime.Now;
                __InterfaceSynchModelRepository.Update(interfaceSynch);
                _CaseContext.SaveChanges();


                //interfaceSynch.FOX_INTERFACE_SYNCH_ID = Helper.getMaximumId("FOX_INTERFACE_SYNCH_ID");
                //interfaceSynch.CASE_ID = obj.CASE_ID;
                //interfaceSynch.Work_ID = obj.Work_ID;
                //interfaceSynch.TASK_ID = obj.TASK_ID;
                //interfaceSynch.PATIENT_ACCOUNT = obj.PATIENT_ACCOUNT;
                //interfaceSynch.PRACTICE_CODE = Profile.PracticeCode;
                //interfaceSynch.MODIFIED_BY = interfaceSynch.CREATED_BY = Profile.UserName;
                //interfaceSynch.MODIFIED_DATE = interfaceSynch.CREATED_DATE = DateTime.Now;
                //interfaceSynch.DELETED = false;
                //interfaceSynch.IS_SYNCED = false;
                //__InterfaceSynchModelRepository.Insert(interfaceSynch);
                //_CaseContext.SaveChanges();
            }
        }
        public List<NoteAlert> GetNoteAlert(AlertSearchRequest request, long practiceCode)
        {
            var _paramsPracticeCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = System.Data.SqlDbType.BigInt, Value = practiceCode };
            var _paramsCurrentPage = new SqlParameter { ParameterName = "CURRENT_PAGE", SqlDbType = System.Data.SqlDbType.Int, Value = request.CurrentPage };
            var _paramsRecordsPerPage = new SqlParameter { ParameterName = "RECORDS_PER_PAGE", SqlDbType = System.Data.SqlDbType.Int, Value = request.RecordPerPage };
            var _paramsSearchText = new SqlParameter { ParameterName = "SEARCH_TEXT", Value = request.SearchText };
            var _paramsPatientAccount = new SqlParameter { ParameterName = "PATIENT_ACCOUNT", SqlDbType = System.Data.SqlDbType.BigInt, Value = request.PATIENT_ACCOUNT };
            var _paramsSortBy = new SqlParameter { ParameterName = "SORT_BY", Value = request.Sort_By };
            var _paramsSortOrder = new SqlParameter { ParameterName = "SORT_ORDER", Value = request.Sort_Order };
            var res = SpRepository<NoteAlert>.GetListWithStoreProcedure(@" exec [FOX_PROC_GET_ALL_NOTES_ALERTS] @PRACTICE_CODE, @CURRENT_PAGE, @RECORDS_PER_PAGE, @SEARCH_TEXT, @PATIENT_ACCOUNT,@SORT_BY,@SORT_ORDER", _paramsPracticeCode, _paramsCurrentPage, _paramsRecordsPerPage, _paramsSearchText, _paramsPatientAccount, _paramsSortBy, _paramsSortOrder);
            return res;
        }
        public ResponseModel CreateUpdateNoteAlert(NoteAlert request, UserProfile profile)
        {
            InterfaceSynchModel interfaceSynch = new InterfaceSynchModel();
             var patientAccount = Convert.ToInt64(request.PATIENT_ACCOUNT_str);
            interfaceSynch.PATIENT_ACCOUNT = patientAccount;
            DateTime effectiveTillDateTime;
            if (DateTime.TryParse(request.EFFECTIVE_TILL_str, out effectiveTillDateTime))
            {
                request.EFFECTIVE_TILL = effectiveTillDateTime;
            }
            else
            {
                request.EFFECTIVE_TILL = null;
            }
            var noteAlert = _noteAlertRepository.GetFirst(t => t.FOX_TBL_ALERT_ID == request.FOX_TBL_ALERT_ID && t.PRACTICE_CODE == profile.PracticeCode);
            if (noteAlert == null)
            {
                noteAlert = new NoteAlert();
                noteAlert.FOX_TBL_ALERT_ID = Helper.getMaximumId("FOX_TBL_ALERT_ID");
                noteAlert.ALERT_TYPE_ID = request.ALERT_TYPE_ID;
                noteAlert.NOTE_DETAIL = request.NOTE_DETAIL;
                noteAlert.EFFECTIVE_TILL = request.EFFECTIVE_TILL;
                noteAlert.PATIENT_ACCOUNT = patientAccount;
                noteAlert.PRACTICE_CODE = profile.PracticeCode;
                noteAlert.CREATED_BY = profile.UserName;
                noteAlert.CREATED_DATE = Helper.GetCurrentDate();
                noteAlert.MODIFIED_BY = profile.UserName;
                noteAlert.MODIFIED_DATE = Helper.GetCurrentDate();
                noteAlert.DELETED = false;
                _noteAlertRepository.Insert(noteAlert);
            }
            else
            {
                noteAlert.ALERT_TYPE_ID = request.ALERT_TYPE_ID;
                noteAlert.NOTE_DETAIL = request.NOTE_DETAIL;
                noteAlert.EFFECTIVE_TILL = request.EFFECTIVE_TILL;
                noteAlert.PRACTICE_CODE = profile.PracticeCode;
                noteAlert.MODIFIED_BY = profile.UserName;
                noteAlert.MODIFIED_DATE = Helper.GetCurrentDate();
                noteAlert.DELETED = request.DELETED;
                _noteAlertRepository.Update(noteAlert);
            }
            try
            {
                _noteAlertRepository.Save();
                //Task 149402:Dev Task: FOX-RT 105. Disabling editing of patient info. from RFO, Usman Nasir
                //InsertInterfaceTeamData(interfaceSynch, profile);
                return new ResponseModel()
                {
                    Success = true,
                    ErrorMessage = "Alert has been created successfully.",
                    ID = noteAlert.FOX_TBL_ALERT_ID + "",
                    Message = "Alert has been created successfully."
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel()
                {
                    Success = false,
                    ErrorMessage = ex.ToString(),
                    ID = noteAlert.FOX_TBL_ALERT_ID + "",
                    Message = ex.ToString()
                };
            }

        }
        public ResponseModel DeleteAlert(long alertId, UserProfile userProfile)
        {
            var alertToRemove = _noteAlertRepository.GetFirst(t => t.FOX_TBL_ALERT_ID == alertId);
            if (alertToRemove != null)
            {
                alertToRemove.MODIFIED_BY = userProfile.UserName;
                alertToRemove.MODIFIED_DATE = Helper.GetCurrentDate();
                alertToRemove.DELETED = true;
                _noteAlertRepository.Update(alertToRemove);
                try
                {
                    _noteAlertRepository.Save();
                    return new ResponseModel()
                    {

                        ErrorMessage = "",
                        ID = alertToRemove.FOX_TBL_ALERT_ID + "",
                        Message = "Deleted Successfully",
                        Success = true
                    };

                }
                catch (Exception ex)
                {

                    return new ResponseModel()
                    {

                        ErrorMessage = ex.ToString(),
                        ID = alertToRemove.FOX_TBL_ALERT_ID + "",
                        Message = ex.ToString(),
                        Success = false
                    };
                }
            }
            else
            {
                return new ResponseModel()
                {

                    ErrorMessage = "No record found.",
                    ID = alertToRemove.FOX_TBL_ALERT_ID + "",
                    Message = "No record found",
                    Success = false
                };
            }
        }

        public bool checkPatientisInterfaced(string PATIENT_ACCOUNT, UserProfile profile)
        {
            var patient_account = long.Parse(PATIENT_ACCOUNT);
            var Patient_interfaced = __InterfaceSynchModelRepository.GetFirst(t => (t.DELETED == false) && t.PRACTICE_CODE == profile.PracticeCode && t.PATIENT_ACCOUNT == patient_account && (t.IS_SYNCED ?? false));
            var patient = _PatientRepository.GetFirst(t => t.Patient_Account == patient_account && !(t.DELETED ?? false));
            bool IS_PATIENT_INTERFACE_SYNCED = false;
            if ((patient != null && !String.IsNullOrWhiteSpace(patient.Chart_Id)) || Patient_interfaced != null)
            {
                IS_PATIENT_INTERFACE_SYNCED = true;
            }
            else
            {
                IS_PATIENT_INTERFACE_SYNCED = false;
            }
            return IS_PATIENT_INTERFACE_SYNCED;
        }
        public List<GeneralNotesInterfaceLog> GetGeneralNotesInterfaceLogs(InterfaceLogSearchRequest request, UserProfile profile)
        {
            var _paramsPracticeCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = System.Data.SqlDbType.BigInt, Value = profile.PracticeCode };
            var _paramsSearchText = new SqlParameter { ParameterName = "OPTION", SqlDbType = System.Data.SqlDbType.VarChar, Value = request.Option};
            var _paramsOption = new SqlParameter { ParameterName = "SEARCH_STRING", SqlDbType = System.Data.SqlDbType.VarChar, Value = request.SearchText };
            var _paramsCurrentPage = new SqlParameter { ParameterName = "CURRENT_PAGE", SqlDbType = System.Data.SqlDbType.Int, Value = request.CurrentPage };
            var _paramsRecordsPerPage = new SqlParameter { ParameterName = "RECORD_PER_PAGE", SqlDbType = System.Data.SqlDbType.Int, Value = request.RecordPerPage };
            var _sortBy = new SqlParameter { ParameterName = "SORT_BY", SqlDbType = System.Data.SqlDbType.VarChar, Value = request.Sort_By};
            var _soryOrder = new SqlParameter { ParameterName = "SORT_ORDER", SqlDbType = System.Data.SqlDbType.VarChar, Value = request.Sort_Order};
            var _paramsPatientAccount = new SqlParameter { ParameterName = "PATIENT_ACCOUNT", SqlDbType = System.Data.SqlDbType.BigInt, Value = request.PATIENT_ACCOUNT };

            var res = SpRepository<GeneralNotesInterfaceLog>.GetListWithStoreProcedure(@" exec FOX_PROC_GET_ALL_INTERFACE_LOGS
                      @PRACTICE_CODE, @OPTION, @SEARCH_STRING, @CURRENT_PAGE, @RECORD_PER_PAGE, @SORT_BY, @SORT_ORDER, @PATIENT_ACCOUNT",
                      _paramsPracticeCode, _paramsOption, _paramsSearchText,_paramsCurrentPage, _paramsRecordsPerPage, _sortBy, _soryOrder, _paramsPatientAccount).ToList();
            return res;
        }
        public bool RetryInterfacing(InterfaceLogSearchRequest request, UserProfile profile)
        {
            List<InterfaceSynchModel> interfaceSynchTrue = new List<InterfaceSynchModel>();
            List<InterfaceSynchModel> interfaceSynchFalse = new List<InterfaceSynchModel>();
          
            var _paramsPracticeCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = System.Data.SqlDbType.BigInt, Value = profile.PracticeCode };
            var _paramsPatientAccount = new SqlParameter { ParameterName = "PATIENT_ACCOUNT", SqlDbType = System.Data.SqlDbType.BigInt, Value = request.PATIENT_ACCOUNT };

            var result = SpRepository<InterfaceSynchModel>.GetListWithStoreProcedure(@" exec SYNCHED_FAILED_PATIENT_RETRY @PRACTICE_CODE, @PATIENT_ACCOUNT",
                      _paramsPracticeCode, _paramsPatientAccount).ToList();
            interfaceSynchTrue = result.FindAll(x => x.IS_SYNCED ==  true);
            interfaceSynchFalse = result.FindAll(x => x.IS_SYNCED == false);

            if (interfaceSynchFalse.Count > 0)
            {
                for (int i = 0; i < interfaceSynchFalse.Count(); i++)
                {
                     InsertInterfaceTeamData2(interfaceSynchFalse[i], profile);
                }
            }

            //if (interfaceSynchTrue.Count > 0 && interfaceSynchFalse.Count > 0)
            //{
            //    for (int i = 0; i < interfaceSynchFalse.Count(); i++)
            //    {

            //       var check = interfaceSynchTrue.Find(e => e.Work_ID == interfaceSynchFalse[i].Work_ID && e.TASK_ID == interfaceSynchFalse[i].TASK_ID);
            //        if (check == null)
            //        {
            //            InsertInterfaceTeamData2(interfaceSynchFalse[i], profile);
            //        }
            //    }
            //}
            //else
            //{
            //    for (int i = 0; i < interfaceSynchFalse.Count(); i++)
            //    {
            //        InsertInterfaceTeamData2(interfaceSynchFalse[i], profile);
            //    }
            //}
            
            return true;
        }
        public string ExportToExcelInterfaceLogs(InterfaceLogSearchRequest request, UserProfile profile)
        {
            try
            {
                string fileName = "Interface_Logs_Lists";
                string exportPath = "";
                string path = string.Empty;
                bool exported;
                request.CurrentPage = 1;
                request.RecordPerPage = 0;
                var CalledFrom = "Interface_Logs";
                string virtualPath = @"/" + profile.PracticeDocumentDirectory + "/" + "Fox/ExportedFiles/";
                exportPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
                fileName = DocumentHelper.GenerateSignatureFileName(fileName) + ".xlsx";
                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                }
                List<GeneralNotesInterfaceLog> result = new List<GeneralNotesInterfaceLog>();
                var pathtowriteFile = exportPath + "\\" + fileName;

                //The following code removes XML invalid characters from a string and returns a new string without them
                string re = @"[^\x09\x0A\x0D\x20-\xD7FF\xE000-\xFFFD\x10000-x10FFFF]";
                
                result = GetGeneralNotesInterfaceLogs(request, profile);
                for (int i = 0; i < result.Count(); i++)
                {
                    result[i].ROW = i + 1;
                    result[i].ACK = System.Text.RegularExpressions.Regex.Replace(result[i].ACK?.ToString() ?? "", re, "");
                    result[i].LOG_MESSAGE = System.Text.RegularExpressions.Regex.Replace(result[i].LOG_MESSAGE?.ToString() ?? "", re, "");
                }
                exported = ExportToExcel.CreateExcelDocument<GeneralNotesInterfaceLog>(result, pathtowriteFile, CalledFrom.Replace(' ', '_'));
                return virtualPath + fileName;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}