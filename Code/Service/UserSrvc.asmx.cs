using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using System.Data.SqlClient;

namespace SDA_Project_Online_Shopping_Service
{
    /// <summary>
    /// Summary description for UserSrvc
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class UserSrvc : System.Web.Services.WebService
    {


        string connection_string =
             "Data Source=.\\SQLEXPRESS;Initial Catalog=Online Shopping Service;Integrated Security=True";

        [WebMethod]
        public string[] u_get_categories() {
            SqlConnection sc = new SqlConnection(connection_string);
            sc.Open();
            SqlCommand cmd = new SqlCommand("select category from items", sc);
            SqlDataReader r = cmd.ExecuteReader();
            List<string> l = new List<string>();
            
            if (r.HasRows) {
                while (r.Read()) {
                    l.Add(r[0].ToString());
                }
            }
            r.Close();
            string[] s = new string[l.Count];
            for(int i = 0; i < l.Count; i++)
            {
                s[i] = l.ElementAt(i).ToString();
            }
            sc.Close();

            return s;
        }

        [WebMethod]
        public string[] u_get_items_by_category(string c) {
            SqlConnection sc = new SqlConnection(connection_string);
            SqlCommand cmd = new SqlCommand("select * from items where category = @c", sc);
            sc.Open();
            cmd.Parameters.Add(new SqlParameter("@c", c));

            SqlDataReader r = cmd.ExecuteReader();
            List<List<string>> l = new List<List<string>>();

            if (r.HasRows)
            {
                while (r.Read())
                {
                    List<string> ll = new List<string>();

                    ll.Add(r[0].ToString());
                    ll.Add(r[1].ToString());
                    ll.Add(r[2].ToString());
                    ll.Add(r[3].ToString());
                    ll.Add(r[4].ToString());
                    ll.Add(r[5].ToString());

                    l.Add(ll);
                }
            }
            r.Close();

            string[] mystr = new string[l.Count];

            for (int i = 0; i < l.Count; i++)
            {
                mystr[i] = "";
                mystr[i] += l.ElementAt(i).ElementAt(0).ToString();
                mystr[i] += "," + l.ElementAt(i).ElementAt(1).ToString();
                mystr[i] += "," + l.ElementAt(i).ElementAt(2).ToString();
                mystr[i] += "," + l.ElementAt(i).ElementAt(3).ToString();
                mystr[i] += "," + l.ElementAt(i).ElementAt(4).ToString();
                mystr[i] += "," + l.ElementAt(i).ElementAt(5).ToString();

            }
            sc.Close();
            return mystr;

        }

        [WebMethod]
        public string u_update_items_in_cart(string uid, int itid, int q)
        {


            string ret;
            SqlConnection sc
                = new SqlConnection
                (connection_string);

            sc.Open();

            SqlCommand cmd = new SqlCommand("select id from Shopping_Carts where UserID=@x;", sc);
            SqlParameter sp1 = new SqlParameter("@x", uid);
            cmd.Parameters.Add(sp1);
            SqlDataReader r = cmd.ExecuteReader();
            int shcid = -1;
            if (r.HasRows)
            {
                int c = 0;
                while (r.Read())
                {
                    c = Convert.ToInt32(r[0].ToString());


                }
                shcid = c;
                r.Close();

                cmd = new SqlCommand("update Shopping_Carts_Items set item_quantity=@y where cartid=@z and itemid=@i;", sc);
                SqlParameter sp2 = new SqlParameter("@y", q);
                SqlParameter sp3 = new SqlParameter("@z", shcid);
                SqlParameter sp4 = new SqlParameter("@i", itid);
                cmd.Parameters.Add(sp2); cmd.Parameters.Add(sp3); cmd.Parameters.Add(sp4);
                cmd.ExecuteNonQuery();
                ret = "ok";
            }
            else
            {
                ret = "no";
            }
            sc.Close();
            return ret;
        }

        [WebMethod]
        public string u_delete_items_from_cart(string uid, int itid)
        {


            string ret;
            SqlConnection sc
                = new SqlConnection
                (connection_string);

            sc.Open();

            SqlCommand cmd = new SqlCommand("select id from Shopping_Carts where UserID=@x;", sc);
            SqlParameter sp1 = new SqlParameter("@x", uid);
            cmd.Parameters.Add(sp1);
            SqlDataReader r = cmd.ExecuteReader();
            int shcid = -1;
            if (r.HasRows)
            {
                int c = 0;
                while (r.Read())
                {
                    c = Convert.ToInt32(r[0].ToString());
                }
                r.Close();
                shcid = c;

                cmd = new SqlCommand("delete from Shopping_Carts_Items where cartid=@z and itemid=@i;", sc);

                SqlParameter sp3 = new SqlParameter("@z", shcid);
                SqlParameter sp4 = new SqlParameter("@i", itid);
                cmd.Parameters.Add(sp3); cmd.Parameters.Add(sp4);
                cmd.ExecuteNonQuery();
                ret = "ok";
            }
            else
            {
                ret = "no";
            }
            sc.Close();
            return ret;
        }

        [WebMethod]
        public string a_add_items(string n, string d, float p, string c, int q)
        {
            SqlConnection con = new SqlConnection(connection_string);
            con.Open();
            SqlCommand cmd = new SqlCommand("insert into items values(@a,@b,@c,@d,@e)", con);
            SqlParameter p1 = new SqlParameter("@a", n);
            SqlParameter p2 = new SqlParameter("@b", d);
            SqlParameter p3 = new SqlParameter("@c", p);
            SqlParameter p4 = new SqlParameter("@d", c);
            SqlParameter p5 = new SqlParameter("@e", q);
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            cmd.Parameters.Add(p3);
            cmd.Parameters.Add(p4);
            cmd.Parameters.Add(p5);

            cmd.ExecuteNonQuery();

            con.Close();
            return "ok";

        }
        
        [WebMethod]
        public string u_Register(string name, string address, string area, string mob, string username, string password)
        {
            SqlConnection con = new SqlConnection(connection_string);

            con.Open();

            SqlCommand check = new SqlCommand("select * from Users where username = @reg_user_name",con);
            check.Parameters.Add(new SqlParameter("@reg_user_name", username));
            SqlDataReader r_check = check.ExecuteReader();
            if (r_check.Read())
            {
                r_check.Close();
                return "User Name Already Exists";
            }
            r_check.Close();

            SqlCommand cmd = new SqlCommand("insert into Users(name,address,area,mobile_no,username,password) values(@a,@b,@c,@d,@e,@f)", con);
            SqlParameter p1 = new SqlParameter("@a", name);
            SqlParameter p2 = new SqlParameter("@b", address);
            SqlParameter p3 = new SqlParameter("@c", area);
            SqlParameter p4 = new SqlParameter("@d", mob);
            SqlParameter p5 = new SqlParameter("@e", username);
            SqlParameter p6 = new SqlParameter("@f", password);
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            cmd.Parameters.Add(p3);
            cmd.Parameters.Add(p4);
            cmd.Parameters.Add(p5);
            cmd.Parameters.Add(p6);
            cmd.ExecuteNonQuery();
            con.Close();

            return "Finished Successfuly";
        }

        [WebMethod]
        public string Login(string username, string password, char aoru)
        {
            SqlConnection con = new SqlConnection(connection_string);

            con.Open();
            if (aoru == 'u')
            {
                SqlCommand cmd = new SqlCommand("select * from Users where username=@a AND password=@b", con);
                SqlParameter p1 = new SqlParameter("@a", username);
                SqlParameter p2 = new SqlParameter("@b", password);
                cmd.Parameters.Add(p1);
                cmd.Parameters.Add(p2);

                SqlDataReader r = cmd.ExecuteReader();
                if (!r.Read())
                {
                    r.Close();
                    con.Close();
                    return "Invalid Data...!";
                }
                else
                {
                    r.Close();
                    con.Close();
                    return "Logged in Successfully!";
                }
            }
            else
            {
                SqlCommand cmd = new SqlCommand("select * from Admins where username=@a AND password=@b", con);
                SqlParameter p1 = new SqlParameter("@a", username);
                SqlParameter p2 = new SqlParameter("@b", password);
                cmd.Parameters.Add(p1);
                cmd.Parameters.Add(p2);

                SqlDataReader r = cmd.ExecuteReader();
                if (!r.Read())
                {
                    r.Close();
                    con.Close();
                    return "Invalid Data...!";
                }
                else
                {
                    r.Close();
                    con.Close();
                    return "Logged in Successfully!";
                }
            }
        }

        [WebMethod]
        public string[] View_Items()
        {
            SqlConnection con = new SqlConnection(connection_string);
            con.Open();
            SqlCommand cmd = new SqlCommand("select * from  Items ", con);
            SqlDataReader r = cmd.ExecuteReader();
            List<List<string>> myList = new List<List<string>>();
            while (r.Read())
            {
                List<string> l = new List<string>();
                l.Add(r["id"].ToString());
                l.Add(r["name"].ToString());
                l.Add(r["description"].ToString());
                l.Add(r["price"].ToString());
                l.Add(r["category"].ToString());
                l.Add(r["quantity_in_stock"].ToString());

                myList.Add(l);

            }
            string[] mystr = new string[myList.Count];
            
            for (int i = 0; i < myList.Count; i++) {
                mystr[i] = "";
                mystr[i] +=  myList.ElementAt(i).ElementAt(0).ToString();
                mystr[i] += "," + myList.ElementAt(i).ElementAt(1).ToString();
                mystr[i] += "," + myList.ElementAt(i).ElementAt(2).ToString();
                mystr[i] += "," + myList.ElementAt(i).ElementAt(3).ToString();
                mystr[i] += "," + myList.ElementAt(i).ElementAt(4).ToString();
                mystr[i] += "," + myList.ElementAt(i).ElementAt(5).ToString();

            }

            
            con.Close();
            r.Close();
            return mystr;
        }
        
        //helper
        public int get_last_unordered_cartid_or_create_new(string user_name)
        {
            //return "start";
            SqlConnection con = new SqlConnection(connection_string);
            con.Open();

            bool create_new_cart = false;

            SqlCommand cmd = new SqlCommand("SELECT max(id) FROM Shopping_Carts where Shopping_Carts.UserID = @uid", con);
            SqlParameter sp = new SqlParameter("@uid", user_name);
            cmd.Parameters.Add(sp);
            SqlDataReader r = cmd.ExecuteReader();

            if (r.Read())
            {
                string temp = r[0].ToString();
                if (temp != "") { 
                    r.Close();
                    int last_id = int.Parse(temp);
                    SqlCommand check = new SqlCommand("select * from Orders where Orders.cartID = @cid", con);
                    check.Parameters.Add(new SqlParameter("@cid", last_id));
                    SqlDataReader rx = check.ExecuteReader();
                    if (rx.HasRows)
                    {
                        create_new_cart = true; // last cart is in orders
                    }
                    rx.Close();
                }
                else
                {

                    create_new_cart = true;

                }
            }
            
            r.Close();

            if (create_new_cart)
            {
                SqlCommand cmd2 = new SqlCommand("insert into Shopping_Carts(UserID) values(@uid)", con);
                cmd2.Parameters.Add(new SqlParameter("@uid", user_name));
                cmd2.ExecuteNonQuery();
            }

            r = cmd.ExecuteReader();
            if (!r.Read())
            {
                throw new Exception("Fatal error..Shopping carts' table is empty");
            }
            int last_cart_id = int.Parse(r[0].ToString());
            r.Close();
            
            cmd.ExecuteNonQuery();

            con.Close();
            return last_cart_id;
        }
        
        [WebMethod]
        public string u_add_to_shopping_cart(string user_name, int item_id, int item_quantity)
        {
            //return "start";
            SqlConnection con = new SqlConnection(connection_string);
            con.Open();

            bool create_new_cart = false;

            SqlCommand cmd = new SqlCommand("SELECT max(id) FROM Shopping_Carts where Shopping_Carts.UserID = @uid", con);
            SqlParameter sp = new SqlParameter("@uid", user_name);
            cmd.Parameters.Add(sp);
            SqlDataReader r = cmd.ExecuteReader();

            if (r.Read())
            {
                string temp = r[0].ToString();
                if (temp != "")
                {
                    r.Close();
                    int last_id = int.Parse(temp);
                    SqlCommand check = new SqlCommand("select * from Orders where Orders.cartID = @cid", con);
                    check.Parameters.Add(new SqlParameter("@cid", last_id));
                    SqlDataReader rx = check.ExecuteReader();
                    if (rx.HasRows)
                    {
                        create_new_cart = true; // last cart is in orders
                    }
                    rx.Close();
                }
                else
                {

                    create_new_cart = true;

                }
            }

            r.Close();

            if (create_new_cart)
            {
                SqlCommand cmd2 = new SqlCommand("insert into Shopping_Carts(UserID) values(@uid)", con);
                cmd2.Parameters.Add(new SqlParameter("@uid", user_name));
                cmd2.ExecuteNonQuery();
            }
            //cmd = new SqlCommand("select * from shopping_carts where id = (select max(id) from shopping_carts)", con);
            r = cmd.ExecuteReader();
            if (!r.Read())
            {
                throw new Exception("Fatal error..Shopping carts' table is empty");
            }
            int last_cart_id = int.Parse(r[0].ToString());
            r.Close();

            cmd = new SqlCommand("insert into Shopping_Carts_Items(cartID, itemID, item_quantity) values(@cid, @itID, @itQ)", con);
            cmd.Parameters.Add(new SqlParameter("@cid", last_cart_id));
            cmd.Parameters.Add(new SqlParameter("@itID", item_id));
            cmd.Parameters.Add(new SqlParameter("@itQ", item_quantity));
            cmd.ExecuteNonQuery();

            con.Close();
            return last_cart_id.ToString();
        }

        [WebMethod]
        public void u_order(string uid, string pmnttp)
        {
            int cid = get_last_unordered_cartid_or_create_new(uid);

            SqlConnection con = new SqlConnection(connection_string);
            con.Open();
            SqlCommand cmd = new SqlCommand("select itemid,item_quantity from shopping_carts_items where cartid=@a  ", con);
            SqlParameter p1 = new SqlParameter("@a", cid);
            cmd.Parameters.Add(p1);

            SqlDataReader r = cmd.ExecuteReader();
            List<Tuple<int, int>> i_q = new List<Tuple<int,int>>();

            if (r.HasRows) {
                while (r.Read()) {
                    int itid = int.Parse(r[0].ToString());
                    int q = int.Parse(r[1].ToString());
                    i_q.Add(new Tuple<int, int>(itid, q));
                    
                }

            }
            r.Close();

            for (int i = 0; i < i_q.Count; i++) {
                int itid = i_q[i].Item1;
                int q = i_q[i].Item2;

                SqlCommand cmd2 = new SqlCommand("update items set quantity_in_stock=quantity_in_stock-@q where id=@itid; ", con);
                SqlParameter p2 = new SqlParameter("@q", q);
                SqlParameter p3 = new SqlParameter("@itid", itid);
                cmd2.Parameters.Add(p2);
                cmd2.Parameters.Add(p3);

                cmd2.ExecuteNonQuery();
            }
            
            SqlCommand cmd3 = new SqlCommand("insert into orders values (@c,@p)", con);
            cmd3.Parameters.Add(new SqlParameter("@c",cid));
            cmd3.Parameters.Add(new SqlParameter("@p", pmnttp));
            cmd3.ExecuteNonQuery();

            con.Close();
        }

        [WebMethod]
        public string[] u_View_Cart(string uid)
        {

            int cartid = get_last_unordered_cartid_or_create_new(uid);

            SqlConnection con = new SqlConnection(connection_string);
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT Shopping_Carts_Items.cartid, Shopping_Carts_Items.itemid ,Items.name ,Items.description , Items.price, Items.category, Shopping_Carts_Items.item_quantity FROM Items,Shopping_Carts_Items WHERE Shopping_Carts_Items.itemid = Items.id AND  Shopping_Carts_Items.cartid =  @a  ", con);
            SqlParameter p1 = new SqlParameter("@a", cartid);
            cmd.Parameters.Add(p1);

            SqlDataReader r = cmd.ExecuteReader();

            List<List<string>> myList = new List<List<string>>();
            while (r.Read())
            {
                List<string> l = new List<string>();
                //l.Add(r["cartid"].ToString());
                l.Add(r["itemid"].ToString());
                l.Add(r["name"].ToString());
                l.Add(r["description"].ToString());
                l.Add(r["price"].ToString());
                l.Add(r["category"].ToString());
                l.Add(r["item_quantity"].ToString());

                myList.Add(l);
            }
            string[] mystr = new string[myList.Count];

            for (int i = 0; i < myList.Count; i++)
            {
                mystr[i] = "";
                mystr[i] += myList.ElementAt(i).ElementAt(0).ToString();
                mystr[i] += "," + myList.ElementAt(i).ElementAt(1).ToString();
                mystr[i] += "," + myList.ElementAt(i).ElementAt(2).ToString();
                mystr[i] += "," + myList.ElementAt(i).ElementAt(3).ToString();
                mystr[i] += "," + myList.ElementAt(i).ElementAt(4).ToString();
                mystr[i] += "," + myList.ElementAt(i).ElementAt(5).ToString();

            }
            r.Close();
            con.Close();

            

            return mystr;
        }

        [WebMethod]
        public int u_get_quantity(int itid) {

            SqlConnection con = new SqlConnection(connection_string);
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT quantity_in_stock FROM Items WHERE id =  @a  ", con);
            SqlParameter p1 = new SqlParameter("@a", itid);
            cmd.Parameters.Add(p1);

            SqlDataReader r = cmd.ExecuteReader();

            List<List<string>> myList = new List<List<string>>();
            int q=-1;
            if (r.Read())
            {
                q = int.Parse(r[0].ToString());
            }
            r.Close();
            con.Close();



            return q;
        }

        [WebMethod]
        public string a_UpdateName(int id, string name)
        {
            SqlConnection conn = new SqlConnection(connection_string);
            conn.Open();
            SqlCommand cmd = new SqlCommand("select * from Items where id=@b", conn);
            SqlCommand com = new SqlCommand("update Items set name=@a where id=@e", conn);
            SqlParameter p1 = new SqlParameter("@a", name);
            SqlParameter p5 = new SqlParameter("@e", id);
            SqlParameter p3 = new SqlParameter("@b", id);
            com.Parameters.Add(p1);
            com.Parameters.Add(p5);
            cmd.Parameters.Add(p3);

            com.ExecuteNonQuery();

            SqlDataReader r = cmd.ExecuteReader();
            if (!r.Read())
            {
                r.Close();
                conn.Close();
                return "Invalid id...!";
            }
            else
            {
                r.Close();
                conn.Close();
                return "ITEM UPDATED Successfully!";
            }

        }

        [WebMethod]
        public string a_UpdateDescription(int id, string desc)
        {
            SqlConnection conn = new SqlConnection(connection_string);
            conn.Open();
            SqlCommand cmd = new SqlCommand("select * from Items where id=@b", conn);
            SqlCommand com = new SqlCommand("update Items set description=@a where id=@e", conn);
            SqlParameter p1 = new SqlParameter("@a", desc);
            SqlParameter p2 = new SqlParameter("@e", id);
            SqlParameter p3 = new SqlParameter("@b", id);
            com.Parameters.Add(p1);
            com.Parameters.Add(p2);
            cmd.Parameters.Add(p3);

            com.ExecuteNonQuery();

            SqlDataReader r = cmd.ExecuteReader();
            if (!r.Read())
            {
                r.Close();
                conn.Close();
                return "Invalid id...!";
            }
            else
            {
                r.Close();
                conn.Close();
                return "ITEM UPDATED Successfully!";
            }
        }

        [WebMethod]
        public string a_UpdatePrice(int id, float price)
        {
            SqlConnection conn = new SqlConnection(connection_string);
            conn.Open();
            SqlCommand cmd = new SqlCommand("select * from Items where id=@b", conn);
            SqlCommand com = new SqlCommand("update Items set price=@a where id=@e", conn);
            SqlParameter p1 = new SqlParameter("@a", price);
            SqlParameter p2 = new SqlParameter("@e", id);
            SqlParameter p3 = new SqlParameter("@b", id);
            com.Parameters.Add(p1);
            com.Parameters.Add(p2);
            cmd.Parameters.Add(p3);

            com.ExecuteNonQuery();

            SqlDataReader r = cmd.ExecuteReader();
            if (!r.Read())
            {
                r.Close();
                conn.Close();
                return "Invalid id...!";
            }
            else
            {
                r.Close();
                conn.Close();
                return "ITEM UPDATED Successfully!";
            }
        }

        [WebMethod]
        public string a_UpdateCategory(int id, string catg)
        {
            SqlConnection conn = new SqlConnection(connection_string);
            conn.Open();
            SqlCommand cmd = new SqlCommand("select * from Items where id=@b", conn);
            SqlCommand com = new SqlCommand("update Items set category=@a where id=@e", conn);
            SqlParameter p1 = new SqlParameter("@a", catg);
            SqlParameter p2 = new SqlParameter("@e", id);
            SqlParameter p3 = new SqlParameter("@b", id);
            com.Parameters.Add(p1);
            com.Parameters.Add(p2);
            cmd.Parameters.Add(p3);

            com.ExecuteNonQuery();

            SqlDataReader r = cmd.ExecuteReader();
            if (!r.Read())
            {
                r.Close();
                conn.Close();
                return "Invalid id...!";
            }
            else
            {
                r.Close();
                conn.Close();
                return "ITEM UPDATED Successfully!";
            }
        }
        
        [WebMethod]
        public string a_UpdateQuantity(int id, int q)
        {
            SqlConnection conn = new SqlConnection(connection_string);
            conn.Open();
            SqlCommand cmd = new SqlCommand("select * from Items where id=@b", conn);
            SqlCommand com = new SqlCommand("update Items set quantity_in_stock=@a where id=@e", conn);
            SqlParameter p1 = new SqlParameter("@a", q);
            SqlParameter p2 = new SqlParameter("@e", id);
            SqlParameter p3 = new SqlParameter("@b", id);
            com.Parameters.Add(p1);
            com.Parameters.Add(p2);
            cmd.Parameters.Add(p3);

            com.ExecuteNonQuery();

            SqlDataReader r = cmd.ExecuteReader();
            if (!r.Read())
            {
                r.Close();
                conn.Close();
                return "Invalid id...!";
            }
            else
            {
                r.Close();
                conn.Close();
                return "ITEM UPDATED Successfully!";
            }
        }
        
        [WebMethod]
        public string a_Delete_Items(int id)
        {
            SqlConnection conn = new SqlConnection(connection_string);
            conn.Open();
            SqlCommand cmd = new SqlCommand("select * from Items where id=@a", conn);
            SqlCommand com = new SqlCommand("delete from Items where id=@e", conn);
            SqlCommand com1 = new SqlCommand("delete from Shopping_Carts_Items where itemid=@b", conn);
            SqlParameter p2 = new SqlParameter("@a", id);
            SqlParameter p1 = new SqlParameter("@e", id);
            SqlParameter p3 = new SqlParameter("@b", id);
            cmd.Parameters.Add(p2);
            com.Parameters.Add(p1);
            com1.Parameters.Add(p3);
            com1.ExecuteNonQuery();
            SqlDataReader r = cmd.ExecuteReader();
            if (!r.Read())
            {
                r.Close();
                com.ExecuteNonQuery();
                conn.Close();
                return "Invalid id...!";
            }
            else
            {
                r.Close();
                com.ExecuteNonQuery();
                conn.Close();
                return "ITEM DELETED Successfully!";
            }
        }



    }
}
