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

namespace Callista_Cafe
{
    /// <summary>
    /// Interaction logic for MenuDashboard.xaml
    /// </summary>
    public partial class MenuDashboard : Window
    {
        public MenuDashboard()
        {
            InitializeComponent();
        }
        MenuItm MenuItem = new MenuItm();
        MenuItm binddataMenuItm = new MenuItm();

        private SqlConnection con;
        private SqlCommand cmd;


        private void MenuItems_Loaded(object sender, RoutedEventArgs e)
        {
            loadGrid();
        }

        public void loadGrid()
        {
            DataTable menuDataTable = MenuItem.select();
            MenuItems.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = menuDataTable });
        }
        private bool BindDataToObject( bool id)
        {
            bool flag = true;
            if (id)
            {
                if (ItemIDTxtBox.Text.ToString().Equals(""))
                {
                    MessageBox.Show("Item_ID ERROR. Failed to Update..!", "Error");
                    flag = false;
                    goto FUNEND;
                }
                else
                {
                    try
                    {
                        binddataMenuItm.item_id = int.Parse(ItemIDTxtBox.Text.ToString());
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
                binddataMenuItm.item_id = 0;
            }

            if (ItemNameTxtBox.Text.ToString().Equals(""))
            {
                MessageBox.Show("Please Enter the Name.", "Info");
                flag = false;
                goto FUNEND;
            }
            else
            {
                try
                {
                    binddataMenuItm.item_name = ItemNameTxtBox.Text.ToString();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Please Enter a valid Name.!", "Error");
                    flag = false;
                    goto FUNEND;
                }
            }
            if (ItemPriceTxtBox.Text.ToString().Equals(""))
            {
                MessageBox.Show("Please Enter the Price.", "Info");
                flag = false;
                goto FUNEND;
            }
            else
            {
                try
                {
                    binddataMenuItm.item_price = float.Parse(ItemPriceTxtBox.Text.ToString());
                }
                catch (Exception e)
                {
                    MessageBox.Show("Please Enter a valid price.!", "Error");
                    flag = false;
                    goto FUNEND;
                }
            }
            if (ItemCategoryComboBox.Text.ToString().Equals(""))
            {
                MessageBox.Show("Please Enter/Select a Category.", "Info");
                flag = false;
                goto FUNEND;
            }
            else
            {
                try
                {
                    binddataMenuItm.item_category = ItemCategoryComboBox.Text;
                }
                catch (Exception e)
                {
                    MessageBox.Show("Please Enter a valid price.!", "Error");
                    flag = false;
                    goto FUNEND;
                }
            }
            FUNEND:{}
            return flag;
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            bool bindingResult = BindDataToObject(false);
            bool result;
            if (bindingResult)
            {
                result=MenuItem.insert(binddataMenuItm);
                if (result)
                {
                    MessageBox.Show("New Item Inserted..!", "Success");
                    reset();
                    loadGrid();
                }
                else
                {
                    MessageBox.Show("Failed to insert. Try Again !", "Error");
                }
            }
            else
            {
                MessageBox.Show("Something went wrong. Please try Again !");
            }
        }

        private void reset()
        {
            ItemIDTxtBox.Text = "";
            ItemNameTxtBox.Text = "";
            ItemCategoryComboBox.Text = "";
            ItemPriceTxtBox.Text = "";
            updateBtn.IsEnabled = false;
            deleteBtn.IsEnabled = false;
            addBtn.IsEnabled = true;
            addRequirementBtn.IsEnabled = false;
        }

        private void MenuItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                ItemIDTxtBox.Text = row_selected[0].ToString();
                ItemNameTxtBox.Text = row_selected[1].ToString();
                ItemCategoryComboBox.Text = row_selected[2].ToString();
                ItemPriceTxtBox.Text = row_selected[3].ToString();
                
                addBtn.IsEnabled = false;
                updateBtn.IsEnabled = true;
                deleteBtn.IsEnabled = true;
                addRequirementBtn.IsEnabled = true;
            }
        }

        private void resetBtn_Click(object sender, RoutedEventArgs e)
        {
            reset();
        }

        private void updateBtn_Click(object sender, RoutedEventArgs e)
        {
            bool bindingResult = BindDataToObject(true);
            bool result = false;
            if (bindingResult)
            {
                result = MenuItem.update(binddataMenuItm);
                if (result)
                {
                    MessageBox.Show("Item Successfully Updated !", "Success");
                    reset();
                    loadGrid();
                }
                else
                {
                    MessageBox.Show("Failed to Update. Try Again !", "Error");
                }
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string S_key = searchTxtBox.Text;
            try
            {
                if (S_key == "")
                {
                    loadGrid();
                }
                else
                {
                    DataTable dt = new DataTable();
                    con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
                    con.Open();
                    SqlDataAdapter sda = new SqlDataAdapter(
                        "SELECT item_id, item_name, item_category, item_price FROM menu_items WHERE item_name LIKE '%"+S_key+"%' OR item_category LIKE'%"+S_key+"%';", con);
                    sda.Fill(dt);
                    MenuItems.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = dt });
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

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            bool id = ItemIDTxtBox.Text.ToString().Equals("");
            bool result = false;
            if (id)
            {
                MessageBox.Show("Please select a valid item.", "Error");
            }
            else
            {
                MenuItm delitm = new MenuItm();
                delitm.item_id = int.Parse(ItemIDTxtBox.Text.ToString());
                result = MenuItem.delete(delitm);
                if (result)
                {
                    MessageBox.Show("Item Successfully Deleted !", "Success");
                    reset();
                    loadGrid();
                }
                else
                {
                    MessageBox.Show("Failed to Delete. Try Again !", "Error");
                }
            }

        }

        private void addRequirementBtn_Click(object sender, RoutedEventArgs e)
        {
            
            bool id = ItemIDTxtBox.Text.ToString().Equals("");
            if (id)
            {
                MessageBox.Show("Try Again..!", "Error");
                reset();
            }
            else
            {
                MenuItemRequirement itmreq = new MenuItemRequirement(int.Parse(ItemIDTxtBox.Text.ToString()));
                MessageBox.Show(int.Parse(ItemIDTxtBox.Text.ToString()).ToString());
                itmreq.ShowDialog();
                reset();
            }
        }
    }
}
