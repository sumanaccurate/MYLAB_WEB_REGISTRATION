using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

public partial class QRSMS_Report : System.Web.UI.Page
{
    static SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStr2"].ConnectionString);
    static SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStr3"].ConnectionString);
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
                string date = lblKey.Text.Substring(9, 4);
                date = date + '-' + lblKey.Text.Substring(13, 2);
                date = date + '-' + lblKey.Text.Substring(15, 2);
                lblRandomKey.Text = date;
                lblPatientId.Text = lblKey.Text.Substring(17);
                getData(lblType.Text, lblPatientId.Text, lblAPIKey.Text, lblRandomKey.Text);
            }
            catch (Exception EX)
            {

            }
        }
    }

    protected void getData(string Type, string PatientId, string APIKey, string date)
    {
        try
        {
          
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
                    ltEmbed.Text = string.Format(embed, ResolveUrl("~/MyPathTest_SMS.ashx?KEY="), lblKey.Text);
                    Response.Redirect("~/MyPathTest_SMS.ashx?KEY=" + lblKey.Text, false);
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
                    GetLabInformation(APIKey);
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
    public void GetLabInformation(string APIKey)
    {
        if (con1.State == ConnectionState.Closed)
        {
            con1.Open();
        }
        cmd = new SqlCommand("USP_GetRegisterUserDetailsByMobileNo", con1);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", "M");
        cmd.Parameters.AddWithValue("@APIKey", APIKey);
        SqlDataAdapter sda = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        sda.Fill(dt);
        if (dt.Rows.Count > 0)
        {
            lbllab.Text = dt.Rows[0]["BusinessName"].ToString();
            lbllab1.Text = dt.Rows[0]["BusinessName"].ToString();
            lblcontact.Text = dt.Rows[0]["ContactPerson"].ToString();
            lblmobile.Text = dt.Rows[0]["MobileNo"].ToString();
            lblemail.Text = dt.Rows[0]["Email"].ToString();
        }
    }
}