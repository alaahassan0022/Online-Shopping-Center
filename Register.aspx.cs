using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SDA_Project_Online_Shopping_Service__Cons_
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            sda_proj_srvc_rfrnc.UserSrvcSoapClient s = new sda_proj_srvc_rfrnc.UserSrvcSoapClient();
            s.u_Register(TextBox1.Text, TextBox2.Text, TextBox3.Text, TextBox4.Text, TextBox5.Text, TextBox6.Text);
            Response.Redirect("~/Login.aspx");
        }
    }
}