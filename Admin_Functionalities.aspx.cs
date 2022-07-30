using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace SDA_Project_Online_Shopping_Service__Cons_
{
    public partial class Admin_Functionalities : System.Web.UI.Page
    {
        int itid = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
           
            sda_proj_srvc_rfrnc.UserSrvcSoapClient s = new sda_proj_srvc_rfrnc.UserSrvcSoapClient();

            sda_proj_srvc_rfrnc.ArrayOfString str = s.View_Items();


            List<string[]> stringList = new List<string[]>();
            for (int i = 0; i < str.Count; i++)
            {

                string[] tmp = str[i].Split(',');
                stringList.Add(tmp);

            }

            
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add("Id", System.Type.GetType("System.String"));
            dt.Columns.Add("Name", System.Type.GetType("System.String"));
            dt.Columns.Add("Description", System.Type.GetType("System.String"));
            dt.Columns.Add("Price", System.Type.GetType("System.Double"));
            dt.Columns.Add("Category", System.Type.GetType("System.String"));
            dt.Columns.Add("Quantity in stock", System.Type.GetType("System.Int32"));
            //dt.Columns.Add("Check",Type.GetType)



            foreach (string[] ss in stringList)
            {
                dr = dt.NewRow();
                dr["Id"] = ss[0];
                dr["Name"] = ss[1];
                dr["Description"] = ss[2];
                dr["Price"] = ss[3];
                dr["Category"] = ss[4];
                dr["Quantity in stock"] = ss[5];

                dt.Rows.Add(dr);
            }

            dt.AcceptChanges();
            GridView1.DataSource = dt;
            GridView1.DataBind();

        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextBox1.Text = GridView1.SelectedRow.Cells[2].Text;
            TextBox2.Text = GridView1.SelectedRow.Cells[3].Text;
            TextBox3.Text = GridView1.SelectedRow.Cells[4].Text;
            TextBox4.Text = GridView1.SelectedRow.Cells[5].Text;
            TextBox5.Text = GridView1.SelectedRow.Cells[6].Text;

            itid = int.Parse(GridView1.SelectedRow.Cells[1].Text);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

            sda_proj_srvc_rfrnc.UserSrvcSoapClient s = new sda_proj_srvc_rfrnc.UserSrvcSoapClient();
            string n = TextBox1.Text,d= TextBox2.Text, c= TextBox4.Text;
            float p = float.Parse(TextBox3.Text);
            int q = int.Parse(TextBox5.Text);
            s.a_add_items(n, d, p, c, q);
            Page_Load(sender, e);

        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            sda_proj_srvc_rfrnc.UserSrvcSoapClient s = new sda_proj_srvc_rfrnc.UserSrvcSoapClient();
            

            string n = TextBox1.Text, d = TextBox2.Text, c = TextBox4.Text;
            float p = float.Parse(TextBox3.Text);
            int q = int.Parse(TextBox5.Text);
            itid = int.Parse(GridView1.SelectedRow.Cells[1].Text);

            s.a_UpdateName(itid,n);
            s.a_UpdateDescription(itid, d);
            s.a_UpdatePrice(itid, p);
            s.a_UpdateCategory(itid, c);
            s.a_UpdateQuantity(itid, q);
            Page_Load(sender, e);

        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            sda_proj_srvc_rfrnc.UserSrvcSoapClient s = new sda_proj_srvc_rfrnc.UserSrvcSoapClient();

            TextBox1.Text= TextBox2.Text= TextBox3.Text= TextBox4.Text= TextBox5.Text= "";
            itid = int.Parse(GridView1.SelectedRow.Cells[1].Text);

            s.a_Delete_Items(itid);
            Page_Load(sender,e);
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
        }

        protected void Button7_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/HomePage.aspx");

        }
    }
}