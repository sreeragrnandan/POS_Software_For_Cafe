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
    class ItemIngredientRequirement
    {
        private SqlConnection con;
        private SqlConnection conn;
        private SqlCommand cmd;
        private SqlCommand cmdd;


        public int menuItemId { get; set; }
        public int ingredientId { get; set; }
        public float ingredientQuantity { get; set; }

        public DataTable ReqDataTable(int menuItemId)
        {
            DataTable ReqDataTable = new DataTable();
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
                cmd = new SqlCommand("SELECT inv.ingredient, ing.quantity, inv.unit FROM inventory AS inv, ingredient_list AS ing WHERE ing.menu_item_id=@menuItemId and inv.id=ing.ingredient_id;", con);
                cmd.Parameters.AddWithValue("@menuItemId", menuItemId);
                SqlDataAdapter Adapter = new SqlDataAdapter(cmd);
                con.Open();
                Adapter.Fill(ReqDataTable);

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error");
            }
            finally
            {
                con.Close();
            }

            return ReqDataTable;
        }

        public DataTable InventoryDataTable()
        {
            DataTable invDataTable = new DataTable();
            try
            {
                conn = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
                cmdd = new SqlCommand("SELECT ingredient, unit FROM inventory;", conn);
                SqlDataAdapter InvAdapter = new SqlDataAdapter(cmdd);
                conn.Open();
                InvAdapter.Fill(invDataTable);

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error");
            }
            finally
            {
                conn.Close();
            }

            return invDataTable;
        }
    }
}
