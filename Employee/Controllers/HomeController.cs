using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Employee.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Login()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult CheckLogin()
        {
            
            string msg = "";
            int rsl;
            /* Models.newlogin newlog = new Models.newlogin(); */ //Model Object Declaration
            Models.Employee newlog = new Models.Employee();
            List<Models.Data.DataValue> lstdata = new List<Models.Data.DataValue>(); //List Declaration
            try
            {
                JavaScriptSerializer jserial = new JavaScriptSerializer();
                var resolveRequest = HttpContext.Request;
                resolveRequest.InputStream.Seek(0, SeekOrigin.Begin);
                string jsonString = new StreamReader(resolveRequest.InputStream).ReadToEnd();
                JObject jsonResponse = JObject.Parse(jsonString);
                string Val = "";
                if (jsonString != "")
                {
                    foreach (JProperty content in jsonResponse.Children())
                    {
                        if (content.Name == "sdata")
                        {
                            Val = content.Value.ToString();
                            lstdata = jserial.Deserialize<List<Models.Data.DataValue>>(Val);
                        }
                    }
                }
                rsl = newlog.LoginCheck(lstdata);

              
                    if (rsl == 200)
                    {
                        msg = "Sucess";
                    }
                    else
                    {
                        msg = "Failed";
                    }
                

            }
            catch (Exception e)
            {
                msg = "Failed";
            }


            return Json(msg, JsonRequestBehavior.AllowGet);
        }

    }
}