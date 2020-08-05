using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;

namespace Callista_Cafe.Classes
{
    class DatabaseFunctions
    {
        private SqlCommand cmd;
        private SqlConnection con;
        private SqlDataReader reader;
        public int getIngredientId(String name)
        {
            int id=0;
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
                con.Open();
                cmd = new SqlCommand("Select id from inventory where ingredient=@name", con);
                cmd.Parameters.AddWithValue("@name", name);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    id = int.Parse(reader["id"].ToString());
                }
                else
                {
                    MessageBox.Show("Something went worng. Please try again !", "Error");
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

            return id;
        }

        public string getMenuItemName(int id)
        {
            string name = null;
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
                con.Open();
                cmd = new SqlCommand("Select item_name from menu_items where item_id=@id", con);
                cmd.Parameters.AddWithValue("@id", id);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    name = reader["item_name"].ToString();
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

            return name;
        }

    }
}
