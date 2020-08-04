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
        public string item_dec { get; set; }

        private SqlConnection con;
        private SqlCommand cmd;
        public DataTable select()
        {
            DataTable itemDataTable = new DataTable() ;
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
                cmd = new SqlCommand("SELECT item_id, item_name, item_category, item_price FROM menu_items;", con);
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

        public bool delete(MenuItm itm)
        {
            bool del_result = false;
            bool update_result = false;
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            try
            {
                con.Open();
                cmd = new SqlCommand("SELECT * FROM menu_items WHERE item_id=@Id", con);
                cmd.Parameters.AddWithValue("@Id", itm.item_id);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    itm.item_name = reader["item_name"].ToString();
                    itm.item_price = float.Parse(reader["item_price"].ToString());
                    itm.item_category = reader["item_category"].ToString();
                    itm.item_dec = reader["description"].ToString();
                    con.Close();
                    con.Open();
                    cmd = new SqlCommand(
                        "INSERT INTO deleted_menu_items (del_item_id, del_item_name, del_item_price, del_item_category, del_item_description) VALUES(@id,@name,@price,@cate,@desc); ",
                        con);
                    cmd.Parameters.AddWithValue("@id", itm.item_id);
                    cmd.Parameters.AddWithValue("@name", itm.item_name);
                    cmd.Parameters.AddWithValue("@price", itm.item_price);
                    cmd.Parameters.AddWithValue("@cate", itm.item_category);
                    cmd.Parameters.AddWithValue("@desc", itm.item_dec);
                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        update_result = true;
                    }
                    else
                    {
                        update_result = false;
                    }
                    con.Close();
                }

                if (update_result)
                {
                    cmd = new SqlCommand(
                        "DELETE FROM menu_items WHERE item_id=@id",
                        con);
                    cmd.Parameters.AddWithValue("@id", itm.item_id);
                    con.Open();
                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        del_result = true;
                    }
                    else
                    {
                        del_result = false;
                    }
                    con.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error");
            }
            finally
            { }
            return del_result;
        }

    }
}
