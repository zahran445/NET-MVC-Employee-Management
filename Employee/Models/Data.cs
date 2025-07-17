using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Employee.Models
{
    public class Data
    {
        public class TblEmployee
        {
            public string Eid { get; set; }
            public string Name { get; set; }
            public string Age { get; set; }
            public string JoinDate { get; set; }
            public string DOB { get; set; }

            public string Salary { get; set; }
            public string DepId { get; set; }

            public string DesigId { get; set; }
            public string Sex { get; set; }
            public string PhNo { get; set; }

            public string NoOfLeave { get; set; }

        }

        public class TblLeaveApp
        {
            public string LeaveId  { get; set; }
            public string Eid  { get; set; }
            public string EmployeeName { get; set; }
            public string LeaveType  { get; set; }
            public string FromDate  { get; set; }
            public string ToDate  { get; set; }

            public string Days  { get; set; }
            public string Reason  { get; set; }

            public string CreateID  { get; set; }
            public string CreateDate  { get; set; }
            public string ModId  { get; set; }

            public string ModDate  { get; set; }

        }

        public class TblMaster
        {
            public string MasterID { get; set; }
            public string MasterName { get; set; }

            public string MasterType { get; set; }
            public string ParentID { get; set; }

        }

        public class TblLogin
        {
            public string UserId { get; set; }
            public string Password { get; set; }

        }

        public class tableColumns
        {
            public string colName { get; set; }
            public string colValue { get; set; }
            public string colType { get; set; }
        }

        public class DataValue
        {



            public string name { get; set; }
            public string value { get; set; }
            public DataValue()
            {
                name = "";
                value = "";
            }
        }



        #region SetReportSettings
        static public ReportDocument SetReportSettings(ReportDocument reportObj = null)
        {
            try
            {
                if (reportObj == null)
                {
                    throw new Exception();
                }
            }
            catch (Exception e) { }
            CrystalDecisions.Shared.ConnectionInfo crcconnectionInfo = new CrystalDecisions.Shared.ConnectionInfo();
            TableLogOnInfos crtablelogoninfos = new TableLogOnInfos();
            TableLogOnInfo crtalbelogoninfo = new TableLogOnInfo();
            Tables CrTables;
            List<string> lst = new List<string>();
            Models.SqlHelper _helper = new Models.SqlHelper();
            string conString = _helper.connectionString();
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(conString);
            lst.Add(builder.UserID);
            lst.Add(builder.Password);
            lst.Add(builder.DataSource);
            lst.Add(builder.InitialCatalog);

            crcconnectionInfo.ServerName = lst[2];
            crcconnectionInfo.DatabaseName = lst[3];
            crcconnectionInfo.UserID = lst[0];
            crcconnectionInfo.Password = lst[1];
            try { CrTables = reportObj.Database.Tables; }
            catch (Exception e)
            {
                return reportObj;
            }

            foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
            {
                crtalbelogoninfo = CrTable.LogOnInfo;
                crtalbelogoninfo.ConnectionInfo = crcconnectionInfo;
                CrTable.ApplyLogOnInfo(crtalbelogoninfo);
                CrTable.Location = crcconnectionInfo.DatabaseName + ".dbo." + CrTable.Name;
            }
            return reportObj;
        }
       #endregion
    }
}
