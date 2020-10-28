using FOX.BusinessOperations.CommonService;
using FOX.BusinessOperations.CommonServices;
using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.CasesModel;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.GeneralNotesModel;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.FacilityLocation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using static FOX.DataModels.Models.Scheduler.SchedulerModel;

namespace FOX.BusinessOperations.Scheduler
{
    public class SchedulerService : ISchedulerService
    {
        private readonly DBContextScheduler _contextSchedulerContext = new DBContextScheduler();
        private readonly GenericRepository<Appointment> _appointmentRepository;
        private readonly GenericRepository<AppointmentStatus> _appointmentStatusRepository;
        private readonly GenericRepository<VisitType> _visitTypeRepository;
        private readonly GenericRepository<Provider> _providerRepository;
        private readonly GenericRepository<ReferralRegion> _regionRepository;
        private readonly GenericRepository<FacilityType> _FacilityTypeRepository;
        private readonly GenericRepository<InterfaceSynchModel> _InterfaceSynchModelRepository;
        private readonly GenericRepository<CancellationReason> _CancellationReasonModelRepository;
        public SchedulerService()
        {
            _appointmentRepository = new GenericRepository<Appointment>(_contextSchedulerContext);
            _appointmentStatusRepository = new GenericRepository<AppointmentStatus>(_contextSchedulerContext);
            _visitTypeRepository = new GenericRepository<VisitType>(_contextSchedulerContext);
            _providerRepository = new GenericRepository<Provider>(_contextSchedulerContext);
            _regionRepository = new GenericRepository<ReferralRegion>(_contextSchedulerContext);
            _FacilityTypeRepository = new GenericRepository<FacilityType>(_contextSchedulerContext);
            _InterfaceSynchModelRepository = new GenericRepository<InterfaceSynchModel>(_contextSchedulerContext);
            _CancellationReasonModelRepository = new GenericRepository<CancellationReason>(_contextSchedulerContext);

        }
        public List<Appointment> GetAllAppointments(AppointmentSearchRequest reg, UserProfile profile)
        {
            List<Appointment> returList = new List<Appointment>();

            if (reg.PATIENT_ACCOUNT != "0")
            {
                reg.DATE_FROM = Helper.GetCurrentDate().AddYears(-100);
                reg.DATE_TO = Helper.GetCurrentDate().AddYears(100);
                reg.DATE_FROM_STR = reg.DATE_FROM.ToString();
                reg.DATE_TO_STR = reg.DATE_TO.ToString();
            }

            reg.DATE_TO = Helper.GetCurrentDate();
            switch (reg.TIME_FRAME)
            {
                case 1:
                    reg.DATE_FROM = Helper.GetCurrentDate().AddDays(-7);
                    break;
                case 2:
                    reg.DATE_FROM = Helper.GetCurrentDate().AddDays(-15);
                    break;
                case 3:
                    reg.DATE_FROM = Helper.GetCurrentDate().AddDays(-30);
                    break;
                case 4:
                    if (!string.IsNullOrEmpty(reg.DATE_FROM_STR))
                        reg.DATE_FROM = Convert.ToDateTime(reg.DATE_FROM_STR);
                    else
                        reg.DATE_FROM = Helper.GetCurrentDate().AddYears(-100);
                    if (!string.IsNullOrEmpty(reg.DATE_TO_STR))
                        reg.DATE_TO = Convert.ToDateTime(reg.DATE_TO_STR);
                    break;
                default:
                    break;
            }
            var response = _appointmentStatusRepository.GetFirst(x => !(x.DELETED ?? false) && x.DESCRIPTION == "Void");
            var resLoc  = _FacilityTypeRepository.GetFirst(x => !(x.DELETED) && x.PRACTICE_CODE == profile.PracticeCode && x.NAME == "Private Home");


  
            SqlParameter _patientAccount = new SqlParameter { ParameterName = "PATIENT_ACCOUNT", Value = reg.PATIENT_ACCOUNT };
            SqlParameter _practiceCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
            SqlParameter _searchText = new SqlParameter { ParameterName = "SEARCH_TEXT", Value = reg.SEARCH_TEXT };
            SqlParameter _dateFrom = Helper.getDBNullOrValue("DATE_FROM", reg.DATE_FROM.ToString());
            SqlParameter _dateTos = Helper.getDBNullOrValue("DATE_TO", reg.DATE_TO.ToString());
            SqlParameter _provider = new SqlParameter { ParameterName = "PROVIDER", Value = reg.PROVIDER_ID };
            SqlParameter _reason = new SqlParameter { ParameterName = "REASON", Value = reg.REASON };
            SqlParameter _status = new SqlParameter { ParameterName = "STATUS", Value = reg.STATUS };
            SqlParameter _restrict_status = new SqlParameter { ParameterName = "RESTRICT_STATUS", Value = response.APPOINTMENT_STATUS_ID };
            SqlParameter _restrict_location = new SqlParameter { ParameterName = "RESTRICT_LOCATION", Value = resLoc.FACILITY_TYPE_ID };
            SqlParameter _region = new SqlParameter { ParameterName = "REGION", Value = reg.REGION };
            SqlParameter _location = new SqlParameter { ParameterName = "LOCATION", Value = reg.LOCATION.ToString() };
            SqlParameter _discipline = new SqlParameter { ParameterName = "DISCILPINE", Value = reg.DISCIPLINE };
            SqlParameter _currentPage = new SqlParameter { ParameterName = "CURRENT_PAGE", SqlDbType = SqlDbType.Int, Value = reg.CURRENT_PAGE };
            SqlParameter _recordPerPage = new SqlParameter { ParameterName = "RECORD_PER_PAGE", SqlDbType = SqlDbType.Int, Value = reg.RECORD_PER_PAGE };
            SqlParameter _sortBy = new SqlParameter { ParameterName = "SORT_BY", Value = reg.SORT_BY };
            SqlParameter _sortOrder = new SqlParameter { ParameterName = "SORT_ORDER", Value = reg.SORT_ORDER };

            returList = SpRepository<Appointment>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_SCHEDULER_LIST
                            @PATIENT_ACCOUNT, @PRACTICE_CODE, @SEARCH_TEXT, @DATE_FROM, @DATE_TO, @PROVIDER, @REASON, @STATUS,  @RESTRICT_STATUS, @RESTRICT_LOCATION, @REGION, @LOCATION, @DISCILPINE, @CURRENT_PAGE, @RECORD_PER_PAGE, @SORT_BY, @SORT_ORDER"
                           , _patientAccount, _practiceCode, _searchText, _dateFrom, _dateTos, _provider, _reason, _status, _restrict_status, _restrict_location, _region, _location, _discipline, _currentPage, _recordPerPage, _sortBy, _sortOrder);

            SqlParameter _patientAccount2 = new SqlParameter { ParameterName = "PATIENT_ACCOUNT", Value = reg.PATIENT_ACCOUNT };
            SqlParameter _practiceCode2 = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
            SqlParameter _searchText2 = new SqlParameter { ParameterName = "SEARCH_TEXT", Value = reg.SEARCH_TEXT };
            SqlParameter _dateFrom2 = Helper.getDBNullOrValue("DATE_FROM", reg.DATE_FROM.ToString());
            SqlParameter _dateTos2 = Helper.getDBNullOrValue("DATE_TO", reg.DATE_TO.ToString());
            SqlParameter _provider2 = new SqlParameter { ParameterName = "PROVIDER", Value = reg.PROVIDER_ID };
            SqlParameter _reason2 = new SqlParameter { ParameterName = "REASON", Value = reg.REASON };
            SqlParameter _status2 = new SqlParameter { ParameterName = "STATUS", Value = reg.STATUS };
            SqlParameter _restrict_status2 = new SqlParameter { ParameterName = "RESTRICT_STATUS", Value = response.APPOINTMENT_STATUS_ID };
            SqlParameter _restrict_location2 = new SqlParameter { ParameterName = "RESTRICT_LOCATION", Value = resLoc.FACILITY_TYPE_ID };
            SqlParameter _region2 = new SqlParameter { ParameterName = "REGION", Value = reg.REGION };
            SqlParameter _location2 = new SqlParameter { ParameterName = "LOCATION", Value = reg.LOCATION.ToString() };
            SqlParameter _discipline2 = new SqlParameter { ParameterName = "DISCILPINE", Value = reg.DISCIPLINE };
            SqlParameter _currentPage2 = new SqlParameter { ParameterName = "CURRENT_PAGE", SqlDbType = SqlDbType.Int, Value = reg.CURRENT_PAGE };
            SqlParameter _recordPerPage2 = new SqlParameter { ParameterName = "RECORD_PER_PAGE", SqlDbType = SqlDbType.Int, Value = 0 };
            SqlParameter _sortBy2 = new SqlParameter { ParameterName = "SORT_BY", Value = reg.SORT_BY };
            SqlParameter _sortOrder2 = new SqlParameter { ParameterName = "SORT_ORDER", Value = reg.SORT_ORDER };
          var fullList = SpRepository<Appointment>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_SCHEDULER_LIST
                            @PATIENT_ACCOUNT, @PRACTICE_CODE, @SEARCH_TEXT, @DATE_FROM, @DATE_TO, @PROVIDER, @REASON, @STATUS,  @RESTRICT_STATUS, @RESTRICT_LOCATION, @REGION, @LOCATION, @DISCILPINE, @CURRENT_PAGE, @RECORD_PER_PAGE, @SORT_BY, @SORT_ORDER"
                           , _patientAccount2, _practiceCode2, _searchText2, _dateFrom2, _dateTos2, _provider2, _reason2, _status2, _restrict_status2, _restrict_location2, _region2, _location2, _discipline2, _currentPage2, _recordPerPage2, _sortBy2, _sortOrder2);

            foreach (var item in returList)
            {
                item.STR_LENGTH = ConvertTimeLength(item.LENGTH);
                item.NAME = getPatientInFormat(item.NAME, item.FC_CODE);
                item.HOME_PHONE = getHomePhoneInFormat(item.HOME_PHONE);
                item.POS_ZIP = getZIPInFormat(item.POS_ZIP);
                item.ZIP = getZIPInFormat(item.ZIP);
                item.Alerts = GetAlerts(profile, item.PATIENT_ACCOUNT.ToString());

                if (item.POS_ADDRESS != null)
                {
                    //For Non-Private Home
                    item.POS_ADDRESS = item.POS_ADDRESS;
                    item.POS_CITY = item.POS_CITY;
                    item.POS_ZIP = item.POS_ZIP;
                    item.POS_STATE = item.POS_STATE;
                }
                else
                {
                    //For Private Home
                    item.POS_ADDRESS = item.ADDRESS;
                    item.POS_CITY = item.CITY;
                    item.POS_STATE = item.STATE;
                    item.POS_ZIP = item.ZIP;
                }
                var filteredList = fullList.Where(x => x.PATIENT_ACCOUNT == item.PATIENT_ACCOUNT).Count();
                if(filteredList > 1)
                {
                    item.IS_RECURSIVE = true;
                }
                else
                {
                    item.IS_RECURSIVE = false;
                }
            }
            return returList;
        }
        public WeekAppointmentsWithDay GetAllAppointmentsWeekly(AppointmentSearchRequest reg, UserProfile profile)
        {
            List<Appointment> returList = new List<Appointment>();

            reg.DATE_TO = Helper.GetCurrentDate();
            switch (reg.TIME_FRAME)
            {
                case 1:
                    reg.DATE_FROM = Helper.GetCurrentDate().AddDays(-7);
                    break;
                case 2:
                    reg.DATE_FROM = Helper.GetCurrentDate().AddDays(-15);
                    break;
                case 3:
                    reg.DATE_FROM = Helper.GetCurrentDate().AddDays(-30);
                    break;
                case 4:
                    if (!string.IsNullOrEmpty(reg.DATE_FROM_STR))
                        reg.DATE_FROM = Convert.ToDateTime(reg.DATE_FROM_STR);
                    else
                        reg.DATE_FROM = Helper.GetCurrentDate().AddYears(-100);
                    if (!string.IsNullOrEmpty(reg.DATE_TO_STR))
                        reg.DATE_TO = Convert.ToDateTime(reg.DATE_TO_STR);
                    break;
                default:
                    break;
            }
            var response = _appointmentStatusRepository.GetFirst(x => !(x.DELETED ?? false) && x.DESCRIPTION == "Void");
            var resLoc = _FacilityTypeRepository.GetFirst(x => !(x.DELETED) && x.PRACTICE_CODE == profile.PracticeCode && x.NAME == "Private Home");

            SqlParameter _patientAccount = new SqlParameter { ParameterName = "PATIENT_ACCOUNT", Value = reg.PATIENT_ACCOUNT };
            SqlParameter _practiceCode = new SqlParameter { ParameterName = "PRACTICE_CODE", SqlDbType = SqlDbType.BigInt, Value = profile.PracticeCode };
            SqlParameter _searchText = new SqlParameter { ParameterName = "SEARCH_TEXT", Value = reg.SEARCH_TEXT };
            SqlParameter _dateFrom = Helper.getDBNullOrValue("DATE_FROM", reg.DATE_FROM.ToString());
            SqlParameter _dateTos = Helper.getDBNullOrValue("DATE_TO", reg.DATE_TO.ToString());
            SqlParameter _provider = new SqlParameter { ParameterName = "PROVIDER", Value = reg.PROVIDER_ID };
            SqlParameter _reason = new SqlParameter { ParameterName = "REASON", Value = reg.REASON };
            SqlParameter _status = new SqlParameter { ParameterName = "STATUS", Value = reg.STATUS };
            SqlParameter _restrict_status = new SqlParameter { ParameterName = "RESTRICT_STATUS", Value = response.APPOINTMENT_STATUS_ID };
            SqlParameter _restrict_location = new SqlParameter { ParameterName = "RESTRICT_LOCATION", Value = resLoc.FACILITY_TYPE_ID };
            SqlParameter _region = new SqlParameter { ParameterName = "REGION", Value = reg.REGION };
            SqlParameter _location = new SqlParameter { ParameterName = "LOCATION", Value = reg.LOCATION.ToString() };
            SqlParameter _discipline = new SqlParameter { ParameterName = "DISCILPINE", Value = reg.DISCIPLINE };
            SqlParameter _currentPage = new SqlParameter { ParameterName = "CURRENT_PAGE", SqlDbType = SqlDbType.Int, Value = reg.CURRENT_PAGE };
            SqlParameter _recordPerPage = new SqlParameter { ParameterName = "RECORD_PER_PAGE", SqlDbType = SqlDbType.Int, Value = 0 };
            SqlParameter _sortBy = new SqlParameter { ParameterName = "SORT_BY", Value = reg.SORT_BY };
            SqlParameter _sortOrder = new SqlParameter { ParameterName = "SORT_ORDER", Value = reg.SORT_ORDER };

            returList = SpRepository<Appointment>.GetListWithStoreProcedure(@"exec FOX_PROC_GET_SCHEDULER_LIST
                            @PATIENT_ACCOUNT, @PRACTICE_CODE, @SEARCH_TEXT, @DATE_FROM, @DATE_TO, @PROVIDER, @REASON, @STATUS,  @RESTRICT_STATUS, @RESTRICT_LOCATION, @REGION, @LOCATION, @DISCILPINE, @CURRENT_PAGE, @RECORD_PER_PAGE, @SORT_BY, @SORT_ORDER"
                            ,_patientAccount, _practiceCode, _searchText, _dateFrom, _dateTos, _provider, _reason, _status, _restrict_status, _restrict_location, _region, _location, _discipline, _currentPage, _recordPerPage, _sortBy, _sortOrder);

            foreach (var item in returList)
            {
                item.STR_LENGTH = ConvertTimeLength(item.LENGTH);
                //item.NAME = getPatientInFormat(item.NAME, item.FC_CODE);
                item.HOME_PHONE = getNumberInFormat(item.HOME_PHONE);
                item.POS_ZIP = getZIPInFormat(item.POS_ZIP);
                item.Alerts = GetAlerts(profile,item.PATIENT_ACCOUNT.ToString());
                if ((item.POS_CODE == null || item.POS_CODE == "") && item.ADDRESS != null && item.ADDRESS != "")
                {
                    item.POS_NAME = item.ADDRESS;
                }
                var filteredList = returList.Where(x => x.PATIENT_ACCOUNT == item.PATIENT_ACCOUNT).Count();
                if (filteredList > 1)
                {
                    item.IS_RECURSIVE = true;
                }
                else
                {
                    item.IS_RECURSIVE = false;
                }
            }

            WeekAppointmentsWithDay weekwise = new WeekAppointmentsWithDay();
            weekwise = GetDayWise(returList);
            return weekwise;
        }
        public List<AppointmentStatus> GetAppointmentStatusList()
        {
            try
            {
                //var response = _appointmentStatusRepository.GetMany(x => !(x.DELETED ?? false) && x.DESCRIPTION != "Void").ToList();
                var response = _appointmentStatusRepository.GetMany(x => !(x.DELETED ?? false)).ToList();

                if (response.Count > 0)
                {
                    return response;
                }
                else
                {
                    return new List<AppointmentStatus>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<VisitType> GetVisitTypeList()
        {
            try
            {
                var response = _visitTypeRepository.GetMany(x => !(x.DELETED ?? false) && x.SHOW_FOR_APPOINTMENT == true).ToList();

                if (response.Count > 0)
                {
                    return response;
                }
                else
                {
                    return new List<VisitType>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CancellationReason> GetCancellationReasonsList()
        {
            try
            {
                var response = _CancellationReasonModelRepository.GetMany(x => (x.DELETED == false)).ToList();

                if (response != null && response.Count > 0)
                {
                    return response;
                }
                else
                {
                    return new List<CancellationReason>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public List<ProviderList> GetProviderList(long practiceCode)
        //{
        //    try
        //    {
        //        var response = _providerRepository.GetMany(x => !(x.DELETED ?? false) && x.PRACTICE_CODE == practiceCode && x.PROVIDER_NAME != null).Select(x => new ProviderList() { FOX_PROVIDER_ID = x.FOX_PROVIDER_ID, PROVIDER_NAME = x.PROVIDER_NAME }).Distinct().ToList();

        //        if (response.Count > 0)
        //        {
        //            return response;
        //        }
        //        else
        //        {
        //            return new List<ProviderList>();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public List<ProviderList> GetProviderList(long practiceCode)
        {
            try
            {
                var response = _providerRepository.GetMany(x => !(x.DELETED ?? false) && x.PRACTICE_CODE == practiceCode && x.FIRST_NAME != null && x.LAST_NAME != null).Select(x => new ProviderList() { FOX_PROVIDER_ID = x.FOX_PROVIDER_ID, PROVIDER_NAME = x.LAST_NAME+", " + x.FIRST_NAME}).OrderBy(t => t.PROVIDER_NAME).Distinct().ToList();

                if (response.Count > 0)
                {
                    return response;
                }
                else
                {
                    return new List<ProviderList>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<RegionList> GetRegionsList(long practiceCode)
        {
            try
            {
                var response = _regionRepository.GetMany(x => !(x.DELETED) && x.PRACTICE_CODE == practiceCode)
                    .Select(x => new RegionList() { REFERRAL_REGION_ID = x.REFERRAL_REGION_ID, REFERRAL_REGION_CODE = x.REFERRAL_REGION_CODE, REFERRAL_REGION_NAME =x.REFERRAL_REGION_NAME })
                    .OrderBy(t => t.REFERRAL_REGION_NAME).Distinct().ToList();

                if (response.Count > 0)
                {
                    return response;
                }
                else
                {
                    return new List<RegionList>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string ConvertTimeLength(int? time)
        {
            if (time < 60)
            {
                return time + " min";
            }
            else if (time == 60)
            {
                return "1 hour";
            }
            else if (time > 60)
            {
                int? hour = time / 60;
                int? min = time % 60;
                string str = hour + " hour " + min + " min";
                return str;
            }
            else
            {
                return "";
            }
        }
        public int RescheduleAppoinment(Appointment req, UserProfile profile)
        {
            if (!string.IsNullOrEmpty(req.TIME_FROM))
            {
                req.TIME_FROM_DATE = Convert.ToDateTime(req.TIME_FROM);
                req.TIME_FROM = req.TIME_FROM_DATE.ToString("hh:mm tt");
            }
            else
                req.TIME_FROM_DATE = Helper.GetCurrentDate().AddYears(-100);

            if (!string.IsNullOrEmpty(req.TIME_TO))
            {
                req.TIME_TO_DATE = Convert.ToDateTime(req.TIME_TO);
                req.TIME_TO = req.TIME_TO_DATE.ToString("hh:mm tt");
            }
            else
                req.TIME_TO_DATE = Helper.GetCurrentDate().AddYears(-100);
            if (!string.IsNullOrEmpty(req.APPOINTMENT_DATE_STR))
                req.APPOINTMENT_DATE = Convert.ToDateTime(req.APPOINTMENT_DATE_STR);
            else
                req.APPOINTMENT_DATE = Helper.GetCurrentDate().AddYears(-100);

            //Check if new timings are not blocked already
            var temp = _appointmentRepository.GetFirst(x => x.APPOINTMENT_DATE == req.APPOINTMENT_DATE && x.TIME_FROM == req.TIME_FROM
            && x.TIME_TO == req.TIME_TO && x.IsBlocked == true && !x.DELETED);

            if (temp != null)
            {
                return 0;
            }

            ///Over laping against Provider with block appointment
            var appOfdateblock = _appointmentRepository.GetMany(x => x.APPOINTMENT_DATE == req.APPOINTMENT_DATE && x.PROVIDER_ID == req.PROVIDER_ID && x.IsBlocked == true && !x.DELETED);
            foreach (var item in appOfdateblock)
            {
                //string startTime = req.TIME_FROM;
                //string endTime = item;

                //When old Time From lies between new Time From and New Time to 
                if (DateTime.Parse(item.TIME_FROM) > DateTime.Parse(req.TIME_FROM)
                    && DateTime.Parse(item.TIME_FROM) < DateTime.Parse(req.TIME_TO)
                    && DateTime.Parse(item.TIME_TO) > DateTime.Parse(req.TIME_TO))
                {
                    //TimeSpan duration = DateTime.Parse(req.TIME_TO).Subtract(DateTime.Parse(item.TIME_FROM));
                    //if (duration.Minutes > 5 && !req.IS_PROVIDER_OVERLAPPING)
                    //{
                    return 0;
                    //}
                }

            }

            foreach (var item in appOfdateblock)
            {
                //When old Time To lies between new Time From and New Time to 
                if (DateTime.Parse(item.TIME_TO) > DateTime.Parse(req.TIME_FROM) && DateTime.Parse(item.TIME_TO) < DateTime.Parse(req.TIME_TO))
                {
                    //TimeSpan duration = DateTime.Parse(item.TIME_TO).Subtract(DateTime.Parse(req.TIME_FROM));
                    //if (duration.Minutes > 5 && !req.IS_PROVIDER_OVERLAPPING)
                    //{
                    return 0;
                    //}
                }
            }

            foreach (var item in appOfdateblock)
            {
                //When new time lies fully in old time 
                if (DateTime.Parse(item.TIME_FROM) < DateTime.Parse(req.TIME_FROM)
                    && DateTime.Parse(item.TIME_FROM) < DateTime.Parse(req.TIME_TO)
                    && DateTime.Parse(item.TIME_TO) > DateTime.Parse(req.TIME_TO)
                    && DateTime.Parse(item.TIME_TO) > DateTime.Parse(req.TIME_FROM)
                    && !req.IS_PROVIDER_OVERLAPPING)
                {
                    return 0;
                }
            }

            foreach (var item in appOfdateblock)
            {
                //When old time lies fully in new time 
                if (DateTime.Parse(item.TIME_FROM) > DateTime.Parse(req.TIME_FROM)
                    && DateTime.Parse(item.TIME_FROM) < DateTime.Parse(req.TIME_TO)
                    && DateTime.Parse(item.TIME_TO) > DateTime.Parse(req.TIME_FROM)
                    && DateTime.Parse(item.TIME_TO) < DateTime.Parse(req.TIME_TO)
                    && !req.IS_PROVIDER_OVERLAPPING)
                {
                    return 0;
                }
            }

            //Check if new timings of Patient are not overlapping
            var temp1 = _appointmentRepository.GetFirst(x => x.APPOINTMENT_DATE == req.APPOINTMENT_DATE && x.TIME_FROM == req.TIME_FROM
            && x.TIME_TO == req.TIME_TO && x.PATIENT_ACCOUNT == req.PATIENT_ACCOUNT && !x.DELETED);

            if (temp1 != null && !req.IS_PATIENT_OVERLAPPING)
            {
                return 1;
            }

            //Check if new timings of Provider are not overlapping
            var temp2 = _appointmentRepository.GetFirst(x => x.APPOINTMENT_DATE == req.APPOINTMENT_DATE && x.TIME_FROM == req.TIME_FROM
            && x.TIME_TO == req.TIME_TO && x.PROVIDER_ID == req.PROVIDER_ID && !x.DELETED);

            if (temp2 != null && !req.IS_PROVIDER_OVERLAPPING)
            {
                return 2;
            }

            ///Over laping against patient
            var appOfdatePatient = _appointmentRepository.GetMany(x => x.APPOINTMENT_DATE == req.APPOINTMENT_DATE && x.PATIENT_ACCOUNT == req.PATIENT_ACCOUNT && !x.DELETED);
            foreach (var item in appOfdatePatient)
            {
                //string startTime = req.TIME_FROM;
                //string endTime = item;

                //When old Time From lies between new Time From and New Time to 
                if (DateTime.Parse(item.TIME_FROM) > DateTime.Parse(req.TIME_FROM)
                    && DateTime.Parse(item.TIME_FROM) < DateTime.Parse(req.TIME_TO)
                    && DateTime.Parse(item.TIME_TO) > DateTime.Parse(req.TIME_TO))
                {
                    TimeSpan duration = DateTime.Parse(req.TIME_TO).Subtract(DateTime.Parse(item.TIME_FROM));
                    if (duration.Minutes > 5 && !req.IS_PATIENT_OVERLAPPING)
                    {
                        return 1;
                    }
                }

            }

            foreach (var item in appOfdatePatient)
            {
                //When old Time To lies between new Time From and New Time to 
                if (DateTime.Parse(item.TIME_TO) > DateTime.Parse(req.TIME_FROM) && DateTime.Parse(item.TIME_TO) < DateTime.Parse(req.TIME_TO))
                {
                    TimeSpan duration = DateTime.Parse(item.TIME_TO).Subtract(DateTime.Parse(req.TIME_FROM));
                    if (duration.Minutes > 5 && !req.IS_PATIENT_OVERLAPPING)
                    {
                        return 1;
                    }
                }
            }

            foreach (var item in appOfdatePatient)
            {
                //When new time lies fully in old time 
                if (DateTime.Parse(item.TIME_FROM) < DateTime.Parse(req.TIME_FROM)
                    && DateTime.Parse(item.TIME_FROM) < DateTime.Parse(req.TIME_TO)
                    && DateTime.Parse(item.TIME_TO) > DateTime.Parse(req.TIME_TO)
                    && DateTime.Parse(item.TIME_TO) > DateTime.Parse(req.TIME_FROM)
                    && !req.IS_PATIENT_OVERLAPPING)
                {
                    return 1;
                }
            }

            foreach (var item in appOfdatePatient)
            {
                //When old time lies fully in new time 
                if (DateTime.Parse(item.TIME_FROM) > DateTime.Parse(req.TIME_FROM)
                    && DateTime.Parse(item.TIME_FROM) < DateTime.Parse(req.TIME_TO)
                    && DateTime.Parse(item.TIME_TO) > DateTime.Parse(req.TIME_FROM)
                    && DateTime.Parse(item.TIME_TO) < DateTime.Parse(req.TIME_TO)
                    && !req.IS_PATIENT_OVERLAPPING)
                {
                    return 1;
                }
            }
          
            ///Over laping against Provider
            var appOfdate = _appointmentRepository.GetMany(x => x.APPOINTMENT_DATE == req.APPOINTMENT_DATE && x.PROVIDER_ID == req.PROVIDER_ID && x.IsBlocked != true && !x.DELETED);
            foreach (var item in appOfdate)
            {
                //string startTime = req.TIME_FROM;
                //string endTime = item;

                //When old Time From lies between new Time From and New Time to 
                if (DateTime.Parse(item.TIME_FROM) > DateTime.Parse(req.TIME_FROM) 
                    && DateTime.Parse(item.TIME_FROM) < DateTime.Parse(req.TIME_TO)
                    && DateTime.Parse(item.TIME_TO) > DateTime.Parse(req.TIME_TO))
                {
                    TimeSpan duration = DateTime.Parse(req.TIME_TO).Subtract(DateTime.Parse(item.TIME_FROM));
                    if (duration.Minutes > 5 && !req.IS_PROVIDER_OVERLAPPING)
                    {
                        return 2;
                    }
                }

            }

            foreach (var item in appOfdate)
            {
                //When old Time To lies between new Time From and New Time to 
                if (DateTime.Parse(item.TIME_TO) > DateTime.Parse(req.TIME_FROM) && DateTime.Parse(item.TIME_TO) < DateTime.Parse(req.TIME_TO))
                {
                    TimeSpan duration = DateTime.Parse(item.TIME_TO).Subtract(DateTime.Parse(req.TIME_FROM));
                    if (duration.Minutes > 5 && !req.IS_PROVIDER_OVERLAPPING)
                    {
                        return 2;
                    }
                }
            }

            foreach (var item in appOfdate)
            {
                //When new time lies fully in old time 
                if (DateTime.Parse(item.TIME_FROM) < DateTime.Parse(req.TIME_FROM) 
                    && DateTime.Parse(item.TIME_FROM) < DateTime.Parse(req.TIME_TO)
                    && DateTime.Parse(item.TIME_TO) > DateTime.Parse(req.TIME_TO)
                    && DateTime.Parse(item.TIME_TO) > DateTime.Parse(req.TIME_FROM)
                    && !req.IS_PROVIDER_OVERLAPPING)
                {
                    return 2;
                }
            }

            foreach (var item in appOfdate)
            {
                //When old time lies fully in new time 
                if (DateTime.Parse(item.TIME_FROM) > DateTime.Parse(req.TIME_FROM)
                    && DateTime.Parse(item.TIME_FROM) < DateTime.Parse(req.TIME_TO)
                    && DateTime.Parse(item.TIME_TO) > DateTime.Parse(req.TIME_FROM)
                    && DateTime.Parse(item.TIME_TO) < DateTime.Parse(req.TIME_TO)
                    && !req.IS_PROVIDER_OVERLAPPING)
                {
                    return 2;
                }
            }

         

            //Check if new date is not overlapping with Provider's vocations
            var temp3 = _appointmentRepository.GetFirst(x => x.APPOINTMENT_ID == req.APPOINTMENT_ID);
            SqlParameter _provider = new SqlParameter { ParameterName = "PROVIDER", SqlDbType = SqlDbType.Int, Value = temp3.PROVIDER_ID };
            SqlParameter _date = Helper.getDBNullOrValue("APPOINMENT_DATE", req.APPOINTMENT_DATE.ToString());
            SqlParameter _practiceCode = new SqlParameter { ParameterName = "PRACTICE_CODE", Value = profile.PracticeCode };

            var PTOresult = SpRepository<string>.GetListWithStoreProcedure(@"exec CHECK_PROVIDER_IS_AVAILABLE @PROVIDER, @APPOINMENT_DATE, @PRACTICE_CODE"
                                                                                                                , _provider, _date, _practiceCode);
            if (PTOresult != null && PTOresult.Count() > 0 && !req.IS_PROVIDER_AVAILABLE)
            {
                return 3;
            }
            else
            {
                if (req.IS_NEW) // Add new Appointment
                {
                    Appointment toInsert = new Appointment();
                    var response = _appointmentStatusRepository.GetFirst(x => !(x.DELETED ?? false) && x.DESCRIPTION == "Pending");
                    var toUpdate = _appointmentRepository.GetFirst(x => x.APPOINTMENT_ID == req.APPOINTMENT_ID);
                    toInsert.APPOINTMENT_ID = Helper.getMaximumId("FOX_APPOINTMENT_ID");
                    toInsert.APPOINTMENT_DATE = req.APPOINTMENT_DATE;
                    toInsert.VISIT_TYPE_ID = req.VISIT_TYPE_ID;
                    toInsert.APPOINTMENT_STATUS_ID = response.APPOINTMENT_STATUS_ID;
                    toInsert.CASE_ID = toUpdate.CASE_ID;
                    toInsert.PROVIDER_ID = toUpdate.PROVIDER_ID;
                    toInsert.PATIENT_ACCOUNT = toUpdate.PATIENT_ACCOUNT;
                    toInsert.PRACTICE_CODE = toUpdate.PRACTICE_CODE;
                    toInsert.TIME_FROM = req.TIME_FROM;
                    toInsert.TIME_TO = req.TIME_TO;
                    toInsert.MODIFIED_BY = profile.UserName;
                    toInsert.MODIFIED_DATE = Helper.GetCurrentDate();
                    toInsert.CREATED_BY = profile.UserName;
                    toInsert.CREATED_DATE = Helper.GetCurrentDate();
                    toInsert.DELETED = false;
                    _appointmentRepository.Insert(toInsert);
                    _appointmentRepository.Save();
                    InsertInterfaceTeamData(toInsert, profile);
                    return 4;
                }
                else // Rescheduled Appointment
                {
                    var response = _appointmentStatusRepository.GetFirst(x => !(x.DELETED ?? false) && x.DESCRIPTION == "Pending");
                    var toUpdate = _appointmentRepository.GetFirst(x => x.APPOINTMENT_ID == req.APPOINTMENT_ID);
                    toUpdate.NOTES = "";
                    toUpdate.APPOINTMENT_DATE = req.APPOINTMENT_DATE;
                    toUpdate.VISIT_TYPE_ID = req.VISIT_TYPE_ID;
                    toUpdate.APPOINTMENT_STATUS_ID = response.APPOINTMENT_STATUS_ID;
                    toUpdate.TIME_FROM = req.TIME_FROM;
                    toUpdate.TIME_TO = req.TIME_TO;
                    toUpdate.MODIFIED_BY = profile.UserName;
                    toUpdate.MODIFIED_DATE = Helper.GetCurrentDate();
                    _appointmentRepository.Update(toUpdate);
                    _appointmentRepository.Save();
                    InsertInterfaceTeamData(toUpdate, profile);
                    return 4;
                }
            }
        }
        public int CancelAppoinment(Appointment req, UserProfile profile)
        {
            var response = _appointmentStatusRepository.GetFirst(x => !(x.DELETED ?? false) && x.DESCRIPTION == "Cancelled");
            var toUpdate = _appointmentRepository.GetFirst(x => x.APPOINTMENT_ID == req.APPOINTMENT_ID);
            if (toUpdate != null)
                {
                    toUpdate.APPOINTMENT_STATUS_ID = response.APPOINTMENT_STATUS_ID;
                    toUpdate.NOTES = req.NOTES;
                    toUpdate.SIGNATURE_PATH = req.SIGNATURE_PATH;
                    toUpdate.MODIFIED_BY = profile.UserName;
                    toUpdate.MODIFIED_DATE = Helper.GetCurrentDate();
                    toUpdate.CANCELLATION_REASON_ID = req.CANCELLATION_REASON_ID;
                    _appointmentRepository.Update(toUpdate);
                    _appointmentRepository.Save();
                    InsertInterfaceTeamData(toUpdate, profile);
                    return 0;
                }
                else
                {
                    return 1;
                }
        }
        public int EditblockAppoinment(Appointment req, UserProfile profile)
        {

            if (!string.IsNullOrEmpty(req.TIME_FROM))
            {
                req.TIME_FROM_DATE = Convert.ToDateTime(req.TIME_FROM);
                req.TIME_FROM = req.TIME_FROM_DATE.ToString("hh:mm tt");
            }
            else
                req.TIME_FROM_DATE = Helper.GetCurrentDate().AddYears(-100);

            if (!string.IsNullOrEmpty(req.TIME_TO))
            {
                req.TIME_TO_DATE = Convert.ToDateTime(req.TIME_TO);
                req.TIME_TO = req.TIME_TO_DATE.ToString("hh:mm tt");
            }
            else
                req.TIME_TO_DATE = Helper.GetCurrentDate().AddYears(-100);
            if (!string.IsNullOrEmpty(req.APPOINTMENT_DATE_STR))
                req.APPOINTMENT_DATE = Convert.ToDateTime(req.APPOINTMENT_DATE_STR);
            else
                req.APPOINTMENT_DATE = Helper.GetCurrentDate().AddYears(-100);

            //Check if new timings of Provider are not overlapping
            var temp2 = _appointmentRepository.GetFirst(x => x.APPOINTMENT_DATE == req.APPOINTMENT_DATE && x.TIME_FROM == req.TIME_FROM
            && x.TIME_TO == req.TIME_TO && x.PROVIDER_ID == req.PROVIDER_ID && !x.DELETED);

            if (temp2 != null)
            {
                return 2;
            }

            var appOfdate = _appointmentRepository.GetMany(x => x.APPOINTMENT_DATE == req.APPOINTMENT_DATE && x.PROVIDER_ID == req.PROVIDER_ID && !x.DELETED);
            foreach (var item in appOfdate)
            {
                //string startTime = req.TIME_FROM;
                //string endTime = item;

                //When old Time From lies between new Time From and New Time to 
                if (DateTime.Parse(item.TIME_FROM) > DateTime.Parse(req.TIME_FROM)
                    && DateTime.Parse(item.TIME_FROM) < DateTime.Parse(req.TIME_TO)
                    && DateTime.Parse(item.TIME_TO) >= DateTime.Parse(req.TIME_TO))
                {
                    //TimeSpan duration = DateTime.Parse(req.TIME_TO).Subtract(DateTime.Parse(item.TIME_FROM));
                    //if (duration.Minutes > 5 && !req.IS_PROVIDER_OVERLAPPING)
                    //{
                        return 2;
                    //}
                }

            }

            foreach (var item in appOfdate)
            {
                //When old Time To lies between new Time From and New Time to 
                if (DateTime.Parse(item.TIME_TO) > DateTime.Parse(req.TIME_FROM) && DateTime.Parse(item.TIME_TO) < DateTime.Parse(req.TIME_TO))
                {
                    //TimeSpan duration = DateTime.Parse(item.TIME_TO).Subtract(DateTime.Parse(req.TIME_FROM));
                    //if (duration.Minutes > 5 && !req.IS_PROVIDER_OVERLAPPING)
                    //{
                        return 2;
                    //}
                }
            }

            foreach (var item in appOfdate)
            {
                //When new time lies fully in old time 
                if (DateTime.Parse(item.TIME_FROM) < DateTime.Parse(req.TIME_FROM)
                    && DateTime.Parse(item.TIME_FROM) < DateTime.Parse(req.TIME_TO)
                    && DateTime.Parse(item.TIME_TO) > DateTime.Parse(req.TIME_TO)
                    && DateTime.Parse(item.TIME_TO) > DateTime.Parse(req.TIME_FROM))
                {
                    return 2;
                }
            }

            foreach (var item in appOfdate)
            {
                //When old time lies fully in new time 
                if (DateTime.Parse(item.TIME_FROM) > DateTime.Parse(req.TIME_FROM)
                    && DateTime.Parse(item.TIME_FROM) < DateTime.Parse(req.TIME_TO)
                    && DateTime.Parse(item.TIME_TO) > DateTime.Parse(req.TIME_FROM)
                    && DateTime.Parse(item.TIME_TO) < DateTime.Parse(req.TIME_TO))
                {
                    return 2;
                }
            }

            var toUpdate = _appointmentRepository.GetFirst(x => x.APPOINTMENT_ID == req.APPOINTMENT_ID);
            if (toUpdate != null)
            {
                toUpdate.APPOINTMENT_DATE = req.APPOINTMENT_DATE;
                toUpdate.TIME_FROM = req.TIME_FROM;
                toUpdate.TIME_TO = req.TIME_TO;
                toUpdate.MODIFIED_BY = profile.UserName;
                toUpdate.MODIFIED_DATE = Helper.GetCurrentDate();
                _appointmentRepository.Update(toUpdate);
                _appointmentRepository.Save();
                toUpdate.CASE_ID = null;
                toUpdate.PATIENT_ACCOUNT = null;
                InsertInterfaceTeamData(toUpdate, profile);
                return 0;
            }
            else
            {
                return 1;
            }
        }
        public int DeleteBlockAppoinment(Appointment req, UserProfile profile)
        {
            var toUpdate = _appointmentRepository.GetFirst(x => x.APPOINTMENT_ID == req.APPOINTMENT_ID);
            if (toUpdate != null)
            {
                toUpdate.DELETED = true;
                toUpdate.MODIFIED_BY = profile.UserName;
                toUpdate.MODIFIED_DATE = Helper.GetCurrentDate();
                _appointmentRepository.Update(toUpdate);
                _appointmentRepository.Save();
                toUpdate.CASE_ID = null;
                toUpdate.PATIENT_ACCOUNT = null;
                InsertInterfaceTeamData(toUpdate, profile);
                return 0;
            }
            else
            {
                return 1;
            }
        }
        public int OnSaveAddBlock(Appointment req, UserProfile profile)
        {

            if (!string.IsNullOrEmpty(req.TIME_FROM))
            {
                req.TIME_FROM_DATE = Convert.ToDateTime(req.TIME_FROM);
                req.TIME_FROM = req.TIME_FROM_DATE.ToString("hh:mm tt");
            }
            else
                req.TIME_FROM_DATE = Helper.GetCurrentDate().AddYears(-100);

            if (!string.IsNullOrEmpty(req.TIME_TO))
            {
                req.TIME_TO_DATE = Convert.ToDateTime(req.TIME_TO);
                req.TIME_TO = req.TIME_TO_DATE.ToString("hh:mm tt");
            }
            else
                req.TIME_TO_DATE = Helper.GetCurrentDate().AddYears(-100);
            if (!string.IsNullOrEmpty(req.APPOINTMENT_DATE_STR))
                req.APPOINTMENT_DATE = Convert.ToDateTime(req.APPOINTMENT_DATE_STR);
            else
                req.APPOINTMENT_DATE = Helper.GetCurrentDate().AddYears(-100);

            //Check if new timings of Provider are not overlapping
            var temp2 = _appointmentRepository.GetFirst(x => x.APPOINTMENT_DATE == req.APPOINTMENT_DATE && x.TIME_FROM == req.TIME_FROM
            && x.TIME_TO == req.TIME_TO && x.PROVIDER_ID == req.PROVIDER_ID && !x.DELETED);

            if (temp2 != null)
            {
                return 2;
            }

            var appOfdate = _appointmentRepository.GetMany(x => x.APPOINTMENT_DATE == req.APPOINTMENT_DATE && x.PROVIDER_ID == req.PROVIDER_ID && !x.DELETED);
            foreach (var item in appOfdate)
            {
                //string startTime = req.TIME_FROM;
                //string endTime = item;
                //When old Time From lies between new Time From and New Time to 
                if (DateTime.Parse(item.TIME_FROM) > DateTime.Parse(req.TIME_FROM)
                    && DateTime.Parse(item.TIME_FROM) < DateTime.Parse(req.TIME_TO)
                    && DateTime.Parse(item.TIME_TO) >= DateTime.Parse(req.TIME_TO))
                {
                    //TimeSpan duration = DateTime.Parse(req.TIME_TO).Subtract(DateTime.Parse(item.TIME_FROM));
                    //if (duration.Minutes > 5 && !req.IS_PROVIDER_OVERLAPPING)
                    //{
                    return 2;
                    //}
                }

            }

            foreach (var item in appOfdate)
            {
                //When old Time To lies between new Time From and New Time to 
                if (DateTime.Parse(item.TIME_TO) > DateTime.Parse(req.TIME_FROM) && DateTime.Parse(item.TIME_TO) < DateTime.Parse(req.TIME_TO))
                {
                    //TimeSpan duration = DateTime.Parse(item.TIME_TO).Subtract(DateTime.Parse(req.TIME_FROM));
                    //if (duration.Minutes > 5 && !req.IS_PROVIDER_OVERLAPPING)
                    //{
                    return 2;
                    //}
                }
            }

            foreach (var item in appOfdate)
            {
                //When new time lies fully in old time 
                if (DateTime.Parse(item.TIME_FROM) < DateTime.Parse(req.TIME_FROM)
                    && DateTime.Parse(item.TIME_FROM) < DateTime.Parse(req.TIME_TO)
                    && DateTime.Parse(item.TIME_TO) > DateTime.Parse(req.TIME_TO)
                    && DateTime.Parse(item.TIME_TO) > DateTime.Parse(req.TIME_FROM))
                {
                    return 2;
                }
            }

            foreach (var item in appOfdate)
            {
                //When old time lies fully in new time 
                if (DateTime.Parse(item.TIME_FROM) > DateTime.Parse(req.TIME_FROM)
                    && DateTime.Parse(item.TIME_FROM) < DateTime.Parse(req.TIME_TO)
                    && DateTime.Parse(item.TIME_TO) > DateTime.Parse(req.TIME_FROM)
                    && DateTime.Parse(item.TIME_TO) < DateTime.Parse(req.TIME_TO))
                {
                    return 2;
                }
            }

            if (req.IS_NEW) // Add Block new Appointment
            {

                Appointment toInsert = new Appointment();
                var response = _appointmentStatusRepository.GetFirst(x => !(x.DELETED ?? false) && x.DESCRIPTION == "Pending");

                toInsert.APPOINTMENT_ID = Helper.getMaximumId("FOX_APPOINTMENT_ID");
                toInsert.APPOINTMENT_DATE = req.APPOINTMENT_DATE;
                toInsert.VISIT_TYPE_ID = 0;
                toInsert.APPOINTMENT_STATUS_ID = 0;
                toInsert.CASE_ID = 0 ;
                toInsert.PROVIDER_ID = req.PROVIDER_ID;
                toInsert.PATIENT_ACCOUNT = 0;
                toInsert.PRACTICE_CODE = profile.PracticeCode;
                toInsert.TIME_FROM = req.TIME_FROM;
                toInsert.TIME_TO = req.TIME_TO;
                toInsert.SIGNATURE_PATH = profile.SIGNATURE_PATH;
                toInsert.MODIFIED_BY = profile.UserName;
                toInsert.MODIFIED_DATE = Helper.GetCurrentDate();
                toInsert.CREATED_BY = profile.UserName;
                toInsert.CREATED_DATE = Helper.GetCurrentDate();
                toInsert.DELETED = false;
                toInsert.IsBlocked = true;
                toInsert.ReasonForBlocked = "Blocked Slot";
                _appointmentRepository.Insert(toInsert);
                _appointmentRepository.Save();
                toInsert.CASE_ID = null;
                toInsert.PATIENT_ACCOUNT = null;
                InsertInterfaceTeamData(toInsert, profile);
                return 0;
            }
            else
            {
                return 1;
            }
        }
        public string ExportToAppointmentList(AppointmentSearchRequest req, UserProfile profile)
        {
            try
            {
                string fileName = "Appointment_List";
                string exportPath = "";
                string path = string.Empty;
                bool exported;
                req.CURRENT_PAGE = 1;
                req.RECORD_PER_PAGE = 0;
                var CalledFrom = "";
                if (req.PATIENT_ACCOUNT == "0")
                {
                    CalledFrom = "Daily_Appointment_List";
                }
                else
                {
                    CalledFrom = "Patient_Scheduler_List";
                }

                string virtualPath = @"/" + profile.PracticeDocumentDirectory + "/" + "Fox/ExportedFiles/";
                exportPath = HttpContext.Current.Server.MapPath("~" + virtualPath);
                fileName = DocumentHelper.GenerateSignatureFileName(fileName) + ".xlsx";
                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                }
                List<Appointment> result = new List<Appointment>();
                var pathtowriteFile = exportPath + "\\" + fileName;
                result = GetAllAppointments(req, profile);
                CultureInfo culture_info = Thread.CurrentThread.CurrentCulture;
                TextInfo text_info = culture_info.TextInfo;
                for (int i = 0; i < result.Count(); i++)
                {
                    //result[i].ROW = i + 1;
                    if(result[i].HOME_PHONE != null && result[i].HOME_PHONE != "" && result[i].NAME != null && result[i].NAME != "")
                    {
                      result[i].NAME = result[i].NAME + " - " + result[i].HOME_PHONE;
                    }
                    else if(result[i].HOME_PHONE != null && result[i].HOME_PHONE != "" && (result[i].NAME == null || result[i].NAME == ""))
                        {
                        result[i].NAME = result[i].HOME_PHONE;
                    }
                    else
                    {
                        result[i].NAME = result[i].NAME;
                    }
                    if (result[i].POS_ADDRESS != null && result[i].POS_CITY != null)
                    {
                        result[i].ADDRESS = checkNull(result[i].POS_CODE) + " " + checkNull(result[i].POS_NAME) + " " + Environment.NewLine + Environment.NewLine +
                       checkNull(result[i].POS_ADDRESS) + ", " + checkNull(text_info.ToTitleCase(result[i].POS_CITY?.ToLower())) + ", " + checkNull(result[i].POS_STATE) + " " + checkNull(result[i].POS_ZIP);
                    }
                    result[i].PROVIDER = string.IsNullOrEmpty(result[i].PROVIDER) ? string.Empty : text_info.ToTitleCase(result[i].PROVIDER);
                }
                exported = ExportToExcel.CreateExcelDocument<Appointment>(result, pathtowriteFile, CalledFrom.Replace(' ', '_'));
                return virtualPath + fileName;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string getHomePhoneInFormat(string phone)
        {
            string str = "";
            if (phone != null && phone != "")
            {
                str ="Home: " + getNumberInFormat(phone);
            }
            return str;
        }
        public string getPatientInFormat(string name, string fc)
        {
            string str ="";
            if ((name == null || name == "") && (fc != null && fc != ""))// when only name is missing
            {
                str = fc;
            }
            if ((fc == null || fc == "") && (name != null && name != "")) // when only fc is missing
            {
                str = name ;
            }
            if ((fc == null || fc == "") && (name == null && name == "")) // when both are missing
            {
                str = "";
            }
            if ((fc != null && fc != "") && (name != null && name != "")) // when both are present
            {
                str = name + " - "+ fc;
            }
            return str;
        }
        public string getNumberInFormat(string number)
        {
            if (number != "" && number != null && number.Length == 10)
            {
                string temp1, temp2, temp3;
                temp1 = number.Substring(0, 3);
                temp2 = number.Substring(3, 3);
                temp3 = number.Substring(6, 4);

                return "(" + temp1 + ") " + temp2 + "-" + temp3;
            }
            else
            {
                return "";
            }

        }
        public string getZIPInFormat(string zip)
        {
            if (zip != "" && zip != null && zip.Length > 5 && zip.Length == 9)
            {
                string temp1, temp2;
                temp1 = zip.Substring(0, 5);
                temp2 = zip.Substring(5, 4);
              

                return temp1 + "-" + temp2;
            }
            else
            {
                return "";
            }

        }
        public string checkNull(string str)
        {
            if (str == null)
            {
                return "";
            }
            else
            {
                return str;
            }
        }
        public List<NoteAlert> GetAlerts(UserProfile profile, string patientAccount)
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
        private WeekAppointmentsWithDay GetDayWise(List<Appointment> WeeklyAppointmentsList)
        {
            WeekAppointmentsWithDay weekAppointmentsWithDay = new WeekAppointmentsWithDay();
            weekAppointmentsWithDay.Monday = new List<Appointment>();
            weekAppointmentsWithDay.Tuesday = new List<Appointment>();
            weekAppointmentsWithDay.Wednesday = new List<Appointment>();
            weekAppointmentsWithDay.Thursday = new List<Appointment>();
            weekAppointmentsWithDay.Friday = new List<Appointment>();
            weekAppointmentsWithDay.Saturday = new List<Appointment>();
            weekAppointmentsWithDay.Sunday = new List<Appointment>();

            var WeekDateWiseList = WeeklyAppointmentsList.Where(c => c.APPOINTMENT_DATE != null).GroupBy(x => x.APPOINTMENT_DATE).ToList();
            foreach (var item in WeekDateWiseList)
            {
                string date = "";
                string Day = "";
                date = DateTime.Parse(item.Key.ToString()).ToString("MM/dd/yyyy");
                if (!string.IsNullOrEmpty(date))
                {
                    Day = Convert.ToDateTime(date).DayOfWeek.ToString();
                    if (!string.IsNullOrEmpty(Day))
                    {
                        //var tempItem = item.OrderByDescending(i => i.DURATION); 
                        for (int i = 0; i < item.Count(); i++)
                        {
                            Appointment AppointmentsOnSpecificDay = new Appointment();
                            AppointmentsOnSpecificDay = item.ElementAt(i);

                            switch (Day)
                            {
                                case "Monday":
                                    weekAppointmentsWithDay.Monday.Add(AppointmentsOnSpecificDay);
                                    break;
                                case "Tuesday":
                                    weekAppointmentsWithDay.Tuesday.Add(AppointmentsOnSpecificDay);
                                    break;
                                case "Wednesday":
                                    weekAppointmentsWithDay.Wednesday.Add(AppointmentsOnSpecificDay);
                                    break;
                                case "Thursday":
                                    weekAppointmentsWithDay.Thursday.Add(AppointmentsOnSpecificDay);
                                    break;
                                case "Friday":
                                    weekAppointmentsWithDay.Friday.Add(AppointmentsOnSpecificDay);
                                    break;
                                case "Saturday":
                                    weekAppointmentsWithDay.Saturday.Add(AppointmentsOnSpecificDay);
                                    break;
                                case "Sunday":
                                    weekAppointmentsWithDay.Sunday.Add(AppointmentsOnSpecificDay);
                                    break;
                            }

                        }

                    }
                }
            }
            return weekAppointmentsWithDay;
        }
        // this is for weekly
        public IEnumerable<SPGetLocations> GetLocations(long practiceCode)
        {
            var param1 = new SqlParameter("practiceCode", SqlDbType.BigInt) { Value = practiceCode };
            var getLocations = SpRepository<SPGetLocations>.GetListWithStoreProcedure("exec GET_ALL_LOCATIONS_FOR_SCHEDULER @practiceCode", param1);
            if (getLocations.Any())
            {
                return getLocations;
            }
            return null;
        }
        public void InsertInterfaceTeamData(Appointment obj, UserProfile Profile)
        {
            InterfaceSynchModel interfaceSynch = new InterfaceSynchModel();

            if (obj.APPOINTMENT_ID == null)
            {

            }
            else
            {
                interfaceSynch.FOX_INTERFACE_SYNCH_ID = Helper.getMaximumId("FOX_INTERFACE_SYNCH_ID");
                interfaceSynch.CASE_ID = obj.CASE_ID;
                interfaceSynch.Work_ID = null;
                interfaceSynch.TASK_ID = null;
                interfaceSynch.APPOINTMENT_ID = obj.APPOINTMENT_ID;
                interfaceSynch.PATIENT_ACCOUNT = obj.PATIENT_ACCOUNT;
                interfaceSynch.APPLICATION = "PORTAL SCHEDULER APPOINTMENTS";
                interfaceSynch.PRACTICE_CODE = Profile.PracticeCode;
                interfaceSynch.MODIFIED_BY = interfaceSynch.CREATED_BY = Profile.UserName;
                interfaceSynch.MODIFIED_DATE = interfaceSynch.CREATED_DATE = DateTime.Now;
                interfaceSynch.DELETED = false;
                interfaceSynch.IS_SYNCED = false;
                _InterfaceSynchModelRepository.Insert(interfaceSynch);
                _InterfaceSynchModelRepository.Save();
            }
        }
    }

}
