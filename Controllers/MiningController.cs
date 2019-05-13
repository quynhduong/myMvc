using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using TRMDesktopUserI.Library.Models;
using TRMDesktopUserI.Library.Models.Details;
using TRMDesktopUserI.Library.Models.Production_Plan;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;

namespace Mvc.Controllers
{
    public class MiningController : Controller
    {
        // GET: Mining
        public ActionResult Index()
        {
            return View();
        }


        //The PUT Method


        [HttpPost]
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
        [HttpPost]
        public async Task<ActionResult> Mining([FromBody]MiningParams mining)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://www.gdfindi.com:442/api/");
            string token = (string)Session["Access_Token"];

            //Define Headers
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));




            //Get The Project Info ( Production orders and production goals)
            // 
            MiningParams miningParams = new MiningParams();
          


            HttpResponseMessage responseP = await client.GetAsync("v1/Projects/" + Session["id"].ToString() + "/parameters");
            var Productionplan = await responseP.Content.ReadAsAsync<ProductionPlan>();
            Productions D = new Productions();
            ProductionPlan prodPlan = new ProductionPlan();
            prodPlan.productions = Productionplan.productions;
            List<Productions> productionList = new List<Productions>(prodPlan.productions);

            productionList.Add(D);

            foreach (var k in productionList)
            {

                if (k.production_orders != null)
                {
                    //Get Production Orders
                    ProductionOrders orders = new ProductionOrders();
                    List<ProductionOrders> orderList = new List<ProductionOrders>(k.production_orders);
                    List<ProductionGoals> goalList = new List<ProductionGoals>(k.production_goals);
                    MiningParams tests = new MiningParams();
                    List<MiningParams> testL = new List<MiningParams>();
                    foreach (var i in orderList)
                    {
                        //Get production plan
                        PlanInfo planInfo = new PlanInfo();
                        List<PlanInfo> planInfoList = new List<PlanInfo>();

                        planInfo.lotsize = i.lotsize;
                        planInfo.product = i.productid;
                        planInfo.islot = i.aslot;
                        planInfoList.Add(planInfo);

                        tests.iniplans = planInfoList;

                        testL.Add(tests);

                    }
                    //Get Production Goal
                    foreach (var i in goalList)
                    {


                        GoalInfo goalInfo = new GoalInfo();
                        List<GoalInfo> goalInfoList = new List<GoalInfo>();

                        goalInfo.production = i.production;
                        goalInfo.product = i.productid;
                        goalInfo.rush = i.rush;
                        goalInfoList.Add(goalInfo);

                        tests.goals = goalInfoList;
                        testL.Add(tests);

                    }
                    PatternCondition patternCondition = new PatternCondition();
                    tests.patternCondition = patternCondition;
                    patternCondition.RenderingType = 0;

                    List<List<int>> list = new List<List<int>>();
                    patternCondition.patterns = list;
                    var rand = new Random();
                    foreach(var item in orderList)
                    { 
                    for (int i = 0; i < item.productid.Count(); i++)
                    {
                        //
                        // Put some integers in the inner lists.
                        //
                        List<int> sublist = new List<int>();
                      
                        for (int v = 1; v < item.productid.Count(); v++)
                        {
                            sublist.Add(v);
                        }
                        //
                        // Add the sublist to the top-level List reference.
                        //
                        list.Add(sublist);

                    }
                    }
                   


                    //Request Headers
                    MyFunction(Pairing.Of("projectid", Session["id"]), Pairing.Of("start", 0), Pairing.Of("output", 0));


                    //Prepare Request Body
                    var response = await client.PostAsJsonAsync("v1/PVDO/Mining?projectid=" + Session["id"] + "&start=0&output=0", tests);

                    var result = await client.GetAsync("v1/PVDO/3/Results");
                    var resultDetails = await response.Content.ReadAsAsync<ReactorResults>();

                    if (response.IsSuccessStatusCode == false)
                    {

                        return View("Index", mining);
                    }
                    else
                    {
                        Console.WriteLine("Successfully simulated!");
                        return RedirectToAction("Index", "Home");
                    }

                }

               

            }
            return RedirectToAction("Index", "Home");
        }
    }
}