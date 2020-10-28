using FOX.BusinessOperations.Scheduler;
using FoxRehabilitationAPI.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using static FOX.DataModels.Models.Scheduler.SchedulerModel;

namespace FoxRehabilitationAPI.Controllers.Scheduler
{
    [ExceptionHandlingFilter]
    public class SchedulerController : BaseApiController
    {
        private readonly ISchedulerService _SchedulerService;
        public SchedulerController(ISchedulerService SchedulerService)
        {
            _SchedulerService = SchedulerService;
        }
        [HttpPost]
        public HttpResponseMessage GetAllAppointments(AppointmentSearchRequest req)
        {
            return Request.CreateResponse(HttpStatusCode.OK,
                _SchedulerService.GetAllAppointments(req, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage GetAllAppointmentsWeekly(AppointmentSearchRequest req)
        {
            return Request.CreateResponse(HttpStatusCode.OK,
                _SchedulerService.GetAllAppointmentsWeekly(req, GetProfile()));
        }
        [HttpGet]
        public HttpResponseMessage GetAppointmentStatusList()
        {
            var profile = GetProfile();
            var result = _SchedulerService.GetAppointmentStatusList();
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }
        [HttpGet]
        public HttpResponseMessage GetVisitTypeList()
        {
            var profile = GetProfile();
            var result = _SchedulerService.GetVisitTypeList();
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }
        [HttpGet]
        public HttpResponseMessage GetCancellationReasonsList()
        {
            var profile = GetProfile();
            var result = _SchedulerService.GetCancellationReasonsList();
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }
        [HttpGet]
        public HttpResponseMessage GetProviderList()
        {
            var profile = GetProfile();
            var result = _SchedulerService.GetProviderList(profile.PracticeCode);
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }
        [HttpGet]
        public HttpResponseMessage GetRegionsList()
        {
            var profile = GetProfile();
            var result = _SchedulerService.GetRegionsList(profile.PracticeCode);
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }
        [HttpPost]
        public HttpResponseMessage RescheduleAppoinment(Appointment req)
        {
            var profile = GetProfile();
            var result = _SchedulerService.RescheduleAppoinment(req, profile);
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }
        [HttpPost]
        public HttpResponseMessage CancelAppoinment(Appointment req)
        {
            var profile = GetProfile();
            var result = _SchedulerService.CancelAppoinment(req, profile);
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }
        [HttpPost] 
        public HttpResponseMessage EditblockAppoinment(Appointment req)
        {
            var profile = GetProfile();
            var result = _SchedulerService.EditblockAppoinment(req, profile);
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }
        [HttpPost] 
        public HttpResponseMessage DeleteBlockAppoinment(Appointment req)
        {
            var profile = GetProfile();
            var result = _SchedulerService.DeleteBlockAppoinment(req, profile);
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }
        [HttpPost]
        public HttpResponseMessage OnSaveAddBlock(Appointment req)
        {
            var profile = GetProfile();
            var result = _SchedulerService.OnSaveAddBlock(req, profile);
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }
        [HttpPost]
        public HttpResponseMessage ExportToAppointmentList(AppointmentSearchRequest req)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _SchedulerService.ExportToAppointmentList(req, GetProfile()));
        }
        //this is for Weekly Appointment
        [HttpGet]
        public HttpResponseMessage GetLocations()
        {
            var profile = GetProfile();
            var result = _SchedulerService.GetLocations(profile.PracticeCode);
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }
    }
}