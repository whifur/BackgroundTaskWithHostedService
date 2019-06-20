using Microsoft.AspNetCore.Http;
 
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Bgwhs.Service
{
    public  interface IScopedProcessingService
    {
        Task ConsumeApi();
    }
    public class ConsumeApiService: IScopedProcessingService
    {
        private readonly IHttpClientFactory _clientFactory;
        //private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<ConsumeApiService> _logger;
        public ConsumeApiService(/*IHttpContextAccessor httpContextAccessor, */IHttpClientFactory clientFactory, ILogger<ConsumeApiService> logger)
        {
            //_httpContextAccessor = httpContextAccessor;
            _clientFactory = clientFactory;
            _logger = logger;
        }

        public async Task ConsumeApi()
        {
            //call api
            try
            {
                UserIdGenerator.Current += 1;
                string apiUrl = string.Format("{0}?UserId={1}", GetApiUrl(), UserIdGenerator.Current);
                var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
                request.Headers.Add("Accept", "application/json");
                request.Headers.Add("User-Agent", "HttpClientFactory-Sample");

                var client = _clientFactory.CreateClient();

                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var ret = await response.Content.ReadAsStringAsync();
                    var okMsg = string.Format("ConsumeApi ok {0} {1} {2}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), System.Environment.NewLine, ret);
                    _logger.LogDebug(okMsg);
                }
                else
                {
                    var errMsg = string.Format("ConsumeApi err {0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                    _logger.LogDebug(errMsg);
                }
            }catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }
        //private Uri GetAbsoluteUri(string path)
        //{
        //    var request =  _httpContextAccessor.HttpContext.Request;  //null coz no http create
        //    UriBuilder uriBuilder = new UriBuilder();
        //    uriBuilder.Scheme = request.Scheme;
        //    uriBuilder.Host = request.Host.Host;
        //    uriBuilder.Port = request.Host.Port.Value;
        //    uriBuilder.Path = path;// request.Path.ToString();
        //    //uriBuilder.Query = request.QueryString.ToString();
        //    return uriBuilder.Uri;
        //}
        private string GetApiUrl()
        {
            return "http://localhost:19846/api/Sync/SynchronizeUserData";
            //var apiUrl = GetAbsoluteUri("api/Sync/SynchronizeUserData").ToString();
            //return apiUrl;
        }
    }

    public static class UserIdGenerator
    {
        public static int Current { get; set; }
    }
}
