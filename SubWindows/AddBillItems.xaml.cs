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
    }
}
