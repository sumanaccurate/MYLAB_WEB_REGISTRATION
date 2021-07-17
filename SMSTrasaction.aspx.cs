using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

public partial class SMSTrasaction : System.Web.UI.Page
{
    static SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString);
    SqlCommand cmd;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SendWhatsApp();
        }
    }

    protected void SendSMS()
    {

    }
    protected void SendEmail()
    {

    }
    protected void SendWhatsApp()
    {

        try
        {

           // con.Open();
            cmd = new SqlCommand("Select T.ID,* from REGISTRATION R inner join TBL_WhatsApp_TRANSACTION as T on R.APIKey=T.APIKey where T.IsSMSSend=1", con);

            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                string postData = "", AttachmentId = "";
                byte[] bytes;
                // lblName.Text=dr["Name"].ToString();
                lbltoMobile.Text = dr["MobileNo"].ToString();
                lblFromMobile.Text = dr["MobileNo"].ToString();
                lblMessages.Text = dr["Text"].ToString();
                lblBusinessName.Text = dr["BusinessName"].ToString();
                lblAPIKey.Text = dr["APIKey"].ToString()+""+dr["RandomKey"].ToString()+""+dr["ID"].ToString();;
                AttachmentId =dr["Id"].ToString();
                bytes = System.Text.Encoding.UTF8.GetBytes(dr["Attachment"].ToString());
                lblDate.Text = dr["Date"].ToString();
                InsertWhatsAppLog(lblAPIKey.Text, lbltoMobile.Text, lblMessages.Text, AttachmentId, Convert.ToDateTime(lblDate.Text), lblType.Text);
                WebResponse response;
                string strTargetUrl = ConfigurationManager.AppSettings.Get("WhatsAPPAPI");
                HttpWebRequest request =
                 (HttpWebRequest)WebRequest.Create(strTargetUrl);
                request.Method = "PUT";
                request.ContentType = "application/json";
                request.KeepAlive = true;
                request.AllowAutoRedirect = false;
                request.UseDefaultCredentials = true;
                request.Accept = "*/*";
                //request.ContentType = "application/json";

                //string strUserName = "FA_11357";
                //string strPassword = "Tk6$6hfCAVhM^lZ_GIX2";
                //string strUserName = "FA_Capital";
                //string strPassword = "xbpoPIqRqrpFH_wGxRqw";
                //var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(strUserName + ":" + strPassword);
                //string strAuth = System.Convert.ToBase64String(plainTextBytes);
                //request.Headers.Add("Authorization", "Basic " + strAuth);
                postData = "{ " + "\n";
                postData = postData + @"""Name"": """ + lblName.Text + @"""," + "\n";
                postData = postData + @"""toMobile"": """ + lbltoMobile.Text + @"""," + "\n";
                postData = postData + @"""FromMobile"": """ + lblFromMobile.Text + @"""," + "\n";
                postData = postData + @"""Messages"": """ + lblMessages.Text + @"""," + "\n";
                postData = postData + @"""BusinessName"": """ + lblBusinessName.Text + @"""," + "\n";
                postData = postData + @"""APIKey"": """ + lblAPIKey.Text + @"""" + "\n";
                postData = postData + " }";
                var data = Encoding.ASCII.GetBytes(postData);
                //request. 
                request.ContentLength = data.Length;
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                JavaScriptSerializer serial1 = new JavaScriptSerializer();
                response = request.GetResponse();
                string result = new StreamReader(response.GetResponseStream()).ReadToEnd();

                //HieracheyResopnce  objresp = new HieracheyResopnce();
            }
        }
        catch (SqlException)
        {

        }
        catch (Exception)
        {

        }
        finally
        {
           // con.Close();
        }
    }

    protected void InsertWhatsAppLog(string APIKey, string PatientMobileNo, string PatientSMS, string AttachmentId, DateTime PatientRegisterDate, string Type)
    {
        try
        {
            string postData = "";
            WebResponse response;
            string strTargetUrl = ConfigurationManager.AppSettings.Get("WhatsAPPLogAPI");
            HttpWebRequest request =
             (HttpWebRequest)WebRequest.Create(strTargetUrl);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.KeepAlive = true;
            request.AllowAutoRedirect = false;
            request.UseDefaultCredentials = true;
            request.Accept = "*/*";
            postData = "{ " + "\n";
            postData = postData + @"""APIKey"": """ + APIKey + @"""," + "\n";
            postData = postData + @"""PatientMobileNo"": """ + PatientMobileNo + @"""," + "\n";
            postData = postData + @"""PatientSMS"": """ + PatientSMS + @"""," + "\n";
            postData = postData + @"""AttachmentId"": """ + AttachmentId + @"""," + "\n";
            postData = postData + @"""PatientRegisterDate"": """ + PatientRegisterDate.ToString("yyyy-MM-dd") + @"""," + "\n";
            postData = postData + @"""Status"":" + " true " + "," + "\n";
            postData = postData + @"""Type"": """ + Type + @"""" + "\n";
            postData = postData + " }";
            var data = Encoding.ASCII.GetBytes(postData);
            //request. 
            request.ContentLength = data.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            JavaScriptSerializer serial1 = new JavaScriptSerializer();
            response = request.GetResponse();
            string result = new StreamReader(response.GetResponseStream()).ReadToEnd();
        }
        catch (Exception)
        {

        }
    }

}