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
    class Bills
    {
        private SqlConnection conn;
        private SqlConnection con;
        private SqlCommand cmd;
        private SqlDataReader reader;

        private SqlTransaction transaction;

        DatabaseFunctions DbFun = new DatabaseFunctions();

        public int bill_id { get; set; }

        public string bill_table { get; set; }

        public string bill_payment { get; set; }

        public string bill_customer { get; set; }

        public  int item_id { get; set; }

        public float item_qty { get; set; }

        public float item_price { get; set; }

        public float item_total { get; set; }

        public DataTable LoadBillTable()
        {
            DataTable dt = new DataTable();
            try
            {
                /*select id ingredient,price,qualtity,e_date,unit,min_quantity, supplier_name from inventory as inv, suppliers as sup where sup.supplier_id=inv.supplier_id*/
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
                cmd = new SqlCommand("select bill.bill_id, bill.payment_method, bill.table_no, bill.bill_datetime,(SELECT COUNT(D1.item_id) FROM detailed_bill AS D1 WHERE D1.bill_id = bill.bill_id) AS item_count,(SELECT STR(SUM(D1.item_total)) + ' Rs' FROM detailed_bill AS D1 WHERE D1.bill_id = bill.bill_id) AS amount,CASE WHEN bill.c_id IS NULL THEN '' ELSE (SELECT c_name FROM customer where c_id= bill.c_id) END AS customer_name from bills as bill WHERE bill.bill_status='PENDING';", con);
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

        public bool insert(Bills bills)
        {
            bool result = false;
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            try
            {
                cmd = new SqlCommand(
                    "INSERT INTO bills (table_no,payment_method,c_id,user_id) VALUES(@table,@payment,@c_id,@user_id)",
                    con);
                cmd.Parameters.AddWithValue("@table", bills.bill_table);
                cmd.Parameters.AddWithValue("@payment", bills.bill_payment);
                if(bills.bill_customer.Equals(""))
                    cmd.Parameters.AddWithValue("@c_id", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@c_id", bills.bill_customer);
                cmd.Parameters.AddWithValue("@user_id", UserInfo.User_Id);
                con.Open();
                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    result = true;
                }
                else
                {
                    MessageBox.Show("Failed to create new bills. Try Again !", "Error");
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

        public DataTable GetItemDataTable()
        {
            DataTable itemDataTable = new DataTable();
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
                cmd = new SqlCommand("SELECT item_id, item_name, item_price, item_category FROM menu_Items", con);
                SqlDataAdapter InvAdapter = new SqlDataAdapter(cmd);
                con.Open();
                InvAdapter.Fill(itemDataTable);

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

        public DataTable GetBilledItemDataTable(int billid)
        {
            DataTable billedItemDataTable = new DataTable();
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
                cmd = new SqlCommand("SELECT DBILL.item_id,(SELECT D1.item_name FROM menu_items AS D1 WHERE D1.item_id=DBILL.item_id) AS item_name, DBill.item_quantity, DBill.item_price, DBill.item_total FROM detailed_bill AS DBill WHERE DBill.bill_id=@bill_id;", con);
                cmd.Parameters.AddWithValue("@bill_id", billid);
                SqlDataAdapter Adapter = new SqlDataAdapter(cmd);
                con.Open();
                Adapter.Fill(billedItemDataTable);

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error");
            }
            finally
            {
                con.Close();
            }

            return billedItemDataTable;
        }

        public bool closeBill(int bill_id)
        {
            bool result = false;
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            try
            {
                cmd = new SqlCommand(
                    "UPDATE bills SET bill_status='CLOSED', bill_amount = @amount, bill_close_datetime=@time WHERE bill_id=@bill_id",
                    con);
                cmd.Parameters.AddWithValue("@bill_id", bill_id);
                cmd.Parameters.AddWithValue("@amount", DbFun.getbilltotal(bill_id).ToString());
                cmd.Parameters.AddWithValue("@time", DateTime.Now.ToString());
                con.Open();
                int rows = cmd.ExecuteNonQuery();

                if (rows > 0)
                {
                    result = true;
                }
                else
                {
                    MessageBox.Show("Something went wrong. Please try again!", "Error");
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

        public bool insertToBilled(Bills bills)
        {
            bool result = false;
            bool updateinv = true;
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            try
            {
                conn.Open();
                cmd = new SqlCommand("SELECT inv.id, ing.quantity AS needed, inv.quantity AS available FROM inventory AS inv, ingredient_list AS ing WHERE ing.menu_item_id=@itemId and inv.id=ing.ingredient_id;", conn);
                cmd.Parameters.AddWithValue("@itemId", bills.item_id);
                reader = cmd.ExecuteReader();
                con.Open();
                transaction = con.BeginTransaction();
                while (reader.Read() && updateinv)
                {
                    float newQTY = float.Parse(reader["available"].ToString()) -
                                   (float.Parse(reader["needed"].ToString()) * bills.item_qty);
                    if (newQTY <= 0)
                    {
                        MessageBoxResult messageBoxResult = MessageBox.Show("Ingredient shortage. Do you want to continue?", "Ingredient shortage", MessageBoxButton.YesNo);
                        switch (messageBoxResult)
                        {
                            case MessageBoxResult.Yes:
                                newQTY = 0;
                                break;
                            case MessageBoxResult.No:
                                updateinv = false;
                                transaction.Rollback();
                                goto WHILEEND;
                        }
                    }
                    SqlCommand invCmd = new SqlCommand("UPDATE inventory SET quantity=@quantity WHERE id=@id", con, transaction);
                    invCmd.Parameters.AddWithValue("@quantity", newQTY.ToString());
                    invCmd.Parameters.AddWithValue("@id", reader["id"]);
                    int rows = invCmd.ExecuteNonQuery();
                    if (rows > 0){}
                    else
                    {
                        MessageBox.Show("Something went wrong. Please try again !", "Error");
                        updateinv = false;
                        transaction.Rollback();
                    }
                    WHILEEND:{}
                }

                if (updateinv)
                {
                    SqlCommand addbill = new SqlCommand("INSERT INTO detailed_bill (bill_id, item_id, item_quantity, item_price, item_total) VALUES (@bill_id, @item_id, @item_quantity, @item_price, @item_total )", con, transaction);
                    addbill.Parameters.AddWithValue("@bill_id", bills.bill_id);
                    addbill.Parameters.AddWithValue("@item_id", bills.item_id);
                    addbill.Parameters.AddWithValue("@item_quantity", bills.item_qty.ToString());
                    addbill.Parameters.AddWithValue("@item_price", bills.item_price.ToString());
                    addbill.Parameters.AddWithValue("@item_total", bills.item_total.ToString());
                    int rows = addbill.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        transaction.Commit();
                        result = true;
                    }
                    else
                    {
                        MessageBox.Show("Something went wrong. Please try again !", "Error");
                        result = false;
                        transaction.Rollback();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                transaction.Rollback();
            }
            finally
            {
                con.Close();
                conn.Close();
            }
            return result;
        }
    }
}
