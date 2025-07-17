using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Employee.Models
{
    public class Employee
    {
        Models.SqlHelper _helper = new Models.SqlHelper();

        public int LoginCheck(List<Models.Data.DataValue> lstload)
        {

            string msg = "";
            int result = 1;
            try
            {






                SqlParameter[] par = {

                          new SqlParameter("@UserId",string.IsNullOrEmpty(getstringvalue("UserName", lstload))?DBNull.Value:(object)getstringvalue("UserName", lstload)),
                          new SqlParameter("@Password",string.IsNullOrEmpty(getstringvalue("Pswd", lstload))?DBNull.Value:(object)getstringvalue("Pswd", lstload)),
                         



                    };
                result = _helper.ExecuteSPNonQuery("SP_tblLogin_Checklogin", par);
            }

            catch (Exception e)
            {
                msg = "failed";
                result = -123;
            }
            return result;
        }

       

        public List<Models.Data.TblEmployee> GetEmployeeList(List<KeyValuePair<string, string>> whereCondition)
        {
            DataSet ds = new DataSet();

            //if any issue check here reason: models is not used in previous code
            List<Models.Data.TblEmployee> clientModel = new List<Data.TblEmployee>();
            try
            {
                ds = _helper.ExecuteDatasetSP("SP_tblEmployeeSelect", "", whereCondition);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    var obj = new Data.TblEmployee
                    {
                        Eid = dr["Eid"].ToString(),
                        Name = dr["Name"].ToString(),
                        Age = dr["Age"].ToString(),
                        JoinDate = dr["JoinDate"].ToString(),

                        DOB = dr["DOB"].ToString(),
                        Salary = dr["Salary"].ToString(),
                        DepId = dr["Department"].ToString(),

                        DesigId = dr["Designation"].ToString(),
                        Sex = dr["Sex"].ToString(),
                        PhNo = dr["PhNo"].ToString(),
                        NoOfLeave = dr["NoOfLeave"].ToString()




                    };
                    clientModel.Add(obj);
                }
            }
            catch (Exception e)
            {

            }
            return clientModel;
        }


        public List<Models.Data.TblLeaveApp> GetLeaveList(List<KeyValuePair<string, string>> whereCondition)
        {
            DataSet ds = new DataSet();

            //if any issue check here reason: models is not used in previous code
            List<Models.Data.TblLeaveApp> clientModel = new List<Data.TblLeaveApp>();
            try
            {
                ds = _helper.ExecuteDatasetSP("SP_tblLeaveAppSelect", "", whereCondition);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    var obj = new Data.TblLeaveApp
                    {
                        LeaveId = dr["LeaveId"].ToString(),
                        LeaveType = dr["LeaveType"].ToString(),
                        FromDate = dr["FromDate"].ToString(),
                        ToDate = dr["ToDate"].ToString(),
                        EmployeeName = dr["EmployeeName"].ToString(),
                        
                        Days = dr["Days"].ToString(),
                        Reason = dr["Reason"].ToString()
                       





                    };
                    clientModel.Add(obj);
                }
            }
            catch (Exception e)
            {

            }
            return clientModel;
        }

        public int SaveEmployee(List<Models.Data.DataValue> lstload)
        {

            string msg = "";
            int result = 1;
            try
            {

                SqlParameter[] par = {

                          new SqlParameter("@Eid",string.IsNullOrEmpty(getstringvalue("Id", lstload))?DBNull.Value:(object)getstringvalue("Id", lstload)),
                          new SqlParameter("@Name",string.IsNullOrEmpty(getstringvalue("Name", lstload))?DBNull.Value:(object)getstringvalue("Name", lstload)),
                          new SqlParameter("@Age",string.IsNullOrEmpty(getstringvalue("Age", lstload))?DBNull.Value:(object)getstringvalue("Age", lstload)),
                          new SqlParameter("@JoinDate",string.IsNullOrEmpty(getstringvalue("JoinDate", lstload))?DBNull.Value:(object)getstringvalue("JoinDate", lstload)),
                          new SqlParameter("@DOB",string.IsNullOrEmpty(getstringvalue("dob", lstload))?DBNull.Value:(object)getstringvalue("dob", lstload)),
                          new SqlParameter("@Salary",string.IsNullOrEmpty(getstringvalue("Salary", lstload))?DBNull.Value:(object)getstringvalue("Salary", lstload)),

                          new SqlParameter("@DepId",string.IsNullOrEmpty(getstringvalue("Department", lstload))?DBNull.Value:(object)getstringvalue("Department", lstload)),
                          new SqlParameter("@DesigId",string.IsNullOrEmpty(getstringvalue("Designation", lstload))?DBNull.Value:(object)getstringvalue("Designation", lstload)),

                          new SqlParameter("@Sex",string.IsNullOrEmpty(getstringvalue("Sex", lstload))?DBNull.Value:(object)getstringvalue("Sex", lstload)),
                          new SqlParameter("@PhNo",string.IsNullOrEmpty(getstringvalue("phno", lstload))?DBNull.Value:(object)getstringvalue("phno", lstload)),






                    };
                result = _helper.ExecuteSPNonQuery("SP_tblEmployeeInsert", par);
            }

            catch (Exception e)
            {
                msg = "failed";
                result = -123;
            }
            return result;
        }

        public int SaveLeave(List<Models.Data.DataValue> lstload)
        {

            string msg = "";
            int result = 1;
            try
            {

                SqlParameter[] par = {

                          new SqlParameter("@Eid",string.IsNullOrEmpty(getstringvalue("Id", lstload))?DBNull.Value:(object)getstringvalue("Id", lstload)),
                          new SqlParameter("@LeaveType",string.IsNullOrEmpty(getstringvalue("type", lstload))?DBNull.Value:(object)getstringvalue("type", lstload)),
                          new SqlParameter("@FromDate",string.IsNullOrEmpty(getstringvalue("FromDate", lstload))?DBNull.Value:(object)getstringvalue("FromDate", lstload)),
                          new SqlParameter("@ToDate",string.IsNullOrEmpty(getstringvalue("ToDate", lstload))?DBNull.Value:(object)getstringvalue("ToDate", lstload)),
                          new SqlParameter("@Days",string.IsNullOrEmpty(getstringvalue("Days", lstload))?DBNull.Value:(object)getstringvalue("Days", lstload)),
                          new SqlParameter("@Reason",string.IsNullOrEmpty(getstringvalue("Reason", lstload))?DBNull.Value:(object)getstringvalue("Reason", lstload)),

                    };
                result = _helper.ExecuteSPNonQuery("SP_tblLeaveAppInsert", par);
            }

            catch (Exception e)
            {
                msg = "failed";
                result = -123;
            }
            return result;
        }




        public int SaveDepDes(List<Models.Data.DataValue> lstload)
        {

            string msg = "";
            int result = 1;
            try
            {


                SqlParameter[] par = {

                          new SqlParameter("@MasterID",string.IsNullOrEmpty(getstringvalue("MId", lstload))?DBNull.Value:(object)getstringvalue("MId", lstload)),
                          new SqlParameter("@MasterName",string.IsNullOrEmpty(getstringvalue("Name", lstload))?DBNull.Value:(object)getstringvalue("Name", lstload)),
                          new SqlParameter("@MasterType",string.IsNullOrEmpty(getstringvalue("type", lstload))?DBNull.Value:(object)getstringvalue("type", lstload)),
                         

                    };
                result = _helper.ExecuteSPNonQuery("SP_tblMasterInserT", par);
            }

            catch (Exception e)
            {
                msg = "failed";
                result = -123;
            }
            return result;
        }






        public List<Models.Data.TblMaster> GetMasterList(List<KeyValuePair<string, string>> whereCondition)
        {
            DataSet ds = new DataSet();

            //if any issue check here reason: models is not used in previous code
            List<Models.Data.TblMaster> clientModel = new List<Data.TblMaster>();
            try
            {
                ds = _helper.ExecuteDatasetSP("sp_tblMasterSel", "", whereCondition);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    var obj = new Data.TblMaster

                    {

                        MasterID = dr["MasterID"].ToString(),
                        MasterName = dr["MasterName"].ToString(),
                        MasterType = dr["MasterType"].ToString(),
                        ParentID = dr["ParentID"].ToString()
                    };
                    clientModel.Add(obj);
                }
            }
            catch (Exception e)
            {

            }
            return clientModel;
        }



        string getstringvalue(string id, List<Data.DataValue> lst)//sqlparameter passing
        {
            string s = "";
            s = (from o in lst where o.name == id select o.value).FirstOrDefault();
            if (s == null) s = "";
            return s;
        }

      
    }
}


