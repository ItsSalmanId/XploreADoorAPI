using FOX.BusinessOperations.SearchOrderServices;
using FOX.DataModels.Models.SearchOrderModel;
using FoxRehabilitationAPI.Filters;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FoxRehabilitationAPI.Controllers
{
    [Authorize]
    [ExceptionHandlingFilter]
    public class SearchOrderController : BaseApiController
    {
        private readonly ISearchOrderServices _searchOrderService;

        public SearchOrderController(ISearchOrderServices searchOrderServices)
        {
            _searchOrderService = searchOrderServices;
        }
        [HttpPost]
        public HttpResponseMessage ExportToExcelSearchOrder(SearchOrderRequest searchOrderRequest)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _searchOrderService.ExportToExcelSearchOrder(searchOrderRequest, GetProfile()));
        }
        [HttpPost]
        public HttpResponseMessage GetSearchOrder(SearchOrderRequest searchOrderRequest)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _searchOrderService.GetSearchOrder(searchOrderRequest, GetProfile()));
        }
    }
}
