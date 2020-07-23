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
using System.Windows.Threading;
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
            if(!Classes.UserInfo.Login)
            {
                MainWindow logWindow = new MainWindow();
                logWindow.Show();
                this.Close();
            }
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
            lblUsername.Content = "Welcome "+ Classes.UserInfo.User_Name;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            dateandtime.Content = DateTime.Now.ToString("dddd , MMM dd yyyy , hh:mm:ss tt");
        }


        private void close_Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void logout_Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow loginWindow = new MainWindow();
            loginWindow.Show();
            this.Close();
        }

        private void billingBtnClick(object sender, RoutedEventArgs e)
        {
            BillingDashboard billingWindow = new BillingDashboard();
            billingWindow.Show();
            this.Close();
        }
    }
}
