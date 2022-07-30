using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace SDA_Project_Online_Shopping_Service__Cons_
{
    public partial class User_Functionalities : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) { 
                Label1.Visible = false;
                DropDownList1.Visible = false;
                Button1.Visible = false;
                Label3.Visible = false;
                Label5.Visible = false;

                DropDownList4.Visible = false;

                Label2.Visible = false;
                Button4.Visible = false;
                Button5.Visible = false;
                DropDownList2.Visible = false;
                Label4.Visible = false;
                DropDownList3.Visible = false;
                Button6.Visible = false;
            }
            
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            if(!IsPostBack)
            Panel2.Visible = false;

            
            Label3.Visible = true;
            Label5.Visible = true;

            DropDownList4.Visible = true;


            sda_proj_srvc_rfrnc.UserSrvcSoapClient s = new sda_proj_srvc_rfrnc.UserSrvcSoapClient();
              
            sda_proj_srvc_rfrnc.ArrayOfString  str = s.View_Items();

            
            List<string[]> stringList = new List<string[]>();
            for (int i = 0; i < str.Count; i++) { 

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

            DropDownList4.Items.Clear();
            DropDownList4.Items.Add("All");
            sda_proj_srvc_rfrnc.UserSrvcSoapClient c = new sda_proj_srvc_rfrnc.UserSrvcSoapClient();
            string[] categs = c.u_get_categories().ToArray();
            for (int i = 0; i < categs.Length; i++)
            {
                if (DropDownList4.Items.FindByValue(categs[i]) == null)
                    DropDownList4.Items.Add(categs[i]);
            }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Label1.Visible = true;
            DropDownList1.Visible = true;
            Button1.Visible = true;
            DropDownList1.Items.Clear();
            Label1.Text = GridView1.SelectedRow.Cells[2].Text;
            //itid =  int.Parse(GridView1.SelectedRow.Cells[1].Text);
            for (int i = 1; i <= (int.Parse(GridView1.SelectedRow.Cells[6].Text)); i++)
                DropDownList1.Items.Add(i.ToString());
        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            
            //GridView1.Sort(e.SortExpression,e.SortDirection);
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            sda_proj_srvc_rfrnc.UserSrvcSoapClient srvcSoapClient = new sda_proj_srvc_rfrnc.UserSrvcSoapClient();

            sda_proj_srvc_rfrnc.ArrayOfString str = srvcSoapClient.u_View_Cart(Login.uid);
            if (str.Count > 0) { 

                if (!IsPostBack)
                    Panel1.Visible = false;
                Label4.Visible = true;
                Button4.Visible = true;
                Button5.Visible = true;
                DropDownList3.Visible = true;
                Button6.Visible = true;

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
                dt.Columns.Add("Quantity to order", System.Type.GetType("System.Int32"));
                //dt.Columns.Add("Check",Type.GetType)



                foreach (string[] ss in stringList)
                {
                    dr = dt.NewRow();
                    dr["Id"] = ss[0];
                    dr["Name"] = ss[1];
                    dr["Description"] = ss[2];
                    dr["Price"] = ss[3];
                    dr["Category"] = ss[4];
                    dr["Quantity to order"] = ss[5];

                    dt.Rows.Add(dr);
                }

                dt.AcceptChanges();
                GridView2.DataSource = dt;
                GridView2.DataBind();
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            sda_proj_srvc_rfrnc.UserSrvcSoapClient srvcSoapClient = new sda_proj_srvc_rfrnc.UserSrvcSoapClient();
            srvcSoapClient.u_add_to_shopping_cart(Login.uid
                , int.Parse(GridView1.SelectedRow.Cells[1].Text)
                , int.Parse(DropDownList1.SelectedItem.Value.ToString()));
            
        }

        protected void GridView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            sda_proj_srvc_rfrnc.UserSrvcSoapClient srvcSoapClient = new sda_proj_srvc_rfrnc.UserSrvcSoapClient();

            Label2.Visible = true;
            DropDownList2.Visible = true;

            DropDownList2.Items.Clear();

            Label2.Text = GridView2.SelectedRow.Cells[2].Text;

            int q = srvcSoapClient.u_get_quantity
                (int.Parse(GridView2.SelectedRow.Cells[1].Text));
            for (int i = 1; i <= q; i++)
                DropDownList2.Items.Add(i.ToString());
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            sda_proj_srvc_rfrnc.UserSrvcSoapClient srvcSoapClient = new sda_proj_srvc_rfrnc.UserSrvcSoapClient();
            int itid = int.Parse(GridView2.SelectedRow.Cells[1].Text);

            srvcSoapClient.u_delete_items_from_cart(Login.uid, itid);
            GridView2.DataBind();
        }

        protected void Button5_Click(object sender, EventArgs e)
        {
            sda_proj_srvc_rfrnc.UserSrvcSoapClient srvcSoapClient = new sda_proj_srvc_rfrnc.UserSrvcSoapClient();
            int itid = int.Parse(GridView2.SelectedRow.Cells[1].Text);
            int nq = int.Parse(DropDownList2.SelectedValue.ToString());
            srvcSoapClient.u_update_items_in_cart(Login.uid, itid,nq);
            GridView2.DataBind();
            
        }

        protected void Button6_Click(object sender, EventArgs e)
        {
            sda_proj_srvc_rfrnc.UserSrvcSoapClient srvcSoapClient = new sda_proj_srvc_rfrnc.UserSrvcSoapClient();
            
            srvcSoapClient.u_order(Login.uid, DropDownList3.SelectedValue.ToString());

            GridView1.DataBind();
            Label1.Visible = false;
            DropDownList1.Visible = false;
            Button1.Visible = false;
            Label3.Visible = false;
            Label5.Visible = false;
            GridView2.DataBind();
            Label2.Visible = false;
            Button4.Visible = false;
            Button5.Visible = false;
            DropDownList2.Visible = false;
            Label4.Visible = false;
            DropDownList3.Visible = false;
            Button6.Visible = false;
            DropDownList4.Visible = false;
        }

        protected void Button7_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/HomePage.aspx");
        }

        protected void DropDownList4_SelectedIndexChanged(object sender, EventArgs e)
        {
                sda_proj_srvc_rfrnc.UserSrvcSoapClient s = new sda_proj_srvc_rfrnc.UserSrvcSoapClient();
            sda_proj_srvc_rfrnc.ArrayOfString str;

            if (DropDownList4.SelectedIndex > 0)
                str = s.u_get_items_by_category(DropDownList4.SelectedValue.ToString());
            else
                str = s.View_Items();

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
    }
}