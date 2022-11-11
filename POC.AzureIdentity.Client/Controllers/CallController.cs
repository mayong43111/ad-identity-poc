using Microsoft.Azure.Services.AppAuthentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace POC.AzureIdentity.Client.Controllers
{
    [Route("[controller]/[action]")]
    public class CallController : Controller
    {
        public async Task<ActionResult> Localhost()
        {
            var url = "http://localhost:5022/";

            var res = await CallController.SendRequest(url);

            return Content(res);
        }

        public async Task<ActionResult> Azure()
        {
            var url = "https://bayer-ad-identity-poc-service.azurewebsites.net/";

            var res = await CallController.SendRequest(url);

            return Content(res);
        }

        private static async Task<string> SendRequest(string url)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(url);

            if (client.BaseAddress.Host.Contains("azure")) //演示偷懒的办法，实际可以作为配置项
            {
                //本地运行，配置环境变量 AzureServicesAuthConnectionString  RunAs=App;AppId={AppId};TenantId={TenantId};AppKey={ClientSecret}
                //https://learn.microsoft.com/en-us/dotnet/api/overview/azure/service-to-service-authentication?view=azure-dotnet

                var azureServiceTokenProvider = new AzureServiceTokenProvider();
                var accessToken = await azureServiceTokenProvider.GetAccessTokenAsync("5fd71e75-feef-4651-844e-c25791591af9"); //演示偷懒的办法，实际可以作为配置项

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            var res = await client.GetAsync("WeatherForecast");

            if (res.IsSuccessStatusCode)
            {
                return await res.Content.ReadAsStringAsync();
            }
            else
            {
                return $"[{res.StatusCode}]";
            }
        }
    }
}