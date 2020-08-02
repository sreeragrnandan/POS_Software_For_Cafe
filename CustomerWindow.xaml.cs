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
        Customer c = new Customer();


        void OnLoad(object sender, RoutedEventArgs e)
        {
            DataTable dt = c.select();
            C_DG.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = dt });
        }

        private void ADD_Click(object sender, RoutedEventArgs e)//Insert
        {
            bool success = false;

            c.cus_name = C_NAME.Text;
            c.cus_location = C_LOC.Text;
            c.cus_mobno = C_MOBNO.Text;
            c.cus_email = C_EMAIL.Text;

            if (c.cus_name == "")
            {
                success = false;
                MessageBox.Show("Please fill the fields");
            }
            else
            {

                success = c.insert(c);

                if (success == true)
                {
                    MessageBox.Show("New Customer details Inserted");
                    clear();
                }
                else
                {
                    MessageBox.Show("Insertion Failed. Try again");
                }

                DataTable dt = c.select();
                C_DG.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = dt });
            }
        }

        private void MODIFY_Click(object sender, RoutedEventArgs e)//Modify
        {
            MessageBoxResult result = MessageBox.Show("Do you want to modify the data ?", "Confirmation", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {

                if (C_ID.Text == "")
                {
                    MessageBox.Show("Please Select a Field");
                }
                else
                {
                    c.cus_name = C_NAME.Text;
                    c.cus_id = int.Parse(C_ID.Text);
                    c.cus_mobno = C_MOBNO.Text;
                    c.cus_location = C_LOC.Text;
                    c.cus_email = C_EMAIL.Text;

                    bool success = c.update(c);
                    if (success == true)
                    {
                        DataTable dt = c.select();
                        C_DG.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = dt });
                        MessageBox.Show("Customer details are updated");
                        clear();

                    }
                    else
                    {
                        MessageBox.Show("Failed To update Try again");
                    }
                }
            }

        }

        private void DELETE_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to Delete the field", "Confirmation", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                bool success = false;


                c.cus_name = C_NAME.Text;
                if (c.cus_name == "")
                {
                    MessageBox.Show("Please select a field to delete");
                }
                else
                {
                    c.cus_id = int.Parse(C_ID.Text);
                    c.cus_name = C_NAME.Text;
                    if (c.cus_name == "")
                    {
                        success = false;
                    }
                    //sageBox.Show(c.cus_id.ToString);
                    else
                    {
                        success = c.delete(c);
                    }
                    if (success == true)
                    {
                        DataTable dt = c.select();
                        C_DG.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = dt });
                        MessageBox.Show("The Data Deleted");
                        clear();
                    }
                    else
                    {
                        MessageBox.Show("Select an item to delete");
                    }
                }
            }


        }

        private void CLEAR_Click(object sender, RoutedEventArgs e)
        {
            clear();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void clear()
        {
            C_ID.Text = "";
            C_NAME.Text = "";
            C_LOC.Text = "";
            C_MOBNO.Text = "";
            C_EMAIL.Text = "";
        }

        private void C_DG_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // For Data grid edit
            DataGrid gd = (DataGrid)sender;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                C_NAME.Text = row_selected["C_NAME"].ToString();
                C_ID.Text = row_selected["C_ID"].ToString();
                C_EMAIL.Text = row_selected["C_EMAIL"].ToString();
                C_MOBNO.Text = row_selected["C_MOBNO"].ToString();
                C_LOC.Text = row_selected["C_LOCATION"].ToString();


            }
        }
        static string myConString = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
        private void C_SEARCH_TextChanged(object sender, TextChangedEventArgs e)
        {
            //get the value from text box

            string key = C_SEARCH.Text;

            SqlConnection conn = new SqlConnection(myConString);
            SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM customer WHERE c_name LIKE '%" + key + "%' OR c_location LIKE '%" + key + "%'", conn);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            C_DG.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = dt });

        }

        private void Refresh_click(object sender, RoutedEventArgs e)
        {
            DataTable dt = c.select();
            C_DG.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = dt });
        }
    }
}
