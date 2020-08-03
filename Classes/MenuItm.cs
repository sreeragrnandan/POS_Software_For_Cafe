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
        private SqlConnection con;
        private SqlCommand cmd;
        public DataTable select()
        {
            DataTable itemDataTable = new DataTable() ;
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
                cmd = new SqlCommand("SELECT item_id,item_name,item_price,item_category FROM menu_items;", con);
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
    }
}
