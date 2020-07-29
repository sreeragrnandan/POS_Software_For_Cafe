using Callista_Cafe.Classes;
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

        private void ADD_Click(object sender, RoutedEventArgs e)
        {

            c.cus_name = C_NAME.Text;
            c.cus_location = C_LOC.Text;
            c.cus_mobno = C_MOBNO.Text;
            c.cus_email = C_EMAIL.Text;


            bool success = c.insert(c);

            if (success == true)
            {
                MessageBox.Show("New Customer Insrted");
                clear();
            }
            else
            {
                MessageBox.Show("Failed");
            }

            DataTable dt = c.select();
            C_DG.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = dt });
        }

        private void MODIFY_Click(object sender, RoutedEventArgs e)
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

        private void DELETE_Click(object sender, RoutedEventArgs e)
        {
            c.cus_id = int.Parse(C_ID.Text);
            //sageBox.Show(c.cus_id.ToString);
            bool success = c.delete(c);
            if (success == true)
            {
                DataTable dt = c.select();
                C_DG.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = dt });
                MessageBox.Show("The Data Deleted successfully");
                clear();
            }
            else
            {
                MessageBox.Show("Failed the Deleting, Try again");
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
    }
}
