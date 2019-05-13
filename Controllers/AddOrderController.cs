using Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Numerics;
using System.Web;
using System.Web.Mvc;
using TRMDesktopUserI.Library.Models.Put;
using Order = Mvc.Models.Order;

namespace Mvc.Controllers
{
    public class AddOrderController : Controller
    {
        // GET: AddOrder

        AddNewOrderEntities db = new AddNewOrderEntities();

        public ActionResult Index()
        {
            List<Customer> OrderAndCustomerList = db.Customers.ToList();
            return View(OrderAndCustomerList);
        }


        //Save Order
        public ActionResult SaveOrder(string name, string address, Order[] order)
        {
            string result = "Error Order Is Not Complete";
            if (name != null || address != null || order != null)
            {
                Guid g = Guid.NewGuid();

                // Convert GUID to BigInteger
                // ex: 134252730720501571475137903438348973637
                int customerId = new int();

                //var customerId = Guid.NewGuid();

                Customer model = new Customer();

                model.CustomerId = customerId;
                model.Name = name;
                model.OrderDate = DateTime.Now;
                db.Customers.Add(model);
                List<Production> productions = new List<Production>();
                List<ProjectParameters> parameters = new List<ProjectParameters>();
                ProjectParameters parameter = new ProjectParameters();
                List<Goal> goals = new List<Goal>();

                Production production = new Production();

                foreach (var item in order)
                {

                    Goal goal = new Goal();

                    goal.productid = item.ProductName;
                    goal.target = item.Quantity;
                    goals.Add(goal);

                }

                production.production_goals = goals;

                productions.Add(production);


                parameter.productions = productions;

                parameters.Add(parameter);


                Session["goal"] = parameter;


                //Post the production goal list to GD.findi
                //Add Headers
                var client = new HttpClient();
                client.BaseAddress = new Uri("https://www.gdfindi.com:442/api/");
                string token = (string)Session["Access_Token"];

                //Define Headers
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var putparameters = client.PutAsJsonAsync("v1/Projects/" + Session["id"] + "/parameters", Session["goal"]).Result;
                if (putparameters.IsSuccessStatusCode != false)
                {
                    result = "Successfully added the order!";
                    foreach (var item in order)
                    {
                      //  var orderId = Guid.NewGuid();
                        int orderId = new int();
                        Order O = new Order();
                        O.OrderId = orderId;
                        O.ProductName = item.ProductName;
                        O.Quantity = item.Quantity;
                        O.Price = item.Price;
                        O.Amount = item.Amount;
                        O.CustomerId = item.CustomerId;
                        db.Orders.Add(O);

                    }

                }
                db.SaveChanges();
                result = "Successfully added the order!";

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}