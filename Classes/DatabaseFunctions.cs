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

        public int getCustomerId(String name)
        {
            int id = 0;
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
                con.Open();
                cmd = new SqlCommand("Select c_id from customer where c_name=@name", con);
                cmd.Parameters.AddWithValue("@name", name);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    id = int.Parse(reader["c_id"].ToString());
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

        public bool areyousure()
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure?", "Update", MessageBoxButton.YesNo);
            switch (messageBoxResult)
            {
                case MessageBoxResult.Yes:
                    return true;
                case MessageBoxResult.No:
                    return false;
            }

            return false;
        }

        public float getbilltotal(int bill_id)
        {
            float total = 0;
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
                con.Open();
                cmd = new SqlCommand("SELECT SUM(item_total) as total FROM detailed_bill WHERE bill_id=@id", con);
                cmd.Parameters.AddWithValue("@id", bill_id);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    total = float.Parse(reader["total"].ToString());
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

            return total;
        }

        public float getbilltotalitems(int bill_id)
        {
            float total = 0;
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
                con.Open();
                cmd = new SqlCommand("SELECT COUNT(item_id) as total FROM detailed_bill WHERE bill_id=@id", con);
                cmd.Parameters.AddWithValue("@id", bill_id);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    total = float.Parse(reader["total"].ToString());
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

            return total;
        }

    }
}
