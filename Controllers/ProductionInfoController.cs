using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TRMDesktopUserI.Library.Models.Production_Plan;

namespace Mvc.Controllers
{
    public class ProductionInfoController : Controller
    {
        // GET: ProductionInfo
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
        public async Task<ActionResult> ProductionInfo(ProductionGoals Info)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://www.gdfindi.com:442/api/");
            string token = (string)Session["Access_Token"];

            //Define Headers
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
            HttpResponseMessage response = await client.GetAsync("v1/Projects/" + Session["id"].ToString() + "/parameters");
            



            if (response.IsSuccessStatusCode == false)
            {
                return View("Index");
            }
            else
            {

                //Get The Project Info ( Production orders and production goals)
                var ProductInfo = await response.Content.ReadAsAsync<ProductionPlan>();
              


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
                        Session["goals"] = element.production_goals;
                        foreach (var item in goalList)
                        {
                            //"product"
                            goals.productid = item.productid;
                            Session["product"] = goals.productid;
                            goals.productid = Info.productid;
                            // var test = JsonConvert.SerializeObject(Session["product"]);
                           // var test2 = JsonConvert.DeserializeObject(test);

                            goals.rush = item.rush;
                            Session["rush"] = goals.rush;


                            goals.target = item.target;
                             goals.target = Info.target;


                            goalList.Add(goals);
                            Session["goalList"] = goalList;
                            return View(goalList);
                            
                        }
                    }
                    return View(D);
                    


                }


                return RedirectToAction("Details", "Details");
            }

        }// end of details 

    }
}