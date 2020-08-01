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
        public int id { get; set; }
        public string ingredient { get; set; }
        public string price { get; set; }
        public string quantity { get; set; }
        public string e_date { get; set; }
        public string unit { get; set; }
        public string min_quantity { get; set; }
        public string supplier_name { get; set; }

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

        public bool insert(InventoryItem itm, bool date)
        {
            bool result = false;
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            try
            {
                if (date)
                {
                    cmd = new SqlCommand(
                        "INSERT INTO inventory (ingredient,price,quantity,e_date,unit,min_quantity,supplier_id) VALUES(@name,@price,@qty,@edate,@unit,@minqty,@supplierid)",
                        con);
                    cmd.Parameters.AddWithValue("@name", itm.ingredient);
                    cmd.Parameters.AddWithValue("@price", itm.price);
                    cmd.Parameters.AddWithValue("@qty", itm.quantity);
                    cmd.Parameters.AddWithValue("@edate", itm.e_date);
                    cmd.Parameters.AddWithValue("@unit", itm.unit);
                    cmd.Parameters.AddWithValue("@minqty", itm.min_quantity);
                    cmd.Parameters.AddWithValue("@supplierid", itm.supplier_name);
                }
                else
                {
                    cmd = new SqlCommand(
                        "INSERT INTO inventory (ingredient,price,quantity,unit,min_quantity,supplier_id) VALUES(@name,@price,@qty,@unit,@minqty,@supplierid)",
                        con);
                    cmd.Parameters.AddWithValue("@name", itm.ingredient);
                    cmd.Parameters.AddWithValue("@price", itm.price);
                    cmd.Parameters.AddWithValue("@qty", itm.quantity);
                    cmd.Parameters.AddWithValue("@unit", itm.unit);
                    cmd.Parameters.AddWithValue("@minqty", itm.min_quantity);
                    cmd.Parameters.AddWithValue("@supplierid", itm.supplier_name);
                }

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
                MessageBox.Show(e.ToString(), "Info");
            }
            finally
            {
                con.Close();
            }
            return result;
        }


        public bool update(InventoryItem itm, bool date)
        {
            bool result = false;
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            try
            {
                if (date)
                {
                    //UPDATE inventory SET ingredient=@name,price=@price,quantity=@qty,e_date=@edate,unit=@unit,min_quantity=@minqty,supplier_id=@supplierid WHERE id=@id
                    cmd = new SqlCommand(
                        "UPDATE inventory SET ingredient=@name, price=@price, quantity=@qty, e_date=@edate, unit=@unit, min_quantity=@minqty, supplier_id=@supplierid WHERE id=@id",
                        con);
                    cmd.Parameters.AddWithValue("@id", itm.id);
                    cmd.Parameters.AddWithValue("@name", itm.ingredient);
                    cmd.Parameters.AddWithValue("@price", itm.price);
                    cmd.Parameters.AddWithValue("@qty", itm.quantity);
                    cmd.Parameters.AddWithValue("@edate", Convert.ToDateTime(itm.e_date));
                    cmd.Parameters.AddWithValue("@unit", itm.unit);
                    cmd.Parameters.AddWithValue("@minqty", itm.min_quantity);
                    cmd.Parameters.AddWithValue("@supplierid", itm.supplier_name);
                }
                else
                {
                    cmd = new SqlCommand(
                        "UPDATE inventory SET ingredient=@name, price=@price, e_date=NULL, quantity=@qty, unit=@unit, min_quantity=@minqty, supplier_id=@supplierid WHERE id=@id",
                        con);
                    cmd.Parameters.AddWithValue("@id", itm.id);
                    cmd.Parameters.AddWithValue("@name", itm.ingredient);
                    cmd.Parameters.AddWithValue("@price", itm.price);
                    cmd.Parameters.AddWithValue("@qty", itm.quantity);
                    cmd.Parameters.AddWithValue("@unit", itm.unit);
                    cmd.Parameters.AddWithValue("@minqty", itm.min_quantity);
                    cmd.Parameters.AddWithValue("@supplierid", itm.supplier_name);
                }

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
                MessageBox.Show(e.ToString(), "Info");
            }
            finally
            {
                con.Close();
            }
            return result;
        }

        public bool delete(InventoryItem delItm)
        {
            bool result = false;
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            try
            {
                cmd = new SqlCommand(
                    "DELETE FROM inventory WHERE id=@id",
                    con);
                cmd.Parameters.AddWithValue("@id", delItm.id);
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
                MessageBox.Show(e.ToString(), "Info");
            }
            finally
            {
                con.Close();
            }
            return result;
        }
    }
}
