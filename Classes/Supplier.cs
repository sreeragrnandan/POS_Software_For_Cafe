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

        public bool insert_query()
        {
            bool result = false;
            return result;
        }

        public bool update_query()
        {
            bool result=false;
            return result;
        }

        public bool delete_query()
        {
            bool result=false;
            return result;
        }
    }
}
