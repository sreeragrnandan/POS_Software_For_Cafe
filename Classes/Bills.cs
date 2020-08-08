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



        public DataTable LoadBillTable()
        {
            DataTable dt = new DataTable();
            try
            {
                /*select id ingredient,price,qualtity,e_date,unit,min_quantity, supplier_name from inventory as inv, suppliers as sup where sup.supplier_id=inv.supplier_id*/
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
                cmd = new SqlCommand("select bill.table_no, bill.bill_datetime,(SELECT COUNT(D1.item_id) FROM detailed_bill AS D1 WHERE D1.bill_id = bill.bill_id) AS item_count,(SELECT STR(SUM(D1.item_total)) + ' Rs' FROM detailed_bill AS D1 WHERE D1.bill_id = bill.bill_id) AS amount,CASE WHEN bill.c_id IS NULL THEN '' ELSE (SELECT c_name FROM customer where c_id= bill.c_id) END AS customer_name from bills as bill WHERE bill.bill_status='Pending';", con);
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
