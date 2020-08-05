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
    /// Interaction logic for MenuItemRequirement.xaml
    /// </summary>
    public partial class MenuItemRequirement : Window
    {
        private int menuItemId;
        public MenuItemRequirement()
        {
            InitializeComponent();
        }
        ItemIngredientRequirement Itm_ing_req = new ItemIngredientRequirement();
        ItemIngredientRequirement InvItem = new ItemIngredientRequirement();
        ItemIngredientRequirement SendData = new ItemIngredientRequirement();
        DatabaseFunctions DbFun = new DatabaseFunctions();

        private SqlConnection con;
        private SqlCommand cmd;


        public MenuItemRequirement(int Menu_Item_ID) : this()
        {
            this.menuItemId = Menu_Item_ID;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void RequiredItemDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            reqLoadGrid();
            ItemTextBox.Text = DbFun.getMenuItemName(menuItemId);
        }

        private void reqLoadGrid()
        {
            DataTable reqDataTable = Itm_ing_req.ReqDataTable(menuItemId);
            RequiredItemDataGrid.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = reqDataTable });
        }

        private void invLoadGrid()
        {
            DataTable invDataTable = InvItem.InventoryDataTable();
            InventoryItemDataGrid.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = invDataTable });
        }

        private void InventoryItemDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            invLoadGrid();
        }

        private void InventoryItemDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                IngredientTextBox.Text = row_selected[0].ToString();
                QtyTextBox.Text = "0";
                addBtn.IsEnabled = true;
                updateBtn.IsEnabled = false;
                deleteBtn.IsEnabled = false;
            }
        }

        private void RequiredItemDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                IngredientTextBox.Text = row_selected[0].ToString();
                QtyTextBox.Text = row_selected[1].ToString();

                addBtn.IsEnabled = false;
                updateBtn.IsEnabled = true;
                deleteBtn.IsEnabled = true;
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            string S_key = Search.Text;
            try
            {
                if (S_key == "")
                {
                    invLoadGrid();
                }
                else
                {
                    DataTable dt = new DataTable();
                    con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
                    con.Open();
                    SqlDataAdapter sda = new SqlDataAdapter(
                        "SELECT ingredient, unit FROM inventory WHERE ingredient LIKE'%" + S_key + "%';", con);
                    sda.Fill(dt);
                    InventoryItemDataGrid.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = dt });
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

        private bool BindData()
        {
            bool result = true;
            if (ItemTextBox.Text.ToString().Equals(""))
            {
                MessageBox.Show("Something went wrong. Please close and reopen the window.", "Error");
                result = false;
                goto FUNEND;
            }
            else
            {
                SendData.menuItemId = menuItemId;
            }
            if (IngredientTextBox.Text.ToString().Equals(""))
            {
                MessageBox.Show("Please select an ingredient.", "Error");
                result = false;
                goto FUNEND;
            }
            else
            {
                try
                {
                    SendData.ingredientId = DbFun.getIngredientId(IngredientTextBox.Text);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Something Went Wrong. Please try Again!", "Error");
                    result = false;
                    goto FUNEND;
                }
            }
            if (QtyTextBox.Text.ToString().Equals("") || QtyTextBox.Text.ToString().Equals("0"))
            {
                MessageBox.Show("Please Enter a quantity.", "Error");
                result = false;
                goto FUNEND;
            }
            else
            {
                try
                {
                    SendData.ingredientQuantity = float.Parse(QtyTextBox.Text.ToString());
                }
                catch (Exception e)
                {
                    MessageBox.Show("Please Enter a valid price !", "Error");
                    result = false;
                    goto FUNEND;
                }
            }
        FUNEND:
            return result;
        }
        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            bool bindData = BindData();
            if (bindData)
            {
                bool result = SendData.insert(SendData);
                if (result)
                {
                    MessageBox.Show("Added Successfully..!", "Info");
                    ResetFun();
                    reqLoadGrid();
                }
                else
                {
                    MessageBox.Show("Failed to add..!", "Error");
                }
            }
        }

        private void resetBtn_Click(object sender, RoutedEventArgs e)
        {
            ResetFun();
        }

        private void ResetFun()
        {
            IngredientTextBox.Text = "";
            QtyTextBox.Text = "";
            addBtn.IsEnabled = false;
            updateBtn.IsEnabled = false;
            deleteBtn.IsEnabled = false;
            reqLoadGrid();
            invLoadGrid();
        }

        private void updateBtn_Click(object sender, RoutedEventArgs e)
        {
            bool bindData = BindData();
            if (bindData)
            {
                bool areyousure = DbFun.areyousure();
                if (areyousure)
                {
                    bool result = SendData.update(SendData);
                    if (result)
                    {
                        MessageBox.Show("Updated Successfully..!", "Info");
                        ResetFun();
                        reqLoadGrid();
                    }
                    else
                    {
                        MessageBox.Show("Failed to Update..!", "Error");
                    }
                }
            }

        }
    }
}
