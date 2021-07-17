<%@ WebHandler Language="C#" Class="MyPathTest_SMS" %>

using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public class MyPathTest_SMS : IHttpHandler {

    static SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStr2"].ConnectionString);
    SqlCommand cmd;
    byte[] bytes;
    string fileName = "ABC";
    public void ProcessRequest (HttpContext context) {
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

        if (Type != "M")
        {

            cmd = new SqlCommand("Select * from TBL_Attachment where APIKey=@APIKey AND Type=@Type and convert(date,Date)=@date and PatientId=@ID", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@APIKey", APIKey);
            cmd.Parameters.AddWithValue("@Type", "Report");
            cmd.Parameters.AddWithValue("@date", date);
            cmd.Parameters.AddWithValue("@ID", PatientId);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                bytes = (byte[])dr["Attach_Ment"];
                fileName = dr["Type"].ToString();
            }
            context.Response.Buffer = true;
            context.Response.Charset = "";
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            context.Response.ContentType = "application/pdf";
            context.Response.BinaryWrite(bytes);
            context.Response.Flush();
            context.Response.End();
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}