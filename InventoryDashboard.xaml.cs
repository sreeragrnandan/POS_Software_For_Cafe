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
        public InventoryDashboard()
        {
            InitializeComponent();
            FillSupplier();
        }
        InventoryItem itm = new InventoryItem();
        InventoryItem addItem = new InventoryItem();

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
            bool date = true;
            try
            {
                if (nameTxtBox.Text.ToString().Equals(""))
                {
                    MessageBox.Show("Please Enter a Valid Name..!", "Error");
                    goto THEEND;
                }
                else
                {
                    addItem.ingredient = nameTxtBox.Text;
                }

                addItem.price = priceTxtBox.Text;

                if (qtyTxtBox.Text.ToString().Equals(""))
                {
                    MessageBox.Show("Please Enter The Quantity..!", "Error");
                    goto THEEND;
                }
                else
                {
                    addItem.quantity = qtyTxtBox.Text;
                }

                if (expDate.Text.ToString().Equals(""))
                {
                    date = false;
                }
                else
                {
                    addItem.e_date = expDate.SelectedDate.ToString();
                }

                if (unitComboBox.Text.ToString().Equals(""))
                {
                    MessageBox.Show("Please select a Unit..!", "Error");
                    goto THEEND;
                }
                else
                {
                    addItem.unit = unitComboBox.Text;
                }

                addItem.min_quantity = minQtyTxtBox.Text;

                if (supplierComboBox.Text.ToString().Equals(""))
                {
                    MessageBox.Show("Please select a Supplier..!", "Error");
                    goto THEEND;
                }
                else
                {
                    addItem.supplier_name = supplierComboBox.SelectedValue.ToString();
                }

                bool result = addItem.insert(addItem,date);
                if (result)
                {
                    MessageBox.Show("New Item Added.", "Info");
                    ResetFun();
                }
                else
                {
                    MessageBox.Show("Failed to add item.", "Info");
                }
                DataTable dt = itm.select();
                InventoryItems.SetBinding(ItemsControl.ItemsSourceProperty, new Binding {Source = dt});
                goto SUCCESS;
                THEEND:
                MessageBox.Show("TRY AGAIN", "Info");
                SUCCESS:{}
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString(), "info");
            }
        
        }

        private void InventoryItems_Loaded(object sender, RoutedEventArgs e)
        {
            DataTable dt = itm.select();
            InventoryItems.SetBinding(ItemsControl.ItemsSourceProperty, new Binding {Source = dt});
        }

        private void InventoryItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                idTextBlock.Text = row_selected[0].ToString();
                nameTxtBox.Text = row_selected[1].ToString();
                priceTxtBox.Text = row_selected[2].ToString();
                qtyTxtBox.Text = row_selected[3].ToString();
                unitComboBox.Text = row_selected[5].ToString();
                minQtyTxtBox.Text = row_selected[6].ToString();
                supplierComboBox.Text = row_selected[7].ToString();
                if (row_selected[4] != null)
                {
                    expDate.Text = row_selected[4].ToString();
                }

                addBtn.IsEnabled = false;
                updateBtn.IsEnabled = true;
                DeleteBtn.IsEnabled = true;
            }

        }

        private void updateBtn_Click(object sender, RoutedEventArgs e)
        {
            bool date = true;
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure?", "Update", MessageBoxButton.YesNo);
            switch (messageBoxResult)
            {
                case MessageBoxResult.Yes:
                    goto CONTINUE;
                    break;
                case MessageBoxResult.No:
                    goto UPDATEEND;
                    break;
            }
            CONTINUE:
            try
            {
                addItem.id = int.Parse(idTextBlock.Text);
                if (nameTxtBox.Text.ToString().Equals(""))
                {
                    MessageBox.Show("Please Enter a Valid Name..!", "Error");
                    goto THEEND;
                }
                else
                {
                    addItem.ingredient = nameTxtBox.Text;
                }

                addItem.price = priceTxtBox.Text;

                if (qtyTxtBox.Text.ToString().Equals(""))
                {
                    MessageBox.Show("Please Enter The Quantity..!", "Error");
                    goto THEEND;
                }
                else
                {
                    addItem.quantity = qtyTxtBox.Text;
                }

                if (expDate.Text.ToString().Equals(""))
                {
                    date = false;
                }
                else
                {
                    addItem.e_date = expDate.SelectedDate.ToString();
                }

                if (unitComboBox.Text.ToString().Equals(""))
                {
                    MessageBox.Show("Please select a Unit..!", "Error");
                    goto THEEND;
                }
                else
                {
                    addItem.unit = unitComboBox.Text;
                }

                addItem.min_quantity = minQtyTxtBox.Text;

                if (supplierComboBox.Text.ToString().Equals(""))
                {
                    MessageBox.Show("Please select a Supplier..!", "Error");
                    goto THEEND;
                }
                else
                {
                    addItem.supplier_name = supplierComboBox.SelectedValue.ToString();
                }

                bool result = addItem.update(addItem, date);
                if (result)
                {
                    MessageBox.Show("Item Updated Successfully.", "Info");
                    ResetFun();
                }
                else
                {
                    MessageBox.Show("Failed to Updated.", "Info");
                }
                DataTable dt = itm.select();
                InventoryItems.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = dt });
                
                THEEND:{}
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString(), "info");
            }

            UPDATEEND:{}
        }

        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            ResetFun();
        }

        public void ResetFun()
        {
            idTextBlock.Text = "";
            nameTxtBox.Text = "";
            priceTxtBox.Text = "";
            qtyTxtBox.Text = "";
            expDate.Text = "";
            unitComboBox.Text = "";
            minQtyTxtBox.Text = "";
            supplierComboBox.Text = "";
            updateBtn.IsEnabled = false;
            addBtn.IsEnabled = true;
            DeleteBtn.IsEnabled = false;
        }
        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            InventoryItem delItm = new InventoryItem();
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure?", "DELETE", MessageBoxButton.YesNo);
            switch (messageBoxResult)
            {
                case MessageBoxResult.Yes:
                    goto DELETECONTINUE;
                    break;
                case MessageBoxResult.No:
                    goto DELETEEND;
                    break;
            }
            DELETECONTINUE:
            delItm.id = int.Parse(idTextBlock.Text);
            bool delResult = itm.delete(delItm);
            if (delResult)
            {
                MessageBox.Show("Deleted Successfully","Info");
                ResetFun();
            }
            else
            {
                MessageBox.Show("Failed to Delete", "Info");
            }
            DataTable dt = itm.select();
            InventoryItems.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = dt });
        DELETEEND:{}
        }
    }
    
}
