﻿using System;
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
                        reset_txtbox();
                    }
                    else
                    {
                        MessageBox.Show("Error Creating New Supplier", "Error");
                    }
                }
                else
                {
                    MessageBox.Show("Please Enter a Valid Name to Proceed","Invalid Entry");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void DeleteSupplierBtnClick(object sender, RoutedEventArgs e)
        {
            Supplier dltsupplier_object = new Supplier();
            MessageBoxResult msgbox = MessageBox.Show("Are You Sure?", "DELETE", MessageBoxButton.YesNo);
            switch (msgbox)
            {
                case MessageBoxResult.Yes:
                    dltsupplier_object.supplier_id = int.Parse(Txtsupplierid.Text);
                    bool result = supplier_object.delete_query(dltsupplier_object);
                    if (result)
                    {
                        MessageBox.Show("Successfully Deleted Supplier", "Success");
                    }
                    else
                    {
                        MessageBox.Show("Failed to Delete Supplier", "Failed");
                    }
                    DataTable dt = supplier_object.select_query();
                    Suppliers.SetBinding(ItemsControl.ItemsSourceProperty, new Binding {Source = dt});
                    break;
                case MessageBoxResult.No:
                    break;
            }
            reset_txtbox();
        }

        private void UpdateSupplierBtnClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult msgbox = MessageBox.Show("Are You Sure?", "Update", MessageBoxButton.YesNo);
            switch (msgbox)
            {
                case MessageBoxResult.Yes:
                    try
                    {
                        if (!Txtsuppliername.Text.ToString().Equals(""))
                        {
                            supplier_object.supplier_id = int.Parse(Txtsupplierid.Text.ToString());
                            supplier_object.supplier_name = Txtsuppliername.Text.ToString();
                            supplier_object.supplier_mobile = Txtsuppliermobile.Text.ToString();

                            bool update_result = supplier_object.update_query(supplier_object);
                            if (update_result)
                            {
                                DataTable dt = supplier_object.select_query();
                                Suppliers.SetBinding(ItemsControl.ItemsSourceProperty, new Binding {Source = dt});
                                MessageBox.Show("Supplier Details Updated Successfully", "Success");
                                reset_txtbox();
                            }
                            else
                            {
                                MessageBox.Show("Error Updating Supplier", "Error");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Please Enter a Supplier Name to Proceed", "Invalid Entry");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }
        private void ResetSupplierBtnClick(object sender, RoutedEventArgs e)
        {
            reset_txtbox();
            DataTable dt = new DataTable();
            dt = supplier_object.select_query();
            Suppliers.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = dt });
        }
        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = supplier_object.select_query();
                Suppliers.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = dt });
                //dt.Dispose();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Suppliers_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = (DataGrid)sender;
            DataRowView selected_row = dg.SelectedItem as DataRowView;

            if (selected_row != null)
            {
                Txtsupplierid.Text = selected_row[0].ToString();
                Txtsuppliername.Text = selected_row[1].ToString();
                Txtsuppliermobile.Text = selected_row[2].ToString();

                AddSupplierBtn.IsEnabled = false;
                UpdateSupplierBtn.IsEnabled = true;
                DeleteSupplierBtn.IsEnabled = true;
            }
        }

        public void reset_txtbox()
        {
            Txtsuppliername.Text = ""; Txtsuppliermobile.Text = ""; Txtsupplierid.Text = "";

            UpdateSupplierBtn.IsEnabled = false;
            AddSupplierBtn.IsEnabled = true;
            DeleteSupplierBtn.IsEnabled = false;
        }

        private void HomeBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AdminDashbord adminDashbord = new AdminDashbord();
                adminDashbord.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}