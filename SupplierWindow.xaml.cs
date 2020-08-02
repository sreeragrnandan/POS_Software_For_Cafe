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

//Added Namespaces
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Callista_Cafe.Classes;

namespace Callista_Cafe
{
    /// <summary>
    /// Interaction logic for SupplierWindow.xaml
    /// </summary>
    public partial class SupplierWindow : Window
    {
        private SqlConnection con;
        private SqlCommand cmd;
        private readonly String Connstring = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
        Supplier supplier_object = new Supplier();


        public SupplierWindow()
        {
            InitializeComponent();
        }

        private void AddSupplierBtnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!Txtsuppliername.Text.ToString().Equals(""))
                {
                    supplier_object.supplier_name = Txtsuppliername.Text.ToString();
                    supplier_object.supplier_mobile = Txtsuppliermobile.Text.ToString();

                    bool insert_result = supplier_object.insert_query(supplier_object);
                    if (insert_result)
                    {
                        DataTable dt = new DataTable();
                        dt = supplier_object.select_query();
                        Suppliers.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = dt });
                        MessageBox.Show("New Supplier Added", "Success");
                    }
                    else
                    {
                        MessageBox.Show("Error Creating New User", "Error");
                    }
                }
                else
                {
                    MessageBox.Show("Please Enter a Supplier Name to Proceed","Invalid Entry");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = supplier_object.select_query();
                Suppliers.SetBinding(ItemsControl.ItemsSourceProperty, new Binding {Source = dt});
                //dt.Dispose();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                
            }
        }

    }
}






//Insert

//cmd = new SqlCommand("Insert into dbo.suppliers values(@sup_id,@sup_name,@sup_mob_no)", con);
//cmd.Parameters.AddWithValue("@sup_id", Txtsupplierid.Text);
//cmd.Parameters.AddWithValue("@sup_name", Txtsuppliername.Text);
//cmd.Parameters.AddWithValue("@sup_mob_no", Txtsuppliermobile.Text);
//try
//{
//var res = cmd.ExecuteNonQuery();
//    if (res == 1)
//{
//    MessageBox.Show("Successful");
//}
//else
//{
//    MessageBox.Show(res.ToString());
//}