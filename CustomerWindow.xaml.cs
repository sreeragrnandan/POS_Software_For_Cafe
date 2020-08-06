using Callista_Cafe.Classes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        public CustomerWindow()
        {
            InitializeComponent();
        }

        private SqlConnection con;
        private SqlCommand cmd;


        Customer cust = new Customer();
        Customer ToSendCustomer = new Customer();


        DatabaseFunctions DbFun = new DatabaseFunctions();


        private int Customer_id=0;

        public void loadGrid()
        {
            DataTable customerDataTable = cust.select();
            Customers.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = customerDataTable });
        }
        private void Customers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                Customer_id = int.Parse(row_selected[0].ToString());
                CusNameTextBox.Text = row_selected[1].ToString();
                mobTxtBox.Text = row_selected[2].ToString();
                dobTxtBox.Text = row_selected[3].ToString();
                emailTxtBox.Text = row_selected[4].ToString();
                locationTxtBox.Text = row_selected[5].ToString();

                addBtn.IsEnabled = false;
                addBtn.IsDefault = false;
                updateBtn.IsEnabled = true;
                delBtn.IsEnabled = true;
            }
        }

        private void Customers_Loaded(object sender, RoutedEventArgs e)
        {
            loadGrid();
        }

        private void reset()
        {
            Customer_id = 0;
            CusNameTextBox.Text = "";
            dobTxtBox.Text = "";
            emailTxtBox.Text = "";
            locationTxtBox.Text = "";
            mobTxtBox.Text = "";
            addBtn.IsEnabled = true;
            updateBtn.IsEnabled = false;
            delBtn.IsEnabled = false;
            addBtn.IsDefault = true;
            loadGrid();
        }

        private void clearBtn_Click(object sender, RoutedEventArgs e)
        {
            reset();
        }

        private bool BindDataToObject(bool id)
        {
            bool flag = true;
            if (id)
            {
                if (Customer_id==0)
                {
                    MessageBox.Show("Customer_ID ERROR..!", "Error");
                    flag = false;
                    goto FUNEND;
                }
                else
                {
                    ToSendCustomer.c_id = Customer_id;
                }
            }
            else
            {
                ToSendCustomer.c_id = 0;
            }

            if (CusNameTextBox.Text.ToString().Equals(""))
            {
                MessageBox.Show("Please Enter the Name.", "Error");
                flag = false;
                goto FUNEND;
            }
            else
            {
                try
                {
                    ToSendCustomer.c_name = CusNameTextBox.Text.ToString();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Please Enter a valid Name.!", "Error");
                    flag = false;
                    goto FUNEND;
                }
            }

            if (!dobTxtBox.Text.Equals(""))
            {
                try
                {
                    ToSendCustomer.c_dob = dobTxtBox.Text;
                }
                catch
                {
                    MessageBox.Show("Please Enter a valid Date.!", "Error");
                    flag = false;
                    goto FUNEND;
                }
            }
            else
            {
                ToSendCustomer.c_dob = "";
            }
            
            ToSendCustomer.c_email = emailTxtBox.Text;
            ToSendCustomer.loc = locationTxtBox.Text;
            ToSendCustomer.c_mob = mobTxtBox.Text;
            FUNEND: { }
            return flag;
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            bool bindingResult = BindDataToObject(false);
            bool result;
            if (bindingResult)
            {
                result = cust.insert(ToSendCustomer);
                if (result)
                {
                    MessageBox.Show("New Item Inserted..!", "Success");
                    reset();
                }
            }
        }

        private void updateBtn_Click(object sender, RoutedEventArgs e)
        {
            bool bindingResult = BindDataToObject(true);
            bool result = false;
            if (bindingResult)
            {
                bool areyousure = DbFun.areyousure();
                if (areyousure)
                {
                    result = cust.update(ToSendCustomer);
                    if (result)
                    {
                        MessageBox.Show("Customer details updated successfully !", "Success");
                        reset();
                        loadGrid();
                    }
                }
            }
        }

        private void delBtn_Click(object sender, RoutedEventArgs e)
        {
            bool id = Customer_id.ToString().Equals("0");
            bool result = false;
            if (id)
            {
                MessageBox.Show("Please select a customer.", "Error");
            }
            else
            {
                bool areyousure = DbFun.areyousure();
                if (areyousure)
                {
                    ToSendCustomer.c_id = Customer_id;
                    result = cust.delete(ToSendCustomer);
                    if (result)
                    {
                        MessageBox.Show("Item Successfully Deleted !", "Success");
                        reset();
                    }
                    else
                    {
                        loadGrid();
                    }
                }
            }
        }

        private void searchTxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string S_key = searchTxtBox.Text;
            try
            {
                if (S_key == "")
                {
                    loadGrid();
                }
                else
                {
                    DataTable dt = new DataTable();
                    con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
                    con.Open();
                    SqlDataAdapter sda = new SqlDataAdapter(
                        "SELECT * FROM customer WHERE c_name LIKE '%" + S_key + "%' OR c_location LIKE'%" + S_key + "%'  OR c_mobno LIKE'%" + S_key + "%';", con);
                    sda.Fill(dt);
                    Customers.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = dt });
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString(), "Info");
            }
            finally
            {
                con.Close();
            }
        }
    }
}
