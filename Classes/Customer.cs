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
        //getter and setter (Data Carriers)
        public int cus_id { get; set; }
        
        public string cus_name { get; set; }

        public string cus_location { get; set; }
    
        public string cus_mobno { get; set; }

        public string cus_email { get; set; }

        static string myConString = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;

        //Select Functions
        public DataTable select()
        {
            SqlConnection conn = new SqlConnection(myConString);
            DataTable dt = new DataTable();
            try 
            {
                string com = "SELECT * FROM customer";
                SqlCommand cmd = new SqlCommand(com,conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                conn.Open();
                adapter.Fill(dt);

            }
            catch (Exception ex){ }

            finally
            {
                conn.Close();
            }
            return dt;
        }

        public bool insert(Customer c)
        {
            bool isSuccess = false;
            SqlConnection conn = new SqlConnection(myConString);

            try
            {
                string com = "INSERT INTO customer(c_name,c_location,c_mobno,c_email) VALUES (@cname,@clocation,@cmobno,@cemail)";
                SqlCommand cmd = new SqlCommand(com, conn);
                cmd.Parameters.AddWithValue("@cname",c.cus_name);
                cmd.Parameters.AddWithValue("@clocation", c.cus_location);
                cmd.Parameters.AddWithValue("@cmobno", c.cus_mobno);
                cmd.Parameters.AddWithValue("@cemail", c.cus_email);


                conn.Open();
                int rows = cmd.ExecuteNonQuery();

                if (rows > 0)
                {
                    isSuccess = true;
                }
                else
                {
                    isSuccess = false;
                }


            }
            
            catch (Exception ex) { }

            finally
            {
                conn.Close();
            }

            return isSuccess;
        }


        public bool update(Customer c)
        {
            bool isSuccess = false;
            SqlConnection conn = new SqlConnection(myConString);

            try
            {
                string com = "UPDATE customer SET c_name=@cname, c_location=@clocation,c_mobno=@cmobno,c_email=@cemail WHERE c_id=@cid";
                SqlCommand cmd = new SqlCommand(com, conn);
                cmd.Parameters.AddWithValue("@cname", c.cus_name);
                cmd.Parameters.AddWithValue("@clocation", c.cus_location);
                cmd.Parameters.AddWithValue("@cmobno", c.cus_mobno);
                cmd.Parameters.AddWithValue("@cemail", c.cus_email);
                cmd.Parameters.AddWithValue("@cid", c.cus_id);

                conn.Open();
                int rows = cmd.ExecuteNonQuery();

                if (rows > 0)
                {
                    isSuccess = true;
                }
                else
                {
                    isSuccess = false;
                }


            }

            catch (Exception ex) { }

            finally
            {
                conn.Close();
            }

            return isSuccess;
        }
        public bool delete(Customer c)
        {
            bool isSuccess = false;
            SqlConnection conn = new SqlConnection(myConString);

            try
            {
                string com = "DELETE FROM customer WHERE c_id=@cid";
                SqlCommand cmd = new SqlCommand(com, conn);
                cmd.Parameters.AddWithValue("@cid", c.cus_id);

                conn.Open();
                int rows = cmd.ExecuteNonQuery();

                if (rows > 0)
                {
                    isSuccess = true;
                }
                else
                {
                    isSuccess = false;
                }


            }

            catch (Exception ex) { }

            finally
            {
                conn.Close();
            }

            return isSuccess;
        }

    }
}
