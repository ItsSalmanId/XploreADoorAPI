using FOX.DataModels.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FOX.DataModels.Models.Scheduler.SchedulerModel;

namespace FOX.BusinessOperations.Scheduler
{
    public interface ISchedulerService
    {
        List<Appointment> GetAllAppointments(AppointmentSearchRequest reg, UserProfile profile);
        WeekAppointmentsWithDay GetAllAppointmentsWeekly(AppointmentSearchRequest reg, UserProfile profile);
        List<AppointmentStatus> GetAppointmentStatusList();
        List<VisitType> GetVisitTypeList();
        List<CancellationReason> GetCancellationReasonsList();
        List<ProviderList> GetProviderList(long practiceCode);
        List<RegionList> GetRegionsList(long practiceCode); 
        int RescheduleAppoinment(Appointment req, UserProfile profile);
        int CancelAppoinment(Appointment req, UserProfile profile);
        int EditblockAppoinment(Appointment req, UserProfile profile); 
        int DeleteBlockAppoinment(Appointment req, UserProfile profile);
        int OnSaveAddBlock(Appointment req, UserProfile profile);
        string ExportToAppointmentList(AppointmentSearchRequest req, UserProfile profile);
        //this is for Weekly Appointment
        IEnumerable<SPGetLocations> GetLocations(long practiceCode);
    }
}
