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
        public MenuItemRequirement(int Menu_Item_ID) : this()
        {
            this.menuItemId = Menu_Item_ID;
        }
    }
}
