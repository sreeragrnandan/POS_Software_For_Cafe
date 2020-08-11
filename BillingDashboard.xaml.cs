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
using System.Windows.Threading;
using Callista_Cafe.Classes;
using Callista_Cafe.SubWindows;

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
            FillComboBox();
        }

        private SqlConnection con;
        private SqlCommand cmd;

        Bills bills = new Bills();

        Bills tosentBills = new Bills();

        DatabaseFunctions DbFun = new DatabaseFunctions();

        private int bill_id;

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

        private bool bindDataBills(bool id)
        {

            bool flag = true;


            if (id)
            {
                if (bill_id.ToString().Equals("") || bill_id.ToString().Equals("0"))
                {
                    MessageBox.Show("BILL ID ERROR. Failed to Update..!", "Error");
                    flag = false;
                    goto FUNEND;
                }
                else
                {
                    try
                    {
                        tosentBills.bill_id = bill_id;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("BILL_ID ERROR. Failed to Update..!", "Error");
                        flag = false;
                        goto FUNEND;
                    }
                }
            }
            else
            {
                tosentBills.bill_id = 0;
            }

            if (tableTxtBox.Text.Equals(""))
            {
                MessageBox.Show("Please enter the table name..!", "Error");
                flag = false;
                goto FUNEND;
            }
            else
            {
                tosentBills.bill_table = tableTxtBox.Text;
            }

            if (PaymentComboBox.Text.ToString().Equals(""))
            {
                MessageBox.Show("Please Select a payment method..!", "Error");
                flag = false;
                goto FUNEND;
            }
            else
            {
                tosentBills.bill_payment = PaymentComboBox.Text;
            }

            if (CustomerComboBox.Text.ToString().Equals(""))
                tosentBills.bill_customer = "";
            else
            {
                try
                {
                    tosentBills.bill_customer = CustomerComboBox.SelectedValue.ToString();
                }
                catch (Exception ex)
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show("Customer Not found. Do you want to add?", "Customer", MessageBoxButton.YesNo);
                    switch (messageBoxResult)
                    {
                        case MessageBoxResult.Yes:
                            CustomerWindow customerWindow = new CustomerWindow();
                            customerWindow.homeBtn.IsEnabled = false;
                            customerWindow.closeBtn.IsEnabled = true;
                            customerWindow.CusNameTextBox.Text = CustomerComboBox.Text;
                            customerWindow.isOpenedForAdd = true;
                            customerWindow.CusNameTextBox.IsEnabled = false;
                            customerWindow.clearBtn.IsEnabled = false;
                            customerWindow.ShowDialog();
                            if (DbFun.getCustomerId(CustomerComboBox.Text.ToString())!=0)
                            {
                                tosentBills.bill_customer = DbFun.getCustomerId(CustomerComboBox.Text.ToString()).ToString();
                            }
                            else
                            {
                                tosentBills.bill_customer = "";
                            }
                            break;
                        case MessageBoxResult.No:
                            tosentBills.bill_customer = "";
                            break;
                    }
                }
            }

            FUNEND: {}
            return flag;
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
            bool bindingResult = bindDataBills(false);
            bool result;
            if (bindingResult)
            {
                result = bills.insert(tosentBills);
                if (result)
                {
                    MessageBox.Show("New Item Inserted..!", "Success");
                    loadBillGrid();
                    reset();
                }
            }

        }

        private void updateBtn_Click(object sender, RoutedEventArgs e)
        {
            

        }


        private void closeBillBtn_Click(object sender, RoutedEventArgs e)
        {
            bool result = false;
            if (DbFun.getbilltotalitems(bill_id) !=0)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Do you want to print the bill?", "Close Bill", MessageBoxButton.YesNoCancel);
                switch (messageBoxResult)
                {
                    case MessageBoxResult.Yes:
                        result = bills.closeBill(bill_id);
                        if (result)
                        {
                            MessageBox.Show("Print will start here", "Printing");
                            reset();
                        }
                        else
                        {
                            MessageBox.Show("Something went wrong.", "Try again");
                        }
                        break;
                    case MessageBoxResult.No:
                        result = bills.closeBill(bill_id);
                        if (result)
                        {
                            MessageBox.Show("Bill Closed", "closed");
                            reset();
                        }
                        else
                        {
                            MessageBox.Show("Something went wrong.", "Try again");
                        }
                        break;
                    case MessageBoxResult.Cancel:
                        break;
                }
            }
            else
            {
                MessageBox.Show("No items found on this bill. Please delete the bill", "Warning");
            }

        }

        private void deleteBillBtn_Click(object sender, RoutedEventArgs e)
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

        private void activeBills_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                bill_id = int.Parse(row_selected[0].ToString());
                tableTxtBox.Text = row_selected[2].ToString();
                CustomerComboBox.Text = row_selected[6].ToString();
                PaymentComboBox.Text = row_selected[1].ToString();
                addBtn.IsEnabled = false;
                deleteBillBtn.IsEnabled = true;
                closeBillBtn.IsEnabled = true;
            }
        }

        private void FillComboBox()
        {
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
                con.Open();
                cmd = new SqlCommand("Select * from customer", con);
                SqlDataAdapter Supplieradapter = new SqlDataAdapter(cmd);
                DataTable SupplierDt = new DataTable();
                Supplieradapter.Fill(SupplierDt);
                CustomerComboBox.ItemsSource = SupplierDt.DefaultView;
                CustomerComboBox.DisplayMemberPath = "c_name";
                CustomerComboBox.SelectedValuePath = "c_id";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Info");
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
        }

        private void reset()
        {
            FillComboBox();
            tableTxtBox.Text = "";
            CustomerComboBox.Text = "";
            addBtn.IsEnabled = true;
            deleteBillBtn.IsEnabled = false;
            closeBillBtn.IsEnabled = false;
        }

        private void resetBtn_Click(object sender, RoutedEventArgs e)
        {
            reset();
        }

        private void ActiveBills_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            AddBillItems addBillItems = new AddBillItems();
            if (row_selected != null)
            {
                addBillItems.bill_id = int.Parse(row_selected[0].ToString()); ;
                addBillItems.TableTextBox.Text = row_selected[2].ToString();
                addBillItems.ShowInTaskbar = false;
                addBillItems.ShowDialog();
            }
        }
    }
}
