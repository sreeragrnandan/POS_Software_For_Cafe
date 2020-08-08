using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;
using Callista_Cafe.Classes;

namespace Callista_Cafe
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SqlCommand cmd;
        private SqlConnection con;
        private SqlDataReader reader;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var res = 0;
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
                con.Open();
                cmd = new SqlCommand("Select * from users where user_name=@UserId", con);
                cmd.Parameters.AddWithValue("@UserId", Username.Text);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    if (reader["password"].ToString().Equals(password.Password, StringComparison.InvariantCulture))
                    {
                        UserInfo.User_Name = Username.Text;
                        UserInfo.User_Id = int.Parse(reader["user_id"].ToString());
                        UserInfo.User_Type = int.Parse(reader["usertype"].ToString());
                        UserInfo.Login = true;
                        res = 1;
                    }
                    else
                    {
                        MessageBox.Show("Password is incorrect...!", "Info");
                    }
                }
                else
                {
                    MessageBox.Show("Username is incorrect...!", "Info");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Info");
            }
            finally
            {
                reader.Close();
                reader.Dispose();
                cmd.Dispose();
                con.Close();
                if (res == 1)
                {
                    if (UserInfo.User_Type == 0)
                    {
                        var adminWindow = new AdminDashbord();
                        adminWindow.Show();
                        Close();
                    }
                    else if (UserInfo.User_Type == 1)
                    {
                        AdminDashbord adminDashbord = new AdminDashbord();
                        adminDashbord.Show();
                        this.Close();
                    }
                    else if (UserInfo.User_Type == 2)
                    {
                        BillingDashboard billingDashboard = new BillingDashboard();
                        billingDashboard.Show();
                        this.Close();
                    }
                }
            }
        }
    }
}