<%@ WebHandler Language="C#" Class="MyPathTest_Report" %>

using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


public class MyPathTest_Report : IHttpHandler {

    static SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString);
    SqlCommand cmd;
    byte[] bytes;
    string fileName = "ABC";
    public void ProcessRequest(HttpContext context)
    {
        string strKey = context.Request.QueryString["KEY"].ToString();
        string Type = strKey.Substring(0, 1);
        string APIKey = strKey.Substring(1, 8);
        string date = strKey.Substring(9, 4);
        date = date + '-' + strKey.Substring(13, 2);
        date = date + '-' + strKey.Substring(15, 2);
        string PatientId = strKey.Substring(17);
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }

        
        if(Type == "M")
        {
            cmd = new SqlCommand("USP_GetSMSLogByAPIKey_Report", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@APIKey", APIKey);
            cmd.Parameters.AddWithValue("@Patientid", PatientId);
            cmd.Parameters.AddWithValue("@date", date);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    bytes = (byte[])dr["Attach_Ment"];
                    fileName = dr["Type"].ToString();
                }
                context.Response.Buffer = true;
                context.Response.Charset = "";
                //context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + PatientId + '_' + fileName + ".pdf");
                context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                context.Response.ContentType = "application/pdf";
                context.Response.BinaryWrite(bytes);
                context.Response.Flush();
                context.Response.End();
            }
            else
            {

            }
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}