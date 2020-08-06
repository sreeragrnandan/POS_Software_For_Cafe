using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Activation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Callista_Cafe.Classes
{
    class Customer
    {
        public int c_id { get; set; }

        public string c_name { get; set; }
        public string c_mob { get; set; }
        public string c_dob { get; set; }
        public string c_email { get; set; }
        public string loc { get; set; }
        
        private SqlConnection con;
        private SqlCommand cmd;
        public DataTable select()
        {
            DataTable customerDataTable = new DataTable();
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
                cmd = new SqlCommand("SELECT * FROM customer;", con);
                SqlDataAdapter itemDataAdapter = new SqlDataAdapter(cmd);
                con.Open();
                itemDataAdapter.Fill(customerDataTable);

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error");
            }
            finally
            {
                con.Close();
            }

            return customerDataTable;
        }

        public bool insert(Customer customer)
        {
            bool result = false;
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            try
            {
                cmd = new SqlCommand(
                    "INSERT INTO customer (c_name, c_mobno, c_dob,c_email,c_location) VALUES(@name,@mob,@dob,@email,@loc)",
                    con);
                cmd.Parameters.AddWithValue("@name", customer.c_name);
                cmd.Parameters.AddWithValue("@mob", customer.c_mob);
                if (customer.c_dob.ToString().Equals(""))
                    cmd.Parameters.AddWithValue("@dob", DBNull.Value);
                else
                {
                    cmd.Parameters.AddWithValue("@dob", customer.c_dob);
                }
                cmd.Parameters.AddWithValue("@email", customer.c_email);
                cmd.Parameters.AddWithValue("@loc", customer.loc);
                con.Open();
                int rows = cmd.ExecuteNonQuery();

                if (rows > 0)
                {
                    result = true;
                }
                else
                {
                    MessageBox.Show("Failed to insert. Try Again !", "Error");
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

        public bool update(Customer customer)
        {
            bool result = false;
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            try
            {
                cmd = new SqlCommand(
                    "UPDATE customer SET c_name=@name , c_mobno=@mob , c_dob=@dob ,c_email=@email ,c_location=@loc WHERE c_id=@id;",
                    con);
                cmd.Parameters.AddWithValue("@id", customer.c_id);
                cmd.Parameters.AddWithValue("@name", customer.c_name);
                cmd.Parameters.AddWithValue("@mob", customer.c_mob);
                if (customer.c_dob.ToString().Equals(""))
                    cmd.Parameters.AddWithValue("@dob", DBNull.Value);
                else
                {
                    cmd.Parameters.AddWithValue("@dob", customer.c_dob);
                }
                cmd.Parameters.AddWithValue("@email", customer.c_email);
                cmd.Parameters.AddWithValue("@loc", customer.loc);
                con.Open();
                int rows = cmd.ExecuteNonQuery();

                if (rows > 0)
                {
                    result = true;
                }
                else
                {
                    MessageBox.Show("Failed to Update. Try Again !", "Error");
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

        public bool delete(Customer customer)
        {
            bool result = false;
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            try
            {
                cmd = new SqlCommand(
                    "DELETE FROM customer WHERE c_id=@id;",
                    con);
                cmd.Parameters.AddWithValue("@id", customer.c_id);
                con.Open();
                int rows = cmd.ExecuteNonQuery();

                if (rows > 0)
                {
                    result = true;
                }
                else
                {
                    MessageBox.Show("Failed to Delete. Try Again !", "Error");
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
