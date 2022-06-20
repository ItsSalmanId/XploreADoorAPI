using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FoxRehabilitationAPI
{
    internal class WorkAroundForOperationCancelledException : DelegatingHandler
    {
        public WorkAroundForOperationCancelledException()
        { }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
       {
            try
            {
                var response = new HttpResponseMessage();
                using (var cts = new CancellationTokenSource())
                {
                    response = !cancellationToken.IsCancellationRequested ? await base.SendAsync(request, cts.Token) : request.CreateResponse(System.Net.HttpStatusCode.OK);
                    //response = await base.SendAsync(request, cts.Token);
                }
                return response;
            }
            catch (Exception)
            {
                return await _BadRequest(request);
            }
        }
        private Task<HttpResponseMessage> _BadRequest(HttpRequestMessage request)
        {
            var response = request.CreateResponse(HttpStatusCode.Gone);
            response.ReasonPhrase = "Client Has Gone";
            return Task.FromResult<HttpResponseMessage>(response);
        }
    }
}