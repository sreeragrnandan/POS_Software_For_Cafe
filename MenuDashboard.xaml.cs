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
        private void MenuItems_Loaded(object sender, RoutedEventArgs e)
        {
            DataTable menuDataTable = MenuItem.select();
            MenuItems.SetBinding(ItemsControl.ItemsSourceProperty, new Binding {Source = menuDataTable});
        }
    }
}
