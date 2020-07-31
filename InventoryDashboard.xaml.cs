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
            FillDataGrid();
        }

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

        public class InventoryItem
        {
            public int id { get; set; }
            public string name { get; set; }
            public float price { get; set; }
            public float quantity { get; set; }
            public DateTime eDate { get; set; }
            public string unit { get; set; }
            public float minQuantity { get; set; }
            public string supplier { get; set; }
        }

        public void FillDataGrid()
        {
            DatabaseFunctions dbFunctions = new DatabaseFunctions();
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
                con.Open();
                cmd = new SqlCommand("Select * from inventory", con);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    InventoryItem item = new InventoryItem();
                    item.id = int.Parse(reader["id"].ToString());
                    item.name = reader["ingredient"].ToString();
                    if (reader["price"].ToString().Equals(""))
                    {
                        item.price = 0.0f;
                    }
                    else
                    {
                        item.price = float.Parse(reader["price"].ToString());
                    }
                    item.quantity = float.Parse(reader["quantity"].ToString());
                    if (reader["e_date"].ToString().Equals(""))
                    {
                        
                    }
                    else
                    {
                        item.eDate = DateTime.Parse(reader["e_date"].ToString());
                    }
                    
                    item.unit = reader["unit"].ToString();
                    if (reader["min_quantity"].ToString().Equals(""))
                    {

                    }
                    else
                    {
                        item.minQuantity = float.Parse(reader["min_quantity"].ToString());
                    }
                    
                    if (reader["supplier_id"].ToString().Equals(""))
                    {
                        item.supplier = null;
                    }
                    else
                    {
                        item.supplier = dbFunctions.getSupplierName(int.Parse(reader["supplier_id"].ToString()));
                    }

                    InventoryItems.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Info");
            }
        }

        private void InventoryItems_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void InventoryItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object item = InventoryItems.SelectedItem; //probably you find this object
            string ID = (InventoryItems.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
            idTextBlock.Text = ID;
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
                FillDataGrid();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString(), "Info");
            }
        }
    }
}
