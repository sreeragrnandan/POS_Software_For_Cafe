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

namespace Callista_Cafe
{
    /// <summary>
    /// Interaction logic for AdminDashbord.xaml
    /// </summary>
    public partial class AdminDashbord : Window
    {
        public AdminDashbord()
        {
            InitializeComponent();
        }

        private void close_Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        
    }
}
