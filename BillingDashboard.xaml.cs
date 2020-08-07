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
            usernameLabel.Content = "User";
        }

        void timer_Tick(object sender, EventArgs e)
        {
            dateandtime.Content = DateTime.Now.ToString("dddd , MMM dd yyyy , hh:mm:ss tt");
        }
    }
}
