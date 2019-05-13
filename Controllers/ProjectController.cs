using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using TRMDataManager.Library.Models;
using TRMDesktopUserI.Library.Models;
using TRMDesktopWPFUI.Models;
using ProjectModel = TRMDataManager.Library.Models.ProjectModel;


namespace Mvc.Controllers
{
    public class ProjectController : Controller
    {
        // GET: Project
        
        public ActionResult Index()
        {

          
            return View();
        }


        public async Task<ActionResult> ProjectList(LoggedInUserModel projectModel)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://www.gdfindi.com:442/api/");
            string token = (string)Session["Access_Token"];

            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync("v1/Projects");
            var data = await response.Content.ReadAsAsync<List<LoggedInUserModel>>();
            

            if (response.IsSuccessStatusCode == false)
            {
                return View("Index");
            }
            else
            {
                List<LoggedInUserModel> myList = new List<LoggedInUserModel>(data);
                LoggedInUserModel p = new LoggedInUserModel();
                foreach (var i in myList)
                {
                    p.name = i.name;
                    p.ID = i.ID;
                    // p.IsChecked = i.IsChecked;
                    Session["data"] = i.ID;
                    myList.Add(p);
                    return View(myList);
                }
                List<LoggedInUserModel> myModel = myList as List<LoggedInUserModel>;



                //return RedirectToAction("ProjectList", "Project",myList);

                return RedirectToAction("ProjectList", "Project");

            }
        }


            //}
            /*public ActionResult detail()
            {
                return View("Details", "Details");

            }*/
           [HttpPost]
            public ActionResult detail(bool Example)
            {
                 
                if (Example != false)
                {
                    var ID = Convert.ToString(Request.Form["ID"]);
                      Session["id"] = ID;
                     return RedirectToAction("Details", "Details");

                }

                return null;
            }
        




    }
}