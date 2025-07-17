using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

namespace Employee.Models
{
    public class SqlHelper
    {
        SqlConnection con;
        public enum SearchConditions
        {
            CONTAINS = 1,
            EXACT = 2,
            GREATER_THAN = 3,
            GREATER_THAN_EQUAL_TO = 4,
            LESSER_THAN = 5,
            LESSER_THAN_EQUAL_TO = 6,
            EXACT_NUMBER = 7,
            STARTS_WITH = 8,
            NOT_EQUAL_TO = 9,
            DATE_GREATER_THAN_EQUAL_TO = 10,
            DATE_LESSER_THAN_EQUAL_TO = 11,
            DATE_EQUAL_TO = 12,
            DATE_GREATER_THAN = 13,
            DATE_LESSER_THAN = 14,
            DATE_BETWEEN = 15,
            IS_NULL = 16
        }
        public class sql
        {
            public string sel { get; set; }
            public string join { get; set; }
            public string frm { get; set; }
            public string ord { get; set; }
            public string where { get; set; }
            public string limit { get; set; }

            public sql()
            {
                sel = "";
                join = "";
                frm = "";
                ord = "";
                where = "";
                limit = "";
            }
        }
        public String connectionString()
        {
            String conString = System.Configuration.ConfigurationManager.ConnectionStrings["FS"].ConnectionString;
            return conString;
        }
        public String selectStatement(SqlHelper.sql _sql)
        {

            StringBuilder s = new StringBuilder();
            s.Append("");
            if (_sql.sel.ToString().Trim() != "")
            {
                s.Append("select ");
                if (_sql.limit.ToString().Trim() != "") s.Append(" TOP " + _sql.limit + " ");
                s.Append(_sql.sel);
                s.Append(" from " + _sql.frm);
                if (_sql.join.ToString().Trim() != "") s.Append(_sql.join);
                s.Append(" where 1=1 ");

                if (_sql.where.ToString().Trim() != "") s.Append(" and " + _sql.where);
                if (_sql.ord.ToString().Trim() != "") s.Append(" order by " + _sql.ord);
            }
            return s.ToString();
        }

        public string getParameter(string paramList, string paramName)
        {
            StringBuilder s = new StringBuilder();
            string sOut = "";
            if (paramList != "")
            {
                bool bFlag = false;
                List<Data.tableColumns> tabCols = new List<Data.tableColumns>();
                string sNodeName = "";
                string sNodeType = "";
                using (XmlReader reader = XmlReader.Create(new StringReader(paramList)))
                {
                    while (reader.Read())
                    {
                        if (bFlag)
                        {
                            switch (reader.NodeType)
                            {
                                case XmlNodeType.Element:

                                    sNodeName = reader.Name.ToString();
                                    //sNodeType = reader.GetAttribute("TYPE").ToString();
                                    break;
                                case XmlNodeType.Text:
                                case XmlNodeType.CDATA:

                                    tabCols.Add(new Data.tableColumns { colName = sNodeName, colValue = reader.Value.ToString(), colType = sNodeType });
                                    sNodeName = "";
                                    sNodeType = "";
                                    break;
                            }
                        }
                        bFlag = true;
                    }
                    reader.Close();
                }
                int iCols = tabCols.Count;

                for (int i = 0; i <= tabCols.Count - 1; i++)
                {
                    var s1 = tabCols[i] as Data.tableColumns;
                    if (s1.colName.ToUpper() == paramName.ToUpper()) sOut = s1.colValue;
                }
            }
            return sOut;
        }
        public string whereClause(string sWhere)
        {
            StringBuilder s = new StringBuilder();
            StringBuilder sWhereClause = new StringBuilder();
            if (sWhere != "")
            {
                bool bFlag = false;
                List<Data.tableColumns> tabCols = new List<Data.tableColumns>();
                string sNodeName = "";
                string sNodeType = "";
                using (XmlReader reader = XmlReader.Create(new StringReader(sWhere)))
                {
                    while (reader.Read())
                    {
                        if (bFlag)
                        {
                            switch (reader.NodeType)
                            {
                                case XmlNodeType.Element:

                                    sNodeName = reader.Name.ToString();
                                    sNodeType = reader.GetAttribute("TYPE").ToString();
                                    break;
                                case XmlNodeType.Text:
                                case XmlNodeType.CDATA:

                                    tabCols.Add(new Data.tableColumns { colName = sNodeName, colValue = reader.Value.ToString(), colType = sNodeType });
                                    sNodeName = "";
                                    sNodeType = "";
                                    break;
                            }
                        }
                        bFlag = true;
                    }
                    reader.Close();
                }
                int iCols = tabCols.Count;

                for (int i = 0; i <= tabCols.Count - 1; i++)
                {
                    var s1 = tabCols[i] as Data.tableColumns;
                    if (s1.colValue != "")
                    {
                        if (i > 0) sWhereClause.Append(" and ");
                        SearchConditions c = (SearchConditions)Enum.Parse(typeof(SearchConditions), s1.colType.ToUpper());
                        if (c == SearchConditions.CONTAINS) sWhereClause.Append(s1.colName + " like '%" + s1.colValue + "%'");
                        if (c == SearchConditions.EXACT) sWhereClause.Append(s1.colName + " = '" + s1.colValue + "'");
                        if (c == SearchConditions.EXACT_NUMBER) sWhereClause.Append(s1.colName + " = " + s1.colValue);
                        if (c == SearchConditions.GREATER_THAN) sWhereClause.Append(s1.colName + " > " + s1.colValue);
                        if (c == SearchConditions.GREATER_THAN_EQUAL_TO) sWhereClause.Append(s1.colName + " >= " + s1.colValue);
                        if (c == SearchConditions.LESSER_THAN) sWhereClause.Append(s1.colName + " < " + s1.colValue);
                        if (c == SearchConditions.LESSER_THAN_EQUAL_TO) sWhereClause.Append(s1.colName + " <= " + s1.colValue);
                        if (c == SearchConditions.STARTS_WITH) sWhereClause.Append(s1.colName + " like '" + s1.colValue + "%'");
                        if (c == SearchConditions.NOT_EQUAL_TO) sWhereClause.Append(s1.colName + " != '" + s1.colValue + "'");
                        if (c == SearchConditions.DATE_GREATER_THAN_EQUAL_TO) sWhereClause.Append(s1.colName + " >= convert(datetime,'" + s1.colValue + "',103)");
                        if (c == SearchConditions.DATE_LESSER_THAN_EQUAL_TO) sWhereClause.Append(s1.colName + " <= convert(datetime,'" + s1.colValue + "',103)");
                        if (c == SearchConditions.DATE_EQUAL_TO) sWhereClause.Append(s1.colName + " = convert(datetime,'" + s1.colValue + "',103)");
                        if (c == SearchConditions.DATE_GREATER_THAN) sWhereClause.Append(s1.colName + " > convert(datetime,'" + s1.colValue + "',103)");
                        if (c == SearchConditions.DATE_LESSER_THAN) sWhereClause.Append(s1.colName + " < convert(datetime,'" + s1.colValue + "',103)");
                        if (c == SearchConditions.IS_NULL) sWhereClause.Append(s1.colName + " is null");

                        if (c == SearchConditions.DATE_BETWEEN)
                        {
                            string[] ar = s1.colValue.Split('|');
                            sWhereClause.Append(" ( " + s1.colName + " BETWEEN convert(datetime,'" + ar[0].ToString() + "',103) AND convert(datetime,'" + ar[1].ToString() + "',103))");
                        }
                    }
                }
            }
            return sWhereClause.ToString();
        }
        public string paramWhereClause(string sWhere, out List<SqlParameter> sparam)
        {
            StringBuilder s = new StringBuilder();
            StringBuilder sWhereClause = new StringBuilder();
            sparam = new List<SqlParameter>();
            if (sWhere != "")
            {
                bool bFlag = false;
                List<Data.tableColumns> tabCols = new List<Data.tableColumns>();
                string sNodeName = "";
                string sNodeType = "";
                using (XmlReader reader = XmlReader.Create(new StringReader(sWhere)))
                {
                    while (reader.Read())
                    {
                        if (bFlag)
                        {
                            switch (reader.NodeType)
                            {
                                case XmlNodeType.Element:

                                    sNodeName = reader.Name.ToString();
                                    sNodeType = reader.GetAttribute("TYPE").ToString();
                                    break;
                                case XmlNodeType.Text:
                                case XmlNodeType.CDATA:

                                    tabCols.Add(new Data.tableColumns { colName = sNodeName, colValue = reader.Value.ToString(), colType = sNodeType });
                                    sNodeName = "";
                                    sNodeType = "";
                                    break;
                            }
                        }
                        bFlag = true;
                    }
                    reader.Close();
                }
                int iCols = tabCols.Count;


                for (int i = 0; i <= tabCols.Count - 1; i++)
                {
                    var s1 = tabCols[i] as Data.tableColumns;
                    if (s1.colValue != "")
                    {
                        if (i > 0) sWhereClause.Append(" and ");
                        SearchConditions c = (SearchConditions)Enum.Parse(typeof(SearchConditions), s1.colType.ToUpper());
                        if (c == SearchConditions.CONTAINS) sWhereClause.Append(s1.colName + " like '%@" + s1.colName + "%'");
                        if (c == SearchConditions.EXACT) sWhereClause.Append(s1.colName + " = @" + s1.colName + "");
                        if (c == SearchConditions.EXACT_NUMBER) sWhereClause.Append(s1.colName + " =  @" + s1.colName);
                        if (c == SearchConditions.GREATER_THAN) sWhereClause.Append(s1.colName + " >  @" + s1.colName);
                        if (c == SearchConditions.GREATER_THAN_EQUAL_TO) sWhereClause.Append(s1.colName + " >=  @" + s1.colName);
                        if (c == SearchConditions.LESSER_THAN) sWhereClause.Append(s1.colName + " <  @" + s1.colName);
                        if (c == SearchConditions.LESSER_THAN_EQUAL_TO) sWhereClause.Append(s1.colName + " <=  @" + s1.colName);
                        if (c == SearchConditions.STARTS_WITH) sWhereClause.Append(s1.colName + " like '@" + s1.colName + "%'");
                        if (c == SearchConditions.NOT_EQUAL_TO) sWhereClause.Append(s1.colName + " != '@" + s1.colName + "'");
                        if (c == SearchConditions.DATE_GREATER_THAN_EQUAL_TO) sWhereClause.Append(s1.colName + " >= convert(datetime,'@" + s1.colName + "',103)");
                        if (c == SearchConditions.DATE_LESSER_THAN_EQUAL_TO) sWhereClause.Append(s1.colName + " <= convert(datetime,'@" + s1.colName + "',103)");
                        if (c == SearchConditions.DATE_EQUAL_TO) sWhereClause.Append(s1.colName + " = convert(datetime,' @" + s1.colName + "',103)");
                        if (c == SearchConditions.DATE_GREATER_THAN) sWhereClause.Append(s1.colName + " > convert(datetime,'@" + s1.colName + "',103)");
                        if (c == SearchConditions.DATE_LESSER_THAN) sWhereClause.Append(s1.colName + " < convert(datetime,'@" + s1.colName + "',103)");
                        if (c == SearchConditions.IS_NULL) sWhereClause.Append(s1.colName + " is null");

                        sparam.Add(new SqlParameter("@" + s1.colName, s1.colValue));
                        //if (c == SearchConditions.DATE_BETWEEN)
                        //{
                        //    string[] ar = s1.colValue.Split('|');
                        //    sWhereClause.Append(" ( " + s1.colName + " BETWEEN convert(datetime,'" + ar[0].ToString() + "',103) AND convert(datetime,'" + ar[1].ToString() + "',103))");
                        //}
                    }
                }
            }
            return sWhereClause.ToString();
        }
        public void updateDate(string typ, string tablename, string columnName, string val, string sWhere)
        {
            string sSql = "update " + tablename + " set " + columnName + " = ";
            if (val == "TODAY")
            {
                sSql = sSql + " getdate() ";
            }
            else
            {
                sSql = sSql + " convert(datetime,'" + val + "',103) ";
            }
            sSql = sSql + " where " + sWhere;
            if (typ == "INSERT")
            {
                if (val != "") ExecuteNonQuery(sSql);
            }
            else
            {
                if (val == "") sSql = "update " + tablename + " set " + columnName + " = NULL where " + sWhere;
                ExecuteNonQuery(sSql);
            }
        }
        public string newID()
        {
            String sOut = "";
            sOut = GetOneValue("select NEWID()");
            return sOut;
        }
        public int ExecuteNonQueryTable(string sSql, SqlParameter[] Params)
        {
            String sConnectionString = connectionString();
            int retval = 0;
            using (var cn = new SqlConnection(sConnectionString))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand(sSql, cn);
                foreach (SqlParameter p in Params)
                {
                    if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(p);

                }
                try
                {

                    retval = cmd.ExecuteNonQuery();
                    cn.Close();
                    cn.Dispose();
                }
                catch (Exception e)
                {
                    string msg = e.Message.ToString();
                }
            }
            return retval;
        }
        public int ExecuteNonQuery(string sSql, SqlParameter[] Params)
        {
            String sConnectionString = connectionString();
            int retval = 0;
            using (var cn = new SqlConnection(sConnectionString))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand(sSql, cn);
                foreach (SqlParameter p in Params)
                {
                    if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(p);
                }
                try
                {
                    retval = cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                }
                cn.Close();
                cn.Dispose();
            }
            return retval;
        }
        public int ExecuteSPNonQuery(string sSql, SqlParameter[] Params)
        {
            String sConnectionString = connectionString();
            int id = 0;
            var cn = new SqlConnection(sConnectionString);
            cn.Open();
            SqlCommand cmd = new SqlCommand(sSql, cn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter parm = new SqlParameter("aaa", SqlDbType.Int);
            parm.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(parm);
            foreach (SqlParameter p in Params)
            {
                cmd.Parameters.Add(new SqlParameter(p.ParameterName, p.SqlValue));
            }
            //  cmd.Parameters.Add(new SqlParameter("@UserName", name));
            //    cmd.Parameters.Add(new SqlParameter("@password", pass));
            //cmd.Parameters.Add(new SqlParameter("@id", ""));
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
            }
            cn.Close();
            id = Convert.ToInt32(parm.Value);
            cn.Dispose();
            return id;
        }
        public int ExecuteSPNonQuery(string sSql, SqlParameter[] Params, string ouptutparm)
        {
            String sConnectionString = connectionString();
            int id = 0;
            var cn = new SqlConnection(sConnectionString);


            cn.Open();
            SqlCommand cmd = new SqlCommand(sSql, cn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter parm = new SqlParameter("aaa", SqlDbType.Int);
            parm.Direction = ParameterDirection.ReturnValue;
            SqlParameter param = new SqlParameter(ouptutparm, System.Data.SqlDbType.NVarChar, 500);

            param.Direction = ParameterDirection.Output;

            cmd.Parameters.Add(parm);
            cmd.Parameters.Add(param);
            foreach (SqlParameter p in Params)
            {
                cmd.Parameters.Add(new SqlParameter(p.ParameterName, p.SqlValue));
            }
            //  cmd.Parameters.Add(new SqlParameter("@UserName", name));
            //    cmd.Parameters.Add(new SqlParameter("@password", pass));
            //cmd.Parameters.Add(new SqlParameter("@id", ""));
            try
            {
                cmd.ExecuteNonQuery();

            }
            catch (Exception e)
            {
            }

            cn.Close();
            id = Convert.ToInt32(parm.Value);
            if (id == 1)
            {
                return Convert.ToInt32(param.Value);
            }
            cn.Dispose();


            return id;
        }
        public string ExecuteSPNonQueryString(string sSql, SqlParameter[] Params, string ouptutparm)
        {
            String sConnectionString = connectionString();
            string id = "";
            var cn = new SqlConnection(sConnectionString);


            cn.Open();
            SqlCommand cmd = new SqlCommand(sSql, cn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter parm = new SqlParameter("aaa", SqlDbType.Int);
            parm.Direction = ParameterDirection.ReturnValue;
            SqlParameter param = new SqlParameter(ouptutparm, System.Data.SqlDbType.NVarChar, 500);

            param.Direction = ParameterDirection.Output;

            cmd.Parameters.Add(parm);
            cmd.Parameters.Add(param);
            foreach (SqlParameter p in Params)
            {
                cmd.Parameters.Add(new SqlParameter(p.ParameterName, p.SqlValue));
            }
            //  cmd.Parameters.Add(new SqlParameter("@UserName", name));
            //    cmd.Parameters.Add(new SqlParameter("@password", pass));
            //cmd.Parameters.Add(new SqlParameter("@id", ""));
            try
            {
                cmd.ExecuteNonQuery();

            }
            catch (Exception e)
            {
            }

            cn.Close();
            id = param.Value.ToString();
            if (id != "")
            {
                return param.Value.ToString();
            }
            cn.Dispose();


            return id;
        }
        public int ExecuteNonQuerySP(string sSql, SqlParameter[] Params)
        {
            String sConnectionString = connectionString();
            int id = 0;
            var cn = new SqlConnection(sConnectionString);


            cn.Open();
            SqlCommand cmd = new SqlCommand(sSql, cn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter parm = new SqlParameter("aaa", SqlDbType.Int);
            parm.Direction = ParameterDirection.ReturnValue;

            cmd.Parameters.Add(parm);
            foreach (SqlParameter p in Params)
            {
                cmd.Parameters.Add(new SqlParameter(p.ParameterName, p.SqlValue));
            }
            //  cmd.Parameters.Add(new SqlParameter("@UserName", name));
            //    cmd.Parameters.Add(new SqlParameter("@password", pass));
            //cmd.Parameters.Add(new SqlParameter("@id", ""));
            try
            {
                cmd.ExecuteNonQuery();

            }
            catch (Exception e)
            {
            }

            cn.Close();
            id = Convert.ToInt32(parm.Value);
            cn.Dispose();

            return id;
        }
        public string ExecuteNonQueryString(string sSql, SqlParameter[] Params)
        {
            String sConnectionString = connectionString();
            string id = "";
            var cn = new SqlConnection(sConnectionString);
            cn.Open();
            SqlCommand cmd = new SqlCommand(sSql, cn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter parm = new SqlParameter("@ReturnCode", SqlDbType.VarChar);
            parm.Size = 50;
            parm.Direction = ParameterDirection.InputOutput;
            cmd.Parameters.Add(new SqlParameter(parm.ParameterName, ""));
            //cmd.Parameters.Add(parm);

            //SqlParameter retval = cmd.Parameters.Add("@ReturnCode", SqlDbType.VarChar);
            //retval.Direction = ParameterDirection.ReturnValue;
            foreach (SqlParameter p in Params)
            {
                cmd.Parameters.Add(new SqlParameter(p.ParameterName, p.SqlValue));
            }
            try
            {
                cmd.ExecuteNonQuery();

            }
            catch (Exception e)
            {
            }
            cn.Close();
            // id = (string)cmd.Parameters["@ReturnValue"].Value;
            //  id = (string)cmd.Parameters["@ReturnCode"].Value;
            id = (string)parm.Value;
            cn.Dispose();

            return id;
        }
        public int ExecuteNonQuery(SqlCommand cmd)
        {
            String sConnectionString = connectionString();
            int retval = 0;
            using (var cn = new SqlConnection(sConnectionString))
            {
                cn.Open();
                cmd.Connection = cn;
                retval = cmd.ExecuteNonQuery();
                cn.Close();
                cn.Dispose();
            }
            return retval;
        }
        public int ExecuteNonQuery(string sSql)
        {
            String sConnectionString = connectionString();
            int retval = 0;
            using (var cn = new SqlConnection(sConnectionString))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand(sSql, cn);
                retval = cmd.ExecuteNonQuery();
                cn.Close();
                cn.Dispose();
            }
            return retval;
        }
        public int ExecuteScalar(string sSql)
        {
            String sConnectionString = connectionString();
            int retval = 0;
            using (var cn = new SqlConnection(sConnectionString))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand(sSql, cn);
                retval = (int)cmd.ExecuteScalar();
                cn.Close();
                cn.Dispose();
            }
            return retval;
        }
        public DataSet ExecuteDataset(string sSql)
        {
            String sConnectionString = connectionString();
            DataSet ds = new DataSet();
            using (var cn = new SqlConnection(sConnectionString))
            {
                cn.Open();
                SqlDataAdapter da = new SqlDataAdapter(sSql, cn);
                da.Fill(ds);
                cn.Close();
                cn.Dispose();
            }
            return ds;
        }
        public DataSet ExecuteDatasetSP(string spName, string Params, List<KeyValuePair<string, string>> kvp)
        {
            DataSet ds = new DataSet();
            con = new SqlConnection(connectionString());
            con.Open();
            SqlCommand cmd = new SqlCommand(spName, con);
            cmd.CommandType = CommandType.StoredProcedure;
            foreach (var s in kvp)
            {
                cmd.Parameters.AddWithValue(s.Key, s.Value);
            }
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.SelectCommand = cmd;
            da.Fill(ds);
            con.Close();
            con.Dispose();
            return ds;
        }
        public DataSet ExecuteDataset(string sSql, List<SqlParameter> Params)
        {
            String sConnectionString = connectionString();
            DataSet ds = new DataSet();
            using (var cn = new SqlConnection(sConnectionString))
            {
                cn.Open();
                SqlDataAdapter da = new SqlDataAdapter(sSql, cn);
                foreach (var s in Params)
                // for (int i = 0; i < Params.Length - 1; i++)
                {
                    da.SelectCommand.Parameters.Add(s);
                }
                da.Fill(ds);
                cn.Close();
                cn.Dispose();
            }
            return ds;
        }
        public DataRow GetOneRow(string sSql, List<SqlParameter> Params)
        {
            DataSet ds = new DataSet();
            ds = ExecuteDataset(sSql, Params);
            DataRow row;
            if (ds.Tables[0].Rows.Count > 0)
            {
                row = ds.Tables[0].Rows[0];
            }
            else
            {
                row = null;
            }
            return row;
        }
        public void executeCommand(SqlCommand s)
        {
            String sConnectionString = connectionString();
            int retval = 0;
            using (var cn = new SqlConnection(sConnectionString))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = s;
                cmd.Connection = cn;
                retval = cmd.ExecuteNonQuery();
                cmd.Dispose();
                cn.Close();
                cn.Dispose();
            }
        }
        public DataRow GetOneRow(string sSql)
        {
            DataSet ds = new DataSet();
            ds = ExecuteDataset(sSql);
            DataRow row;
            if (ds.Tables[0].Rows.Count > 0)
            {
                row = ds.Tables[0].Rows[0];
            }
            else
            {
                row = null;
            }
            return row;
        }
        public string GetOneValue(string sSql)
        {
            DataSet ds = new DataSet();
            ds = ExecuteDataset(sSql);
            DataRow row;
            string sOutput;
            if (ds.Tables[0].Rows.Count > 0)
            {
                row = ds.Tables[0].Rows[0];
                sOutput = row[0].ToString();
            }
            else
            {
                row = null;
                sOutput = "";
            }
            return sOutput;
        }
        public List<string> getConectiondetails()
        {
            List<string> lst = new List<string>();
            string conString = connectionString();
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(conString);
            lst.Add(builder.UserID);
            lst.Add(builder.Password);
            lst.Add(builder.DataSource);
            lst.Add(builder.InitialCatalog);
            return lst;
        }
        public int countData(string sString)
        {
            String sConnectionString = connectionString();
            int i = 0;
            using (var cn = new SqlConnection(sConnectionString))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand(sString, cn);
                i = Convert.ToInt32(cmd.ExecuteScalar());
                cmd.Dispose();
                cn.Close();
                cn.Dispose();
            }
            return i;
        }
        public void insertXML(string stable, string sxml)
        {
            processXML(stable, sxml, "INSERT", "");
        }
        public void updateXML(string stable, string sxml, string swhere)
        {
            processXML(stable, sxml, "UPDATE", swhere);
        }
        private void processXML(string sTable, string sXML, string sMode, string sWhere)
        {
            List<Data.tableColumns> tabCols = new List<Data.tableColumns>();
            bool bFlag = false;
            string sNodeName = "";
            string sNodeType = "";
            bool hasValue = true;
            using (XmlReader reader = XmlReader.Create(new StringReader(sXML)))
            {
                while (reader.Read())
                {
                    if (bFlag)
                    {
                        switch (reader.NodeType)
                        {
                            case XmlNodeType.Element:
                                if (hasValue == false)
                                {
                                    tabCols.Add(new Data.tableColumns { colName = sNodeName, colValue = "", colType = "VALUE" });
                                }
                                hasValue = false;
                                sNodeName = reader.Name.ToString();
                                if (reader.HasAttributes)
                                {
                                    sNodeType = reader.GetAttribute("TYPE").ToString();
                                    switch (sNodeType)
                                    {
                                        case "CURDATE":
                                            tabCols.Add(new Data.tableColumns { colName = sNodeName, colValue = "getdate()", colType = "STATIC" });
                                            break;
                                        case "NEWID":
                                            tabCols.Add(new Data.tableColumns { colName = sNodeName, colValue = "newid()", colType = "STATIC" });
                                            break;
                                        case "DATE":
                                            sNodeType = "DATE";
                                            break;
                                    }
                                }
                                else
                                {
                                    sNodeType = "VALUE";
                                }
                                break;
                            case XmlNodeType.Text:
                            case XmlNodeType.CDATA:
                                hasValue = true;
                                if (sNodeType == "DATE")
                                {
                                    if (reader.Value.ToString() == "CURDATE")
                                    {
                                        tabCols.Add(new Data.tableColumns { colName = sNodeName, colValue = "getdate()", colType = "STATIC" });
                                    }
                                    else
                                    {
                                        if (reader.Value.ToString() == "NULL")
                                        {
                                            tabCols.Add(new Data.tableColumns { colName = sNodeName, colValue = "", colType = "NULLDATE" });
                                        }
                                        else
                                        {
                                            tabCols.Add(new Data.tableColumns { colName = sNodeName, colValue = reader.Value.ToString(), colType = "DATE" });
                                        }
                                    }
                                }
                                else
                                {
                                    if (reader.Value.ToString() == "CURDATE")
                                    {
                                        tabCols.Add(new Data.tableColumns { colName = sNodeName, colValue = "getdate()", colType = "STATIC" });
                                    }
                                    else if (reader.Value.ToString() == "NEWID")
                                    {
                                        tabCols.Add(new Data.tableColumns { colName = sNodeName, colValue = "newid()", colType = "STATIC" });
                                    }
                                    else
                                    {
                                        tabCols.Add(new Data.tableColumns { colName = sNodeName, colValue = reader.Value.ToString(), colType = sNodeType });
                                    }
                                }
                                sNodeName = "";
                                sNodeType = "";
                                break;
                        }
                    }
                    bFlag = true;
                }
                reader.Close();
            }
            int iCols = tabCols.Count;
            string sIns = "";
            string sVal = "";
            string sUpd = "";
            SqlCommand cmd = new SqlCommand();
            bool bAddFlag = false;
            for (int i = 0; i <= tabCols.Count - 1; i++)
            {
                var s = tabCols[i] as Data.tableColumns;
                if (sIns != "") sIns = sIns + ",";
                sIns = sIns + s.colName;
                if (sVal != "") sVal = sVal + ",";
                if (sUpd != "") sUpd = sUpd + ",";
                bAddFlag = false;
                switch (s.colType)
                {
                    case "VALUE":
                        sVal = sVal + "@" + s.colName;
                        sUpd = sUpd + s.colName + " = @" + s.colName;
                        bAddFlag = true;
                        break;
                    case "NULLDATE":
                        sVal = sVal + "NULL";
                        sUpd = sUpd + s.colName + " = NULL";
                        bAddFlag = true;
                        break;
                    case "DATE":
                        sVal = sVal + "convert(datetime,@" + s.colName + ",103)";
                        sUpd = sUpd + s.colName + " = convert(datetime,@" + s.colName + ",103)";
                        bAddFlag = true;
                        break;
                    case "STATIC":
                        sUpd = sUpd + s.colName + " = " + s.colValue;
                        sVal = sVal + s.colValue;
                        bAddFlag = false;
                        break;
                }
                if (bAddFlag)
                {
                    SqlParameter param = new SqlParameter("@" + s.colName, s.colValue);
                    cmd.Parameters.Add(param);
                }
            }
            string sSql = "";
            if (sMode == "INSERT")
            {
                sSql = "INSERT INTO " + sTable + "(" + sIns + ") VALUES(" + sVal + ")";
            }
            else
            {
                string sWhereClause = "";
                if (sWhere.IndexOf("<where>") == 0)
                {
                    sWhereClause = whereClause(sWhere);
                }
                else
                {
                    sWhereClause = sWhere;
                }
                sSql = "UPDATE " + sTable + " set " + sUpd + " where " + sWhereClause;
            }
            String sConnectionString = connectionString();
            using (var cn = new SqlConnection(sConnectionString))
            {
                cn.Open();
                cmd.Connection = cn;
                cmd.CommandText = sSql;
                int retval = cmd.ExecuteNonQuery();
                cmd.Dispose();
                cn.Dispose();
            }
        }
        public void delete(string stable, string sWhere)
        {
            string sSql = "delete from " + stable + " where 1=1";
            if (sWhere != "")
            {
                string sWhereClause = "";
                if (sWhere.IndexOf("<where>") == 0)
                {
                    sWhereClause = whereClause(sWhere);
                }
                else
                {
                    sWhereClause = sWhere;
                }
                if (sWhereClause != "") sSql = sSql + " and " + sWhereClause;
            }
            ExecuteNonQuery(sSql);
        }
        public bool hasRows(String sql, SqlParameter[] sqlParams)
        {
            String sConnectionString = connectionString();
            bool bFlag = false;
            using (var cn = new SqlConnection(sConnectionString))
            {
                var cmd = new SqlCommand(sql, cn);
                for (int i = 0; i < sqlParams.Length; i++)
                {
                    cmd.Parameters.Add(sqlParams[i]);
                }
                cn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    bFlag = true;
                }
                reader.Dispose();
                cn.Dispose();
            }
            return bFlag;
        }
        public void Writelog(string msg)
        {
            string emsg = "";
            try
            {
                System.IO.StreamWriter file = new System.IO.StreamWriter(@"c:\\iris.log", true);
                String s = System.IO.Directory.GetCurrentDirectory();
                //file.WriteLine(s);
                file.WriteLine(msg);
                file.Close();
            }
            catch (Exception ex)
            {
                emsg = ex.Message.ToString();
            }
        }
        public string insertData(SqlCommand cmd, string sql)
        {
            string sOut = "";
            try
            {
                String sConnectionString = connectionString();
                con = new SqlConnection(sConnectionString);

                con.Open();
                cmd.Connection = con;
                cmd.CommandText = sql;
                int retval = cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Close();
                con.Dispose();

            }
            catch (Exception ex)
            {
                sOut = ex.Message.ToString();
            }
            return sOut;
        }
        public DataSet ExecuteDatasetwithparam(string spName, SqlParameter[] Params)
        {
            DataSet ds = new DataSet();
            con = new SqlConnection(connectionString());
            con.Open();
            SqlCommand cmd = new SqlCommand(spName, con);
            cmd.CommandType = CommandType.StoredProcedure;

            foreach (SqlParameter p in Params)
            {
                cmd.Parameters.Add(new SqlParameter(p.ParameterName, p.SqlValue));
            }
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.SelectCommand = cmd;
            da.Fill(ds);
            cmd.Parameters.Clear();
            con.Close();
            con.Dispose();
            return ds;
        }
    }
}
