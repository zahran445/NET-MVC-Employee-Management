using CrystalDecisions.Shared;
using CrystalDecisions.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Item.ReportView
{
    public partial class ReportViewer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CrystalDecisions.CrystalReports.Engine.ReportDocument report = (CrystalDecisions.CrystalReports.Engine.ReportDocument)Session["reportObj"];
            CrystalReportViewer.ReportSource = report;
            try
            {
                string imgname = Regex.Replace(Session["ReportName"].ToString(), "[^\\w\\._]", "");

                //string strFilePath = "c:\\temp\\" + imgname + ".pdf";
                // report.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, strFilePath);
                CrystalReportViewer.ReportSource = report;
                report.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "ExportedReport");


            }
            catch (Exception ex)
            {
                // Models.General.ErrorLogging(ex);
                //ProductApplication.Models.General.WriteDataLog("3" + ex.Message);
            }
            finally
            {
                Session["reportObj"].ToString();
                Session.Remove("reportObj");
                report.Close();
                report.Dispose();
            }
        }
    }
}