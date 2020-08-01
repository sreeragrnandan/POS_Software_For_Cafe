using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
using Callista_Cafe.Classes;
using SqlDataAdapter = System.Data.SqlClient.SqlDataAdapter;

namespace Callista_Cafe
{
    /// <summary>
    /// Interaction logic for InventoryDashboard.xaml
    /// </summary>
    public partial class InventoryDashboard : Window
    {
        private SqlConnection con;
        private SqlCommand cmd;
        private SqlDataReader reader;
        public InventoryDashboard()
        {
            InitializeComponent();
            FillSupplier();
        }
        InventoryItem itm = new InventoryItem();

        public void FillSupplier()
        {
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
                con.Open();
                cmd = new SqlCommand("Select * from suppliers", con);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                supplierComboBox.ItemsSource = dt.DefaultView;
                supplierComboBox.DisplayMemberPath = "supplier_name";
                supplierComboBox.SelectedValuePath = "supplier_id";
                cmd.Dispose();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Info");
            }
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
                con.Open();
                cmd = new SqlCommand("INSERT INTO inventory (ingredient,price,quantity,e_date,unit,min_quantity,supplier_id) VALUES(@name,@price,@qty,@edate,@unit,@minqty,@supplierid)", con);
                cmd.Parameters.AddWithValue("@name", nameTxtBox.Text);
                cmd.Parameters.AddWithValue("@price", priceTxtBox.Text);
                cmd.Parameters.AddWithValue("@qty", qtyTxtBox.Text);
                cmd.Parameters.AddWithValue("@edate", expDate.DisplayDate);
                cmd.Parameters.AddWithValue("@unit", unitComboBox.Text);
                cmd.Parameters.AddWithValue("@minqty", minQtyTxtBox.Text);
                cmd.Parameters.AddWithValue("@supplierid", supplierComboBox.SelectedValue);
                cmd.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString(), "Info");
            }
        }

        private void InventoryItems_Loaded(object sender, RoutedEventArgs e)
        {
            DataTable dt = itm.select();
            InventoryItems.SetBinding(ItemsControl.ItemsSourceProperty, new Binding {Source = dt});
        }
    }
}
