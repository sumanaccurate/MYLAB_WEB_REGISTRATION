using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

public partial class QRReport : System.Web.UI.Page
{
    static SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString);
    SqlCommand cmd;
    byte[] bytes;
    string fileName = "ABC";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                if (Request.QueryString["KEY"] != "" && Request.QueryString["KEY"] != null)
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
                else
                {
                    div.Visible = true;
                }
            }
            catch (Exception EX)
            {

            }
        }
    }

    protected void Btn_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/MyPathTest_Report.ashx?KEY=" + lblKey.Text, false);
    }

    protected void getData(string Type, string PatientId, string APIKey, string date)
    {
        try
        {
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
                    div.Visible = true;
                    div1.Visible = false;
                    divNotFound.Visible = false;

                    foreach (DataRow dr in dt.Rows)
                    {
                        bytes = (byte[])dr["Attach_Ment"];
                        fileName = dr["Type"].ToString();
                    }
                    string base64PDF = System.Convert.ToBase64String(bytes, 0, bytes.Length);
                    string embed = "<object data=\"data:application/pdf;base64,{0}\" type=\"application/pdf\" width=\"1000px\" height=\"1000px\">";
                    embed += "<embed src=\"data:application/pdf;base64,{0}\" type=\"application/pdf\" /></object>";
                    
                    //ltEmbed.Text = string.Format(embed, ResolveUrl("~/MyPathTest_Report.ashx?KEY="), lblKey.Text);
                    ltEmbed.Text = string.Format(embed,  base64PDF);

                    Response.Redirect("~/MyPathTest_Report.ashx?KEY=" + lblKey.Text, false);
                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    bytes = (byte[])dr["Attach_Ment"];
                    //    fileName = dr["Type"].ToString();
                    //}
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
        catch (SqlException ex)
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
        if (con.State == ConnectionState.Closed)
        {
            con.Open();
        }
        cmd = new SqlCommand("USP_GetRegisterUserDetailsByMobileNo", con);
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