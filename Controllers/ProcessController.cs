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
using static TRMDesktopUserI.Library.Models.Details.ProductionProcess;

namespace Mvc.Controllers
{
    public class ProcessController : Controller
    {
        // GET: Process
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
        public async Task<ActionResult> process(Process processes)
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
            //HttpResponseMessage response = await client.GetAsync("v1/Components/Processes/20740");


            if (response.IsSuccessStatusCode == false)
            {
                return View("Index");
            }
            else
            {

                 //Details
                var productionProcess = await response.Content.ReadAsAsync<ProjectDetail>();

                if (productionProcess != null)
                {
                    ProjectDetail D = new ProjectDetail();
                     D.Material = productionProcess.Material;
                     D.productionProcess = productionProcess.productionProcess;
                     ProductionProcess productionprocess = new ProductionProcess();
                     List<ProductionProcess> productionProcesses = new List<ProductionProcess>(productionProcess.productionProcess);
                   foreach(var i in productionProcesses)
                   {
                       productionprocess.processes = i.processes;
                        Process process = new Process();
                        List<Process> processList = new List<Process>(i.processes);
                        foreach(var k in processList)
                        {
                            process.id = k.id;
                            process.name = k.name;
                            process.desc = k.desc;
                            process.x = k.x;
                            process.y = k.y;
                            process.worktime = k.worktime;
                            process.setup = k.setup;
                            process.frequency = k.frequency;
                            process.inputProcess = k.inputProcess;
                            process.outputProcess = k.outputProcess;
                            process.input = k.input;
                            process.disabled = k.disabled;
                            processList.Add(process);
                            return View(processList);
                        }

                       productionProcesses.Add(productionprocess);

                       return View(productionProcesses);

                   }

                   
                }


                return RedirectToAction("Details", "Details");
            }

        }// end of details 

    }
}