using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

//
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Callista_Cafe
{
    /// <summary>
    /// Interaction logic for SupplierWindow.xaml
    /// </summary>
    public partial class SupplierWindow : Window
    {
        SqlConnection con;
        SqlCommand cmd;
        String Connstring = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
        SqlDataReader reader;
        public SupplierWindow()
        {
            InitializeComponent();
        }

        private void SubmitSupplierClick(object sender, RoutedEventArgs e)
        {
            cmd = new SqlCommand("Insert into dbo.suppliers values(@sup_id,@sup_name,@sup_mob_no)",con);
            cmd.Parameters.AddWithValue("@sup_id", Txtsupplierid.Text);
            cmd.Parameters.AddWithValue("@sup_name", Txtsuppliername.Text);
            cmd.Parameters.AddWithValue("@sup_mob_no", Txtsuppliermobile.Text);
            try
            {
                var res = cmd.ExecuteNonQuery();
                if (res == 1)
                {
                    MessageBox.Show("Successful");
                }
                else
                {
                    MessageBox.Show(res.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(Connstring);
            try
            {
                con = new SqlConnection(Connstring);
                con.Open();
                //cmd = new SqlCommand("Insert into dbo.suppliers values(@Sup_id,@Sup_name,@Sup_mob_no)", con);
                //cmd.Parameters.AddWithValue("Sup_id", 8);
                //cmd.Parameters.AddWithValue("Sup_name", "sup8");
                //cmd.Parameters.AddWithValue("Sup_mob_no", "364645");
                //int value = cmd.ExecuteNonQuery();
                //MessageBox.Show(value.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            } 
            finally {
                //reader.Close();
                //reader.Dispose();
                //cmd.Dispose();
                //con.Close();
            }
        }

        private void SupplierWindow_OnUnloaded(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Success");
            con.Close();
        }
    }
}
