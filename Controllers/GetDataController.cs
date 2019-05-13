using Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using TRMDesktopUserI.Library.Models;
using TRMDesktopUserI.Library.Models.Production_Plan;
using TRMDesktopUserI.Library.Models.Put;
using Customer = Mvc.Models.Customer;
using Goal = TRMDesktopUserI.Library.Models.Put.Goal;
using Order = Mvc.Models.Order;

namespace Mvc.Controllers
{
    public class GetDataController : Controller
    {
        // GET: GetData

        public ActionResult ProductionGoal()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://www.gdfindi.com:442/api/");
            string token = (string)Session["Access_Token"];

            //Define Headers
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.GetAsync("v1/Projects/" + Session["id"].ToString() + "/parameters").Result;




            if (response.IsSuccessStatusCode == false)
            {
                return View("Index");
            }
            else
            {

                //Get The Project Info ( Production orders and production goals)
                var ProductInfo = response.Content.ReadAsAsync<ProductionPlan>().Result;



                if (ProductInfo != null)
                {
                    ProductionPlan productionPlan = new ProductionPlan();
                    productionPlan.productions = ProductInfo.productions;
                    Productions D = new Productions();
                    List<Productions> productionList = new List<Productions>(productionPlan.productions);
                    productionList.Add(D);


                    foreach (var element in productionList)
                    {
                        ProductionGoals goals = new ProductionGoals();
                        List<ProductionGoals> goalList = new List<ProductionGoals>(element.production_goals);
                        foreach (var item in goalList)
                        {
                            //"product"
                            goals.productid = item.productid;



                            goals.rush = item.rush;


                            goals.target = item.target;


                            goalList.Add(goals);
                            return View(goalList);

                        }
                    }
                    return View(D);



                }


                return View("Home", "Index");
            }

        }// end of details 



        public ActionResult Index()
        {
                  
            var goalSession = Session["goals"];
           

            return View(goalSession);
        }

        [HttpGet]
        public ActionResult Create()
        {


            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection formCollection)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://www.gdfindi.com:442/api/");
            string token = (string)Session["Access_Token"];

            //Define Headers
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync("v1/Projects/" + Session["id"].ToString() + "/parameters").Result;

            var ProductInfo = response.Content.ReadAsAsync<ProductionPlan>().Result;
            Productions productionk = new Productions();
            ProductionPlan prodPlan = new ProductionPlan();
            prodPlan.productions = ProductInfo.productions;
            List<Productions> productionsL = new List<Productions>(prodPlan.productions);

           
               
                    Goal goal = new Goal();
                    List<Goal> goals = new List<Goal>();
                   
                    goal.productid = formCollection["productid"];

                     

                    goal.target = Convert.ToInt32(formCollection["target"]);
                    goal.Product = formCollection["Product"];
                    goal.Production = Convert.ToInt32(formCollection["Production"]);
                    goals.Add(goal);
                   
                    Session["goals"] = goals;
                   return RedirectToAction("Index", "GetData", Session["goals"]);


        
 

        }


    }
}
