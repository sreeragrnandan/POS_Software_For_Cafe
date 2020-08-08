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
            FillComboBox();
        }
        InventoryItem itm = new InventoryItem();
        InventoryItem addItem = new InventoryItem();

        InventoryItem toSendInventoryItem = new InventoryItem();

        DatabaseFunctions DbFun = new DatabaseFunctions();

        private int item_id = 0;

        public void FillComboBox()
        {
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
                con.Open();
                cmd = new SqlCommand("Select * from suppliers", con);
                SqlDataAdapter Supplieradapter = new SqlDataAdapter(cmd);
                DataTable SupplierDt = new DataTable();
                Supplieradapter.Fill(SupplierDt);
                supplierComboBox.ItemsSource = SupplierDt.DefaultView;
                supplierComboBox.DisplayMemberPath = "supplier_name";
                supplierComboBox.SelectedValuePath = "supplier_id";
                cmd.Dispose();
                con.Close();

                con.Open();
                cmd = new SqlCommand("select distinct unit from inventory;", con);
                SqlDataAdapter Unitadapter = new SqlDataAdapter(cmd);
                DataTable Unitdt = new DataTable();
                Unitadapter.Fill(Unitdt);
                unitComboBox.ItemsSource = Unitdt.DefaultView;
                unitComboBox.DisplayMemberPath = "unit";
                unitComboBox.SelectedValuePath = "unit";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Info");
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
        }

        private bool BindDataToObject(bool id)
        {
            bool flag = true;

            if (id)
            {
                if (item_id.ToString().Equals("") || item_id.ToString().Equals("0"))
                {
                    MessageBox.Show("Item_ID ERROR. Failed to Update..!", "Error");
                    flag = false;
                    goto FUNEND;
                }
                else
                {
                    try
                    {
                        toSendInventoryItem.id = item_id;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Item_ID ERROR. Failed to Update..!", "Error");
                        flag = false;
                        goto FUNEND;
                    }
                }
            }
            else
            {
                toSendInventoryItem.id = 0;
            }

            if (nameTxtBox.Text.Equals(""))
            {
                MessageBox.Show("Please enter the name..!", "Error");
                flag = false;
                goto FUNEND;
            }
            else
            {
                toSendInventoryItem.ingredient = nameTxtBox.Text;
            }

            if (!priceTxtBox.Text.Equals(""))
            {
                try
                {
                    toSendInventoryItem.price = float.Parse(priceTxtBox.Text).ToString();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Please enter a valid price..!", "Error");
                    flag = false;
                    goto FUNEND;
                }
            }
            else
            {
                toSendInventoryItem.price = "";
            }
            
            if (qtyTxtBox.Text.ToString().Equals(""))
            {
                MessageBox.Show("Please enter the quantity..!", "Error");
                flag = false;
                goto FUNEND;
            }
            else
            {
                try
                {
                    toSendInventoryItem.quantity = float.Parse(qtyTxtBox.Text);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Please Enter a valid quantity.!", "Error");
                    flag = false;
                    goto FUNEND;
                }
            }

            if (!expDate.Text.Equals(""))
            {
                try
                {
                    toSendInventoryItem.e_date = Convert.ToDateTime(expDate.Text).ToString();
                }
                catch
                {
                    MessageBox.Show("Please enter a valid date..!", "Error");
                    flag = false;
                    goto FUNEND;
                }
            }
            else
            {
                toSendInventoryItem.e_date = "";
            }


            if (unitComboBox.Text.ToString().Equals(""))
            {
                MessageBox.Show("Please select/enter an unit..!", "Error");
                flag = false;
                goto FUNEND;
            }
            else
            {
                toSendInventoryItem.unit = unitComboBox.Text;
            }

            if (!minQtyTxtBox.Text.Equals(""))
            {
                try
                {
                    toSendInventoryItem.min_quantity = float.Parse(minQtyTxtBox.Text).ToString();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Please enter a valid minimum quantity..!", "Error");
                    flag = false;
                    goto FUNEND;
                }
            }
            else
            {
                toSendInventoryItem.min_quantity = "";
            }

            if (supplierComboBox.Text.ToString().Equals(""))
            {
                toSendInventoryItem.supplier_name = "";
            }
            else
            {
                try
                {
                    toSendInventoryItem.supplier_name = supplierComboBox.SelectedValue.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Please select a supplier form the list..!", "Error");
                    flag = false;
                    goto FUNEND;
                }

            }
            FUNEND: { }
            return flag;
        }



        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            bool bindingResult = BindDataToObject(false);
            bool result;
            if (bindingResult)
            {
                result = itm.insert(toSendInventoryItem);
                if (result)
                {
                    MessageBox.Show("New Item Inserted..!", "Success");
                    reset();
                }
            }

        }

        private void InventoryItems_Loaded(object sender, RoutedEventArgs e)
        {
            loadGrid();
        }

        private void loadGrid()
        {
            DataTable dt = itm.select();
            InventoryItems.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = dt });
        }

        private void InventoryItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                item_id = int.Parse(row_selected[0].ToString());
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
                addQtyBtn.IsEnabled = true;
            }

        }

        private void updateBtn_Click(object sender, RoutedEventArgs e)
        {
            bool bindingResult = BindDataToObject(true);
            bool result = false;
            if (bindingResult)
            {
                bool areyousure = DbFun.areyousure();
                if (areyousure)
                {
                    result = itm.update(toSendInventoryItem);
                    if (result)
                    {
                        MessageBox.Show(" Inventory item details updated successfully !", "Success");
                        reset();
                    }
                }
            }
        }

        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            reset();
        }

        public void reset()
        {
            item_id=0;
            nameTxtBox.Text = "";
            priceTxtBox.Text = "";
            qtyTxtBox.Text = "";
            expDate.Text = "";
            unitComboBox.Text = "";
            minQtyTxtBox.Text = "";
            supplierComboBox.Text = "";
            addQtyTxtBox.Text = "0";
            updateBtn.IsEnabled = false;
            addBtn.IsEnabled = true;
            DeleteBtn.IsEnabled = false;
            addQtyBtn.IsEnabled = false;
            FillComboBox();
            loadGrid();
        }
        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            bool id = item_id.ToString().Equals("0");
            bool result = false;
            if (id)
            {
                MessageBox.Show("Please select an item.", "Error");
            }
            else
            {
                bool areyousure = DbFun.areyousure();
                if (areyousure)
                {
                    toSendInventoryItem.id = item_id;
                    result = itm.delete(toSendInventoryItem);
                    if (result)
                    {
                        MessageBox.Show(" Inventory item deleted successfull..!", "Success");
                        reset();
                    }
                }
            }
        }

        private void searchTxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string S_key = searchTxtBox.Text;
            try
            {
                if (S_key == "")
                {
                    DataTable dt = itm.select();
                    InventoryItems.SetBinding(ItemsControl.ItemsSourceProperty, new Binding {Source = dt});
                }
                else
                {
                    DataTable dt = new DataTable();
                    con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
                    con.Open();
                    SqlDataAdapter sda = new SqlDataAdapter(
                        "select id, ingredient,price,quantity,convert(varchar, inv.e_date, 3) as e_date,unit,min_quantity, CASE WHEN supplier_id IS NULL THEN '' ELSE (SELECT supplier_name FROM suppliers where supplier_id=inv.supplier_id) END AS supplier_name FROM inventory as inv WHERE ingredient LIKE '%" + S_key+ "%'", con);
                    sda.Fill(dt);
                    InventoryItems.SetBinding(ItemsControl.ItemsSourceProperty, new Binding {Source = dt});
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString(), "Info");
            }
            finally
            {
                con.Close();
            }

        }

        private void addQtyBtn_Click(object sender, RoutedEventArgs e)
        {
            bool result = false;
            float addqty = 0.0f;
            try
            {
                addqty = float.Parse(addQtyTxtBox.Text);
            }
            catch
            {
                MessageBox.Show("Invalid input. Please check the input..!", "Error");
                goto END;
            }
            if (item_id.ToString().Equals("0") || addqty <= 0 || item_id==0) 
            {
                MessageBox.Show("Check the input..!", "Error");
            }
            else
            {
                bool areyousure = DbFun.areyousure();
                if (areyousure)
                {
                    toSendInventoryItem.id = item_id;
                    result = itm.addQty(toSendInventoryItem,float.Parse(addQtyTxtBox.Text));
                    if (result)
                    {
                        MessageBox.Show(" Quantity updated successfull..!", "Success");
                        reset();
                    }
                }
            }
            END:{}
        }


        private void HomeBtn_Click(object sender, RoutedEventArgs e)
        {
            AdminDashbord adm = new AdminDashbord();
            adm.Show();
            this.Close();
        }
    }
    
}
