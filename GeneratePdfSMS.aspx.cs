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

public partial class GeneratePdfSMS : System.Web.UI.Page
{
    static SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStr2"].ConnectionString);
    SqlCommand cmd;
    byte[] bytes;
    string fileName = "ABC";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                lblKey.Text = Request.QueryString["KEY"].ToString(); //
                lblType.Text = lblKey.Text.Substring(0, 1);
                lblAPIKey.Text = lblKey.Text.Substring(1, 8);
                lblRandomKey.Text = lblKey.Text.Substring(9, 12);
                lblPatientId.Text = lblKey.Text.Substring(21);
                getData(lblType.Text, lblPatientId.Text, lblAPIKey.Text, lblRandomKey.Text);
            }
            catch (Exception EX)
            {

            }
        }
    }

    protected void getData(string Type, string PatientId, string APIKey, string RandomKey)
    {
        try
        {
            //string path = Server.MapPath("~/PDF/71_Recipt.pdf");
            //WebClient client = new WebClient();
            //Byte[] buffer = client.DownloadData(path);

            //if (buffer != null)
            //{
            //    Response.ContentType = "application/pdf";
            //    Response.AddHeader("content-length", buffer.Length.ToString());
            //    Response.BinaryWrite(buffer);
            //}

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            if (Type != "M")
            {
                cmd = new SqlCommand("Select * from TBL_Attachment where APIKey=@APIKey and RandomKey=@RandomKey and PatientId=@ID", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@APIKey", APIKey);
                cmd.Parameters.AddWithValue("@RandomKey", RandomKey);
                cmd.Parameters.AddWithValue("@ID", PatientId);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    div.Visible = true;
                    div1.Visible = true;
                    divNotFound.Visible = false;
                    foreach (DataRow dr in dt.Rows)
                    {
                        bytes = (byte[])dr["Attach_Ment"];
                        fileName = dr["Type"].ToString();
                    }

                    string embed = "<object data=\"{0}{1}\" type=\"application/pdf\" width=\"500px\" height=\"600px\">";
                    embed += "If you are unable to view file, you can download from <a href = \"{0}{1}&download=1\">here</a>";
                    embed += " or download <a target = \"_blank\" href = \"http://get.adobe.com/reader/\">Adobe PDF Reader</a> to view the file.";
                    ltEmbed.Text = string.Format(embed, ResolveUrl("~/MyPathTest.ashx?KEY="), lblKey.Text);
                    //HttpContext.Current.Response.Buffer = true;
                    //HttpContext.Current.Response.Charset = "";
                    //HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" + APIKey + '_' + PatientId + '_' + RandomKey + '_' + fileName + ".pdf");
                    //HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    //MemoryStream pdfStream = new MemoryStream();
                    //pdfStream.Write(bytes, 0, bytes.Length);
                    //pdfStream.Position = 0;
                    //HttpContext.Current.Response.ContentType = "application/pdf";
                    //HttpContext.Current.Response.BinaryWrite(bytes);
                    //HttpContext.Current.Response.Flush();
                    //HttpContext.Current.Response.End();
                    Response.Redirect("~/MyPathTest.ashx?KEY=" + lblKey.Text, false);
                    foreach (DataRow dr in dt.Rows)
                    {
                        bytes = (byte[])dr["Attachment"];
                        fileName = dr["Type"].ToString();
                    }

                }
                else
                {
                    div.Visible = false;
                    div1.Visible = false;
                    divNotFound.Visible = true;
                }
            }
            else
            {
                cmd = new SqlCommand("USP_GetSMSLogByAPIKey", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@APIKey", APIKey);
                cmd.Parameters.AddWithValue("@Patientid", PatientId);
                cmd.Parameters.AddWithValue("@RandomKey", RandomKey);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    div.Visible = true;
                    div1.Visible = false;
                    divNotFound.Visible = false;

                    foreach (DataRow dr in dt.Rows)
                    {
                        bytes = (byte[])dr["Attach_Ment"];
                        fileName = dr["Type"].ToString();
                    }

                    string embed = "<object data=\"{0}{1}\" type=\"application/pdf\" width=\"1000px\" height=\"1000px\">";
                    embed += "If you are unable to view file, you can download from <a href = \"{0}{1}&download=0\">here</a>";
                    embed += " or download <a target = \"_blank\" href = \"http://get.adobe.com/reader/\">Adobe PDF Reader</a> to view the file.";
                    ltEmbed.Text = string.Format(embed, ResolveUrl("~/MyPathTest.ashx?KEY="), lblKey.Text);

                    Response.Redirect("~/MyPathTest.ashx?KEY=" + lblKey.Text, false);
                    foreach (DataRow dr in dt.Rows)
                    {
                        bytes = (byte[])dr["Attach_Ment"];
                        fileName = dr["Type"].ToString();
                    }

                }
                else
                {
                    div.Visible = false;
                    div1.Visible = false;
                    divNotFound.Visible = true;
                }
            }
        }
        catch (SqlException)
        {

        }
        catch (Exception e)
        {

        }
        finally
        {
            con.Close();
        }
    }
}