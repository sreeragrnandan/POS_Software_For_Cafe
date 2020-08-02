using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;

namespace Callista_Cafe.Classes
{
    class DatabaseFunctions
    {
        private SqlCommand cmd;
        private SqlConnection con;
        private SqlDataReader reader;
        public string getSupplierName(int id)
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            con.Open();
            cmd = new SqlCommand("Select * from suppliers where supplier_id=@supplier_id", con);
            cmd.Parameters.AddWithValue("@supplier_id", id);
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return reader["supplier_name"].ToString();
            }
            else
            {
                return null;
            }
        }
    }
}
