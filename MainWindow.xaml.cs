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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace Callista_Cafe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader reader;
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
            int res = 0;
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
                con.Open();
                cmd = new SqlCommand("Select * from users where user_name=@UserId", con);
                cmd.Parameters.AddWithValue("@UserId", Username.Text.ToString());
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    if (reader["password"].ToString().Equals(password.Password.ToString(), StringComparison.InvariantCulture))
                    {
                        
                        Classes.UserInfo.User_Name = Username.Text.ToString();
                        Classes.UserInfo.User_Id = int.Parse(reader["user_id"].ToString());
                        Classes.UserInfo.User_Type = int.Parse(reader["usertype"].ToString());
                        Classes.UserInfo.Login = true;
                        res = 1;
                    }
                    else
                    {
                        MessageBox.Show("Password is incorrect...!","Info");
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
                if(res == 1)
                {
                    if (Classes.UserInfo.User_Type == 0)
                    {
                        AdminDashbord adminWindow = new AdminDashbord();
                        adminWindow.Show();
                        this.Close();
                    }
                    else if(Classes.UserInfo.User_Type == 1)
                    {
                        MessageBox.Show("Not Available");
                    }
                    else
                    {
                        MessageBox.Show("Something went wrong please contact your Administrator.", "Error");
                    }
                }
            }
        }
    }
}
