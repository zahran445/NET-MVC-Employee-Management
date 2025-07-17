using CrystalDecisions.CrystalReports.Engine;
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
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EmployeeList()
        {
            Models.Employee client = new Models.Employee();

            System.Web.HttpContext.Current.Session["reportObj"] = "hi";
            List<Models.Data.TblEmployee> lstdata = new List<Models.Data.TblEmployee>();
            List<KeyValuePair<string, string>> whereCondition = new List<KeyValuePair<string, string>>();
            lstdata = client.GetEmployeeList(whereCondition);

            return View(lstdata);
        }

        public ActionResult AddEmployee(string id = null)
        {
            Models.Data.TblEmployee pTable = new Models.Data.TblEmployee();
            Models.Employee client = new Models.Employee();
            List<Models.Data.TblEmployee> lstdata = new List<Models.Data.TblEmployee>();
            List<KeyValuePair<string, string>> whereCondition = new List<KeyValuePair<string, string>>();

            Models.Employee Employee = new Models.Employee();
            List<KeyValuePair<string, string>> whereCondition1 = new List<KeyValuePair<string, string>>();
            whereCondition1 = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("@MasterType", "1")
            };
            List<Models.Data.TblMaster> deptlstdata = Employee.GetMasterList(whereCondition1);


            whereCondition1 = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("@MasterType", "2")
            };
            List<Models.Data.TblMaster> desiglstdata = Employee.GetMasterList(whereCondition1);
            ViewBag.DepartmentList = deptlstdata;
            ViewBag.DesignationList = desiglstdata;


            if (id != null)
            {
                whereCondition = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("@Eid", id)

            };
                lstdata = client.GetEmployeeList(whereCondition);
            }

            if (lstdata.Count > 0)
            {
                pTable = lstdata[0];

                return View(pTable);
            }

            else
            {
                return View();


            }
        }

        public ActionResult LeaveList()
        {
           

            Models.Employee client = new Models.Employee();
            List<Models.Data.TblLeaveApp> lstdata = new List<Models.Data.TblLeaveApp>();
            List<KeyValuePair<string, string>> whereCondition = new List<KeyValuePair<string, string>>();
            lstdata = client.GetLeaveList(whereCondition);
            return View(lstdata);
        }


        public ActionResult addLeave()
        {
            Models.Employee Employee = new Models.Employee();
            List<KeyValuePair<string, string>> whereCondition1 = new List<KeyValuePair<string, string>>();
            whereCondition1 = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("@MasterType", "3")
            };
            List<Models.Data.TblMaster> typelstdata = Employee.GetMasterList(whereCondition1);
            ViewBag.typeList = typelstdata;


            Models.Employee client = new Models.Employee();
            List<Models.Data.TblEmployee> lstdata = new List<Models.Data.TblEmployee>();
            List<KeyValuePair<string, string>> whereCondition = new List<KeyValuePair<string, string>>();
            lstdata = client.GetEmployeeList(whereCondition);

            Models.Employee obj1 = new Models.Employee();

            return View(lstdata);
        }


            public ActionResult AddDepartment(string id = null)
        {
            Models.Data.TblMaster pTable = new Models.Data.TblMaster();
            Models.Employee client = new Models.Employee();
            List<Models.Data.TblMaster> lstdata = new List<Models.Data.TblMaster>();
            List<KeyValuePair<string, string>> whereCondition = new List<KeyValuePair<string, string>>();


            if (id != null)
            {
                whereCondition = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("@MasterType","1"),
                new KeyValuePair<string, string>("@MasterID",id)

            };
                lstdata = client.GetMasterList(whereCondition);
            }

            if (lstdata.Count > 0)
            {
                pTable = lstdata[0];

                return View(pTable);
            }

            else
            {
                return View();


            }
        }

        public ActionResult AddDesignation(string id = null)
        {
            Models.Data.TblMaster pTable = new Models.Data.TblMaster();
            Models.Employee client = new Models.Employee();
            List<Models.Data.TblMaster> lstdata = new List<Models.Data.TblMaster>();
            List<KeyValuePair<string, string>> whereCondition = new List<KeyValuePair<string, string>>();


            if (id != null)
            {
                whereCondition = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("@MasterType","2"),
                new KeyValuePair<string, string>("@MasterID",id)

            };
                lstdata = client.GetMasterList(whereCondition);
            }

            if (lstdata.Count > 0)
            {
                pTable = lstdata[0];

                return View(pTable);
            }

            else
            {
                return View();


            }
        }




        public ActionResult DepList()
        {
            Models.Employee Employee = new Models.Employee();
            List<KeyValuePair<string, string>> whereCondition1 = new List<KeyValuePair<string, string>>();
            whereCondition1 = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("@MasterType", "1")
            };
            List<Models.Data.TblMaster> deptlstdata = Employee.GetMasterList(whereCondition1);
            ViewBag.DepartmentList = deptlstdata;
            return View();
        }

        public ActionResult DesigList()
        {


            Models.Employee Employee = new Models.Employee();
            List<KeyValuePair<string, string>> whereCondition1 = new List<KeyValuePair<string, string>>();
            whereCondition1 = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("@MasterType", "2")
            };

            List<Models.Data.TblMaster> desiglstdata = Employee.GetMasterList(whereCondition1);
            ViewBag.DesignationList = desiglstdata;

            return View();



        }




        public ActionResult SaveEmployee()
        {

            string msg = "";
            int rsl;

            Models.Employee obj1 = new Models.Employee();
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
                int rslt = obj1.SaveEmployee(lstdata);
                msg = "Sucess";
            }

            catch (Exception e)
            {
                msg = "Failed";
            }
            return Json(msg);
        }


        public ActionResult SaveDepartment()
        {
            string msg = "";
            int rsl;

            Models.Employee obj1 = new Models.Employee();
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
                int rslt = obj1.SaveDepDes(lstdata);
                msg = "Sucess";
            }

            catch (Exception e)
            {
                msg = "Failed";
            }
            return Json(msg);

        }



        public ActionResult SaveDesignation()
        {
            string msg = "";
            int rsl;

            Models.Employee obj1 = new Models.Employee();
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
                int rslt = obj1.SaveDepDes(lstdata);
                msg = "Sucess";
            }

            catch (Exception e)
            {
                msg = "Failed";
            }
            return Json(msg);

        }
        public ActionResult PrintReport(string type)
        {
            var Id1 = 1;
            string rept = "";
            string ReptName = "";
            System.Web.HttpContext.Current.Session["reportObj"] = "hi";


            string Id = form("hdid");


            if (type == "1")
            {
                rept = "RptEmployeeDep.rpt";
                ReptName = "RptEmployeeDep";
            }
            else if (type == "2")
            {
                rept = "RptEmployeeDesig.rpt";
                ReptName = "RptEmployeeDesig";

            }
            else if (type == "3")
            {
                rept = "RptLeaveReport.rpt";
                ReptName = "RptLeaveReport";

            }
            else
            {
                rept = "RptEmployee.rpt";
                ReptName = "RptEmployeeDesig";

            }





            //var id1 = Int16.Parse(Id);
            try
            {

                ReportDocument report = new ReportDocument();
                report.Load(Path.Combine(Server.MapPath("~/Reports/" + rept)));
                ReportDocument aa1 = new ReportDocument();
                //aa1.OpenSubreport("RecieptSubReport.rpt");
                report.SetParameterValue(0, null);

                //report.SetParameterValue(2, 0);


                report = Models.Data.SetReportSettings(report);
                System.Web.HttpContext.Current.Session["reportObj"] = report;
                System.Web.HttpContext.Current.Session["reportName"] = ReptName + "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff");
                //System.Web.HttpContext.Current.Session["reportObj1"] = aa1;
                //aa1 = Models.Data.SetReportSettings(aa1);

                Response.Redirect(@"~\ReportView\ReportViewer.aspx");
            }
            catch (Exception e) { }
            return View();
        }

        public ActionResult SaveLeave()
        {

            string msg = "";
            int rsl;

            Models.Employee obj1 = new Models.Employee();
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
                int rslt = obj1.SaveLeave(lstdata);
                msg = "Sucess";
            }

            catch (Exception e)
            {
                msg = "Failed";
            }
            return Json(msg);
        }


        public string form(String optname)
        {
            String sOut = "";
            if (string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString[optname]))
            {
                if (string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Form[optname]))
                {
                    sOut = "";
                }
                else
                {
                    sOut = System.Web.HttpContext.Current.Request.Form[optname].ToString();
                }
            }
            else
            {
                sOut = System.Web.HttpContext.Current.Request.QueryString[optname].ToString();
            }
            return sOut;
        }


        public ActionResult ReportView()
        {



            return View();



        }

    }

}


