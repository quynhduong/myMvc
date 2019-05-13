using Caliburn.Micro;
using Mvc.GdfindiAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TRMDesktopUserI.Library.Models;
using TRMDesktopWPFUI.Library.Api;
using TRMDesktopWPFUI.Models;
using TRMDesktopWPFUserInterface;
using APIHelper = TRMDesktopWPFUI.Library.Api.APIHelper;

namespace Mvc.Controllers
{
    public class LoginController : Controller
    {
        public HttpClient apiClient;
        // GET: Login
        public ActionResult Index()
        {
           
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Autherize(AuthenticatedUser userModel)
        {
            using (var client = new HttpClient())
            {
                //Define Headers
                client.BaseAddress = new Uri("https://www.gdfindi.com:442/api/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

           
                //Prepare Request Body
                List<KeyValuePair<string, string>> requestData = new List<KeyValuePair<string, string>>();  
                    requestData.Add(new KeyValuePair<string, string>("grant_type", "password"));
                    requestData.Add(new KeyValuePair<string, string>("username", userModel.UserName));
                    requestData.Add(new KeyValuePair<string, string>("password", userModel.Password));


                    FormUrlEncodedContent requestBody = new FormUrlEncodedContent(requestData);

                    //Request Token
                    var response = await client.PostAsync("token", requestBody);


                     var userDetails = await response.Content.ReadAsAsync<AuthenticatedUser>();
                     
                    if (response.IsSuccessStatusCode==false)
                    {
                        userModel.LoginErrorMessage = "Wrong username or password";
                        return View("Index", userModel);
                    }
                    else
                    {
                        Session["userName"] = userDetails.UserName;
                        Session["Access_Token"] = userDetails.Access_Token;
                        return RedirectToAction("Index", "Home");
                }
            }
        }

        public ActionResult LogOut()
        {

            return RedirectToAction("Index","Login");
           
        }

        public ActionResult projectdetail()
        {
            return View("ProjectList", "Project");
            
        }
     
    }
}

  

