using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Unity;
using FOX.BusinessOperations.OriginalQueueService;
using FOX.BusinessOperations.CompleteQueueService;
using FOX.BusinessOperations.UnAssignedQueueService;
using FOX.BusinessOperations.AccountService;
using FOX.BusinessOperations.GeneralNotesService;
using FOX.BusinessOperations.TaskServices;
using FOX.BusinessOperations.CaseServices;
using FOX.BusinessOperations.GroupServices;

namespace FoxRehabilitationAPI.App_Start
{
    public static class DIContainer
    {
        public static UnityContainer GetContainer()
        {
            var container = new UnityContainer();
            container.RegisterType<FOX.BusinessOperations.Security.ITokenService, FOX.BusinessOperations.Security.TokenService>();
            container.RegisterType<FOX.BusinessOperations.Security.IUserServices, FOX.BusinessOperations.Security.UserService>();
            container.RegisterType<FOX.BusinessOperations.SettingsService.UserMangementService.IUserManagementService, FOX.BusinessOperations.SettingsService.UserMangementService.UserManagementService>();
            container.RegisterType<IOriginalQueueService, OriginalQueueService>();
            container.RegisterType<FOX.BusinessOperations.PatientServices.IPatientService, FOX.BusinessOperations.PatientServices.PatientService>();
            container.RegisterType<FOX.BusinessOperations.IndexedQueueService.IIndexedQueueServices, FOX.BusinessOperations.IndexedQueueService.IndexedQueueServices>();
            container.RegisterType<IUnAssignedQueueService, UnAssignedQueueService>();
            container.RegisterType<ICompleteQueueService, CompleteQueueService>();
            container.RegisterType<FOX.BusinessOperations.AssignedQueueService.IAssignedQueueServices, FOX.BusinessOperations.AssignedQueueService.AssignedQueueServices>();
            container.RegisterType<FOX.BusinessOperations.DashboardServices.IDashboardService, FOX.BusinessOperations.DashboardServices.DashboardService>();
            container.RegisterType<FOX.BusinessOperations.SearchOrderServices.ISearchOrderServices, FOX.BusinessOperations.SearchOrderServices.SearchOrderServices>();
            container.RegisterType<FOX.BusinessOperations.IndexInfoServices.IIndexInfoService, FOX.BusinessOperations.IndexInfoServices.IndexInfoService>();
            container.RegisterType<FOX.BusinessOperations.SupervisorWorkService.ISupervisorWorkService, FOX.BusinessOperations.SupervisorWorkService.SupervisorWorkService>();
            container.RegisterType<FOX.BusinessOperations.CommonServices.ICommonServices, FOX.BusinessOperations.CommonServices.CommonServices>();
            container.RegisterType<FOX.BusinessOperations.StoreProcedure.SettingsService.ReferralSourceService.IReferralSourceService, FOX.BusinessOperations.StoreProcedure.SettingsService.ReferralSourceService.ReferralSourceService>();
            container.RegisterType<FOX.BusinessOperations.SettingsService.FacilityLocationService.IFacilityLocationService, FOX.BusinessOperations.SettingsService.FacilityLocationService.FacilityLocationService>();
            container.RegisterType<FOX.BusinessOperations.RequestForOrder.IRequestForOrderService, FOX.BusinessOperations.RequestForOrder.RequestForOrderService>();
            container.RegisterType<FOX.BusinessOperations.ReportingServices.ReferralReportServices.IReferralReportServices, FOX.BusinessOperations.ReportingServices.ReferralReportServices.ReferralReportServices>();
            container.RegisterType<FOX.BusinessOperations.RequestForOrder.UploadOrderImages.IUploadOrderImagesService, FOX.BusinessOperations.RequestForOrder.UploadOrderImages.UploadOrderImagesService>();
            container.RegisterType<FOX.BusinessOperations.CommonServices.UploadFiles.IUploadFilesServices, FOX.BusinessOperations.CommonServices.UploadFiles.UploadFilesServices>();
            container.RegisterType<FOX.BusinessOperations.PatientSurveyService.IPatientSurveyService, FOX.BusinessOperations.PatientSurveyService.PatientSurveyService>();
            container.RegisterType<FOX.BusinessOperations.PatientSurveyService.SearchSurveyService.ISearchSurveyService, FOX.BusinessOperations.PatientSurveyService.SearchSurveyService.SearchSurveyService>();
            container.RegisterType<FOX.BusinessOperations.PatientSurveyService.UploadDataService.IUploadDataService, FOX.BusinessOperations.PatientSurveyService.UploadDataService.UploadDataService>();
            container.RegisterType<FOX.BusinessOperations.PatientSurveyService.SurveyReportsService.ISurveyReportsService, FOX.BusinessOperations.PatientSurveyService.SurveyReportsService.SurveyReportsService>();
            container.RegisterType<FOX.BusinessOperations.CaseServices.ICaseServices, CaseServices>();
            container.RegisterType<FOX.BusinessOperations.TaskServices.ITaskServices, TaskServices>();
            container.RegisterType<FOX.BusinessOperations.Security.IPasswordHistoryService, FOX.BusinessOperations.Security.PasswordHistoryService>();
            container.RegisterType<FOX.BusinessOperations.RequestForOrder.IndexInformationServices.IIndexInformationService, FOX.BusinessOperations.RequestForOrder.IndexInformationServices.IndexInformationService>();
            container.RegisterType<IAccountServices, AccountServices>();
            container.RegisterType<IGeneralNotesServices, GeneralNotesServices>();
            container.RegisterType<FOX.BusinessOperations.SettingsService.ReferralRegionServices.IReferralRegionService, FOX.BusinessOperations.SettingsService.ReferralRegionServices.ReferralRegionService>();
            container.RegisterType<FOX.BusinessOperations.FoxPHDService.IFoxPHDService, FOX.BusinessOperations.FoxPHDService.FoxPHDService>();
            container.RegisterType<FOX.BusinessOperations.PatientDocumentsService.IPatientDocumentsService, FOX.BusinessOperations.PatientDocumentsService.PatientDocumentsService>();
            container.RegisterType<FOX.BusinessOperations.ReconciliationService.IReconciliationService, FOX.BusinessOperations.ReconciliationService.ReconciliationService>();

            container.RegisterType<IGroupService, GroupService>();
            // container.RegisterType<FOX.BusinessOperations.ReconnectCallService.IReconnectCallService, FOX.BusinessOperations.ReconnectCallService.ReconnectCallService>();
            container.RegisterType<FOX.BusinessOperations.SettingsService.ClinicianSetupService.IClinicianSetupService, FOX.BusinessOperations.SettingsService.ClinicianSetupService.ClinicianSetupService>();
            container.RegisterType<FOX.BusinessOperations.PatientMaintenanceService.PatientInsuranceService.IPatientInsuranceService, FOX.BusinessOperations.PatientMaintenanceService.PatientInsuranceService.PatientInsuranceService>();
            container.RegisterType<FOX.BusinessOperations.PatientMaintenanceService.IPatientMaintenanceService, FOX.BusinessOperations.PatientMaintenanceService.PatientMaintenanceService>();
            container.RegisterType<FOX.BusinessOperations.IndexInfoServices.UploadWorkOrderFiles.IUploadWorkOrderFilesService, FOX.BusinessOperations.IndexInfoServices.UploadWorkOrderFiles.UploadWorkOrderFilesService>();
            container.RegisterType<FOX.BusinessOperations.QualityAssuranceService.EvaluationSetupService.IEvaluationSetupService, FOX.BusinessOperations.QualityAssuranceService.EvaluationSetupService.EvaluationSetupService>();
            container.RegisterType<FOX.BusinessOperations.QualityAssuranceService.PerformAuditService.IPerformAuditService, FOX.BusinessOperations.QualityAssuranceService.PerformAuditService.PerformAuditService>();
            container.RegisterType<FOX.BusinessOperations.QualityAssuranceService.QADashboardService.IQADashboardService, FOX.BusinessOperations.QualityAssuranceService.QADashboardService.QADashboardService>();
            container.RegisterType<FOX.BusinessOperations.QualityAssuranceService.QAReportService.IQAReportService, FOX.BusinessOperations.QualityAssuranceService.QAReportService.QAReportService>();
            container.RegisterType<FOX.BusinessOperations.Scheduler.ISchedulerService, FOX.BusinessOperations.Scheduler.SchedulerService>();
            container.RegisterType<FOX.BusinessOperations.HrAutoEmail.IHrAutoEmailService, FOX.BusinessOperations.HrAutoEmail.HrAutoEmailService>();
            container.RegisterType<FOX.BusinessOperations.SignatureRequiredServices.ISignatureRequiredService, FOX.BusinessOperations.SignatureRequiredServices.SignatureRequiredService>();
            return container;
        }
    }
}