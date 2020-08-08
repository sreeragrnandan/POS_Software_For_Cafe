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
        private SqlConnection con;
        private SqlCommand cmd;

        public int bill_id { get; set; }

        public string bill_table { get; set; }

        public string bill_payment { get; set; }

        public string bill_customer { get; set; }

        public DataTable LoadBillTable()
        {
            DataTable dt = new DataTable();
            try
            {
                /*select id ingredient,price,qualtity,e_date,unit,min_quantity, supplier_name from inventory as inv, suppliers as sup where sup.supplier_id=inv.supplier_id*/
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
                cmd = new SqlCommand("select bill.bill_id, bill.payment_method, bill.table_no, bill.bill_datetime,(SELECT COUNT(D1.item_id) FROM detailed_bill AS D1 WHERE D1.bill_id = bill.bill_id) AS item_count,(SELECT STR(SUM(D1.item_total)) + ' Rs' FROM detailed_bill AS D1 WHERE D1.bill_id = bill.bill_id) AS amount,CASE WHEN bill.c_id IS NULL THEN '' ELSE (SELECT c_name FROM customer where c_id= bill.c_id) END AS customer_name from bills as bill WHERE bill.bill_status='Pending';", con);
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
    }
}
