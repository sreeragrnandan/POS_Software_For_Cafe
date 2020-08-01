using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;

namespace Callista_Cafe.Classes
{
    class InventoryItem
    {
        /*public int id { get; set; }
        public string ingredient { get; set; }
        public float price { get; set; }
        public float quantity { get; set; }
        public DateTime e_date { get; set; }
        public string unit { get; set; }
        public float min_quantity { get; set; }
        public string supplier_name { get; set; }*/

        private SqlConnection con;
        private SqlCommand cmd;
        public DataTable select()
        {
            DataTable dt = new DataTable();
            try
            {
                /*select id ingredient,price,qualtity,e_date,unit,min_quantity, supplier_name from inventory as inv, suppliers as sup where sup.supplier_id=inv.supplier_id*/
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
                cmd = new SqlCommand("select id, ingredient,price,quantity,e_date,unit,min_quantity, supplier_name from inventory as inv, suppliers as sup where sup.supplier_id=inv.supplier_id", con);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                con.Open();
                adapter.Fill(dt);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Info");
            }
            finally
            {
                con.Close();
            }

            return dt;
        }
    }
}
