using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SDA_Project_Online_Shopping_Service__Cons_
{
    public partial class Login : System.Web.UI.Page
    {
        public static string uid="";
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            char aoru;
            if (RadioButtonList1.SelectedIndex == 0) { 
                aoru = 'u';
                uid = TextBox1.Text;
            }
            else
                aoru = 'a';

            sda_proj_srvc_rfrnc.UserSrvcSoapClient s = new sda_proj_srvc_rfrnc.UserSrvcSoapClient();
            string st = s.Login(TextBox1.Text, TextBox2.Text,aoru);
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + st + "');", true);

            if(aoru=='u')
                Response.Redirect("~/User_Functionalities.aspx");
            else
                Response.Redirect("~/Admin_Functionalities.aspx");

        }

        protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}