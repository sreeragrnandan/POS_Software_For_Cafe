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
using System.Windows.Threading;
using Callista_Cafe.Classes;

namespace Callista_Cafe
{
    /// <summary>
    /// Interaction logic for BillingDashboard.xaml
    /// </summary>
    public partial class BillingDashboard : Window
    {
        public BillingDashboard()
        {
            InitializeComponent();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
            usernameLabel.Content = UserInfo.User_Name;
        }

        Bills bills = new Bills();


        void timer_Tick(object sender, EventArgs e)
        {
            dateandtime.Content = DateTime.Now.ToString("dddd , MMM dd yyyy , hh:mm:ss tt");
        }

        private void activeBills_Loaded(object sender, RoutedEventArgs e)
        {
            loadBillGrid();
        }

        private void loadBillGrid()
        {
            DataTable dt = bills.LoadBillTable();
            activeBills.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = dt });
        }


        private void HomeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (UserInfo.User_Type <= 1)
            {
                AdminDashbord adminDashbord = new AdminDashbord();
                adminDashbord.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("You are not authorized to view this page !", "Warning");
            }
        }

        private void CustomerBtn_Click(object sender, RoutedEventArgs e)
        {
            CustomerWindow customerWindow = new CustomerWindow();
            customerWindow.homeBtn.IsEnabled = false;
            customerWindow.closeBtn.IsEnabled = true;
            customerWindow.ShowDialog();
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void openBillBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void closeBillBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void deleteBillVtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void logOutBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
