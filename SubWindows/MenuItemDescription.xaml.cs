using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.AccessControl;
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
    /// Interaction logic for MenuItemDescription.xaml
    /// </summary>
    public partial class MenuItemDescription : Window
    {
        
        private string item_desc = "";
        private int item_id = 0;
        private string code = "";
        public MenuItemDescription()
        {
            InitializeComponent();
            
        }
        private SqlConnection con;
        private SqlCommand cmd;
        public MenuItemDescription(int id, string desc, string c_code) : this()
        {
            this.item_id = id;
            this.item_desc = desc;
            this.code = c_code;
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DescTextBox.Text = item_desc;

            if (UserInfo.User_Type == 0)
            {
                CCodeTxtBox.Text = code;
            }
            else
            {
                CCodeTxtBox.IsEnabled = false;
            }
        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            if(DescTextBox.Text.Equals(item_desc) && CCodeTxtBox.Text.Equals(code))
                this.Close();
            else
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure?", "CLOSE", MessageBoxButton.YesNo);
                switch (messageBoxResult)
                {
                    case MessageBoxResult.Yes:
                        this.Close();
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }
        }

        private void updateBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure?", "CLOSE", MessageBoxButton.YesNo);
            switch (messageBoxResult)
            {
                case MessageBoxResult.Yes:
                    update();
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }

        private void update()
        {
            bool flag = true;
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            if (UserInfo.User_Type == 0)
            {
                cmd = new SqlCommand(
                    "UPDATE menu_items SET c_code=@code, description=@desc WHERE item_id=@id",
                    con);
                cmd.Parameters.AddWithValue("@id", item_id);
                cmd.Parameters.AddWithValue("@code", CCodeTxtBox.Text);
                cmd.Parameters.AddWithValue("@desc", DescTextBox.Text);
            }
            else
            {
                cmd = new SqlCommand(
                    "UPDATE menu_items SET  description=@desc WHERE item_id=@id",
                    con);
                cmd.Parameters.AddWithValue("@id", item_id);
                cmd.Parameters.AddWithValue("@desc", DescTextBox.Text);
            }

            try
            {
                con.Open();
                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    MessageBox.Show("Updated successfully..!", "Success");
                }
                else
                {
                    MessageBox.Show("Failed to Update. Try Again !", "Error");
                }
            }
            catch (Exception exception)
            {
                if (exception.ToString().Contains("Violation of UNIQUE KEY"))
                {
                    MessageBox.Show("Item already exist !", "Error");
                }
                else
                    MessageBox.Show("Something went wrong. Please try again.", "Error");
            }
            finally
            {
                con.Close();
                this.Close();
            }
        }
    }
}
