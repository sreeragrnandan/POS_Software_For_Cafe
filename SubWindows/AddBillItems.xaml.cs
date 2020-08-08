using System;
using System.Collections.Generic;
using System.Data;
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

namespace Callista_Cafe.SubWindows
{
    /// <summary>
    /// Interaction logic for AddBillItems.xaml
    /// </summary>
    public partial class AddBillItems : Window
    {
        public AddBillItems()
        {
            InitializeComponent();
        }

        Bills BillFunctions = new Bills();

        public int bill_id = 0;

        public int item_id = 0;

        public float item_price = 0;


        Bills  tobilledBills = new Bills();

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void QtyTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            LoadItemsGrid();
        }

        private void LoadItemsGrid()
        {
            DataTable menuitemsDataTable = BillFunctions.GetItemDataTable();
            MenuItemsDatagrid.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = menuitemsDataTable });
        }

        private void Close_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BilledItemsGrid_Loaded(object sender, RoutedEventArgs e)
        {
            LoadBilledItemGrid();
        }

        private void LoadBilledItemGrid()
        {
            DataTable menuitemsDataTable = BillFunctions.GetBilledItemDataTable(bill_id);
            BilledItemsGrid.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = menuitemsDataTable });
        }

        private void MenuItemsDatagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            DataGrid gd = (DataGrid)sender;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                item_id = int.Parse(row_selected[0].ToString());
                ItemTextBox.Text = row_selected[1].ToString();
                item_price = float.Parse(row_selected[2].ToString());
                QtyTextBox.Text = "0";
                deleteBtn.IsEnabled = false;
                addBtn.IsEnabled = true;
            }

        }

        private bool bindDataToBill()
        {
            bool result = true;
            if (bill_id == 0)
            {
                MessageBox.Show("Bill ID Error. Try again !", "Error");
                result = false;
                goto FUNEND;
            }
            else
            {
                tobilledBills.bill_id = bill_id;
            }

            if (item_id == 0)
            {
                MessageBox.Show("Item ID Error. Try again !", "Error");
                result = false;
                goto FUNEND;
            }
            else
            {
                tobilledBills.item_id = item_id;
            }

            if (QtyTextBox.Text.Equals("0"))
            {
                MessageBox.Show("Enter a quantity !", "Error");
                result = false;
                goto FUNEND;
            }
            else
            {
                try
                {
                    tobilledBills.item_qty = float.Parse(QtyTextBox.Text);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Enter a valid quantity !", "Error");
                    result = false;
                    goto FUNEND;
                }
            }

            if (item_price==0)
            {
                MessageBox.Show("Item Price error!. Try again.", "Error");
                result = false;
                goto FUNEND;
            }
            else
            {
                try
                {
                    tobilledBills.item_price = item_price;
                }
                catch (Exception e)
                {
                    MessageBox.Show("Item Price error!. Try again.", "Error");
                    result = false;
                    goto FUNEND;
                }
            }

            tobilledBills.item_total = tobilledBills.item_qty*tobilledBills.item_price;

        FUNEND:
            return result;
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            bool bindData = bindDataToBill();
            if (bindData)
            {
                bool result = BillFunctions.insertToBilled(tobilledBills);
                if (result)
                {
                    reset();
                }
            }
        }

        private void reset()
        {
            item_id = 0;
            item_price = 0;
            ItemTextBox.Text = "";
            QtyTextBox.Text = "0";
            LoadBilledItemGrid();
        }
    }
}
