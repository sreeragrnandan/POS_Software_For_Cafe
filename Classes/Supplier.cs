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
    class Supplier
    {
        public int supplier_id { get; set; }
        public string supplier_name { get; set; }
        public string supplier_mobile { get; set; }

        private SqlConnection con;
        private readonly String ConString = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
        private SqlCommand cmd;
        public DataTable select_query()
        {
            DataTable dt = new DataTable();
            try
            {
                con = new SqlConnection(ConString);
                con.Open();
                cmd = new SqlCommand("Select * from suppliers", con);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                adapter.Fill(dt);
                adapter.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return dt;
        }

        public bool insert_query(Supplier sup)
        {
            bool result = false;

            try
            {
                con = new SqlConnection(ConString);
                cmd = new SqlCommand("Insert into suppliers(supplier_name,supplier_mobile) values(@sup_name,@sup_mobile)",con);
                cmd.Parameters.AddWithValue("@sup_name", sup.supplier_name);
                cmd.Parameters.AddWithValue("@sup_mobile", sup.supplier_mobile);

                con.Open();
                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    result = true;
                }
                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return result;
        }

        public bool update_query(Supplier sup)
        {
            bool result=false;
            try
            {
                con=new SqlConnection(ConString);
                cmd = new SqlCommand("Update Suppliers set supplier_name=@sup_name,supplier_mobile=@sup_mobile WHERE supplier_id=@sup_id",con);
                cmd.Parameters.AddWithValue("@sup_id", sup.supplier_id);
                cmd.Parameters.AddWithValue("@sup_name", sup.supplier_name);
                cmd.Parameters.AddWithValue("@sup_mobile", sup.supplier_mobile);

                con.Open();
                int rows = cmd.ExecuteNonQuery();

                if (rows > 0)
                {
                    result = true;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally{con.Close();}
            return result;
        }

        public bool delete_query(Supplier sup)
        {
            bool result=false;
            con = new SqlConnection(ConString);
            try
            {
                cmd = new SqlCommand("Delete from dbo.suppliers where supplier_id=@sup_id",con);
                cmd.Parameters.AddWithValue("@sup_id", sup.supplier_id);
                con.Open();
                int rows = cmd.ExecuteNonQuery();
                if (rows>0)
                {
                    result = true;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }finally{con.Close();}
            return result;
        }
    }
}
