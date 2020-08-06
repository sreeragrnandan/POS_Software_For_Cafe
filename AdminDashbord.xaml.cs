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
//
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Callista_Cafe
{
    /// <summary>
    /// Interaction logic for AdminDashbord.xaml
    /// </summary>
    public partial class AdminDashbord : Window
    {
        private SqlConnection con;
        private SqlCommand cmd;
        private string ConString = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
        private SqlDataAdapter adapter;
        public AdminDashbord()
        {
            InitializeComponent();


            ///Enable This Before Merging into Master

            //if(!Classes.UserInfo.Login)
            //{
            //    MainWindow logWindow = new MainWindow();
            //    logWindow.Show();
            //    this.Close();
            //}

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

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            InventoryDashboard inv = new InventoryDashboard();
            inv.Show();
            this.Close();
        }

        private void MenuBtn_Click(object sender, RoutedEventArgs e)
        {
            MenuDashboard MenuWindow = new MenuDashboard();
            MenuWindow.Show();
            this.Close();
        }

        private void CustBtn_Click(object sender, RoutedEventArgs e)
        {
            CustomerWindow CustWindow = new CustomerWindow();
            CustWindow.Show();
            this.Close();
        }

        private void SupplierBtm_Click(object sender, RoutedEventArgs e)
        {
            SupplierWindow supWindow = new SupplierWindow();
            supWindow.Show();
            this.Close();
        }

        private void InventoryRemaining_OnLoaded(object sender, RoutedEventArgs e)
        {
            DataTable dt = new DataTable();
            try
            {
                con = new SqlConnection(ConString);
                con.Open();
                cmd = new SqlCommand("SELECT inv.ingredient,inv.quantity,inv.price, CASE WHEN inv.supplier_id IS NULL THEN '' ELSE (SELECT CONCAT(sup.supplier_name,' ',sup.supplier_mobile) FROM dbo.suppliers as sup WHERE sup.supplier_id=inv.supplier_id) END AS supplier_details FROM dbo.inventory as inv WHERE inv.quantity<inv.min_quantity", con);
                adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
                InventoryRemaining.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = dt });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                con.Close();
                adapter.Dispose();
            }
        }

        private void ExpiredItems_OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                string date = DateTime.Today.AddDays(7).ToString("yyyy-MM-dd");
                con = new SqlConnection(ConString);
                con.Open();
                cmd = new SqlCommand("select inv.ingredient,convert(varchar, inv.e_date, 3) as e_date,inv.price from dbo.inventory as inv where inv.e_date <= @date", con);
                cmd.Parameters.AddWithValue("@date", date.ToString());
                adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
                ExpiredItems.SetBinding(ItemsControl.ItemsSourceProperty, new Binding {Source = dt});
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                con.Close();
            }
        }
    }
}
