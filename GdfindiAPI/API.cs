using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using TRMDesktopUserI.Library.Models;
using TRMDesktopWPFUI.Models;

namespace Mvc.GdfindiAPI
{
    public class APIHelper
    {
        public HttpClient apiClient;
        public ILoggedInUserModel _loggedInUser;



        public APIHelper(ILoggedInUserModel loggedInUser)

        {
            GetInitializeClient();
            _loggedInUser = loggedInUser;
        }

       

        public void GetInitializeClient()
        {
            string api = ConfigurationManager.AppSettings["api"];

            apiClient = new HttpClient();
            apiClient.BaseAddress = new Uri(api);
            apiClient.DefaultRequestHeaders.Accept.Clear();
            apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

      
         async Task<AuthenticatedUser> Authenticate(string username, string password)

        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password),
            });
            
            using (HttpResponseMessage response = await apiClient.PostAsync("token", data))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<AuthenticatedUser>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

         async Task GetLoggedInUserInfo(string token)
            {
                apiClient.DefaultRequestHeaders.Clear();
                apiClient.DefaultRequestHeaders.Accept.Clear();
                apiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Get Project lists
                using (HttpResponseMessage response = await apiClient.GetAsync("v1/Projects"))
                {

                    if (response.IsSuccessStatusCode)
                    {

                    //var data = await response.Content.ReadAsAsync<List<LoggedInUserModel>>();
                    var data = 2;
                        var result = JsonConvert.SerializeObject(data);
                        var Projectinfo = JsonConvert.DeserializeObject(result);
                    }
                    else
                    {
                        throw new Exception(response.ReasonPhrase);

                    }
                }
            
        }
        
    }
}
