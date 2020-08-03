using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Callista_Cafe.Classes
{
    class MenuItm
    {
        public int item_id { get; set; }
        public string item_name { get; set; }
        public string item_category { get; set; }
        public float item_price { get; set; }

        private SqlConnection con;
        private SqlCommand cmd;
        public DataTable select()
        {
            DataTable itemDataTable = new DataTable() ;
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
                cmd = new SqlCommand("SELECT item_id,item_name,item_category,item_price FROM menu_items;", con);
                SqlDataAdapter itemDataAdapter = new SqlDataAdapter(cmd);
                con.Open();
                itemDataAdapter.Fill(itemDataTable);

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error");
            }
            finally
            {
                con.Close();
            }

            return itemDataTable;
        }

        public bool insert(MenuItm itm)
        {
            bool result = false;
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            try
            {
                cmd = new SqlCommand(
                    "INSERT INTO menu_items (item_name, item_price, item_category) VALUES(@name,@price,@category)",
                    con);
                cmd.Parameters.AddWithValue("@name", itm.item_name);
                cmd.Parameters.AddWithValue("@price", itm.item_price);
                cmd.Parameters.AddWithValue("@category", itm.item_category);
                con.Open();
                int rows = cmd.ExecuteNonQuery();

                if (rows > 0)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error");
            }
            finally
            {
                con.Close();
            }
            return result;
        }

        public bool update( MenuItm itm)
        {
            bool result = false;
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            try
            {
                cmd = new SqlCommand(
                    "UPDATE menu_items SET item_name=@name, item_price=@price, item_category=@category WHERE item_id=@id",
                    con);
                cmd.Parameters.AddWithValue("@id", itm.item_id);
                cmd.Parameters.AddWithValue("@name", itm.item_name);
                cmd.Parameters.AddWithValue("@price", itm.item_price);
                cmd.Parameters.AddWithValue("@category", itm.item_category);
                con.Open();
                int rows = cmd.ExecuteNonQuery();

                if (rows > 0)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error");
            }
            finally
            {
                con.Close();
            }
            return result;
        }

    }
}
