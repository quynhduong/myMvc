using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TRMDesktopUserI.Library.Models;
using TRMDesktopUserI.Library.Models.Details;

namespace Mvc.Controllers
{
    public class PropertiesController : Controller
    {
        // GET: Properties
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
        public async Task<ActionResult> properties(Agent agentproperties)
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


            HttpResponseMessage response = await client.GetAsync("v1/Projects/" + Session["id"].ToString() + "?target[0]=1&target[1]=2&target[2]=7");



            if (response.IsSuccessStatusCode == false)
            {
                return View("Index");
            }
            else
            {

                //Details
                var detail = await response.Content.ReadAsAsync<ProjectDetail>();

                if (detail != null)
                {
                    ProjectDetail D = new ProjectDetail();
                    D.layouts = detail.layouts;
                    Layout layOut = new Layout();
                    List<Layout> LayoutList = new List<Layout>(detail.layouts);
                    foreach (var i in LayoutList)
                    {
                        layOut.agents = i.agents;
                        Agent Agent = new Agent();
                        List<Agent> AgentList = new List<Agent>(i.agents);
                        foreach (var item in AgentList)
                        {
                            Agent.properties = item.properties;
                            Property property = new Property();
                            List<Property> properties = new List<Property>(item.properties);
                            foreach(var element in properties)
                            {
                                property.name = element.name;
                                property.type = element.type;
                                property.value = element.value;
                                properties.Add(property);
                                return View(properties);
                            }
                            
                            AgentList.Add(Agent);
                            return View(AgentList);
                        }

                        LayoutList.Add(layOut);

                        return View(LayoutList);

                    }

                    //return View(prodProc);
                }


                return RedirectToAction("Details", "Details");
            }

        }// end of details 

    }
}
