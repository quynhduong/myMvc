using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using TRMDesktopUserI.Library.Models.Details;
using TRMDesktopUserI.Library.Models.Production_Plan;
using TRMDesktopUserI.Library.Models.Put;

namespace Mvc.Controllers
{
    public class ProjectParametersController : Controller
    {
      
        public ActionResult Index()
        {
            return View();
        }

        
        public ActionResult Save()
        {

            return RedirectToAction("Index", "ProjectParameters");

        }
        //PUT(Edit) the productions parameters
        
        public async Task<ActionResult> projectParameters(ProductionGoals prod )
        {

            Session["a"] = prod.productid;
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
                    var a = prod.productid;
                    var b = prod.target;
                    //Get Production Orders
                    ProductionOrders orders = new ProductionOrders();
                    List<ProductionOrders> orderList = new List<ProductionOrders>(k.production_orders);
                    List<ProductionGoals> goalList = new List<ProductionGoals>(k.production_goals);
                    ProjectParameters parameters = new ProjectParameters();
                    List<ProjectParameters> parametersList = new List<ProjectParameters>();
                    Production production = new Production();
                    List<Production> productions = new List<Production>();
                    foreach (var i in orderList)
                    {
                        //Get production plan
                        Order order = new Order();
                        List<Order> orderInfoList = new List<Order>();
                        order.productid = i.productid;
                        //order.Aslot = i.aslot;
                        order.lotsize = i.lotsize;
                        orderInfoList.Add(order);

                        production.production_orders = orderInfoList;
                        productions.Add(production);
                        parameters.productions = productions;
                        parametersList.Add(parameters);

                    }
                    //Get Production Goal
                    foreach (var i in goalList)
                    {


                        Goal goalInfo = new Goal();
                        List<Goal> goalInfoList = new List<Goal>();

                        goalInfo.target = i.target;
                        goalInfo.productid = i.productid;
                        //goalInfo.Production = i.production;
                        goalInfoList.Add(goalInfo);

                        production.production_goals = goalInfoList;
                        productions.Add(production);
                        parameters.productions = productions;
                        parametersList.Add(parameters);
                        

                    }

                    RedirectToAction("Index", "ProjectParameters");

                    //await response
                    var response = await client.PutAsJsonAsync("v1/Projects/"+Session["id"]+"/parameters", parameters);

                    
                    

                    if (response.IsSuccessStatusCode == false)
                    {

                        return View("Index");
                    }
                    else
                    {
                        return View(goalList);
                    }

                }



            }
            return RedirectToAction("Index", "Home");
        }
    }
}