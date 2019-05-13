using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TRMDesktopUserI.Library.Models;
using TRMDesktopUserI.Library.Models.Details;
using static TRMDesktopUserI.Library.Models.ProjectDetail;

namespace Mvc.Controllers
{
    public class DetailsController : Controller
    {
        // GET: Details
        public ActionResult Index()
        {
            return View();
        }
        public void MyFunction(params KeyValuePair<string, object>[] pairs)
        {
            // ...
        }
        public static class Pairing
        {
            public static KeyValuePair<string, object> Of(string key, object value)
            {
                
                return new KeyValuePair<string, object>(key, value);
                
            }
        }
        public async Task<ActionResult> Details(ProjectDetail details)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://www.gdfindi.com:442/api/");
            string token = (string)Session["Access_Token"];

            //Define Headers
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //Request params
            

            MyFunction(Pairing.Of("target[0]", 1), Pairing.Of("target[1]", 2), Pairing.Of("target[2]", 7));
            
              
            HttpResponseMessage response = await client.GetAsync("v1/Projects/"+Session["id"].ToString()+"?target[0]=1&target[1]=2&target[2]=7");
            //HttpResponseMessage response = await client.GetAsync("v1/Projects/20740?target[0]=1&target[1]=2&target[2]=7");



            if (response.IsSuccessStatusCode == false)
                {
                    return View("Index");
                }
                else
                {

               //Get The Project Detail
                var ProjectDetail = await response.Content.ReadAsAsync<ProjectDetail>();


                if (ProjectDetail != null)
                {
                    ProjectDetail D = new ProjectDetail();
                      D.id = ProjectDetail.id;
                      D.name = ProjectDetail.name;
                      D.owner = ProjectDetail.owner;
                      D.version = ProjectDetail.version;
                    
                      return View(D);
                    


                }

              
                return RedirectToAction("Details", "Details");
            }

        }// end of details 
      
    }
}