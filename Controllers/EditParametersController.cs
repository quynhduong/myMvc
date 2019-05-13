using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TRMDesktopUserI.Library.Models.Put;

namespace Mvc.Controllers
{
    public class EditParametersController : Controller
    {
        // GET: EditParameters
        public ActionResult Edit()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Edit(Goal productiongoal)
        {
            using (var client = new HttpClient())
            {
                string token = (string)Session["Access_Token"];

                //Define Headers
                client.BaseAddress = new Uri("https://www.gdfindi.com:442/api/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                
               
                ProjectParameters parameters = new ProjectParameters();
                List<ProjectParameters> parametersList = new List<ProjectParameters>();
                Production production = new Production();
                List<Production> productions = new List<Production>();
                Goal goal = new Goal();
                List<Goal> goalInfoList = new List<Goal>();
                foreach (var item in productions)
                {
                   
                    goal.productid = productiongoal.productid;
                    goal.target = productiongoal.target;
                    goalInfoList.Add(goal);
                    parameters.productions = productions;
                    parametersList.Add(parameters);
                }

                var response = await client.PutAsJsonAsync("v1/Projects/" + Session["id"] + "/parameters", parameters);


                if (response.IsSuccessStatusCode == false)
                {
                    return View("Index");
                }
                else
                {
                    //Session["userName"] = userDetails.UserName;
                    // Session["Access_Token"] = userDetails.Access_Token;
                    // return RedirectToAction("Index", "Home");
                    return View(goalInfoList);
                }
            }
        }

        public ActionResult LogOut()
        {

            return RedirectToAction("Index", "Login");

        }

        public ActionResult projectdetail()
        {
            return View("ProjectList", "Project");

        }

    }
}