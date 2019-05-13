using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TRMDesktopUserI.Library.Models;
using TRMDesktopUserI.Library.Models.Production_Plan;

namespace Mvc.Controllers
{
    public class ProductionOrdersController : Controller
    {
        // GET: ProductionOrders
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Callback2()
        {
         //   Request.InputStream.Seek(0, SeekOrigin.Begin);
           // string jsonData = new StreamReader(Request.InputStream).ReadToEnd();

            // ...
            string json;
            using (var reader = new StreamReader(Request.InputStream))
            {
                json = reader.ReadToEnd();
            }
            var haha = JsonConvert.SerializeObject(json);
            var car = JsonConvert.DeserializeObject<ProductionPlan>(haha);
            return View();
        }
        [HttpPost]
        public ActionResult Callback()
        {
            //Stream req = Request.InputStream;
            Request.InputStream.Seek(0, SeekOrigin.Begin);
            string json = new StreamReader(Request.InputStream).ReadToEnd();

            // ...
            MiningParams plan = null;
            try
            {
                // assuming JSON.net/Newtonsoft library from http://json.codeplex.com/
                plan = JsonConvert.DeserializeObject<MiningParams>(json);
            }
            catch (Exception ex)
            {
                // Try and handle malformed POST body
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return null;
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
        public async Task<ActionResult> ProductionOrders(Productions Info)
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

            HttpResponseMessage response = await client.GetAsync("v1/Projects/" + Session["id"].ToString() + "/parameters");




            if (response.IsSuccessStatusCode == false)
            {
                return View("Index");
            }
            else
            {
                

                //Get The Project Info ( Production orders and production goals)
                var ProductInfo = await response.Content.ReadAsAsync<ProductionPlan>();
               

                Session["MiningParams"] = ProductInfo;
                

                

                if (ProductInfo != null)
                {
                    ProductionPlan productionPlan = new ProductionPlan();
                    productionPlan.productions = ProductInfo.productions;
                    Productions D = new Productions();
                    List<Productions> productionList = new List<Productions>(productionPlan.productions);
                    productionList.Add(D);


                    foreach (var element in productionList)
                    {
                        ProductionOrders orders = new ProductionOrders();
                        List<ProductionOrders> orderList = new List<ProductionOrders>(element.production_orders);
                        Session["iniplans"] = element.production_orders;
                        
                        



                        foreach (var item in orderList)
                        {
                           List<MiningParams> mL = new List<MiningParams>();
                            MiningParams mP = new MiningParams();
                            /*if (mP == null)
                            { 
                            foreach(var h in mL)
                            {
                                h.iniplans = mP.iniplans;
                                List<PlanInfo> k = new List<PlanInfo>();
                                PlanInfo a = new PlanInfo();
                                foreach(var it in k)
                                {
                                    it.productid = item.productid;
                                   // Session["orders"] = it.productid;

                                    it.lotsize = item.lotsize;
                                    //Session["lotsize"] = it.lotsize;

                                    it.islot = item.aslot;

                                    Callback();
                                }
                                
                            }
                            }*/
                            orders.productid = item.productid;
                            Session["orders"] = item.productid;
                           


                            orders.lotsize = item.lotsize;
                            Session["lotsize"] = item.lotsize;

                            orders.aslot = item.aslot;
                            Session["aslot"] = item.aslot;

                            orderList.Add(orders);
                            return View(orderList);
                        }
                    }
                    return View(D);



                }


                return RedirectToAction("Details", "Details");
            }

        }// end of details 

    }
}